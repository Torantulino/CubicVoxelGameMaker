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
        //Todo: 2 here is block type
        texture_manager.Block_Material.SetFloat("_SliceRange", 5);
        BlockFace b = new BlockFace(texture_manager.Block_Material, 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
