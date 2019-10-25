using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class LevelManager : MonoBehaviour
{
    public ChunkManager Chunk_Manager;

    World world;

    /// Awake is called when the script instance is being loaded.
    void Awake()
    {
        // This instantiates the shared block material so other threads can access it 
        // (As loading must be carried out in main thread)
        Material block_mat = TextureManager.Block_Material;
    }

    // Start is called before the first frame update
    void Start()
    {   
        // Create managers
        Chunk_Manager = new ChunkManager();
        world = new World();


        for(int x = 0; x < 8; x++)
        {
            for (int z = 0; z < 8; z++)
            {
                world.CreateChunk(new Vector2Int(x, z));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        world.UpdateWorld();
    }
}
