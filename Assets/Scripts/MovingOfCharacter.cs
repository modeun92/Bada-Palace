using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingOfCharacter : MonoBehaviour
{
    public float MovingSpeed;
    public Rigidbody2D Moving;
    public Vector2 JoyStickAngle;

    private bool IsJoyStickUsed;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(GameTotalEventManager.Instance.GameState == GameState.PLAYING)
        {
            Moving.velocity = IsJoyStickUsed? RespondJoyStick() : RespondKeyBoardInput();
        }
        Debug.Log("moving: " + GameTotalEventManager.Instance.GameState.ToString());
    }
    public void SetJoyStickSwitch(bool power)
    {
        IsJoyStickUsed = power;
    }
    private Vector2 RespondJoyStick()
    {
        return new Vector2(JoyStickAngle.x * MovingSpeed, JoyStickAngle.y * MovingSpeed);
    }
    private Vector2 RespondKeyBoardInput()
    {
        Vector2 direction = new Vector2();
        if (Input.GetKey(KeyCode.UpArrow))
        { direction.y = 1f; }
        if (Input.GetKey(KeyCode.DownArrow))
        { direction.y = -1f; }
        if (Input.GetKey(KeyCode.LeftArrow))
        { direction.x = -1f; }
        if (Input.GetKey(KeyCode.RightArrow))
        { direction.x = 1f; }
        direction.Normalize();
        direction.x *= MovingSpeed;
        direction.y *= MovingSpeed;
        Debug.Log(string.Format("x: {0}, y: {1}", direction.x, direction.y));
        return direction;
    }
}
