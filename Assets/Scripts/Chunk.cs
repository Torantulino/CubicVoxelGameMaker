using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    public bool has_loaded = false;
    public bool needs_updating = true;
    
    public GameObject Game_Object = new GameObject("Chunk");

    public Block[,,] blocks = 
        new Block[World.CHUNK_SIZE, World.WORLD_HEIGHT, World.CHUNK_SIZE];

    public Chunk()
    {
        for (int i = 0; i < World.CHUNK_SIZE; i++)
        {
            for (int j = 0; j < World.WORLD_HEIGHT; j++)
            {
                for (int k = 0; k < World.CHUNK_SIZE; k++)
                {
                    Block b = new Block((int)BlockInfo.BlockType.Grass,
                        new Vector3(i,j,k));
                    blocks[i,j,k] = b;
                }
            }
        }

        has_loaded = true;
    }
}
