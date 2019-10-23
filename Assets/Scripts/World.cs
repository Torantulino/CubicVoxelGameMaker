using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class World
{
    public static int CHUNK_SIZE = 16;
    public static int WORLD_HEIGHT = 64;

    ChunkManager chunk_manager = new ChunkManager();

    public void CreateChunk(int _type, Vector2Int _chunk_pos)
    {
        chunk_manager.CreateChunk(_type, _chunk_pos);
    }

    public void UpdateWorld()
    {
        foreach (KeyValuePair<Vector2Int, Chunk> _pair in chunk_manager.Chunks)
        {
            Chunk _chunk = _pair.Value;
            CullHiddenFaces(_chunk);
        }
    }

    private void CullHiddenFaces(Chunk _chunk)
    {
        if (!_chunk.hasLoaded)
            return;

        for (int x = 0; x < CHUNK_SIZE; x++)
        {
            for (int y = 0; y < WORLD_HEIGHT; y++)
            {
                for (int z = 0; z < CHUNK_SIZE; z++)
                {
                    try
                    {
                        // X
                        if (x + 1 != CHUNK_SIZE && _chunk.blocks[x + 1, y, z].Game_Object != null)
                            _chunk.blocks[x + 1, y, z].Faces[1].Game_Object.SetActive(false);
                        // -X 
                        if (x != 0 && _chunk.blocks[x - 1, y, z].Game_Object != null)
                            _chunk.blocks[x - 1, y, z].Faces[0].Game_Object.SetActive(false);
                        // Z
                        if (z + 1 != CHUNK_SIZE && _chunk.blocks[x, y, z + 1].Game_Object != null)
                            _chunk.blocks[x, y, z + 1].Faces[3].Game_Object.SetActive(false);
                        // -Z 
                        if (z != 0 && _chunk.blocks[x, y, z - 1].Game_Object != null)
                            _chunk.blocks[x, y, z - 1].Faces[2].Game_Object.SetActive(false);
                        // Y
                        if (y + 1 != WORLD_HEIGHT && _chunk.blocks[x, y + 1, z].Game_Object != null)
                            _chunk.blocks[x, y + 1, z].Faces[5].Game_Object.SetActive(false);
                        // -Y 
                        if (y != 0 && _chunk.blocks[x, y - 1, z].Game_Object != null)
                            _chunk.blocks[x, y - 1, z].Faces[4].Game_Object.SetActive(false);
                    }
                    catch (System.IndexOutOfRangeException)
                    {
                        Debug.LogError("Out of Range at: (" + x + ", " + y + ", " + z + ")");
                        throw;
                    }

                    if (x == 0)
                        _chunk.blocks[x + 1, y, z].Faces[1].Game_Object.transform.localScale = Vector3.one * 10.0f;

                }
            }
        }
    }
}
