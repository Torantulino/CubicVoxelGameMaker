using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class World
{
    public static int CHUNK_SIZE = 16;
    public static int WORLD_HEIGHT = 64;
    TextureManager texture_manager = new TextureManager();

    
    Dictionary<Vector2Int, Chunk> Chunks = new Dictionary<Vector2Int, Chunk>();
    
    public void CreateBlock(int _type, Tuple<int, int, int> _chunk_pos, Tuple<int, int, int> _block_pos)
    {
        
    }
    public void CreateChunk(int _type, Vector2Int _chunk_pos, MonoBehaviour mono)
    {
        mono.StartCoroutine(LoadChunk(_type, _chunk_pos));
    }

    private IEnumerator LoadChunk(int _type, Vector2Int _chunk_pos)
    {
        Chunk _chunk = new Chunk(texture_manager);

        Chunks[new Vector2Int(_chunk_pos.x, _chunk_pos.y)] = _chunk; 

        yield return new WaitForSeconds(0);
    }

    {
        Chunk _chunk = new Chunk();

        Chunks[_chunk_pos.Item1,_chunk_pos.Item2, _chunk_pos.Item3] = _chunk; 
    }
}
