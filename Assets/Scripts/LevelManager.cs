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

    /// Awake is called when the script instance is being loaded.
    void Awake()
    {
        player = GameObject.Find("Player");

        SaveData save_data = LoadGame("test_world14");
        if (save_data != null)
        {
            Chunk_Manager = new ChunkManager(save_data);
            player.transform.position = save_data.Player_Position + Vector3.up * 0.05f;
        }
        else
            Chunk_Manager = new ChunkManager();

        world = new World(this);

        // This instantiates the shared block material so other threads can access it 
        // (As loading must be carried out in main thread)
        Material block_mat = TextureManager.Block_Material;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        world.UpdateWorld();
    }

    void OnApplicationQuit()
    {
        SaveGame("test_world14");
    }

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

        // Custom selectors
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

    private SaveData LoadGame(string _filename)
    {
        string path = Application.persistentDataPath + "/" + _filename + ".dat";

        // Open Filestream
        FileStream file_stream;
        if (File.Exists(path))
            file_stream = File.OpenRead(path);
        else
        {
            Debug.LogWarning("File not found");
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

public class Vector2IntSerializationSurrogate : ISerializationSurrogate
{

    // Method called to serialize a Vector3 object
    public void GetObjectData(System.Object obj, SerializationInfo info, StreamingContext context)
    {

        Vector2Int v2 = (Vector2Int)obj;
        info.AddValue("x", v2.x);
        info.AddValue("y", v2.y);
    }

    // Method called to deserialize a Vector3 object
    public System.Object SetObjectData(System.Object obj, SerializationInfo info,
                                       StreamingContext context, ISurrogateSelector selector)
    {
        Vector2Int v2 = (Vector2Int)obj;
        v2.x = (int)info.GetValue("x", typeof(int));
        v2.y = (int)info.GetValue("y", typeof(int));
        obj = v2;
        return obj;
    }
}

public class Vector3SerializationSurrogate : ISerializationSurrogate
{

    // Method called to serialize a Vector3 object
    public void GetObjectData(System.Object obj, SerializationInfo info, StreamingContext context)
    {

        Vector3 v3 = (Vector3)obj;
        info.AddValue("x", v3.x);
        info.AddValue("y", v3.y);
        info.AddValue("z", v3.z);
    }

    // Method called to deserialize a Vector3 object
    public System.Object SetObjectData(System.Object obj, SerializationInfo info,
                                       StreamingContext context, ISurrogateSelector selector)
    {

        Vector3 v3 = (Vector3)obj;
        v3.x = (float)info.GetValue("x", typeof(float));
        v3.y = (float)info.GetValue("y", typeof(float));
        v3.z = (float)info.GetValue("z", typeof(float));
        obj = v3;
        return obj;
    }
}

