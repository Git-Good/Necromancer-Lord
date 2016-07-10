﻿using UnityEngine;
using System.Collections;

public class FootmanBehaviour : MonoBehaviour {

	public int health = 300;
	public int mana = 0;
	public int attack = 30;
	public int attackpoint = 1;
	public int movementpoint = 2;
	public int range = 1;
	public int armor = 35;
	public int armorpiercing = 15;
    public int vision = 2;

	public int lasthealth = 300;
	public int currattackpoint;
	public int currmovementpoint;

	public bool idle;
}
