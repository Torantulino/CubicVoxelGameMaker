using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject Screen_Overlay;
    private bool touching_ground = true;    //TODO: Implement
    private int jumps_remaining = 2;

    private Rigidbody player_rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        //Screen_Overlay = transform.GetChild(0).gameObject;
        //Screen_Overlay.transform.gameObject.transform.localScale =
            new Vector3(2.0f, World.SEA_LEVEL, 1.0f);
        //Screen_Overlay.GetComponent<MeshRenderer>().material.mainTextureScale =
            new Vector2(2.0f, World.SEA_LEVEL);


    }

    // Update is called once per frame
    void Update()
    {

        PlayerControlAndResponse();
        //SimulateCameraEffects();      //TODO: MOVE TO CAMERA EFFECTS SPECIFIC CLASS ON MAIN CAMERA
        player_rigidbody = GetComponent<Rigidbody>();
    }

    private void PlayerControlAndResponse()
    {
        // Head Movement
        //TODO: Could Lerp here for smoothness? Optional?
        float newRotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * Config.LOOK_SENSITIVITY;
        float newRotationY = transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * Config.LOOK_SENSITIVITY;
        transform.localEulerAngles = new Vector3(newRotationY, newRotationX, 0f);

        // Player Movement
        // Move Forwards
        if (Input.GetKey(Config.MOVE_FORWARDS))
        {
            transform.position = transform.position + (transform.forward * Config.MOVEMENT_SPEED * Time.deltaTime);
        }
        // Move Left
        if (Input.GetKey(Config.MOVE_LEFT))
        {
            transform.position = transform.position + (-transform.right * Config.MOVEMENT_SPEED * Time.deltaTime);
        }
        // Move Right
        if (Input.GetKey(Config.MOVE_RIGHT))
        {
            transform.position = transform.position + (transform.right * Config.MOVEMENT_SPEED * Time.deltaTime);
        }
        // Move Back
        if (Input.GetKey(Config.MOVE_BACKWARDS))
        {
            transform.position = transform.position + (-transform.forward * Config.MOVEMENT_SPEED * Time.deltaTime);
        }
        // No Movement Related Input
        if (!Input.GetKey(Config.MOVE_FORWARDS) && !Input.GetKey(Config.MOVE_BACKWARDS) && !Input.GetKey(Config.MOVE_LEFT) && !Input.GetKey(Config.MOVE_RIGHT))
        {
            if(touching_ground)
            {
                //Stop
            }
        }
        // Jump
        if (Input.GetKey(Config.JUMP))
        {
            if(touching_ground && jumps_remaining > 0)
            {
                Vector3 velocity = Vector3.zero;
                velocity += Vector3.up * Config.JUMP_POWER;
                player_rigidbody.velocity = velocity;

                touching_ground = false;

                //GetComponent<Rigidbody>().AddForce(Vector3.up * Config.JUMP_POWER);
            }
            
        }
        else
                touching_ground = true; //TODO: IMPLEMENT PROPERLY;



    }

    private void SimulateCameraEffects()
    {
        Vector3 sea_offset = Noise.GetSeaOffset();

        // Water in Eyes
        if (Camera.main.transform.position.y <= World.SEA_LEVEL - sea_offset.y)
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
                Screen_Overlay.transform.localScale.y / 2.0f,
                0.84f);

            //Test_Cube.transform.position = sea_position;
        }
        else
            Screen_Overlay.SetActive(false);
    }
}
