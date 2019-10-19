using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    TextureManager texture_manager = new TextureManager();
    public int type;
    public bool hitboxEnabled;

    public GameObject Game_Object;
    public BlockFace[] Faces = new BlockFace[6];
    private Vector3[] normals = new Vector3[6]
    {
        Vector3.left,
        Vector3.right,
        Vector3.forward,
        Vector3.back,
        Vector3.up,
        Vector3.down
    };

    public Block(int _type, Vector3 _pos, bool _hitboxEnabled = true)
    {
        // Set properties
        type = _type;
        hitboxEnabled = _hitboxEnabled;
        
        // Create GameObject
        GameObject Game_Object = new GameObject("Block");

        // Create faces
        for (int i=0; i<6; i++)
        {
            BlockFace b = new BlockFace(
                texture_manager.Block_Material,
                BlockInfo.Face_Textures[_type, i],
                normals[i]
            );
            b.Game_Object.transform.parent = Game_Object.transform;
            b.Game_Object.name = "Face_" + i;
            Faces[i] = b;
        }
        // Set object positions
        Game_Object.transform.position = _pos;
    }
}
