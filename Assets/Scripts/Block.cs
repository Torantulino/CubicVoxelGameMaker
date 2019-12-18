﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Block
{
    public int type = (int)BlockInfo.BlockType.Air;
    public BlockFace[] Faces = new BlockFace[6];
    public Vector3 Position;

    public Block(int _type, Vector3 _pos, bool _hitboxEnabled = true)
    {
        // Set properties
        type = _type;
        Position = _pos;

        // Air requires no faces
        if (_type == (int)BlockInfo.BlockType.Air)
            return;

        // Create faces
        for (int i = 0; i < 6; i++)
        {
            BlockFace _face = new BlockFace(
                BlockInfo.Face_Textures[_type, i],
                BlockInfo.normals[i]
            );
            Faces[i] = _face;
        }
    }
}
