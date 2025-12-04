using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

[RequireComponent(typeof(CharacterController))]
public class VRCharacterController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float movementSpeed = 2.0f;  // Movement speed in units per second
    public float gravity = 9.81f;       // Gravity force

    private CharacterController characterController;
    private Vector3 verticalVelocity = Vector3.zero;

    void Awake()
    {
        // Automatically require and cache the CharacterController component.
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("CharacterController component not found on the VR rig object!");
        }
    }

    void Update()
    {
        // Gather input from the left-hand controller's primary 2D axis.
        Vector2 inputAxis = Vector2.zero;
        InputDevice leftHand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

        if (leftHand.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis))
        {
            // Calculate a movement direction relative to the headset’s orientation.
            Transform cameraTransform = Camera.main.transform;
            Vector3 forward = cameraTransform.forward;
            forward.y = 0; 
            forward.Normalize();

            Vector3 right = cameraTransform.right;
            right.y = 0;
            right.Normalize();

            Vector3 desiredMoveDirection = (forward * inputAxis.y + right * inputAxis.x);
            Vector3 movement = desiredMoveDirection * movementSpeed;

            characterController.Move(movement * Time.deltaTime);
        }


        if (!characterController.isGrounded)
        {
            verticalVelocity.y -= gravity * Time.deltaTime;
        }
        else if (verticalVelocity.y < 0)
        {
            verticalVelocity.y = 0f;
        }
        characterController.Move(verticalVelocity * Time.deltaTime);
    }
}
