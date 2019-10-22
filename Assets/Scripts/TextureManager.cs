﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TextureManager
{
    public static int TEXTURE_SIZE_PX = 16;

    private Texture2D[] block_textures;
    private Texture2DArray texture_array;
    private Texture2DArray Texture_Array
    {
        get
        {
            if (texture_array == null || texture_array.depth == 0)
            {
                texture_array = CreateTextureArray();
            }
            return texture_array;
        }
    }
    private Material block_material;
    public Material Block_Material
    {
        get
        {
            if (block_material == null)
            {
                block_material = Resources.Load<Material>("Materials/BlockMaterial");
                block_material.SetTexture("_TexArr", Texture_Array);
            }
            return block_material;
        }
    }
    private Mesh mesh;
    public Mesh Mesh
    {
        get
        {
            if(mesh == null)
            {
                mesh = new Mesh();
            }
            return mesh;
        }
    }

    // Creates a TextureArray (Only works on modern GPUs) which appears 
    // to the GPU as a single object and uses a single drawcall
    public Texture2DArray CreateTextureArray()
    {
        // //Load textures
        Texture2D[] block_textures = Resources.LoadAll<Texture2D>("Textures/Blocks/Opaque");

        // Sort array based on numeric name prefix
        block_textures = block_textures.OrderBy(s => int.Parse(s.name.Split('_')[0])).ToArray();

        // Create texture array
        Texture2DArray _texture_array = new Texture2DArray(
            TEXTURE_SIZE_PX,
            TEXTURE_SIZE_PX,
            block_textures.Length,
            TextureFormat.RGB24,
            false);

        // Settings
        _texture_array.wrapMode = TextureWrapMode.Clamp;
        _texture_array.filterMode = FilterMode.Bilinear;

        // Iterate over textures adding pixel data to Texture Array
        for (int i = 0; i < block_textures.Length; i++)
        {
            Debug.Log("Loading " + block_textures[i].name);
            Texture2D _texture = block_textures[i];
            _texture_array.SetPixels(_texture.GetPixels(0), i, 0);
        }
        _texture_array.Apply();

        return _texture_array;
    }
}
