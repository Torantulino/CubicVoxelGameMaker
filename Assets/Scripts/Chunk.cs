using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    public bool has_loaded = false;
    public bool needs_updating = true;
    public bool unload = false;
    public Vector2Int Position;

    public Block[,,] blocks = 
        new Block[World.CHUNK_SIZE, World.WORLD_HEIGHT, World.CHUNK_SIZE];

    public Chunk(Vector2Int _position)
    {
        // Set Properties
        Position = _position;

        for (int x = 0; x < World.CHUNK_SIZE; x++)
        {
            for (int y = 0; y < World.WORLD_HEIGHT; y++)
            {
                for (int z = 0; z < World.CHUNK_SIZE; z++)
                {
                    // Get landscape height
                    int height = Noise.GetBlockHeight(x, z, Position);

                    // Set block type
                    int block_type;
                    // Air
                    if(y > height)
                        block_type = (int)BlockInfo.BlockType.Air;
                    // Grass
                    else if(y == height)
                    {
                        //Snowy
                        if(y >= World.WORLD_HEIGHT - World.random.Next(23, 25))
                            block_type = (int)BlockInfo.BlockType.Snow;
                        //Normal
                        else
                            block_type = (int)BlockInfo.BlockType.Grass;
                    }
                    // Dirt
                    else if(y > height - World.random.Next(3, 6)) // Random.Range(3,6))
                        block_type = (int)BlockInfo.BlockType.Dirt;
                    // Stone & Dirt
                    else
                    {
                        if(World.random.Next(0, 100) <= 70)
                            block_type = (int)BlockInfo.BlockType.Stone;
                        else
                            block_type = (int)BlockInfo.BlockType.Dirt;
                    }

                    // Create block
                    Block b = new Block(block_type,
                        new Vector3(x,y,z));
                    blocks[x,y,z] = b;
                }
            }
        }

        //TODO: Surely this is obselete if within constructor
        has_loaded = true;
    }
}
