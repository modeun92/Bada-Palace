using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : MonoBehaviour
{
    public float MoveSpeed = 4f;
    public JoyStickController JoyStick;
    public RuntimeAnimatorController FemaleController;
    public RuntimeAnimatorController MaleController;

    private Rigidbody2D mRigidbody;
    private Animator mAnimator;
    // Start is called before the first frame update
    void Start()
    {
        mRigidbody = GetComponent<Rigidbody2D>();
        mAnimator = GetComponent<Animator>();
        SetGender(true);
    }
    // Update is called once per frame
    void Update()
    {
        if (GameTotalEventManager.Instance.GameState == GameState.PLAYING)
        {
            Move();
        }
        else
        {
            Debug.Log($"game state = {GameTotalEventManager.Instance.GameState}");
            Invoke(nameof(StopMoving), 0.2f);
        }
    }
    private void StopMoving()
    {
        mRigidbody.velocity = new Vector2();
    }
    public void SetGender(bool aIsFemale)
    {
        if (aIsFemale)
        {
            mAnimator.runtimeAnimatorController = FemaleController;
        }
        else
        {
            mAnimator.runtimeAnimatorController = MaleController;
        }
    }
    public void Move()
    {
        Vector2 lVelocity;
        if (JoyStick.JoyStickState)
        {
            lVelocity = RespondToJoyStick();
        }
        else
        {
            lVelocity = RespondToKeyBoardInput();
        }
        mAnimator.SetBool("IsMoving", lVelocity.magnitude != 0f);
        mRigidbody.velocity = lVelocity * MoveSpeed;

        Debug.Log($"velocity : {mRigidbody.velocity}");
    }
    private Vector2 RespondToJoyStick()
    {
        return JoyStick.NormalizedMovingVelocity;
    }
    private Vector2 RespondToKeyBoardInput()
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
        return direction;
    }
}
