using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerAI : BaseAI
{
	[SerializeField]
	TMPro.TMP_Text orderTimeText;
    [SerializeField]
    GameObject orderBubble;

	int maxScore;

	float currentOrderWaitingTime;
	public bool isWaitingForOrder;
	public bool isEating;
	float scoreMultiplyer = 1f;
	float orderWaitingTime;

	private void OnEnable()
	{
		StaticHelper.scoreTriggerReached += StaticHelper_scoreTriggerReached;
	}

	private void OnDisable()
	{
		StaticHelper.scoreTriggerReached -= StaticHelper_scoreTriggerReached;
	}

	private void StaticHelper_scoreTriggerReached(int value)
	{
		scoreMultiplyer += value * .3f;
	}

	private void Update()
	{
		if (isWaitingForOrder)
		{
			currentOrderWaitingTime -= Time.deltaTime;
			if (currentOrderWaitingTime <= 0f) 
				StaticHelper.Gameover = true;
			orderTimeText.text = Mathf.CeilToInt(currentOrderWaitingTime).ToString();
		}
	}

	public void Order(int score, float time)
	{
		orderWaitingTime = time;
        orderBubble.SetActive(true);
        isWaitingForOrder = true;
		currentOrderWaitingTime = orderWaitingTime;
		maxScore = Mathf.FloorToInt(score * scoreMultiplyer);
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		if (isWaitingForOrder)
		{
			Debug.Log("Slurrp");
			orderBubble.SetActive(false);
			StaticHelper.PlayerScore += GetScore();
			Debug.Log($"SCORE : {StaticHelper.PlayerScore}");
			isWaitingForOrder = false;
			isEating = true;
			StartCoroutine(Eat());
		}
	}

	int GetScore()
	{
		float timeRatio = currentOrderWaitingTime / orderWaitingTime;
		if (timeRatio > .8f)
		{
			return maxScore;
		}
		if (timeRatio < .2f)
		{
			return Mathf.RoundToInt(maxScore * .2f);
		}
		return Mathf.RoundToInt(timeRatio * maxScore);
	}

	IEnumerator Eat()
	{
		yield return new WaitForSeconds(1f);
		isEating = false;
	}
}
