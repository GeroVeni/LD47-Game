using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public delegate void OnPlayerMovementListener(float dist);
    public event OnPlayerMovementListener OnPlayerMovement;

    // Public fields
    public CharacterController2D controller;

    public float runSpeed;


    // Private fields
    Vector2 previousPosition;
    float horizontalMove = 0f;
    bool jump = false;

    // Start is called before the first frame update
    void Start()
    {
        previousPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Get player input
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        // Jump
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
    }

    public void SetOnPlayerMovementListener(OnPlayerMovementListener listener)
    {
        OnPlayerMovement += listener;
    }

    private void FixedUpdate()
    {
        // Measure movement since last frame
        Vector2 deltaPos = (Vector2)transform.position - previousPosition;
        float horizontalDeltaPos = Mathf.Abs(deltaPos.x);
        OnPlayerMovement?.Invoke(horizontalDeltaPos);
        previousPosition = transform.position;

        // Move player
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        jump = false;
    }
}
