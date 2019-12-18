using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;


public class World
{
    public static int CHUNK_SIZE = 16;
    public static int WORLD_HEIGHT = 64;
    public static int SEA_LEVEL = 16;
    public static float NOISE_SCALE = 27.6f;
    public static int RENDER_DISTANCE = 2;
    public static int SEA_RENDER_DISTANCE = 1;
    public static int SEA_TILE_SIZE = 612;
    public static bool ISLANDS = true;

    public static System.Random random = new System.Random(1); //TODO: Add seed to save data when implementing multiple world saving
    ChunkManager chunk_manager;
    LevelManager level_manager;

    public World(LevelManager _level_manager)
    {
        level_manager = _level_manager;
        chunk_manager = level_manager.Chunk_Manager;
    }
    public void UpdateWorld()
    {
        foreach (KeyValuePair<Vector2Int, Chunk> _pair in chunk_manager.Chunks)
        {
            Chunk _chunk = _pair.Value;
            CullHiddenFaces(_chunk);
        }

        chunk_manager.UpdateChunks();

        // Simulate Sea
        foreach (KeyValuePair<Vector2Int, GameObject> sea_tile in chunk_manager.Ocean_Tiles)
        {
            Vector3 offset = Noise.GetSeaOffset();

            sea_tile.Value.transform.position = new Vector3(sea_tile.Key.x * SEA_TILE_SIZE, World.SEA_LEVEL, sea_tile.Key.y * SEA_TILE_SIZE) + offset;
        }
    }

