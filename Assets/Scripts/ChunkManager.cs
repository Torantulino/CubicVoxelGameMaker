using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using System.Threading;
using System.Collections.Concurrent;

public class ChunkManager
{
    public readonly ConcurrentDictionary<Vector2Int, Chunk> Chunks = new ConcurrentDictionary<Vector2Int, Chunk>();
    public HashSet<Vector2Int> LoadedChunks = new HashSet<Vector2Int>();
    
    // Loads and Unloads chunks as needed
    public void UpdateChunks()
    {
        // Casting to Int in C# rounds towards 0!
        Vector2Int _player_position 
            = new Vector2Int( Mathf.FloorToInt(Camera.main.transform.position.x / World.CHUNK_SIZE), 
                              Mathf.FloorToInt(Camera.main.transform.position.z / World.CHUNK_SIZE) );

        // Calculate chunk extremeties
        int min_x = _player_position.x - World.RENDER_DISTANCE;
        int max_x = _player_position.x + World.RENDER_DISTANCE;
        int min_z = _player_position.y - World.RENDER_DISTANCE;
        int max_z = _player_position.y + World.RENDER_DISTANCE;

        //Debug.Log("Player PosX: " + _player_position.x);
        //Debug.Log("Min_X: " + min_x);

        // Unload Chunks


        // Load Chunks
        for (int x = min_x; x <= max_x; x++)
        {
            for (int z = min_z; z <= max_z; z++)
            {
                Vector2Int _chunk = new Vector2Int(x, z);

                // Check Hashmap to ensure currently loading chunks aren't recalled
                if(!LoadedChunks.Contains(_chunk))
                {
                    LoadedChunks.Add(_chunk);
                    LoadChunk(_chunk);
                }
            }
        }
    }

    private void LoadChunk(Vector2Int _chunk_pos)
    {
        if(Chunks.ContainsKey(_chunk_pos))
        {
            //Debug.LogError("Requested Chunk " + _chunk_pos + " Already loaded.");
            return;
        }
        else
        {
            Thread thread_generate_chunk = new Thread(() => GenerateChunk(_chunk_pos));
            thread_generate_chunk.Start();
        }
    }

    private void GenerateChunk(Vector2Int chunk_pos)
    {
        Chunk _chunk = new Chunk(chunk_pos);

        Chunks[new Vector2Int(chunk_pos.x, chunk_pos.y)] = _chunk; 
    }
}