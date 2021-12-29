/*
 *	Authors: Argiro Georgopoulou
 *			Anastasios Chouliaropoulos
 * 
 */

using UnityEngine;
using System.Collections;

public class Aggro : MonoBehaviour {
	
	public bool enableSphereDetection = true;				// Enable detection in a SPHERE around the enemy
	public bool enableConeDetection = true;					// Enable CONE detection in front of the enemy
	public float sphereDetectionRange = 2;					// Ranged of SPHERE detection
	public float coneDetectionRange = 5;					// Ranged of CONE detection
	public float maxChaseDistance = 10;						// max distance between the enemy and the target
	public float maxAwayDistance = 15;						// max distance that the enemy can go away from its initial position
	public string tagOfTarget = "Player";					// Targets with this tag will trigger the engagement area of the enemy
	
	/// <summary>
	/// Defines the State of the engagement area.
	/// Different AI can be coded using this state.
	/// e.g. when it's SPHERE and Player is Running trigger Enemy ATTACK, if Player is Walking do nothing.
	/// </summary>
	public engagementArea _state;							// change to public, to be able to change through the editor
	public enum engagementArea{CLEAR, SPHERE, CONE};
	public engagementArea state {
		get { return _state; }
		set { _state = value; 
			if (value == engagementArea.CLEAR)
				target = null;
		}
	}
	
	/// <summary>
	/// Target of the enemy.
	/// </summary>
	private GameObject _target;
	public GameObject target {
		get { return _target; }
		set { _target = value; }
	}
	
	private Transform _transform;
	private Vector3 _initialPosition;
	
	void Awake () {
		_transform = GetComponent<Transform>();
		_initialPosition = _transform.position;									// cache the initial position
	}
	
	void Update () {
		if (enableSphereDetection || enableConeDetection) {
			if (enableSphereDetection)
				radiusDetection (_transform.position, sphereDetectionRange);	// cast sphere detection
			if (enableConeDetection)
				coneDetection (_transform.position, coneDetectionRange);		// cast cone detection
			// check chase and away distance
			checkChaseDistance();
			checkAwayDistance();
		}
	}
	
	/// <summary>
	/// Detection in a SPHERE around the enemy.
	/// </summary>
	/// <param name='center'>
	/// Origin of the detection.
	/// </param>
	/// <param name='radius'>
	/// Max radious of the detection
	/// </param>
	void radiusDetection (Vector3 center, float radius) {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);	// creates a sphere where the player can be detected
		foreach(Collider col in hitColliders){
			if(col.gameObject.tag == tagOfTarget){							// if the player is within the radius we have a target
				target = col.gameObject;									// cache target
				state = engagementArea.SPHERE;								// change the state
			}
		}
	}
	
	/// <summary>
	/// CONE detection in front of the enemy.
	/// </summary>
	/// <param name='center'>
	/// Origin of the detection.
	/// </param>
	/// <param name='radius'>
	/// Max distance of the detection
	/// </param>
	void coneDetection ( Vector3 center, float radius) {
		Collider[] hitColliders = Physics.OverlapSphere(center, radius);	// creates a sphere where the player can be detected
		foreach(Collider col in hitColliders){
			if(col.gameObject.tag == tagOfTarget){							// if the player is within the radius we have a target
				target = col.gameObject;									// cache target
			}
		}
		
		float cone;
		cone = Mathf.Cos(30 * Mathf.Deg2Rad);
		//if player is within the cone viw of the enemy
		if((target != null) && Vector3.Dot(_transform.forward, (target.transform.position-_transform.position).normalized) > cone){
			//if the player is close to the enemy
			if(Vector3.Distance(_transform.position, target.transform.position) < radius){
				state = engagementArea.CONE;
			}
		}
	}
	
	/// <summary>
	/// Checks the chase distance. If the target is far from the enemy state is set back to CLEAR
	/// </summary>
	void checkChaseDistance () {
		if (target) {
			float distBetween = Vector3.Distance(_transform.position, target.transform.position);
			if (distBetween > maxChaseDistance) {
				state = engagementArea.CLEAR;
			}
		}
	}
	
	/// <summary>
	/// Checks the distance away from the initial position of the enemy.
	/// </summary>
	void checkAwayDistance () {
		float distBetween = Vector3.Distance(_transform.position, _initialPosition);
		if (distBetween > maxAwayDistance) {
			state = engagementArea.CLEAR;
		}
	}
	
	//======================================================================//
	//================== CODE BELOW IS USED FOR DEBUGGING ==================//
	//======================================================================//
	void OnDrawGizmos(){
		Gizmos.color = Color.blue;
    	if (enableSphereDetection) Gizmos.DrawWireSphere (transform.position, sphereDetectionRange);				//Draws a blue sphere to see the sphereDetectionRange
		Gizmos.color = Color.green;
		if (enableConeDetection) Gizmos.DrawRay (transform.position, transform.forward * coneDetectionRange);		//Draws a green line to see the coneDetectionRange
		
		if (state == engagementArea.CONE || state == engagementArea.SPHERE) {
			Gizmos.color = Color.red;
			Gizmos.DrawRay (transform.position, transform.forward * maxChaseDistance);								//Draws a red line to see the max chase distance
		}
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(_initialPosition, maxAwayDistance);
	}
}
