using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class ChunkManager
{

    public readonly Dictionary<Vector2Int, Chunk> Chunks = new Dictionary<Vector2Int, Chunk>();

    public void CreateChunk(Vector2Int _chunk_pos)
    {
        LoadChunk loadChunk = new LoadChunk();
        loadChunk.chunk_pos = _chunk_pos;
        loadChunk.chunk_manager = this; 
        
        loadChunk.Execute(); //TODO: Not starting job
    }
}

public struct LoadChunk : IJob
{
    public Vector2Int chunk_pos;
    public ChunkManager chunk_manager;

    public void Execute()
    {
        Chunk _chunk = new Chunk(chunk_pos);

        chunk_manager.Chunks[new Vector2Int(chunk_pos.x, chunk_pos.y)] = _chunk; 
    }
}
