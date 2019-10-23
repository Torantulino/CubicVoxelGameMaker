using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockFace
{
    public bool Render = true;
    public Mesh mesh = new Mesh();

    Vector3[] vertices = new Vector3[8]
    {
            new Vector3(0.0f, 0.0f, 0.0f), //0
            new Vector3(1.0f, 0.0f, 0.0f), //1
            new Vector3(0.0f, 1.0f, 0.0f), //2
            new Vector3(1.0f, 1.0f, 0.0f), //3
            new Vector3(0.0f, 0.0f, 1.0f), //4
            new Vector3(1.0f, 0.0f, 1.0f), //5
            new Vector3(0.0f, 1.0f, 1.0f), //6
            new Vector3(1.0f, 1.0f, 1.0f)  //7
    };

    public BlockFace(Material _material, int _type, Vector3 _normal)
    {
        // Position

        // Set vertices
        // RIGHT
        if(_normal == Vector3.right)
        {
            mesh.vertices = new Vector3[4]
            {
            vertices[1],
            vertices[5],
            vertices[7],
            vertices[3]
            };
        }
        // LEFT
        else if(_normal == Vector3.left)
        {
            mesh.vertices = new Vector3[4]
            {
            vertices[0],
            vertices[2],
            vertices[6],
            vertices[4]
            };
        }
        // FORWARD
        else if(_normal == Vector3.forward)
        {
            mesh.vertices = new Vector3[4]
            {
            vertices[5],
            vertices[4],
            vertices[6],
            vertices[7]
            };
        }        
        // BACK
        else if(_normal == Vector3.back)
        {
            mesh.vertices = new Vector3[4]
            {
            vertices[0],
            vertices[1],
            vertices[3],
            vertices[2]
            };
        }
        // UP
        else if(_normal == Vector3.up)
        {
            mesh.vertices = new Vector3[4]
            {
            vertices[2],
            vertices[3],
            vertices[7],
            vertices[6]
            };
        }
        // Down
        else if(_normal == Vector3.down)
        {
            mesh.vertices = new Vector3[4]
            {
            vertices[4],
            vertices[5],
            vertices[1],
            vertices[0]
            };
        }

        // Construct triangles
        //TODO: counter clockwise or clockwise?
        int[] tris = new int[6]
        {
            // Lower Left
            0, 2, 1,
            // Upper Right
            0, 3, 2
        };
        mesh.triangles = tris;

        //TODO: investiage
        Vector3[] normals = new Vector3[4]
        {
            Vector3.back,
            Vector3.back,
            Vector3.back,
            Vector3.back
        };
        mesh.normals = normals;

        // Set UVs, with third coordinate indicating index within texture atlas
        List<Vector3> uv = new List<Vector3>
        {
            new Vector3(0.0f, 0.0f, _type),
            new Vector3(1.0f, 0.0f, _type),
            new Vector3(0.0f, 1.0f, _type),
            new Vector3(1.0f, 1.0f, _type)
        };
        mesh.SetUVs(0, uv);
    }
}
