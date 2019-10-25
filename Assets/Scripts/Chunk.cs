using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    public bool has_loaded = false;
    public bool needs_updating = true;
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
                    // Get height from noise
                    // Chunk position used for continuation between chunks
                    // WORLD_HEIGHT - 10 used as scale to prevent mountains up to build limit
                    float scale = World.WORLD_HEIGHT-10;
                    float noise_sample = Mathf.PerlinNoise((float)(x + (Position.x)*World.CHUNK_SIZE) * World.NOISE_SCALE,
                                                            (float)(z + (Position.y)*World.CHUNK_SIZE) * World.NOISE_SCALE);
                    int height = (int)(scale * noise_sample);

                    // Set block type
                    int block_type;
                    // Grass
                    if(y > height)
                        block_type = (int)BlockInfo.BlockType.Air;
                    else if(y == height)
                        block_type = (int)BlockInfo.BlockType.Grass;
                    // Dirt
                    else if(y > height - Random.Range(3,6))
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
}
