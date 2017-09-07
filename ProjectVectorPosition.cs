using UnityEngine;
using System.Collections;

public class ProjectVectorPosition : MonoBehaviour 
{

    [SerializeField] Transform target;
    [SerializeField] Transform lineStart;
    [SerializeField] Transform lineEnd;
    private Transform thisTransform;
    
    void Awake () 
    {
        thisTransform = GetComponent<Transform>();
    }
	
    void Update ()
    {
	// Calculate normal
	Vector3 normal = (lineEnd.position - lineStart.position).normalized;

	// Update position
	Vector3 position = lineStart.position + Vector3.Project(target.position-lineStart.position, normal);

	// Clamp position between min and max
	position.x = Mathf.Clamp(position.x, Mathf.Min(lineStart.position.x, lineEnd.position.x), Mathf.Max(lineStart.position.x, lineEnd.position.x));
	position.y = Mathf.Clamp(position.y, Mathf.Min(lineStart.position.y, lineEnd.position.y), Mathf.Max(lineStart.position.y, lineEnd.position.y));
	position.z = Mathf.Clamp(position.z, Mathf.Min(lineStart.position.z, lineEnd.position.z), Mathf.Max(lineStart.position.z, lineEnd.position.z));

        // Update this gameObject's transform position
	thisTransform.position = position;
    }
}
