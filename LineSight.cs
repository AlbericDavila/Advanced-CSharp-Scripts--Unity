using UnityEngine;
using System.Collections;

public class LineSight : MonoBehaviour
{

	public enum SightSensitivity {STRICT, LOOSE};    // How sensitive should we be to sight
    public SightSensitivity sensitivity;             // Sight sensitivity
    public bool canSeeTarget = false;                // Can AI see the target?
    public float fieldOfView = 45f;                  // AI's forward field of view (meassured in angles)
	public Transform target;
	public Transform eyePoint;                       // Reference to eyes, where FOV starts
    public Vector3 lastKnowSighting = Vector3.zero;  // Last known player/object sighting, if any
    private Transform thisTransform;
	private SphereCollider thisCollider;	
                                                    
    void Awake()
	{
		thisTransform = GetComponent<Transform>();
		thisCollider = GetComponent<SphereCollider>();
		lastKnowSighting = thisTransform.position;
        sensitivity = SightSensitivity.STRICT;
    }
	
	bool InFOV()
	{
		// Get direction to target
		Vector3 DirToTarget = target.position - eyePoint.position;

		// Get angle between forward and look direction
		float Angle = Vector3.Angle(eyePoint.forward, DirToTarget);

		// Is target/player within field of view?
		if(Angle <= fieldOfView)
			return true;

		// Not within view
		return false;
	}
	
	bool ClearLineofSight()
	{
		RaycastHit Info;
	
		if(Physics.Raycast(eyePoint.position, (target.position - eyePoint.position).normalized, out Info, thisCollider.radius))
		{
			// If what touches the line if sight is the player, then AI can see the player,
            // Set canSeeTarget to true
			if(Info.transform.CompareTag("Player"))
				return true;
		}

		return false;
	}
	
	void UpdateSight()
	{
		switch(sensitivity)
		{
            // Can see player if player is in AI's field of view AND is in plain sight (not behind objects)
			case SightSensitivity.STRICT:
				canSeeTarget = InFOV() && ClearLineofSight();
			break;

            // Can see player if player is in AI's field of view OR is in plain sight (not behind objects)
            case SightSensitivity.LOOSE:
				canSeeTarget = InFOV() || ClearLineofSight();
			break;
		}
	}
	
	void OnTriggerStay(Collider Other)
	{
		UpdateSight();

		// Update last known sighting
		if(canSeeTarget)
			lastKnowSighting =  target.position;
	}
}