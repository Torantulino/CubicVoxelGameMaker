using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextShake : MonoBehaviour
{
    private Vector3 initial_pos;
    private Vector3 inital_rot;

    // Start is called before the first frame update
    void Start()
    {
        initial_pos = transform.position;
        inital_rot = transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        // Simulate wacky main menu text sway
        transform.position = initial_pos + Noise.GetSeaOffset() * 100.0f;
        transform.eulerAngles = inital_rot + 100.0f * new Vector3 (Noise.GetSeaOffset().x, Noise.GetSeaOffset().y, Noise.GetSeaOffset().z);
    }
}
