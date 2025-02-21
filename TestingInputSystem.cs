// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.InputSystem;

// public class TestingInputSystem : MonoBehaviour
// {
//     private Rigidbody2D rigidBody2D;
//     private PlayerInputActions playerInputActions;
//     private float jumpSpeed = 2f;

//     private void Awake()
//     {
//         rigidBody2D = GetComponent<Rigidbody2D>();
//         playerInputActions = new PlayerInputActions();
//         playerInputActions.Player.Enable();
//         playerInputActions.Player.Jump.performed += Jump_performed;
//     }

//     private void FixedUpdate()
//     {
//         Vector2 inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>();
//         float moveSpeed = 2f;
//         transform.position += new Vector3(inputVector.x, 0, inputVector.y) * moveSpeed * Time.deltaTime;
//     }

//     public void Jump_performed(InputAction.CallbackContext context)
//     {
//         Debug.Log(context);
//         Vector2 force = new Vector2(0, 1) * jumpSpeed;
//         rigidBody2D.AddForce(force, ForceMode2D.Impulse);
//         Debug.Log("Jump!" + context.phase);
//     }
// }