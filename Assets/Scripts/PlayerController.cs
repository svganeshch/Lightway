using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.XR;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    CharacterController characterController;
    PlayerInput input;
    Transform cam;

    //Action
    InputAction moveAction;
    InputAction jumpAction;
    InputAction sprintAction;

    //Vars
    public float DefaultMoveSpeed = 2.5f;
    public float DefaultSprintSpeed = 4.0f;
    public float maxForce = 1.0f;
    public float rotationSmoothTime = 0.1f;
    public float jumpStrength = 1.0f;
    float rotationSmoothVelocity;

    float GRAVITY = -9.81f;
    float moveSpeed = 1.0f;
    float jumpVelocity = 0;
    bool isGrounded = false;
    Vector3 moveVector;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        input = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cam = Camera.main.transform;

        moveSpeed = DefaultMoveSpeed;

        moveAction = input.actions.FindAction("Move");
        jumpAction = input.actions.FindAction("Jump");
        sprintAction = input.actions.FindAction("Sprint");

        sprintAction.Disable();
        jumpAction.Disable();
    }

    private void Update()
    {
        Move();
        SetAnims();
    }

    private void Move()
    {
        moveVector = moveAction.ReadValue<Vector2>().normalized;
        isGrounded = characterController.isGrounded;

        if (isGrounded && jumpVelocity < 0)
        {
            jumpVelocity = 0f;
        }

        if (moveVector != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(moveVector.x, moveVector.y) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSmoothVelocity, rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            if (sprintAction.enabled)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    moveSpeed = DefaultSprintSpeed;
                    Debug.Log("sprinting");
                }
                else if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    moveSpeed = DefaultMoveSpeed;
                }
            }

            characterController.Move(moveSpeed * Time.deltaTime * moveDir.normalized);

            //animator.SetFloat("Velocity", 0.5f);
        }
        else
        {
            //animator.SetFloat("Velocity", 0f);
        }

        if (jumpAction.WasPerformedThisFrame())
        {
            jumpVelocity += Mathf.Sqrt(-3.0f * GRAVITY * jumpStrength);
        }

        jumpVelocity += GRAVITY * Time.deltaTime;
        characterController.Move(new Vector3(0, jumpVelocity, 0) * Time.deltaTime);
    }

    private void SetAnims()
    {
        if (moveVector == Vector3.zero)
        {
            animator.SetFloat("Velocity", 0f);
        }
        else if (moveSpeed == DefaultMoveSpeed)
        {
            animator.SetFloat("Velocity", 0.5f);
        }
        else if (moveSpeed == DefaultSprintSpeed)
        {
            animator.SetFloat("Velocity", 1f);
        }
    }
}
