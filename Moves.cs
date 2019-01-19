using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Moves {
	ATTACK = 0,
	MAGIC = 1,
	DEFEND = 2
}

public class Move {
	public static Move ATTACK = new Move(Moves.ATTACK, "HEALTH 0", "#u attacked #e!", (user, target, mult) => {
		var baseDamage = user.attack;
		target.ChangeHealth(Mathf.RoundToInt(-baseDamage * mult));
	});
	public static Move MAGIC = new Move(Moves.MAGIC, "STAMINA 2", "#u used magic on #e!", (user, target, mult) => {
		var baseDamage = user.attack + 2;
		target.ChangeHealth(Mathf.RoundToInt(-baseDamage * mult));
		
		user.ChangeStamina(-2);
	});
	public static Move DEFEND = new Move(Moves.DEFEND, "HEALTH 0", "#u rested!", (user, target, mult) => {
		// Multiplier is never used because DEFEND expects no minigame
		user.ChangeHealth(2);
		user.ChangeStamina(1);
	});

	Moves type;
	string requiredStat;
	int requiredAmount;
	string display;
	Action<Entity, Entity, float> use;

	public Moves Type { get { return type; }}
	public string RequiredStat { get { return requiredStat; }}
	public int RequiredAmount { get { return requiredAmount; }}
	public string Display { get { return display; }}
	public Action<Entity, Entity, float> Use { get { return use; }}

	public Move(Moves _type, string _requirement, string _display, Action<Entity, Entity, float> _use) {
		type = _type;
		requiredStat = _requirement.Split(' ')[0];
		requiredAmount = int.Parse(_requirement.Split(' ')[1]);
		display = _display;
		use = _use;
	}

	public static Move GetMove(Moves move) {
		switch(move) {
			case Moves.ATTACK:
				return Move.ATTACK;
			case Moves.MAGIC:
				return Move.MAGIC;
			case Moves.DEFEND:
				return Move.DEFEND;
		}
		return Move.ATTACK;
	}

	public void UseMoveOn(Entity user, Entity target, float multiplier, TextDisplay textDisplay) {
		if (multiplier == 0)
			textDisplay.UpdateText(user.name + " failed the move!");
		else
			textDisplay.UpdateText(Display.Replace("#u", user.name).Replace("#e", target.name));
		Use(user, target, multiplier);
	}
}
