// Used to generate Texture Array asset
// Menu button is available in GameObject > Create Texture Array
// See CHANGEME in the file
using UnityEngine;
using UnityEditor;

public class TextureArray : MonoBehaviour {

    [MenuItem("GameObject/Create Texture Array")]
    static void Create()
    {      
        Texture2D[] block_textures = Resources.LoadAll<Texture2D>("Textures/Blocks/Opaque");
        // CHANGEME: TextureFormat.RGB24 is good for PNG files with no alpha channels. Use TextureFormat.RGB32 with alpha.
        // See Texture2DArray in unity scripting API.
        Texture2DArray textureArray = new Texture2DArray(16, 16, block_textures.Length, TextureFormat.RGB24, false);

        // CHANGEME: If your files start at 001, use i = 1. Otherwise change to what you got.
        for (int i = 0; i < block_textures.Length; i++)
        {
            Debug.Log("Loading " + block_textures[i].name);
            Texture2D tex = block_textures[i];
            textureArray.SetPixels(tex.GetPixels(0), i, 0);
        }
        textureArray.Apply();

        // CHANGEME: Path where you want to save the texture array. It must end in .asset extension for Unity to recognise it.
        //string path = "Assets/SmokeTextureArray.asset";
        //AssetDatabase.CreateAsset(textureArray, path);
        //Debug.Log("Saved asset to " + path);
    }
}

// After this, you will have a Texture Array asset which you can assign to the shader's Tex attribute!
