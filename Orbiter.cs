using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class Orbiter : MonoBehaviour 
{
    [SerializeField] Transform pivot;
    [SerializeField] float pivotDistance = 5f;        //Distance to maintain from pivot
    [SerializeField] float rotationSpeed = 10f;
    private Transform thisTransform;
    private Quaternion rotationalDestination = Quaternion.identity;
    private float rotationX = 0f;
    private float rotationY = 0f;

    void Awake()
    {
    	thisTransform = GetComponent<Transform>();
    }

    void Update()
    {
    // Get input from input axis (Joysticks or arrows)
     float horizontalMovementInput = CrossPlatformInputManager.GetAxis("Horizontal");
     float verticalMovementInput = CrossPlatformInputManager.GetAxis("Vertical");

     rotationX += verticalMovementInput * Time.deltaTime * rotationSpeed;
     rotationY += horizontalMovementInput * Time.deltaTime * rotationSpeed;

     Quaternion quaternionRotationY = Quaternion.Euler(0f,rotationY,0f);
     rotationalDestination = quaternionRotationY * Quaternion.Euler(rotationX,0f,0f);

    // Update rotation
    thisTransform.rotation = rotationalDestination;

    //Update position
    thisTransform.position = pivot.position + thisTransform.rotation * Vector3.forward * -pivotDistance;
    }
}
