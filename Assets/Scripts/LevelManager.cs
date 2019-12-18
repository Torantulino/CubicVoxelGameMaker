using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.U2D;

public class LevelManager : MonoBehaviour
{
    public ChunkManager Chunk_Manager;
    public World world;
    private GameObject player;

    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        player = GameObject.Find("Player");

        SaveData save_data = LoadGame("test_world14");
        if (save_data != null)
        {
            Physics.gravity = Vector3.zero;
            Chunk_Manager = new ChunkManager(save_data);
            player.transform.position = save_data.Player_Position + Vector3.up * 0.15f;
        }
        else
            Chunk_Manager = new ChunkManager();

        world = new World(this);

        // This instantiates the shared block material so other threads can access it 
        // (As loading must be carried out in main thread)
        Material block_mat = TextureManager.Block_Material;
    }

    // Update is called once per frame
    void Update()
    {
        world.UpdateWorld();
    }

    // Save game on quit. In future specify user provided filename
    void OnApplicationQuit()
    {
        SaveGame("test_world14");
    }

    // Saves the game with the specified filename
    private void SaveGame(string _filename)
    {
        string path = Application.persistentDataPath + "/" + _filename + ".dat";

        // Open Filestream
        FileStream file_stream;
        if (File.Exists(path))
            file_stream = File.OpenWrite(path);
        else
            file_stream = File.Create(path);

        BinaryFormatter binary_formatter = new BinaryFormatter();
        SurrogateSelector surrogate_selector = new SurrogateSelector();

        // Custom serialisation surrogates
        Vector2IntSerializationSurrogate vector2i_selector = new Vector2IntSerializationSurrogate();
        Vector3SerializationSurrogate vector3_selector = new Vector3SerializationSurrogate();

        // Add
        surrogate_selector.AddSurrogate(typeof(Vector2Int),
            new StreamingContext(StreamingContextStates.All), vector2i_selector);
        surrogate_selector.AddSurrogate(typeof(Vector3),
            new StreamingContext(StreamingContextStates.All), vector3_selector);

        binary_formatter.SurrogateSelector = surrogate_selector;

        // Save file
        SaveData current_data = new SaveData(Chunk_Manager, player.transform.position);
        binary_formatter.Serialize(file_stream, current_data);

        file_stream.Close();
    }

    // Loads the game with specifed filename
    private SaveData LoadGame(string _filename)
    {
        string path = Application.persistentDataPath + "/" + _filename + ".dat";

        // Open Filestream
        FileStream file_stream;
        if (File.Exists(path))
            file_stream = File.OpenRead(path);
        else
        {
            Debug.LogWarning("Save file not found");
            return null;
        }

        BinaryFormatter binary_formatter = new BinaryFormatter();
        SurrogateSelector surrogate_selector = new SurrogateSelector();

        // Custom selectors
        Vector2IntSerializationSurrogate vector2i_selector = new Vector2IntSerializationSurrogate();
        Vector3SerializationSurrogate vector3_selector = new Vector3SerializationSurrogate();

        // Add
        surrogate_selector.AddSurrogate(typeof(Vector2Int),
            new StreamingContext(StreamingContextStates.All), vector2i_selector);
        surrogate_selector.AddSurrogate(typeof(Vector3),
            new StreamingContext(StreamingContextStates.All), vector3_selector);


        binary_formatter.SurrogateSelector = surrogate_selector;

        // Load file
        SaveData loaded_data = (SaveData)binary_formatter.Deserialize(file_stream);
        file_stream.Close();

        return loaded_data;
    }

}

// Serialisable class contraining level sava data
[System.Serializable]
public class SaveData
{
    public List<Vector2Int> Keys = new List<Vector2Int>();

    public List<Chunk> Values;

    public Vector3 Player_Position;

    public SaveData(ChunkManager _chunk_manager, Vector3 _player_position)
    {
        Player_Position = _player_position;
        Keys = _chunk_manager.Modified_Chunks.Keys.ToList();
        Values = _chunk_manager.Modified_Chunks.Values.ToList();
    }
}

// Serialisation surrogate for Vector2Int
public class Vector2IntSerializationSurrogate : ISerializationSurrogate
{
    // How to Serialise Vector2Int
    public void GetObjectData(System.Object obj, SerializationInfo info, StreamingContext context)
    {

        Vector2Int vector2 = (Vector2Int)obj;
        info.AddValue("x", vector2.x);
        info.AddValue("y", vector2.y);
    }

    // How to Deserialise Vector2Int
    public System.Object SetObjectData(System.Object obj, SerializationInfo info,
                                       StreamingContext context, ISurrogateSelector selector)
    {
        Vector2Int vector2 = (Vector2Int)obj;
        vector2.x = (int)info.GetValue("x", typeof(int));
        vector2.y = (int)info.GetValue("y", typeof(int));
        obj = vector2;
        return obj;
    }
}

// Serialisation surrogate for Vector3s
public class Vector3SerializationSurrogate : ISerializationSurrogate
{
    // How to Serialise Vector3
    public void GetObjectData(System.Object obj, SerializationInfo info, StreamingContext context)
    {

        Vector3 vector3 = (Vector3)obj;
        info.AddValue("x", vector3.x);
        info.AddValue("y", vector3.y);
        info.AddValue("z", vector3.z);
    }

    // How to Deserialise Vector3
    public System.Object SetObjectData(System.Object obj, SerializationInfo info,
                                       StreamingContext context, ISurrogateSelector selector)
    {
        Vector3 vector3 = (Vector3)obj;
        vector3.x = (float)info.GetValue("x", typeof(float));
        vector3.y = (float)info.GetValue("y", typeof(float));
        vector3.z = (float)info.GetValue("z", typeof(float));
        obj = vector3;
        return obj;
    }
}

