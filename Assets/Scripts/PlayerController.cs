using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject Screen_Overlay;
    public GameObject Test_Cube;
    // Start is called before the first frame update
    void Start()
    {
        Screen_Overlay = transform.GetChild(0).gameObject;
        Screen_Overlay.transform.gameObject.transform.localScale = 
            new Vector3 (2.0f, World.SEA_LEVEL, 1.0f);
        Screen_Overlay.GetComponent<MeshRenderer>().material.mainTextureScale = 
            new Vector2(2.0f, World.SEA_LEVEL);


        //Test_Cube = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //Test_Cube.transform.localScale = Vector3.one / 10.0f;
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 sea_offset = Noise.GetSeaOffset();
        
        // Water in Eyes
        if(Camera.main.transform.position.y <= World.SEA_LEVEL - sea_offset.y)
        {
            Screen_Overlay.SetActive(true);


            Vector3 camera_offset = Camera.main.nearClipPlane * (1.84f * Camera.main.transform.forward);
            
            // Vector3 sea_position = new Vector3(
            //         transform.position.x,
            //         World.SEA_LEVEL + sea_offset.y, 
            //         transform.position.z) +
            //         new Vector3(camera_offset.x, 0.0f, camera_offset.z);
            
            //Vector3 sea_screen_pos = Camera.main.WorldToScreenPoint(sea_position);
            

            Screen_Overlay.transform.localPosition = new Vector3(
                0.0f,
                
                (World.SEA_LEVEL + sea_offset.y) - 
                (transform.position.y + camera_offset.y) -
                Screen_Overlay.transform.localScale.y/2.0f,
                0.84f);         

            //Test_Cube.transform.position = sea_position;
        }
        else
            Screen_Overlay.SetActive(false);
    }
}
