using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAI : BaseAI
{
	new SpriteRenderer renderer;
	HashSet<GameObject> contacts;

	private void Start()
	{
		base.Start();
		renderer = GetComponent<SpriteRenderer>();
		contacts = new HashSet<GameObject>();
	}

	void Update()
    {
        movement.UpdateMovement(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Kid"))
		{
			contacts.Add(collision.gameObject);
			renderer.color = Color.red;
			StaticHelper.Gameover = true;
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Kid"))
		{
			contacts.Remove(collision.gameObject);
			if (contacts.Count == 0)
				renderer.color = Color.white;
		}
	}
}
