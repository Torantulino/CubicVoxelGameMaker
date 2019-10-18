using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class World : MonoBehaviour
{
    public static int CHUNK_SIZE = 16;

    Chunk[,,] Chunks = new Chunk[,,] { };
    
    public void CreateBlock(int _type, Tuple<int, int, int> _chunk_pos, Tuple<int, int, int> _block_pos)
    {
        
    }
    public void CreateChunk(int _type, Tuple<int, int, int> _chunk_pos)
    {
        Chunk _chunk = new Chunk();

        Chunks[_chunk_pos.Item1,_chunk_pos.Item2, _chunk_pos.Item3] = _chunk; 
    }
}
