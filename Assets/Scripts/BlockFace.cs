using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockFace
{
    private int size = 1;
    public GameObject Game_Object;

    public BlockFace(Material _material, int _type, Vector3 _normal, Mesh _mesh)
    {
        // Create object
        Game_Object = new GameObject("face");
        _normal *= -1.0f;

        // Position
        {
            if (_normal == Vector3.up)
            {
                Game_Object.transform.position += new Vector3(-0.5f, 0.0f, 0.5f);

                // Rotate
                Game_Object.transform.rotation = Quaternion.LookRotation(_normal, Vector3.back);

            }
            else if (_normal == Vector3.down)
            {
                Game_Object.transform.position = -_normal;
                Game_Object.transform.position += new Vector3(-0.5f, 0.0f, -0.5f);

                // Rotate
                Game_Object.transform.rotation = Quaternion.LookRotation(_normal, Vector3.forward);
            }
            else
            {
                Vector3 _right = (Vector3.Cross(_normal, Vector3.up));
                Game_Object.transform.position = -_normal / 2.0f + _right / 2.0f;
                // Rotate
                Game_Object.transform.rotation = Quaternion.LookRotation(_normal);
            }
        }

        // Create Components
        MeshRenderer mesh_renderer = Game_Object.AddComponent<MeshRenderer>();
        mesh_renderer.sharedMaterial = _material;
        
        MeshFilter mesh_filter = Game_Object.AddComponent<MeshFilter>();
        mesh_filter.mesh = _mesh;

        // Set vertices
        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(0.0f, 0.0f, 0.0f),
            new Vector3(size, 0.0f, 0.0f),
            new Vector3(0.0f, size, 0.0f),
            new Vector3(size, size, 0.0f)
        };
        mesh_filter.mesh.vertices = vertices;

        // Construct triangles
        int[] tris = new int[6]
        {
            // Lower Left
            0, 2, 1,
            // Upper Right
            2, 3, 1
        };
        mesh_filter.mesh.triangles = tris;

        // Todo: investiage
        Vector3[] normals = new Vector3[4]
        {
            Vector3.back,
            Vector3.back,
            Vector3.back,
            Vector3.back
        };
        mesh_filter.mesh.normals = normals;

        // Set UVs, with third coordinate indicating index within texture atlas
        List<Vector3> uv = new List<Vector3>
        {
            new Vector3(0.0f, 0.0f, _type),
            new Vector3(1.0f, 0.0f, _type),
            new Vector3(0.0f, 1.0f, _type),
            new Vector3(1.0f, 1.0f, _type)
        };
        mesh_filter.mesh.SetUVs(0, uv);
    }
}
