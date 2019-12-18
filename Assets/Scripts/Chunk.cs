using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Chunk
{
    public bool has_loaded = false;
    public bool needs_updating = true;
    public bool unload = false;
    public Vector2Int Position;
    public Block[,,] blocks =
        new Block[World.CHUNK_SIZE, World.WORLD_HEIGHT, World.CHUNK_SIZE];

    public Chunk(Vector2Int _position, bool generate_landscape = true)
    {
        // Set Properties
        Position = _position;

        if(!generate_landscape)
            return;

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
                    // Above hieght-map
                    if (y > height)
                    {
                        //Air
                        block_type = (int)BlockInfo.BlockType.Air;
                    }
                    // Top Soil
                    else if (y == height)
                    {
                        // Snowy Grass
                        if (y >= World.WORLD_HEIGHT - 3.0f / 8.0f * World.WORLD_HEIGHT)
                            block_type = (int)BlockInfo.BlockType.Snow;
                        // Dry Grass
                        else if (y > World.WORLD_HEIGHT - 4.0f / 8.0f * World.WORLD_HEIGHT)
                            block_type = (int)BlockInfo.BlockType.Dry_Grass;
                        // Sand
                        else if (y < World.SEA_LEVEL)
                            block_type = (int)BlockInfo.BlockType.Sand;
                        // Dark Grass
                        else if (y < World.SEA_LEVEL + (1.0f / 16.0f * World.WORLD_HEIGHT))
                            block_type = (int)BlockInfo.BlockType.Dark_Grass;
                        // Grass
                        else
                            block_type = (int)BlockInfo.BlockType.Grass;
                    }
                    // Dirt
                    else if (y > height - World.random.Next(3, 6))
                        block_type = (int)BlockInfo.BlockType.Dirt;
                    // Stone & Dirt
                    else
                    {
                        if (World.random.Next(0, 100) <= 70)
                            block_type = (int)BlockInfo.BlockType.Stone;
                        else
                            block_type = (int)BlockInfo.BlockType.Dirt;
                    }

                    // Create block
                    Block b = new Block(block_type,
                        new Vector3(x, y, z));
                    blocks[x, y, z] = b;
                }
            }
        }

        //TODO: Surely this is obselete if within constructor
        has_loaded = true;
    }

}
