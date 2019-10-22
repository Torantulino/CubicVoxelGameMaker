using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class LevelManager : MonoBehaviour
{
    World world;
    // Start is called before the first frame update
    void Start()
    {   
        world = new World();
        world.CreateChunk((int)BlockInfo.BlockType.Grass, new Vector2Int(0,0), this);
    }

    // Update is called once per frame
    void Update()
    {
        world.UpdateWorld();
    }
}
