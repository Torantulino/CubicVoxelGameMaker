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
    public HashSet<Vector2Int> ActiveChunks = new HashSet<Vector2Int>();
    public ConcurrentDictionary<Vector2Int, GameObject> Chunk_GameObjects = new ConcurrentDictionary<Vector2Int, GameObject>();
    public ConcurrentDictionary<Vector2Int, GameObject> Ocean_Tiles = new ConcurrentDictionary<Vector2Int, GameObject>();

    // Loads and Unloads chunks as needed
    public void UpdateChunks()
    {
        // LAND
        {
            // Round down to get current chunk
            Vector2Int rounded_player_position
                = new Vector2Int(Mathf.FloorToInt(Camera.main.transform.position.x / World.CHUNK_SIZE),
                                  Mathf.FloorToInt(Camera.main.transform.position.z / World.CHUNK_SIZE));
            // Actual player position
            Vector2 actual_player_position
                = new Vector2((Camera.main.transform.position.x / World.CHUNK_SIZE),
                                (Camera.main.transform.position.z / World.CHUNK_SIZE));

            // Calculate chunk extremeties
            int min_x = rounded_player_position.x - World.RENDER_DISTANCE;
            int max_x = rounded_player_position.x + World.RENDER_DISTANCE;
            int min_z = rounded_player_position.y - World.RENDER_DISTANCE;
            int max_z = rounded_player_position.y + World.RENDER_DISTANCE;

            //TODO: REMOVE DEBUG METRICS
            // int unloaded_chunks = 0;
            // int loaded_chunks = 0;

            // Unload Chunks
            {
                List<Vector2Int> to_remove = new List<Vector2Int>();
                foreach (Vector2Int _chunk_pos in Chunks.Keys)
                {
                    if (Vector2.Distance(actual_player_position, _chunk_pos) > ((float)(World.RENDER_DISTANCE)) + 1.33f)
                    {
                        Chunks[_chunk_pos].unload = true;
                        Chunks[_chunk_pos].needs_updating = true;
                        to_remove.Add(_chunk_pos);

                        // unloaded_chunks++;
                    }
                }
                // Remove from collection
                foreach (Vector2Int _pos in to_remove)
                {
                    ActiveChunks.Remove(_pos);
                }
            }

            // Load Chunks
            for (int x = min_x; x <= max_x; x++)
            {
                for (int z = min_z; z <= max_z; z++)
                {
                    Vector2Int _chunk_pos = new Vector2Int(x, z);

                    // Disregard corner chunks outside true render distance
                    if (Vector2.Distance(actual_player_position, _chunk_pos) > ((float)World.RENDER_DISTANCE))
                        continue;

                    // Check Hashmap to ensure currently loading chunks aren't recalled
                    if (!ActiveChunks.Contains(_chunk_pos))
                    {
                        ActiveChunks.Add(_chunk_pos);
                        LoadChunk(_chunk_pos);

                        // loaded_chunks++;
                    }
                }
            }
        }
        // SEA
        {
            // Round down to get current chunk
            Vector2Int rounded_player_position
                = new Vector2Int(Mathf.FloorToInt(Camera.main.transform.position.x / World.SEA_TILE_SIZE),
                                  Mathf.FloorToInt(Camera.main.transform.position.z / World.SEA_TILE_SIZE));
            // Actual player position
            Vector2 actual_player_position
                = new Vector2((Camera.main.transform.position.x / World.SEA_TILE_SIZE),
                                (Camera.main.transform.position.z / World.SEA_TILE_SIZE));
                                
            // Calculate chunk extremeties
            int min_x = rounded_player_position.x - World.SEA_RENDER_DISTANCE;
            int max_x = rounded_player_position.x + World.SEA_RENDER_DISTANCE;
            int min_z = rounded_player_position.y - World.SEA_RENDER_DISTANCE;
            int max_z = rounded_player_position.y + World.SEA_RENDER_DISTANCE;

            //Unload Ocean Tiles
            {
                List<Vector2Int> to_remove = new List<Vector2Int>();
                foreach (Vector2Int _tile_pos in Ocean_Tiles.Keys)
                {
                    if (Vector2.Distance(actual_player_position, _tile_pos) > ((float)(World.SEA_RENDER_DISTANCE)))
                    {
                        UnityEngine.Object.Destroy(Ocean_Tiles[_tile_pos]);
                        to_remove.Add(_tile_pos);
                    }
                }
                // Remove from collection
                foreach (Vector2Int _pos in to_remove)
                {
                    GameObject to_delete;
                    Ocean_Tiles.TryRemove(_pos, out to_delete);
                }
            }

            // Load Ocean Tiles
            for (int x = min_x; x <= max_x; x++)
            {
                for (int z = min_z; z <= max_z; z++)
                {
                    Vector2Int _chunk_pos = new Vector2Int(x, z);

                    // Disregard already loaded chunks
                    if (Ocean_Tiles.ContainsKey(_chunk_pos))
                        continue;

                    // Create Tile
                    Ocean_Tiles[_chunk_pos] = GameObject.CreatePrimitive(PrimitiveType.Plane);
                    Ocean_Tiles[_chunk_pos].transform.localScale = new Vector3(World.SEA_TILE_SIZE / 10.0f, 1.0f, World.SEA_TILE_SIZE / 10.0f);
                    Ocean_Tiles[_chunk_pos].GetComponent<MeshRenderer>().material = TextureManager.Sea_Material;
                }

            }
        }

        // Debug.Log("Chunks Unloaded: " + unloaded_chunks + " | Chunks Loaded: " + loaded_chunks);
        // Debug.Log("TOTAL: " + (unloaded_chunks + loaded_chunks));
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