using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockFace
{
    private int size = 10;
    public GameObject gameObject;

    public BlockFace(Material _material, int _type)
    {
        // Create object
        gameObject = new GameObject("face");

        // Create Components
        MeshRenderer mesh_renderer = gameObject.AddComponent<MeshRenderer>();
        MeshFilter mesh_filter = gameObject.AddComponent<MeshFilter>();
        mesh_renderer.material = _material;

        
        Mesh mesh = new Mesh();
        mesh_filter.mesh = mesh;

        // Set vertices
        var vertices = new Vector3[4]
        {
            new Vector3(0.0f, 0.0f, 0.0f),
            new Vector3(size, 0.0f, 0.0f),
            new Vector3(0.0f, size, 0.0f),
            new Vector3(size, size, 0.0f)
        };
        mesh.vertices = vertices;

        // Construct triangles
        var tris = new int[6]
        {
            // lower left triangle
            0, 2, 1,
            // upper right triangle
            2, 3, 1
        };
        mesh.triangles = tris;

        // Todo: pass in as argument
        var normals = new Vector3[4]
        {
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward
        };
        mesh.normals = normals;

        // Set UVs, with third coordinate indicating index within texture atlas
        var uv = new List<Vector3>
        {
            new Vector3(0.0f, 0.0f, _type),
            new Vector3(1.0f, 0.0f, _type),
            new Vector3(0.0f, 1.0f, _type),
            new Vector3(1.0f, 1.0f, _type)
        };
        mesh.SetUVs(0, uv);
    }
}
