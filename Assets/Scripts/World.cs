using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class World
{
    public static int CHUNK_SIZE = 16;
    public static int WORLD_HEIGHT = 64;
    public static int SEA_LEVEL = 32;
    public static float NOISE_SCALE = 0.033f;
    public static System.Random random = new System.Random(); //Can take seed


    ChunkManager chunk_manager = new ChunkManager();

    public void CreateChunk(Vector2Int _chunk_pos)
    {
        chunk_manager.CreateChunk(_chunk_pos);
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
        if (!_chunk.has_loaded || !_chunk.needs_updating)
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
                        if (x + 1 != CHUNK_SIZE && _chunk.blocks[x + 1, y, z].type != (int)BlockInfo.BlockType.Air
                            && _chunk.blocks[x, y, z].type != (int)BlockInfo.BlockType.Air)
                        {
                            _chunk.blocks[x + 1, y, z].Faces[1].Render = false;
                        }
                        // -X 
                        if (x != 0 && _chunk.blocks[x - 1, y, z].type != (int)BlockInfo.BlockType.Air
                            && _chunk.blocks[x, y, z].type != (int)BlockInfo.BlockType.Air)
                        {
                            _chunk.blocks[x - 1, y, z].Faces[0].Render = false;
                        }
                        // Z
                        if (z + 1 != CHUNK_SIZE && _chunk.blocks[x, y, z + 1].type != (int)BlockInfo.BlockType.Air
                            && _chunk.blocks[x, y, z].type != (int)BlockInfo.BlockType.Air)
                        {
                            _chunk.blocks[x, y, z + 1].Faces[3].Render = false;
                        }
                        // -Z 
                        if (z != 0 && _chunk.blocks[x, y, z - 1].type != (int)BlockInfo.BlockType.Air
                            && _chunk.blocks[x, y, z].type != (int)BlockInfo.BlockType.Air)
                        {
                            _chunk.blocks[x, y, z - 1].Faces[2].Render = false;
                        }
                        // Y
                        if (y + 1 != WORLD_HEIGHT && _chunk.blocks[x, y + 1, z].type != (int)BlockInfo.BlockType.Air
                            && _chunk.blocks[x, y, z].type != (int)BlockInfo.BlockType.Air)
                        {
                            // Bottom
                            _chunk.blocks[x, y + 1, z].Faces[5].Render = false;
                        }
                        // -Y 
                        if (y != 0 && _chunk.blocks[x, y - 1, z].type != (int)BlockInfo.BlockType.Air
                            && _chunk.blocks[x, y, z].type != (int)BlockInfo.BlockType.Air)
                        {
                            // Top
                            _chunk.blocks[x, y - 1, z].Faces[4].Render = false;
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

        CombineInstance[] combine_meshes = new CombineInstance[no_faces];

        int current_mesh = 0;
        foreach (Block block in _chunk.blocks)
        {
            // Disregard Air
            if(block.type == (int)BlockInfo.BlockType.Air)
                continue;

            for (int i = 0; i < block.Faces.Length; i++)
            {
                if(!block.Faces[i].Render)
                    continue;

                combine_meshes[current_mesh].mesh = new Mesh();
                combine_meshes[current_mesh].mesh.vertices = block.Faces[i].Vertices;
                combine_meshes[current_mesh].mesh.triangles = block.Faces[i].Triangles;
                combine_meshes[current_mesh].mesh.normals = block.Faces[i].Normals;
                combine_meshes[current_mesh].mesh.SetUVs(0, block.Faces[i].UVs);

                //Matrix4x4.Translate(block.Position)
                combine_meshes[current_mesh].transform = Matrix4x4.Translate(block.Position);//.localToWorldMatrix;
                current_mesh++;
            }
        }

        GameObject chunk_object = new GameObject("Chunk");
        chunk_object.transform.position = new Vector3(_chunk.Position.x * World.CHUNK_SIZE, 0.0f, _chunk.Position.y * World.CHUNK_SIZE);

        MeshRenderer mesh_renderer = chunk_object.AddComponent<MeshRenderer>();
        MeshFilter mesh_filter = chunk_object.AddComponent<MeshFilter>();

        mesh_filter.mesh = new Mesh();
        mesh_filter.mesh.CombineMeshes(combine_meshes, true);
        
        // Set material
        mesh_renderer.material = TextureManager.Block_Material;

        _chunk.needs_updating = false;
    }
}
