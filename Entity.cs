using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {

	public GameObject barPanel;
	Bar healthBar;
	Bar staminaBar;

	public int maxHealth;
	int health;
	public int maxStamina;
	int stamina;

	public bool IsDead { get { return health == 0; }}

	public string name;
	public int attack;

	void Start () {
		var bars = barPanel.GetComponentsInChildren<Bar>();
		healthBar = bars[0];
		staminaBar = bars[1];

		health = maxHealth;
		stamina = maxStamina;
	}
	
	public Move MakeChoice () {
		if (health <= maxHealth / 4 || stamina == 0)
			return Move.DEFEND;
		else if (stamina >= Move.MAGIC.RequiredAmount)
			return Move.MAGIC;
		else
			return Move.ATTACK;
	}

	public bool CanUseMove(Move move) {
		if (move.RequiredStat == "HEALTH")
			return health >= move.RequiredAmount;
		else if (move.RequiredStat == "STAMINA")
			return stamina >= move.RequiredAmount;
		
		return false;
	}

	public void ChangeHealth (int amount) {
		health = Mathf.Clamp(health + amount, 0, maxHealth);
		healthBar.SetValue((float)health / (float)maxHealth);
	}

	public void ChangeStamina (int amount) {
		stamina = Mathf.Clamp(stamina + amount, 0, maxStamina);
		staminaBar.SetValue((float)stamina / (float)maxStamina);
	}
}
