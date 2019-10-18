using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                block_material.SetTexture("_MyArr", Texture_Array);
            }
            return block_material;
        }
    }



    // Creates a TextureArray (Only works on modern GPUs) which appears 
    // to the GPU as a single object and uses a single drawcall
    public Texture2DArray CreateTextureArray()
    {
        // //Load textures
        // block_textures = Resources.LoadAll<Texture2D>("Textures/Blocks/Opaque");

        // // Create texture array
        // Texture2DArray _texture_array = new Texture2DArray(
        //     TEXTURE_SIZE_PX,
        //     TEXTURE_SIZE_PX,
        //     block_textures.Length,
        //     TextureFormat.RGBA32,
        //     true,
        //     false);

        // // Settings
        // _texture_array.wrapMode = TextureWrapMode.Repeat;
        // _texture_array.filterMode = FilterMode.Bilinear;

        // // Iterate over textures adding pixel data to Texture Array
        // for (int i = 0; i < block_textures.Length; i++)
        // {
        //     _texture_array.SetPixels(block_textures[i].GetPixels(0), i, 0);
        // }

        // // Apply changes
        // _texture_array.Apply();

        Texture2D[] block_textures = Resources.LoadAll<Texture2D>("Textures/Blocks/Opaque");
        // CHANGEME: TextureFormat.RGB24 is good for PNG files with no alpha channels. Use TextureFormat.RGB32 with alpha.
        // See Texture2DArray in unity scripting API.
        Texture2DArray _texture_array = new Texture2DArray(16, 16, block_textures.Length, TextureFormat.RGB24, false);

        // CHANGEME: If your files start at 001, use i = 1. Otherwise change to what you got.
        for (int i = 0; i < block_textures.Length; i++)
        {
            Debug.Log("Loading " + block_textures[i].name);
            Texture2D tex = block_textures[i];
            _texture_array.SetPixels(tex.GetPixels(0), i, 0);
        }
        _texture_array.Apply();


        return _texture_array;
    }
}
