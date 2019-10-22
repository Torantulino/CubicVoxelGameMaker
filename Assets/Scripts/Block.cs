using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    public int type;
    public bool hitboxEnabled;

    public GameObject Game_Object;
    public BlockFace[] Faces = new BlockFace[6];


    public Block(int _type, Vector3 _pos, TextureManager texture_manager, bool _hitboxEnabled = true)
    {
        // Set properties
        type = _type;
        hitboxEnabled = _hitboxEnabled;
        
        // Create GameObject
        Game_Object = new GameObject("Block");

        // Create faces
        for (int i = 0; i < 6; i++)
        {
            BlockFace _face = new BlockFace(
                texture_manager.Block_Material,
                BlockInfo.Face_Textures[_type, i],
                BlockInfo.normals[i],
                texture_manager.Mesh
            );
            // Set face properties
            _face.Game_Object.transform.parent = Game_Object.transform;
            _face.Game_Object.name = "Face_" + i;
            Faces[i] = _face;
        }
        // Set object positions
        Game_Object.transform.position = _pos;
    }
}
