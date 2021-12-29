/*
 *	Author: Anastasios Chouliaropoulos
 * 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Patrol : MonoBehaviour 
{
	
	public float patrolSpeed = 2f;									// The nav mesh agent's speed when patrolling.
	
	private List<Transform> wpnts = new List<Transform>();
	private int curIndex = 0;
	private int countIndex;
	private UnityEngine.AI.NavMeshAgent _agent;
	
	public Transform[] waypoints;
	/// <summary>
	/// Patrol state. Enables or Disables the enemy patrol between the waypoints
	/// </summary>
	public patrolState _state;										// change to public, to be able to change through the editor
	public enum patrolState{NOPATROL, PATROL};
	public patrolState state {
		get { return _state; }
		set { _state = value; }
	}
	
	void Awake () 
	{
		// inits
		_agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		
		// Keep wpnts on a list
		foreach (Transform tr in waypoints) 
		{
				wpnts.Add(tr);
		}
		
		// cache the number of wpnts
		countIndex = wpnts.Count;
	}
	
	// Update is called once per frame
	void Update () 
	{
		// call patrol if there is no path assigned to the navmeshagent
		if (state == patrolState.PATROL) 
		{
			if (!_agent.hasPath) 
			{
				patrol ();
			}
			// offset between the enemy and the waypoint
			else if (_agent.remainingDistance < 1) 
			{
				_agent.ResetPath();
			}
		}
		else if (state == patrolState.NOPATROL) 
		{
			_agent.ResetPath();
		}
	}
	
	public void patrol() 
	{
		// Set an appropriate speed for the NavMeshAgent.
		_agent.speed = patrolSpeed;
		
		// go to the next waypoint, use of modulo to get the correct numbers on every iteration
		_agent.SetDestination(wpnts[curIndex%countIndex].position);
		curIndex++;
	}
}
