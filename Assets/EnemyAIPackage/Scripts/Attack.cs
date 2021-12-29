/*
 *	Author: Anastasios Chouliaropoulos
 * 
 */

using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour {
	
	private Transform _transform;
	protected UnityEngine.AI.NavMeshAgent _agent;
	private Animator anim;	/// mecanim
	private HashIDs hash;
	
	/// <summary>
	/// Defines the Type of the enemy attack. Can be adjusted by the inspector.
	/// </summary>
	private static int MELEE;
	private static int RANGED;
	public attackType _type;										
	public enum attackType{MELEE, RANGED};
	public attackType type {
		get { return _type; }
		set { _type = value; }
	}
	
	/// <summary>
	/// State of attack. Enables or Disables the enemy attack. Used by other scripts.
	/// Calls the appropriate functionality depending on the type of attack
	/// </summary>
	public attackState _state;											// change to public, to be able to change through the editor
	public enum attackState{NOATTACK, ATTACK};
	public attackState state {
		get { return _state; }
		set { _state = value; 
			if (value == attackState.NOATTACK) {
				target = null;
				StopCoroutine("rangedAttack");
			}
		}
	}
	
	/// <summary>
	/// Target of the enemy. Defined by external script e.g. aggro
	/// </summary>
	private GameObject _target;
	public GameObject target {
		get { return _target; }
		set { _target = value; }
	}
	
	/* ---------------Melee inits-------------------- */
	public float chaseSpeed = 5f;										// The nav mesh agent's speed when chasing.
	public float maxMeleeDistance = 2;
	public float meleeAttackCoolDown = 2;
	private float attackTimer;
	/* --------------Ranged inits-------------------- */
	public Transform fireProjectileAnchor;
	public GameObject projectile;
	public GameObject marker;
	public float reachTime = 1.5f;
	public float verticalOnlyMin = 0.5f;
	public float rangedAttackCoolDown = 1;
	public float shootDelay = 2;
	private bool isShooting = false;
	
	void Awake () {
		// cache the components
		_transform = GetComponent<Transform>();
		_agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		anim = GetComponent<Animator>();	/// mecanim
		hash = GameObject.FindGameObjectWithTag(Tags.gameManager).GetComponent<HashIDs>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		// decrease timer every frame. Used for the cooldown in the enemy attack
		if (attackTimer > 0)
			attackTimer -= Time.deltaTime;
		
		if (state == attackState.NOATTACK) {
			anim.SetBool(hash.shootBool, false);	/// mecanim
			anim.SetBool(hash.punchBool, false);	/// mecanim
		}
		else if (state == attackState.ATTACK) {
			if ( target )
			{	
				// Don't rotate him on Y
				Vector3 targetPosition = new Vector3(target.transform.position.x, this.transform.position.y, target.transform.position.z);
				if (type == attackType.MELEE) 
				{
					// rotate towards the target
					if (!isLookingAtTarget(target))
						_transform.LookAt(targetPosition);
					
					if ( isAwayFromTarget(target) ) 
					{
						chaseTarget();
					}
					else 
					{
						// attack when timer is less than 0
						if ( attackTimer <= 0 )
							meleeAttack(target);
					}
				}
				else if (type == attackType.RANGED)
				{
					if (!isShooting) 
					{
						_transform.LookAt(targetPosition);
						StartCoroutine(rangedAttack(target.transform));
					}
				}
			}
		}
	}
	
	/// <summary>
	/// Chases the target. Used when type is melee attack.
	/// </summary>
	/// <param name='target'>
	/// Target.
	/// </param>
	void chaseTarget() {
		
		// Set the appropriate speed for the NavMeshAgent.
		_agent.speed = chaseSpeed;
		
		// Move towards target
		_agent.SetDestination(target.transform.position);
		anim.SetBool(hash.shootBool, false);	/// mecanim
		anim.SetBool(hash.punchBool, false);	/// mecanim
		Debug.Log("Chasing");
	}
	
	/// <summary>
	/// Functionality for the melee attack of the enemy
	/// </summary>
	/// <param name='target'>
	/// Target.
	/// </param>
	void meleeAttack (GameObject target) {
		// reset timer on attack
		attackTimer = meleeAttackCoolDown;
		
		anim.SetBool(hash.punchBool, true);	/// mecanim
		Debug.Log("Attacking");
	}
	
	/// <summary>
	/// Functionality for the ranged attack of the enemy.
	/// </summary>
	/// <returns>
	/// The attack.
	/// </returns>
	/// <param name='targetTr'>
	/// Target tr.
	/// </param>
	IEnumerator rangedAttack (Transform targetTr) {
		// lock coroutine
		isShooting = true;
		Vector3 tempTr = targetTr.position;
		Debug.Log("I'm about to shoot in " + shootDelay + " sec...");
		// instantiate marker
		GameObject temp = Instantiate(marker, new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z), marker.transform.rotation) as GameObject;
		yield return new WaitForSeconds(shootDelay);
		
		anim.SetBool(hash.shootBool, true);	/// mecanim
		// instantiate projectile
		GameObject ball = Instantiate(projectile, fireProjectileAnchor.position, fireProjectileAnchor.rotation) as GameObject;
		ball.GetComponent<Rigidbody>().velocity = CalculateVelocity(tempTr);
		Destroy(ball, 5);
		Debug.Log("...Fired!");
		// destroy marker
		Destroy(temp);
		yield return new WaitForSeconds(rangedAttackCoolDown);
		// unlock coroutine
		isShooting = false;
	}
	
	/// <summary>
	/// Checks if enemy is away from target.
	/// </summary>
	/// <returns>
	/// True or false, depending if it is away or not.
	/// </returns>
	/// <param name='target'>
	/// If set to <c>true</c> target.
	/// </param>
	bool isAwayFromTarget (GameObject target) {
		// distance between target and enemy
		float distBetween = Vector3.Distance(_transform.position, target.transform.position);

		// Is away from target?
		if (distBetween > maxMeleeDistance) return true;
		else return false;
	}
	
	/// <summary>
	/// Checks if enemy is in front of the target.
	/// </summary>
	/// <returns>
	/// TRUE if enemy is in front of the target.
	/// FALSE if not.
	/// </returns>
	/// <param name='target'>
	/// Target.
	/// </param>
	bool isLookingAtTarget (GameObject target) {
		// direction between target and enemy
		Vector3 dir = (target.transform.position - _transform.position).normalized;
		float direction = Vector3.Dot(dir, _transform.forward);
		if (direction > 0) return true;
		else return false;
	}
	
	/// <summary>
	/// Calculates the velocity. Used for the trajectory of projectile for the ranged attack
	/// </summary>
	/// <returns>
	/// The velocity.
	/// </returns>
	/// <param name='target'>
	/// Target.
	/// </param>
	Vector3 CalculateVelocity(Vector3 target) {
		Vector3 direction = (target - fireProjectileAnchor.position);
		float gravity = Physics.gravity.magnitude;
		float yVelocity = (direction.y / reachTime) + (0.5f * gravity * reachTime);
		
		if(TargetTooClose(target)) {
			return new Vector3(0, yVelocity, 0);
		} else {
			return new Vector3(direction.x / reachTime, yVelocity, direction.z / reachTime);
		}
	}
	
	bool TargetTooClose(Vector3 target){
		Vector3 targetPosition = target;
		Vector3 leveledTarget = new Vector3(targetPosition.x, fireProjectileAnchor.position.y, targetPosition.z);
		return Vector3.Distance(leveledTarget, fireProjectileAnchor.position) <= verticalOnlyMin;
	}
	
	//======================================================================//
	//================== CODE BELOW IS USED FOR DEBUGGING ==================//
	//===================== DRAWS the TRAJECTORY ARROW =====================//
	//======================================================================//
	void OnDrawGizmos() {
		if (state == attackState.ATTACK) {
			if (!target) return;
			Vector3 initialVelocity = CalculateVelocity(target.transform.position);
			float deltaTime = reachTime / initialVelocity.magnitude;
			int drawSteps = (int)(initialVelocity.magnitude - 0.5f);
			Vector3 currentPosition = fireProjectileAnchor.position;
			Vector3 previousPosition = currentPosition;
			Gizmos.color = Color.red;
			
			if(IsParabolicVelocity(initialVelocity)) {
				for(int i = 0; i < drawSteps; i++) {
					currentPosition += (initialVelocity * deltaTime) + (0.5f * Physics.gravity * deltaTime * deltaTime);
					initialVelocity += Physics.gravity * deltaTime;
					Gizmos.DrawLine(previousPosition, currentPosition);
					//////////////////////////////////////////////////////////////////////////////////
					// If the next loop is the last iteration, then don't update the previous position
					// vector so it can be used to draw the gizmos arrow.
					if((i+1) < drawSteps) {
						previousPosition = currentPosition;
					}
				}
				DrawArrow(previousPosition, (currentPosition - previousPosition));
			} else {
				Vector3 newUpDirection = new Vector3(currentPosition.x, target.transform.position.y, currentPosition.z);
				Gizmos.DrawLine(currentPosition, newUpDirection);
				DrawArrow(newUpDirection, new Vector3(0f, 0.01f, 0f));
			}
		}
	}
	
	void DrawArrow(Vector3 position, Vector3 direction) {
		int[] arrowAngles = new int[] { 225, 135 };
		foreach(int angle in arrowAngles) {
			Vector3 endPoint = Quaternion.LookRotation(direction) * Quaternion.Euler(0, angle, 0) * Vector3.forward;
			Gizmos.DrawRay(position + direction, endPoint * 0.5f);
		}
	}

	bool IsParabolicVelocity(Vector3 velocity) {
		return !(velocity.x == 0 && velocity.z == 0);
	}
}
