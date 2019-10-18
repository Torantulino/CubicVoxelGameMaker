using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class LevelManager : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {   
        Block block = new Block((int)BlockInfo.BlockType.Sand, Vector3.up*2.0f);
        Block block2 = new Block((int)BlockInfo.BlockType.Stone, Vector3.down*2.0f);
        Block block3 = new Block((int)BlockInfo.BlockType.Dirt, Vector3.left*2.0f);
        Block block4 = new Block((int)BlockInfo.BlockType.Grass, Vector3.right*2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
