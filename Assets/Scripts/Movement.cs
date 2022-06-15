using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    float forceMultiplier;
    [SerializeField]
    float speed;

    Vector2 inputDirection;
    Rigidbody2D rb;
    new Transform transform;

	private void Start()
	{
        transform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate()
    {
        var targetVelocity = inputDirection * speed; 
        var difference = targetVelocity - rb.velocity;
        rb.AddForce(difference * forceMultiplier);
    }

    public void UpdateMovement(Vector2 input)
    {
        inputDirection = input;
    }

    public void UpdatePosition(Vector2 input)
	{
        transform.position = input;
	}
}
