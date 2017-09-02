using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class TerrainHover : MonoBehaviour 
{
	private Transform thisTransform;
    private Vector3 destinationUpVector = Vector3.zero;
    private Vector3 newPosition;
    private float horizontalMovementInput;  // Ranges from 1 to 0
    private float verticalMovementInput;    // Ranges from 1 to 0
    public float maxSpeed = 10f;
	public float distanceFromGround = 2f;
	public float angleSpeed = 5f;           // Speed of the change of angles of this transform
    public float rotationSpeed = 90f;       // Speed of the change of rotation of this transform

    // Use this for initialization
    void Awake () 
	{
		thisTransform = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		horizontalMovementInput = CrossPlatformInputManager.GetAxis("Horizontal");
		verticalMovementInput = CrossPlatformInputManager.GetAxis("Vertical");

        newPosition = thisTransform.position;
        newPosition += thisTransform.forward * verticalMovementInput * maxSpeed * Time.deltaTime;
        newPosition += thisTransform.right * horizontalMovementInput * maxSpeed * Time.deltaTime;

        // Raycast from object to floor, verifying the distance from it
        RaycastHit Hit;
		if(Physics.Raycast(thisTransform.position, -Vector3.up, out Hit))
		{
            // Set distance from object to ground
            newPosition.y = (Hit.point + Vector3.up * distanceFromGround).y;
			destinationUpVector = Hit.normal;
		}

        // Update position
		thisTransform.position = newPosition;
        
        // Update distance from ground
		thisTransform.up = Vector3.Slerp(thisTransform.up, destinationUpVector, angleSpeed*Time.deltaTime);
	}
}
