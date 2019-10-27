using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class World
{
    public static int CHUNK_SIZE = 16;
    public static int WORLD_HEIGHT = 64;
    public static int SEA_LEVEL = 24;
    public static float NOISE_SCALE = 27.6f;
    public static int RENDER_DISTANCE = 3;
    public static int SEA_RENDER_DISTANCE = 1;
    public static int SEA_TILE_SIZE = 612;
    public static System.Random random = new System.Random(); //Can take seed
    ChunkManager chunk_manager = new ChunkManager();

    public World()
    {
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
            Vector3 offset = new Vector3(
                Mathf.PerlinNoise(Time.timeSinceLevelLoad * 0.25f, 0.1f) / 2.0f, // Main Waves
                World.SEA_LEVEL - 0.05f - Mathf.PerlinNoise(Time.timeSinceLevelLoad * 0.25f, 0.2f) / 3.0f,
                Mathf.PerlinNoise(1.0f, Time.timeSinceLevelLoad * 0.25f) / 4.0f // Ripples
                );

            sea_tile.Value.transform.position = new Vector3(sea_tile.Key.x * SEA_TILE_SIZE, 0.0f, sea_tile.Key.y * SEA_TILE_SIZE) + offset;
        }
    }

    private void CullHiddenFaces(Chunk _chunk)
    {
        if (!_chunk.has_loaded || !_chunk.needs_updating)
            return;

        // Unload chunk
        if(_chunk.unload)
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

        // Check 
        for (int x = 0; x < CHUNK_SIZE; x++)
        {
            for (int y = 0; y < WORLD_HEIGHT; y++)
            {
                for (int z = 0; z < CHUNK_SIZE; z++)
                {
                    try
                    {
                        // If this block isn't at the edge of the chunk
                        // And neither this or the next block is air
                        // Don't render the face facing the next block
                        // -------------------------------------------
                        Block this_block = _chunk.blocks[x, y, z];

                        // X
                        if (x != CHUNK_SIZE - 1
                            && this_block.type != (int)BlockInfo.BlockType.Air
                            && _chunk.blocks[x + 1, y, z].type != (int)BlockInfo.BlockType.Air)
                        {
                            this_block.Faces[0].Render = false;
                        }
                        // (Chunk) 'Edge' Case
                        else if(x == CHUNK_SIZE - 1
                            && this_block.type != (int)BlockInfo.BlockType.Air)
                        {
                            // Get neighbouring chunk position                            
                            Vector2Int neighbour_chunk_pos = new Vector2Int(_chunk.Position.x + 1, _chunk.Position.y);

                            int neighbour_block_height = Noise.GetBlockHeight(0, z, neighbour_chunk_pos);

                            // If neighbour is underground, don't render
                            // TODO: Check player modified blocks here when implemented
                            if (y <= neighbour_block_height)
                            {
                                this_block.Faces[0].Render = false;
                            }
                        }
                        
                        // -X 
                        if (x != 0 
                            && this_block.type != (int)BlockInfo.BlockType.Air
                            && _chunk.blocks[x - 1, y, z].type != (int)BlockInfo.BlockType.Air)
                        {
                            this_block.Faces[1].Render = false;
                        }
                        // (Chunk) 'Edge' Case
                        else if(x == 0
                            && this_block.type != (int)BlockInfo.BlockType.Air)
                        {
                            // Get neighbouring chunk position                            
                            Vector2Int neighbour_chunk_pos = new Vector2Int(_chunk.Position.x - 1, _chunk.Position.y);

                            int neighbour_block_height = Noise.GetBlockHeight(CHUNK_SIZE - 1, z, neighbour_chunk_pos);

                            // If neighbour is underground, don't render
                            // TODO: Check player modified blocks here when implemented
                            if (y <= neighbour_block_height)
                            {
                                this_block.Faces[1].Render = false;
                            }
                        }

                        // Z
                        if (z != CHUNK_SIZE - 1 
                            && this_block.type != (int)BlockInfo.BlockType.Air
                            && _chunk.blocks[x, y, z + 1].type != (int)BlockInfo.BlockType.Air)
                        {
                            this_block.Faces[2].Render = false;
                        }
                        // (Chunk) 'Edge' Case
                        else if(z == CHUNK_SIZE - 1
                            && this_block.type != (int)BlockInfo.BlockType.Air)
                        {
                            // Get neighbouring chunk position                            
                            Vector2Int neighbour_chunk_pos = new Vector2Int(_chunk.Position.x, _chunk.Position.y + 1);

                            int neighbour_block_height = Noise.GetBlockHeight(x, 0, neighbour_chunk_pos);

                            // If neighbour is underground, don't render
                            // TODO: Check player modified blocks here when implemented
                            if (y <= neighbour_block_height)
                            {
                                this_block.Faces[2].Render = false;
                            }
                        }
                        
                        // -Z 
                        if (z != 0 
                            && this_block.type != (int)BlockInfo.BlockType.Air
                            && _chunk.blocks[x, y, z - 1].type != (int)BlockInfo.BlockType.Air)
                        {
                            this_block.Faces[3].Render = false;
                        }
                        // (Chunk) 'Edge' Case
                        else if(z == 0
                            && this_block.type != (int)BlockInfo.BlockType.Air)
                        {
                            // Get neighbouring chunk position                            
                            Vector2Int neighbour_chunk_pos = new Vector2Int(_chunk.Position.x, _chunk.Position.y - 1);

                            int neighbour_block_height = Noise.GetBlockHeight(x, CHUNK_SIZE - 1, neighbour_chunk_pos);

                            // If neighbour is underground, don't render
                            // TODO: Check player modified blocks here when implemented
                            if (y <= neighbour_block_height)
                            {
                                this_block.Faces[3].Render = false;
                            }
                        }
                        
                        // Y
                        if (y != WORLD_HEIGHT - 1
                            && this_block.type != (int)BlockInfo.BlockType.Air
                            && _chunk.blocks[x, y + 1, z].type != (int)BlockInfo.BlockType.Air)
                        {
                            this_block.Faces[4].Render = false;
                        }

                        // -Y 
                        //TODO: Realistically, only player edits need to be checked here, unless natural caves/tunnels/overhangs are implemented.
                        if (this_block.type != (int)BlockInfo.BlockType.Air)
                        {
                            this_block.Faces[5].Render = false;
                        }                       
                    }
                    catch (System.IndexOutOfRangeException)
                    {
                        Debug.LogError("Out of Range at: (" + x + ", " + y + ", " + z + ")");
                        throw;
                    }
                }
            }
        }

        CombineMeshes(_chunk);
    }

    private void CombineMeshes(Chunk _chunk)
    {
        // Obtain references
        LevelManager level_manager = GameObject.FindObjectOfType<LevelManager>();

        //Count faces
        int no_faces = 0;
        foreach (Block block in _chunk.blocks)
        {
            // Disregard Air
            if(block.type == (int)BlockInfo.BlockType.Air)
                continue;

            for (int i = 0; i < block.Faces.Length; i++)
            {
                if(!block.Faces[i].Render)
                    continue;
                no_faces ++;
            }
        }

        // Iterate over every mesh in chunk and add to collection
        CombineInstance[] combine_meshes = new CombineInstance[no_faces];
        int current_mesh_iterator = 0;
        foreach (Block block in _chunk.blocks)
        {
            // Disregard Air
            if(block.type == (int)BlockInfo.BlockType.Air)
                continue;

            for (int i = 0; i < block.Faces.Length; i++)
            {
                if(!block.Faces[i].Render)
                    continue;

                combine_meshes[current_mesh_iterator].mesh = new Mesh();
                combine_meshes[current_mesh_iterator].mesh.vertices = block.Faces[i].Vertices;
                combine_meshes[current_mesh_iterator].mesh.triangles = block.Faces[i].Triangles;
                combine_meshes[current_mesh_iterator].mesh.normals = block.Faces[i].Normals;
                combine_meshes[current_mesh_iterator].mesh.SetUVs(0, block.Faces[i].UVs);

                //Matrix4x4.Translate(block.Position)
                combine_meshes[current_mesh_iterator].transform = Matrix4x4.Translate(block.Position);//.localToWorldMatrix;
                current_mesh_iterator++;
            }
        }

        GameObject chunk_object;
        MeshRenderer mesh_renderer;
        MeshFilter mesh_filter;

        // If Chunk is loaded, update
        if(chunk_manager.Chunk_GameObjects.ContainsKey(_chunk.Position))
        {
            // Get Gameobject
            chunk_object = chunk_manager.Chunk_GameObjects[_chunk.Position];
            // Get Components
            mesh_renderer = chunk_manager.Chunk_GameObjects[_chunk.Position].GetComponent<MeshRenderer>();
            mesh_filter = chunk_manager.Chunk_GameObjects[_chunk.Position].GetComponent<MeshFilter>();

            // Delete old mesh
            UnityEngine.Object.Destroy(mesh_filter.mesh);
        }
        // Else create
        else
        {
            // Create Gameobject
            chunk_object = new GameObject("Chunk");
            // Add Components
            mesh_renderer = chunk_object.AddComponent<MeshRenderer>();
            mesh_filter = chunk_object.AddComponent<MeshFilter>();
        }

        // Position
        chunk_object.transform.position = new Vector3(_chunk.Position.x * World.CHUNK_SIZE, 0.0f, _chunk.Position.y * World.CHUNK_SIZE);

        // Combine meshes into single mesh
        mesh_filter.mesh = new Mesh();
        mesh_filter.mesh.CombineMeshes(combine_meshes, true);

        // Delete old meshes
        for(int i=0; i< no_faces; i++)
        {
            UnityEngine.Object.Destroy(combine_meshes[i].mesh);
        }
        
        // Set material
        mesh_renderer.material = TextureManager.Block_Material;

        // Add GameObject to collection
        chunk_manager.Chunk_GameObjects[_chunk.Position] = chunk_object;

        // Set flag to show chunk is up-to-date
        _chunk.needs_updating = false;
    }
}
