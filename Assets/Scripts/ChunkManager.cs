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

    public void CreateChunk(Vector2Int _chunk_pos)
    {
        Thread thread_load_chunk = new Thread(() => LoadChunk(_chunk_pos));
        thread_load_chunk.Start();
    }

    public void LoadChunk(Vector2Int chunk_pos)
    {
        Chunk _chunk = new Chunk(chunk_pos);

        Chunks[new Vector2Int(chunk_pos.x, chunk_pos.y)] = _chunk; 
    }
}