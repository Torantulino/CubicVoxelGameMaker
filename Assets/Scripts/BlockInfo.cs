using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BlockInfo
{
    public enum BlockType
    {
        Air = 0,
        Grass = 1,
        Dirt = 2,
        Stone = 3,
        Sand = 4,
        LightStone = 5,
        Snow = 6,
        Water = 7,
    }

    // Number of texture in atlas for each face
    // X, -X, Z, -Z, Y, -Y 
    public static int[,] Face_Textures = new int[8,6]
    {
        {0,0,0,0,0,0},  //Air
        {1,1,1,1,0,2},  //Grass
        {2,2,2,2,2,2},  //Dirt
        {3,3,3,3,3,3},  //Stone
        {4,4,4,4,4,4},  //Sand
        {5,5,5,5,5,5},  //LightStone
        {7,7,7,7,6,2},  //Snow
        {8,8,8,8,8,8}   //Water
    };

    public static Vector3[] normals = new Vector3[6]
    {
        Vector3.right,
        Vector3.left,
        Vector3.forward,
        Vector3.back,
        Vector3.up,
        Vector3.down
    };
}