using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class LevelManager : MonoBehaviour
{
    TextureManager texture_manager = new TextureManager();

    // Start is called before the first frame update
    void Start()
    {   
        //Todo: 4 here is block type
        BlockFace b = new BlockFace(
            texture_manager.Block_Material,
            BlockInfo.Face_Textures[(int)BlockInfo.BlockType.Sand,0]
        );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
