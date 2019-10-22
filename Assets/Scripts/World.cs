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

    public void UpdateWorld()
    {
        foreach (KeyValuePair<Vector2Int, Chunk> _pair in Chunks)
        {
            Chunk _chunk = _pair.Value;
            CullHiddenFaces(_chunk);
        }
    }

    private void CullHiddenFaces(Chunk _chunk)
    {
        if(!_chunk.hasLoaded)
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
                        if (y  + 1 != WORLD_HEIGHT && _chunk.blocks[x, y + 1, z].Game_Object != null)
                            _chunk.blocks[x, y + 1, z].Faces[5].Game_Object.SetActive(false);
                        // -Y 
                        if (y != 0 && _chunk.blocks[x, y - 1, z].Game_Object != null)
                            _chunk.blocks[x, y - 1, z].Faces[4].Game_Object.SetActive(false);
                    }
                    catch (System.IndexOutOfRangeException)
                    {
                        Debug.LogError("Out of Range at: (" + x +", " + y + ", " + z + ")");
                        throw;
                    }

                    if(x==0)
                    _chunk.blocks[x + 1, y, z].Faces[1].Game_Object.transform.localScale = Vector3.one * 10.0f;

                }
            }
        }
    }
}
