using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    public static int GetBlockHeight(int _block_x, int _block_z, Vector2Int _chunk_pos)
    {   
        // MOUNTAINS:
        // no_octaves = 2
        // frequency = 1
        // amplitude = 1
        // lacunarity = 2
        // persistance = 0.5

        // ISLANDS:
        // no_octaves = 3
        // frequency = 1
        // amplitude = 1
        // lacunarity = 0.7
        // persistance = 3.0

        const int min_value = 1;
        const float x_offset = 1234;
        const float z_offset = 4321;

        const int no_octaves = 3;

        float frequency = 1.0f;
        float amplitude = 1.0f;

        const float lacunarity = 0.70f;
        const float persistance = 3.0f;

        float height = 0;
        Vector2 block_pos = new Vector2(_block_x + (_chunk_pos.x * World.CHUNK_SIZE),
            _block_z + (_chunk_pos.y * World.CHUNK_SIZE));
        
        float max_possiblility = 0;
        float min_possiblility = 0;
        for (int i = 0; i < no_octaves; i++)
        {   
            // The higher the frequency, the further appart the sample points will be
            // Meaning the height values will change more rapidly
            float x = (block_pos.x + (i * x_offset)) / World.NOISE_SCALE * frequency;
            float y = (block_pos.y + (i * z_offset)) / World.NOISE_SCALE * frequency;

            float noise_sample = Mathf.PerlinNoise(x, y) * 2.0f - 1.0f;
            
            height += noise_sample * amplitude;
            max_possiblility += 1.0f * amplitude;
            min_possiblility += - 1.0f * amplitude;

            frequency *= lacunarity;
            amplitude *= persistance;
        }

        if(World.ISLANDS)
            min_possiblility = -2.0f;

        float normalised_height = Mathf.InverseLerp(min_possiblility, max_possiblility, height);
        height = normalised_height * World.WORLD_HEIGHT;

        //Inforce minimum
        if(height < min_value)
            height = min_value;

        return (int)height;
    }

    public static Vector3 GetSeaOffset()
    {
        return new Vector3(
                Mathf.PerlinNoise(Time.timeSinceLevelLoad * 0.25f, 0.1f) / 2.0f, // Main Waves
                - 0.05f - Mathf.PerlinNoise(Time.timeSinceLevelLoad * 0.25f, 0.2f) / 3.0f,
                Mathf.PerlinNoise(1.0f, Time.timeSinceLevelLoad * 0.25f) / 4.0f // Ripples
                );
    }
}
