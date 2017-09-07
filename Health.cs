using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour 
{
	public float HealthPoints
	{
		get{return healthPoints;}
		set
		{
			healthPoints = value;

			// If health is < 0 then die, you rebel scum
			if(healthPoints <= 0)
				Destroy(gameObject);
		}
	}

	[SerializeField] float healthPoints = 100f;
}
