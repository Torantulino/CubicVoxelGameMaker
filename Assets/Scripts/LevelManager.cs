using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class LevelManager : MonoBehaviour
{

    public TextureManager Texture_Manager;

    public ChunkManager Chunk_Manager;

    World world;
    // Start is called before the first frame update
    void Start()
    {   
        // Create managers
        Texture_Manager = new TextureManager();
        Chunk_Manager = new ChunkManager();
        world = new World();


        for(int x = 0; x < 2; x++)
        {
            for (int z = 0; z < 2; z++)
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
