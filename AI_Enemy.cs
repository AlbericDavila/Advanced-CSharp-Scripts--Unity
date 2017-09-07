using UnityEngine;
using System.Collections;

public class AI_Enemy : MonoBehaviour
{
	
	public enum ENEMY_STATE {PATROL, CHASE, ATTACK};
	
	public ENEMY_STATE CurrentState
	{
		get{return currentstate;}

		set
		{
			// Update current state
			currentstate = value;

			// Stop all running coroutines
			StopAllCoroutines();

			switch(currentstate)
			{
				case ENEMY_STATE.PATROL:
					StartCoroutine(AIPatrol());
				break;

				case ENEMY_STATE.CHASE:
					StartCoroutine(AIChase());
				break;

				case ENEMY_STATE.ATTACK:
					StartCoroutine(AIAttack());
				break;
			}
		}
	}
	
	[SerializeField] ENEMY_STATE currentstate = ENEMY_STATE.PATROL;
        public Health PlayerHealth;                             //Reference to player health	
        public Transform PatrolDestination;                     //Reference to patrol destination
        public float MaxDamage = 10f;                           //Damage amount per second
        private LineSight ThisLineSight;                        //Reference to line of sight component
	private UnityEngine.AI.NavMeshAgent ThisAgent;          //Reference to nav mesh agent
	private Transform ThisTransform;                        //Reference to transform	
	private Transform PlayerTransform;

	void Awake()
	{
		ThisLineSight = GetComponent<LineSight>();
		ThisAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		ThisTransform = GetComponent<Transform>();
		PlayerTransform = PlayerHealth.GetComponent<Transform>();
	}
	
	void Start()
	{
		// Starting state
		CurrentState = ENEMY_STATE.PATROL;
	}
	
	public IEnumerator AIPatrol()
	{
		// PATROL loop
		while(currentstate == ENEMY_STATE.PATROL)
		{
			// Set strict search, only chase when player is in field of view and in plain sight
			ThisLineSight.sensitivity = LineSight.SightSensitivity.STRICT;

			// Move to patrol position
			ThisAgent.Resume();
			ThisAgent.SetDestination(PatrolDestination.position);

			// Wait until path is computed
			while(ThisAgent.pathPending)
				yield return null;

			// If we can see the target then start chasing
			if(ThisLineSight.canSeeTarget)
			{
				ThisAgent.Stop();
				CurrentState = ENEMY_STATE.CHASE;
				yield break;
			}

			// Wait until next frame
			yield return null;
		}
	}
	
	public IEnumerator AIChase()
	{
		// CHASE loop
		while(currentstate == ENEMY_STATE.CHASE)
		{
			// Set loose search
			ThisLineSight.sensitivity = LineSight.SightSensitivity.LOOSE;

			// Chase to last known position
			ThisAgent.Resume();
			ThisAgent.SetDestination(ThisLineSight.lastKnowSighting);

			// Wait until path is computed
			while(ThisAgent.pathPending)
				yield return null;

			// Have we reached destination?
			if(ThisAgent.remainingDistance <= ThisAgent.stoppingDistance)
			{
				// Stop agent
				ThisAgent.Stop();

				// Reached destination but cannot see player, return to PATROL
				if(!ThisLineSight.canSeeTarget)
					CurrentState = ENEMY_STATE.PATROL;
				else // Reached destination and can see player. Reached attacking distance. ATTACK
					CurrentState = ENEMY_STATE.ATTACK;

				yield break;
			}

			// Wait until next frame
			yield return null;
		}
	}
	
	public IEnumerator AIAttack()
	{
		// ATTACK loop
		while(currentstate == ENEMY_STATE.ATTACK)
		{
			// Chase to player position
			ThisAgent.Resume();
			ThisAgent.SetDestination(PlayerTransform.position);

			// Wait until path is computed
			while(ThisAgent.pathPending)
				yield return null;

			// If player got away
			if(ThisAgent.remainingDistance > ThisAgent.stoppingDistance)
			{
				//Change back to chase
				CurrentState = ENEMY_STATE.CHASE;
				yield break;
			}
			else
			{
				//Attack
				PlayerHealth.HealthPoints -= MaxDamage * Time.deltaTime;
			}

			//Wait until next frame
			yield return null;
		}

		yield break;
	}
}
