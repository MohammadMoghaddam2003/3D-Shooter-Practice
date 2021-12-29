/*
 *	Author: Anastasios Chouliaropoulos
 * 
 */

using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {
	
	public healthState _state;										
	public enum healthState{ALIVE, DEAD};
	public healthState state {
		get { return _state; }
		set { _state = value; }
	}
	
	public int maxHealth = 5;								// max health
	public int curHealth = 5;								// current health
	public bool canRegenerate = true;						// enable HP regeneration
	public float regenerationCoolDown = 15;
	
	private float regenerationTimer = 0;					// have cooldown on attack
	
	// Update is called once per frame
	void Update () {
		// decrease timer every frame
		if (canRegenerate) {
			if (regenerationTimer > 0)
				regenerationTimer -= Time.deltaTime;
			
			if (regenerationTimer <= 0 && curHealth < maxHealth) {
				adjustCurrentHealth(maxHealth);
				Debug.Log(name + "'s HP regenerated! " + regenerationCoolDown + " seconds passed since last hit");
			}
		}
	}
	
	void knockOut() {
		Debug.Log("Enemy Died");
		state = healthState.DEAD;
	}
	
	void resurrect() {
		curHealth = maxHealth;
		state = healthState.ALIVE;
	}

	/// <summary>
	/// Adjusts the current health.
	/// Call the amount of hp you want to add or remove (+/-)
	/// Enable HP regeneration "canRegenerate" for the enemy to replace HP if is not hit for "regenerationCoolDown" seconds
	/// </summary>
	/// <param name='adj'>
	/// Adj.
	/// </param>
	public void adjustCurrentHealth(int adj) {
		// refresh regenration timer every time it gets hit
		regenerationTimer = regenerationCoolDown;
		
		curHealth += adj;
		
		if (curHealth <= 0) {
			knockOut();
		}
		
		// prevent player's health from going higher than maxHealth
		if (curHealth > maxHealth)
			curHealth = maxHealth;
	}
}