    private void CullHiddenFaces(Chunk _chunk)
    {
        ProfilerMarker CullHiddenFacesMarker = new ProfilerMarker("CullHiddenFaces()");

        if (!_chunk.has_loaded || !_chunk.needs_updating)
            return;

        // Unload chunk
        if (_chunk.unload)
        {
            // Destroy Game Object (If unloading before creation is complete return until loaded)
            if (!chunk_manager.Chunk_GameObjects.ContainsKey(_chunk.Position))
                return;
            UnityEngine.Object.Destroy(chunk_manager.Chunk_GameObjects[_chunk.Position]);

            // Remove from collections
            {
                Chunk to_delete;
                chunk_manager.Chunks.TryRemove(_chunk.Position, out to_delete);
            }
            {
                GameObject to_delete;
                chunk_manager.Chunk_GameObjects.TryRemove(_chunk.Position, out to_delete);
            }
            return;
        }

        CullHiddenFacesMarker.Begin();

        // Apply user modifications (This has to be done before the main loop due to adjacent blocks)
        for (int x = 0; x < CHUNK_SIZE; x++)
        {
            for (int y = 0; y < WORLD_HEIGHT; y++)
            {
                for (int z = 0; z < CHUNK_SIZE; z++)
                {
                    // Apply user modifications to normal chunk
                    if (chunk_manager.Modified_Chunks.ContainsKey(_chunk.Position) && chunk_manager.Modified_Chunks[_chunk.Position].blocks[x, y, z] != null)
                        _chunk.blocks[x, y, z] = chunk_manager.Modified_Chunks[_chunk.Position].blocks[x, y, z];
                }
            }
        }

        // Iterate over every block in chunk
        for (int x = 0; x < CHUNK_SIZE; x++)
        {
            for (int y = 0; y < WORLD_HEIGHT; y++)
            {
                for (int z = 0; z < CHUNK_SIZE; z++)
                {
                    // Reset 'Render' flags
                    if (_chunk.blocks[x, y, z].type != (int)BlockInfo.BlockType.Air)
                    {
                        foreach (BlockFace face in _chunk.blocks[x, y, z].Faces)
                            face.Render = true;
                    }

                    try
                    {
                        // If this block isn't at the edge of the chunk
                        // And neither this or the next block is air
                        // Don't render the face facing the next block
                        // TODO: Can face normals be implemented here to shrink code?
                        // -------------------------------------------
                        Block this_block = _chunk.blocks[x, y, z];

                        // Positive X Face
                        if (x != CHUNK_SIZE - 1 // Not an edge face
                            && this_block.type != (int)BlockInfo.BlockType.Air  // Block not air
                            && _chunk.blocks[x + 1, y, z].type != (int)BlockInfo.BlockType.Air) // Not facing air
                        {
                            this_block.Faces[0].Render = false;
                        }
                        // (Chunk) 'Edge' Case
                        // -1 used here as index starts at 0
                        else if (x == CHUNK_SIZE - 1    // Face is on edge of chunk
                            && this_block.type != (int)BlockInfo.BlockType.Air) // Block not air
                        {
                            // Get neighbouring chunk position                            
                            Vector2Int neighbour_chunk_pos = new Vector2Int(_chunk.Position.x + 1, _chunk.Position.y);
                            int neighbour_block_height = Noise.GetBlockHeight(0, z, neighbour_chunk_pos);

                            // Check user modifications
                            if (chunk_manager.Modified_Chunks.ContainsKey(neighbour_chunk_pos))
                            {
                                // If the neighboring block has been modified and isn't air, don't render face
                                if (chunk_manager.Modified_Chunks[neighbour_chunk_pos].blocks[0, y, z] != null &&
                                chunk_manager.Modified_Chunks[neighbour_chunk_pos].blocks[0, y, z].type != (int)BlockInfo.BlockType.Air)
                                {
                                    this_block.Faces[0].Render = false;
                                }
                                // Else if it hasn't been modified and is underground, don't render face
                                else if (chunk_manager.Modified_Chunks[neighbour_chunk_pos].blocks[0, y, z] == null &&
                                y <= neighbour_block_height)
                                {
                                    this_block.Faces[0].Render = false;
                                }
                            }
                            // Else if no modifications and underground, don't render face
                            else if (y <= neighbour_block_height)
                            {
                                this_block.Faces[0].Render = false;
                            }
                        }

                        // Negavtive X Face 
                        if (x != 0  // Not an edge face
                            && this_block.type != (int)BlockInfo.BlockType.Air  // Block not air
                            && _chunk.blocks[x - 1, y, z].type != (int)BlockInfo.BlockType.Air) // Not facing air
                        {
                            this_block.Faces[1].Render = false;
                        }
                        // (Chunk) 'Edge' Case
                        else if (x == 0 // Face is on edge of chunk
                            && this_block.type != (int)BlockInfo.BlockType.Air) // Block not air
                        {
                            // Get neighbouring chunk position                            
                            Vector2Int neighbour_chunk_pos = new Vector2Int(_chunk.Position.x - 1, _chunk.Position.y);
                            int neighbour_block_height = Noise.GetBlockHeight(CHUNK_SIZE - 1, z, neighbour_chunk_pos);

                            // Check user modifications
                            if (chunk_manager.Modified_Chunks.ContainsKey(neighbour_chunk_pos))
                            {
                                // If the neighboring block has been modified and isn't air, don't render face
                                if (chunk_manager.Modified_Chunks[neighbour_chunk_pos].blocks[World.CHUNK_SIZE - 1, y, z] != null 
                                    && chunk_manager.Modified_Chunks[neighbour_chunk_pos].blocks[World.CHUNK_SIZE - 1, y, z].type != (int)BlockInfo.BlockType.Air)
                                {
                                    this_block.Faces[1].Render = false;
                                }
                                // Else if it hasn't been modified and is underground, don't render face
                                else if (chunk_manager.Modified_Chunks[neighbour_chunk_pos].blocks[World.CHUNK_SIZE - 1, y, z] == null
                                    && y <= neighbour_block_height)
                                {
                                    this_block.Faces[1].Render = false;
                                }
                            }
                            // Else if no modifications and underground, don't render face
                            else if (y <= neighbour_block_height)
                            {
                                this_block.Faces[1].Render = false;
                            }
                        }

                        // Z
                        if (z != CHUNK_SIZE - 1 // Not an edge face
                            && this_block.type != (int)BlockInfo.BlockType.Air  // Block not air
                            && _chunk.blocks[x, y, z + 1].type != (int)BlockInfo.BlockType.Air) // Not facing air
                        {
                            this_block.Faces[2].Render = false;
                        }
                        // (Chunk) 'Edge' Case
                        else if (z == CHUNK_SIZE - 1    // Face is on edge of chunk
                            && this_block.type != (int)BlockInfo.BlockType.Air) // Block not air
                        {
                            // Get neighbouring chunk position                            
                            Vector2Int neighbour_chunk_pos = new Vector2Int(_chunk.Position.x, _chunk.Position.y + 1);
                            int neighbour_block_height = Noise.GetBlockHeight(x, 0, neighbour_chunk_pos);

                            // Check user modifications
                            if (chunk_manager.Modified_Chunks.ContainsKey(neighbour_chunk_pos))
                            {
                                // If the neighboring block has been modified and isn't air, don't render face
                                if (chunk_manager.Modified_Chunks[neighbour_chunk_pos].blocks[x, y, 0] != null 
                                    && chunk_manager.Modified_Chunks[neighbour_chunk_pos].blocks[x, y, 0].type != (int)BlockInfo.BlockType.Air)
                                {
                                    this_block.Faces[2].Render = false;
                                }
                                // Else if it hasn't been modified and is underground, don't render face
                                else if (chunk_manager.Modified_Chunks[neighbour_chunk_pos].blocks[x, y, 0] == null
                                    && y <= neighbour_block_height)
                                {
                                    this_block.Faces[2].Render = false;
                                }
                            }
                            // Else if no modifications and underground, don't render face
                            else if (y <= neighbour_block_height)
                            {
                                this_block.Faces[2].Render = false;
                            }
                        }

                        // -Z 
                        if (z != 0  // Not an edge face
                            && this_block.type != (int)BlockInfo.BlockType.Air  // Block not air
                            && _chunk.blocks[x, y, z - 1].type != (int)BlockInfo.BlockType.Air) // Not facing air
                        {
                            this_block.Faces[3].Render = false;
                        }
                        // (Chunk) 'Edge' Case
                        else if (z == 0 // Face is on edge of chunk
                            && this_block.type != (int)BlockInfo.BlockType.Air) // Block not air
                        {
                            // Get neighbouring chunk position                            
                            Vector2Int neighbour_chunk_pos = new Vector2Int(_chunk.Position.x, _chunk.Position.y - 1);
                            int neighbour_block_height = Noise.GetBlockHeight(x, CHUNK_SIZE - 1, neighbour_chunk_pos);

                            // Check user modifications
                            if (chunk_manager.Modified_Chunks.ContainsKey(neighbour_chunk_pos))
                            {
                                // If the neighboring block has been modified and isn't air, don't render face
                                if (chunk_manager.Modified_Chunks[neighbour_chunk_pos].blocks[x, y, World.CHUNK_SIZE - 1] != null 
                                    && chunk_manager.Modified_Chunks[neighbour_chunk_pos].blocks[x, y, World.CHUNK_SIZE - 1].type != (int)BlockInfo.BlockType.Air)
                                {
                                    this_block.Faces[3].Render = false;
                                }
                                // Else if it hasn't been modified and is underground, don't render face
                                else if (chunk_manager.Modified_Chunks[neighbour_chunk_pos].blocks[x, y, World.CHUNK_SIZE - 1] == null
                                    && y <= neighbour_block_height)
                                {
                                    this_block.Faces[3].Render = false;
                                }
                            }
                            // Else if no modifications and underground, don't render face
                            else if (y <= neighbour_block_height)
                            {
                                this_block.Faces[3].Render = false;
                            }
                        }

                        // Y
                        if (y != WORLD_HEIGHT - 1   // Not at height limit
                            && this_block.type != (int)BlockInfo.BlockType.Air  // Block not air
                            && _chunk.blocks[x, y + 1, z].type != (int)BlockInfo.BlockType.Air) // Not facing air
                        {
                            this_block.Faces[4].Render = false;
                        }

                        // -Y 
                        if (y != 0  // Not bottom block
                            && this_block.type != (int)BlockInfo.BlockType.Air // Block not air
                            && _chunk.blocks[x, y - 1, z].type != (int)BlockInfo.BlockType.Air) // Not facing air
                        {
                            this_block.Faces[5].Render = false;
                        }
                        //Bottom block
                        else if (y == 0)
                            this_block.Faces[5].Render = false;
                    }
                    catch (System.IndexOutOfRangeException e)
                    {
                        Debug.LogError("Out of Range at: (" + x + ", " + y + ", " + z + ")");
                        Debug.LogError(e.StackTrace);
                        throw;
                    }
                }
            }
        }

        CullHiddenFacesMarker.End();
        CombineMeshes(_chunk);
    }

