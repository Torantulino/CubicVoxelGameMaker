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
    }

    // Number of texture in atlas for each face
    // Left, Right, Front, Back, Top, Bottom
    public static int[,] Face_Textures = new int[5,6]
    {
        {0,0,0,0,0,0},  //Air
        {1,1,1,1,0,2},  //Grass
        {2,2,2,2,2,2},  //Dirt
        {3,3,3,3,3,3},  //Stone
        {4,4,4,4,4,4}   //Sand
    };
}