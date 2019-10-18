using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public int[,,] blocks = new int[World.CHUNK_SIZE,World.CHUNK_SIZE,World.CHUNK_SIZE];
}
