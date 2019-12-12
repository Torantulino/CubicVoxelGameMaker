using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class BlockFace
{
    public bool Render = true;    
    public Vector3[] Vertices;
    public int[] Triangles;
    public Vector3[] Normals;
    public List<Vector3> UVs;
    private Vector3[] vertices = new Vector3[8]
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

    public BlockFace(int _type, Vector3 _normal)
    {
        // Set vertices
        // RIGHT
        if(_normal == Vector3.right)
        {
            Vertices = new Vector3[4]
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
            Vertices = new Vector3[4]
            {
                vertices[4],
                vertices[0],
                vertices[2],
                vertices[6]
            };
        }
        // FORWARD
        else if(_normal == Vector3.forward)
        {
            Vertices = new Vector3[4]
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
            Vertices = new Vector3[4]
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
            Vertices = new Vector3[4]
            {
                vertices[2],
                vertices[3],
                vertices[7],
                vertices[6]
            };
        }
        // Down
        else if (_normal == Vector3.down)
        {
            Vertices = new Vector3[4]
            {
                vertices[4],
                vertices[5],
                vertices[1],
                vertices[0]
            };
        }

        // Construct triangles
        Triangles = new int[6]
        {
            // Lower Left
            0, 3, 1,
            // Upper Right
            1, 3, 2
        };

        //TODO: investiage
        Normals = new Vector3[4]
        {
            Vector3.back,
            Vector3.back,
            Vector3.back,
            Vector3.back
        };

        // Set UVs, with third coordinate indicating index within texture atlas
        UVs = new List<Vector3>
        {
            new Vector3(0.0f, 0.0f, _type),
            new Vector3(1.0f, 0.0f, _type),
            new Vector3(1.0f, 1.0f, _type),
            new Vector3(0.0f, 1.0f, _type)
        };
    }
}
