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
    public static KeyCode FREE_HEAD = KeyCode.LeftAlt;  //TODO: IMPLEMENT

    // GAME DEFAULTS
    public static float MOVEMENT_SPEED = 5.0f;
    public static float JUMP_POWER = 7.5f;
    public static float STEP_HEIGHT = 1.0f; // IMPLEMENT
    public static float PLAYER_REACH = 10.0f;

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
