using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public abstract class BaseAI : MonoBehaviour
{
	protected Movement movement;
	protected void Start()
	{
		movement = GetComponent<Movement>();
	}
}
