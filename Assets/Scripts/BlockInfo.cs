using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockInfo : MonoBehaviour
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
    public int[,] Face_Textures = new int[5,6]
    {
        {0,0,0,0,0,0},
        {1,1,1,1,0,2},
        {2,2,2,2,2,2},
        {3,3,3,3,3,3},
        {4,4,4,4,4,4}
    };
}