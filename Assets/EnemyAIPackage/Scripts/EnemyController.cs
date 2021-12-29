using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {
	
	protected Attack attack;
	protected Patrol patrol;
	protected Aggro aggro;
	
	void Awake () {
		attack = GetComponent<Attack>();
		patrol = GetComponent<Patrol>();
		aggro = GetComponent<Aggro>();
	}
	
	// Use this for initialization
	void Start () {
		// start patrolling
		patrol.state = Patrol.patrolState.PATROL;
	}
	
	// Update is called once per frame
	void Update () {
		
		/*----------------------------------------*/
		switch(patrol.state) {
		case Patrol.patrolState.NOPATROL:
			break;
		case Patrol.patrolState.PATROL:
			break;
		default:
			break;
		}
		/*----------------------------------------*/
		switch(attack.state) {
		case Attack.attackState.NOATTACK:
			break;
		case Attack.attackState.ATTACK:
			attack.target = aggro.target;
			break;
		default:
			break;
		}
		/*----------------------------------------*/
		switch(aggro.state) {
		case Aggro.engagementArea.CLEAR:
			patrol.state = Patrol.patrolState.PATROL;
			attack.state = Attack.attackState.NOATTACK;
			break;
		case Aggro.engagementArea.SPHERE:
			patrol.state = Patrol.patrolState.NOPATROL;		// if enemy is in engagement area change the patrol state to NOPATROL
			attack.state = Attack.attackState.ATTACK;		// and enable the ATTACK state
			break;
		case Aggro.engagementArea.CONE:
			patrol.state = Patrol.patrolState.NOPATROL;
			attack.state = Attack.attackState.ATTACK;
			break;
		default:
			break;
		}
		/*----------------------------------------*/
	}
}
