using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    public bool hasLoaded = false;
    public Block[,,] blocks = 
        new Block[World.CHUNK_SIZE, World.WORLD_HEIGHT, World.CHUNK_SIZE];

    public Chunk(TextureManager texture_manager)
    {
        for (int i = 0; i < World.CHUNK_SIZE; i++)
        {
            for (int j = 0; j < World.WORLD_HEIGHT; j++)
            {
                for (int k = 0; k < World.CHUNK_SIZE; k++)
                {
                    Block b = new Block((int)BlockInfo.BlockType.Grass, new Vector3(i,j,k), texture_manager);
                    blocks[i,j,k] = b;
                }
            }
        }

        hasLoaded = true;
    }
}
