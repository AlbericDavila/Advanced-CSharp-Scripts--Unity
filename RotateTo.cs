using UnityEngine;
using System.Collections;

public class RotateTo : MonoBehaviour 
{
    //Reference to target object
    [SerializeField] Transform target;

    //Rotation speed
    [SerializeField] float rotationSpeed = 30f;

    //Damped Speed
    [SerializeField] float damping = 10f;

    void Update () 
    {
    	rotateTowardsWithDamp();
    }

    //Call this function in update to directly and continually look at a target position
    void lookAtImmediate()
    {
    	transform.rotation = Quaternion.LookRotation(target.position-transform.position,Vector3.up);
    }

    /// <summary>
    /// Rotate this object towards referenced target object
    /// </summary>
    void rotateTowards()
    {
	//Get look to rotation
	Quaternion DestRot = Quaternion.LookRotation(target.position-transform.position,Vector3.up);

	//Update rotation
	transform.rotation = Quaternion.RotateTowards(transform.rotation, DestRot, rotationSpeed * Time.deltaTime);
    }
	
    /// <summary>
    /// Rotate this object with damping towards referenced target object
    /// </summary>
    void rotateTowardsWithDamp()
    {
	//Get look to rotation
	Quaternion rotationalDestination = Quaternion.LookRotation(target.position-transform.position,Vector3.up);

	//Calc smooth rotate
	Quaternion smoothRotation = Quaternion.Slerp(transform.rotation, rotationalDestination, 1f - (Time.deltaTime*damping));

	//Update Rotation
	transform.rotation = smoothRotation;
    }

    /// <summary>
    /// Spin object about axis
    /// </summary>
    /// <param name="rotationalAxis"></param>
    void spinObject(Vector3 rotationalAxis)
    {
	//Update rotation
	transform.rotation *= Quaternion.Euler(rotationSpeed*Time.deltaTime*rotationalAxis.x,
					       rotationSpeed*Time.deltaTime*rotationalAxis.y,
					       rotationSpeed*Time.deltaTime*rotationalAxis.z);
    }
}
