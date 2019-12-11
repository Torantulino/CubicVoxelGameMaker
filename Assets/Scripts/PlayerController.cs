using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject Screen_Overlay;
    public GameObject Head;
    private bool touching_ground = true;    //TODO: Implement
    private int jumps_remaining = 2;

    private Rigidbody player_rigidbody;

    private LevelManager level_manager;
    private ChunkManager chunk_manager;

    // Start is called before the first frame update
    void Start()
    {
        //Screen_Overlay = transform.GetChild(0).gameObject;
        //Screen_Overlay.transform.gameObject.transform.localScale =
        new Vector3(2.0f, World.SEA_LEVEL, 1.0f);
        //Screen_Overlay.GetComponent<MeshRenderer>().material.mainTextureScale =
        new Vector2(2.0f, World.SEA_LEVEL);

        level_manager = FindObjectOfType<LevelManager>();
        chunk_manager = level_manager.Chunk_Manager;

        // Find head
        Head = GameObject.Find("Head");

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {

        PlayerControlAndResponse();
        //SimulateCameraEffects();      //TODO: MOVE TO CAMERA EFFECTS SPECIFIC CLASS ON MAIN CAMERA
        player_rigidbody = GetComponent<Rigidbody>();

        // UnLock cursor
        if(Input.GetKey(KeyCode.Escape))
            Cursor.lockState = CursorLockMode.None;
    }

    private void PlayerControlAndResponse()
    {
        // Head Movement
        //TODO: Could Lerp here for smoothness? Optionally?
        float new_rotation_x = Head.transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * Config.LOOK_SENSITIVITY;
        float new_rotation_y = Head.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * Config.LOOK_SENSITIVITY;
        // Enforce limits
        // 0 == Straight Ahead
        // 90 == Down etc.
        if(new_rotation_x < 275.0f && new_rotation_x > 150.0f)
            new_rotation_x = 275.0f;
        if(new_rotation_x > 85.0f && new_rotation_x < 150.0f)
            new_rotation_x = 85.0f;

        Head.transform.localEulerAngles = new Vector3(new_rotation_x, new_rotation_y, 0.0f);

        // // If not holding freelook key, rotate body with head around y axis
        if(!Input.GetKey(Config.FREE_HEAD))
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + Head.transform.localEulerAngles.y, 0.0f);
            Head.transform.localEulerAngles = new Vector3(Head.transform.localEulerAngles.x, 0.0f, 0.0f);
        }


        // Block Breaking / Placing
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            // Debug Ray
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * Config.PLAYER_REACH, Color.magenta, 1.0f);

            // Define ray forwards through camera
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hit_data;

            // Raycast, and return true if an object in layer 'Blocks' was hit within range
            if (Physics.Raycast(ray, out hit_data, Config.PLAYER_REACH, LayerMask.GetMask("Blocks")))
            {
                // Get position of hit block
                Vector3 hit_block_pos = GetHitBlock(hit_data.normal, hit_data.point);

                // Round to get hit chunk
                Vector2Int hit_chunk
                    = new Vector2Int(Mathf.FloorToInt(hit_block_pos.x / World.CHUNK_SIZE),
                                  Mathf.FloorToInt(hit_block_pos.z / World.CHUNK_SIZE));

                // Get origin (position) of hit chunk in world
                Vector2 chunk_pos = hit_chunk * World.CHUNK_SIZE;

                // Get coords of hit block within parent chunk
                Vector3Int hit_block = new Vector3Int(Mathf.FloorToInt(hit_block_pos.x - chunk_pos.x), Mathf.FloorToInt(hit_block_pos.y), Mathf.FloorToInt(hit_block_pos.z - chunk_pos.y));


                // Block Building (Right Click)
                if (Input.GetMouseButtonDown(1))
                {   
                    int new_block_x = hit_block.x + (int)hit_data.normal.x;
                    int new_block_y = hit_block.y + (int)hit_data.normal.y;
                    int new_block_z = hit_block.z + (int)hit_data.normal.z;
                    int new_chunk_x = hit_chunk.x;
                    int new_chunk_z = hit_chunk.y;

                    // Positive X Edge
                    if(new_block_x >= World.CHUNK_SIZE)
                    {
                        new_chunk_x ++;
                        new_block_x = 0;
                    }
                    // Negative X Edge
                    else if(new_block_x < 0)
                    {
                        new_chunk_x --;
                        new_block_x = World.CHUNK_SIZE-1;
                    }
                    
                    // Positive Z Edge
                    if(new_block_z >= World.CHUNK_SIZE)
                    {
                        new_chunk_z ++;
                        new_block_z = 0;
                    }
                    // Negative Z Edge
                    else if(new_block_z < 0)
                    {
                        new_chunk_z --;
                        new_block_z = World.CHUNK_SIZE-1;
                    }

                    // Height Limit
                    if (new_block_y >= World.WORLD_HEIGHT)
                        return; // Disregard blocks placed above world limit. Could play sound effect here.

                    // Apply changes 
                    Vector3Int new_block_pos = new Vector3Int(
                        new_block_x,
                        new_block_y, 
                        new_block_z
                    );
                    Vector2Int new_chunk_pos = new Vector2Int(new_chunk_x, new_chunk_z);

                    // Place block
                    chunk_manager.SetBlock((int)BlockInfo.BlockType.Light_Stone, new_chunk_pos, new_block_pos);
                }
                // Block Breaking (Left Click)
                else
                    chunk_manager.SetBlock((int)BlockInfo.BlockType.Air, hit_chunk, hit_block);
            }
        }

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
            if (touching_ground)
            {
                //Stop
            }
        }
        // Jump
        if (Input.GetKey(Config.JUMP))
        {
            if (touching_ground && jumps_remaining > 0)
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

    // Gets the position of the block that was hit by raycast at _point on face with _normal
    private Vector3 GetHitBlock(Vector3 _normal, Vector3 _point)
    {
        // Isolate the compononet along the normal vector
        Vector3 p_n = Vector3.Scale(_point, _normal);
        Vector3 point_normal_axis = new Vector3(
            (_normal.x == 0) ? 0 : p_n.x / _normal.x,
            (_normal.y == 0) ? 0 : p_n.y / _normal.y,
            (_normal.z == 0) ? 0 : p_n.z / _normal.z);

        // Isolate the remaining axes
        Vector3 point_other_axes = _point - point_normal_axis;

        // Calculate normal component of block position
        Vector3 block_pos_normal_axis = point_normal_axis - (_normal / 2.0f);

        // Calculate other components of block position
        Vector3 block_pos_other_axes = new Vector3(
            Mathf.Round(point_other_axes.x + 0.5f) - 0.5f,
            Mathf.Round(point_other_axes.y + 0.5f) - 0.5f,
            Mathf.Round(point_other_axes.z + 0.5f) - 0.5f);

        // Combine
        Vector3 hit_block_pos = block_pos_normal_axis + block_pos_other_axes;

        // Cancel out negative 0.5f offset along normal axis 
        hit_block_pos += new Vector3(
            Mathf.Abs(_normal.x / 2.0f),
            Mathf.Abs(_normal.y / 2.0f),
            Mathf.Abs(_normal.z / 2.0f));

        return hit_block_pos;
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
