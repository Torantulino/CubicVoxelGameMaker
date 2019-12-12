using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config : MonoBehaviour
{
    // USER INPUT
    public static float LOOK_SENSITIVITY = 3.0f;
    public static bool INVERT_Y_AXIS = false;  //TODO: IMPLEMENT

    // INPUT CONTROLS
    public static KeyCode MOVE_FORWARDS = KeyCode.W;
    public static KeyCode MOVE_LEFT = KeyCode.A;
    public static KeyCode MOVE_RIGHT = KeyCode.D;
    public static KeyCode MOVE_BACKWARDS = KeyCode.S;
    public static KeyCode JUMP = KeyCode.Space;
    public static KeyCode SPRINT = KeyCode.LeftShift;
    public static KeyCode FREE_HEAD = KeyCode.LeftAlt;
    public static KeyCode PIPETTE = KeyCode.F;

    // GAME DEFAULTS
    public static float MOVEMENT_SPEED = 5.0f;
    public static float JUMP_POWER = 9.0f;
    public static float STEP_HEIGHT = 0.5f; // TODO: IMPLEMENT
    public static float PLAYER_REACH = 10.0f;
    public static float GRAVITY = -25.0f;
    public static float PLAYER_HEIGHT = 1.8f;   //TODO: APPLY TO PLAYER BODY
    public static float MAX_WATER_VELOCITY = 5.0f;
    public static float JUMP_INTERVAL = 0.5f;

    // GRAPHCS SETTINGS
    public static int FOV;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