    private void CombineMeshes(Chunk _chunk)
    {
        ProfilerMarker CombineMeshesMarker = new ProfilerMarker("CombineMeshes()");
        CombineMeshesMarker.Begin();

        // Obtain references
        LevelManager level_manager = GameObject.FindObjectOfType<LevelManager>();

        // Iterate over every mesh in chunk, 
        // manually building up a single combined mesh
        Mesh combined_mesh = new Mesh();
        {
            List<Vector3> combined_vertices = new List<Vector3>();
            List<int> combined_triangles = new List<int>();
            List<Vector3> combined_normals = new List<Vector3>();
            List<Vector3> combined_uvs = new List<Vector3>();
            int current_mesh_iterator = 0;
            foreach (Block block in _chunk.blocks)
            {
                // Disregard Air
                if (block.type == (int)BlockInfo.BlockType.Air)
                    continue;

                for (int i = 0; i < block.Faces.Length; i++)
                {
                    if (!block.Faces[i].Render)
                        continue;

                    // Add transformed vertex data
                    foreach (Vector3 vert in block.Faces[i].Vertices)
                        combined_vertices.Add(vert + block.Position);

                    // Reindex triangles
                    foreach (int tri in block.Faces[i].Triangles)
                        combined_triangles.Add(tri + (4 * current_mesh_iterator));

                    // combine normals and add UVs to combined lists
                    combined_normals.AddRange(block.Faces[i].Normals);
                    combined_uvs.AddRange(block.Faces[i].UVs);

                    current_mesh_iterator++;
                }
            }

            // Apply combined data to mesh
            combined_mesh.vertices = combined_vertices.ToArray();
            combined_mesh.triangles = combined_triangles.ToArray();
            combined_mesh.normals = combined_normals.ToArray();
            combined_mesh.SetUVs(0, combined_uvs);
        }

        GameObject chunk_object;
        MeshRenderer mesh_renderer;
        MeshFilter mesh_filter;
        MeshCollider chunk_collider;

        // If Chunk GameObject is already instantiated: Update
        if (chunk_manager.Chunk_GameObjects.ContainsKey(_chunk.Position))
        {
            // Get Gameobject
            chunk_object = chunk_manager.Chunk_GameObjects[_chunk.Position];
            // Get Components
            mesh_renderer = chunk_manager.Chunk_GameObjects[_chunk.Position].GetComponent<MeshRenderer>();
            mesh_filter = chunk_manager.Chunk_GameObjects[_chunk.Position].GetComponent<MeshFilter>();
            chunk_collider = chunk_manager.Chunk_GameObjects[_chunk.Position].GetComponent<MeshCollider>();

            // Delete old mesh
            UnityEngine.Object.Destroy(mesh_filter.mesh);
        }
        // Else: Instantiate new GameObject
        else
        {
            // Create Gameobject
            chunk_object = new GameObject("Chunk");
            // Add Components
            mesh_renderer = chunk_object.AddComponent<MeshRenderer>();
            mesh_filter = chunk_object.AddComponent<MeshFilter>();
            chunk_collider = chunk_object.AddComponent<MeshCollider>();

            // Set one-time component properties
            mesh_renderer.receiveShadows = false;
            mesh_renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            chunk_collider.cookingOptions = MeshColliderCookingOptions.None;

            // Assign object to 'Blocks' layer
            chunk_object.layer = LayerMask.NameToLayer("Blocks");
        }

        // Position chunk
        chunk_object.transform.position = new Vector3(_chunk.Position.x * World.CHUNK_SIZE, 0.0f, _chunk.Position.y * World.CHUNK_SIZE);

        // Combine meshes into single mesh
        mesh_filter.mesh = combined_mesh;

        // Set material
        mesh_renderer.material = TextureManager.Block_Material;

        // Set collider data
        chunk_collider.sharedMesh = mesh_filter.mesh;

        // Add GameObject to collection
        chunk_manager.Chunk_GameObjects[_chunk.Position] = chunk_object;

        // Set flag to show chunk is up-to-date
        _chunk.needs_updating = false;

        CombineMeshesMarker.End();
    }
}
