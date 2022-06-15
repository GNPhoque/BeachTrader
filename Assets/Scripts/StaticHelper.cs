using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticHelper
{
	private static int playerScore;

	static int lastTriggerIndex;
	private static bool gameover;
	public static bool firstPlay = true;
	
	static List<int> scoreTriggers = new List<int>() { 200, 500, 1200, 1800, 2600, 3500, 5000, 7000, 10000, int.MaxValue };
	public static bool Gameover { get => gameover; set { gameover = value; onGameOver?.Invoke(); } }

	public static event Action<int> scoreValueChanged;
	public static event Action<int> scoreTriggerReached;
	public static event Action onGameOver;

	public static int PlayerScore 
	{ 
		get => playerScore; 
		set 
		{ 
			playerScore = value; 
			if(scoreTriggers[lastTriggerIndex]<value)
			{
				Debug.Log($"Score trigger reached {scoreTriggers[lastTriggerIndex]}");
				scoreTriggerReached?.Invoke(lastTriggerIndex++);
			}
			scoreValueChanged?.Invoke(value);
		} 
	}

}
