using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    TextureManager texture_manager = new TextureManager();
    public int type;
    public bool hitboxEnabled;

    public GameObject Game_Object;

    public Block(int _type, Vector3 _pos, bool _hitboxEnabled = true){
        type = _type;
        hitboxEnabled = _hitboxEnabled;
        
        GameObject Game_Object = new GameObject("Block");

        BlockFace b3 = new BlockFace(
            texture_manager.Block_Material,
            BlockInfo.Face_Textures[_type, 0],
            Vector3.left
        );
        b3.Game_Object.transform.parent = Game_Object.transform;

        BlockFace b4 = new BlockFace(
            texture_manager.Block_Material,
            BlockInfo.Face_Textures[_type, 1],
            Vector3.right
        );
        b4.Game_Object.transform.parent = Game_Object.transform;

        BlockFace b = new BlockFace(
            texture_manager.Block_Material,
            BlockInfo.Face_Textures[_type, 2],
            Vector3.forward
        );  
        b.Game_Object.transform.parent = Game_Object.transform;

        BlockFace b2 = new BlockFace(
            texture_manager.Block_Material,
            BlockInfo.Face_Textures[_type,3],
            Vector3.back
        ); 
        b2.Game_Object.transform.parent = Game_Object.transform;

        BlockFace b5 = new BlockFace(
            texture_manager.Block_Material,
            BlockInfo.Face_Textures[_type, 4],
            Vector3.up
        );
        b5.Game_Object.transform.parent = Game_Object.transform;

        BlockFace b6 = new BlockFace(
            texture_manager.Block_Material,
            BlockInfo.Face_Textures[_type, 5],
            Vector3.down
        );
        b6.Game_Object.transform.parent = Game_Object.transform;

        Game_Object.transform.position = _pos;
    }
}
