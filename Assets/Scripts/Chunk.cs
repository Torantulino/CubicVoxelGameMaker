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
        for (int x = 0; x < World.CHUNK_SIZE; x++)
        {
            for (int y = 0; y < World.WORLD_HEIGHT; y++)
            {
                for (int z = 0; z < World.CHUNK_SIZE; z++)
                {
                    int block_type;

                    // Grass
                    if(y > World.SEA_LEVEL)
                        block_type = (int)BlockInfo.BlockType.Air;
                    else if(y == World.SEA_LEVEL)
                        block_type = (int)BlockInfo.BlockType.Grass;
                    // Dirt
                    else if(y > World.SEA_LEVEL - 5)
                        block_type = (int)BlockInfo.BlockType.Dirt;
                    // Stone & Dirt
                    else
                    {
                        if(Random.value <= 0.7)
                            block_type = (int)BlockInfo.BlockType.Stone;
                        else
                            block_type = (int)BlockInfo.BlockType.Dirt;
                    }
                        
                    Block b = new Block(block_type,
                        new Vector3(x,y,z));
                    blocks[x,y,z] = b;
                }
            }
        }

        has_loaded = true;
    }

    //This doesn't work, and will anyway be replaced by polling hightmap
    // private bool CheckIfTopBlock(int x, int y, int z)
    // {
    //     for (int i = y; i < World.WORLD_HEIGHT; i++)
    //     {
    //         if(blocks[x, i, z].type == (int)BlockInfo.BlockType.Air)
    //             return false;
    //     }
    //     return true;
    // }
}
