using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class VRPlayerMovement : MonoBehaviour
{
    // Speed factor for movement
    public float speed = 2.0f;
    private CharacterController characterController;

    void Start()
    {
        // Get the CharacterController component on the rig
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("No CharacterController found on the VR rig object!");
        }
    }

    void Update()
    {
        Vector2 inputAxis = Vector2.zero;
        InputDevice leftHand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

        if (leftHand.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis))
        {
            Transform head = Camera.main.transform;
            Vector3 forward = head.forward;
            forward.y = 0;
            forward.Normalize();

            Vector3 right = head.right;
            right.y = 0;
            right.Normalize();
            Vector3 movement = (forward * inputAxis.y + right * inputAxis.x);
            characterController.SimpleMove(movement * speed);
        }
    }
}
