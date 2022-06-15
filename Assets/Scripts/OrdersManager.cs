using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class OrdersManager : MonoBehaviour
{
	[SerializeField]
	List<CustomerAI> customers;
	[SerializeField]
	float timeBetweenOrders;

	[SerializeField]
	GameObject firstPLayPanel;
	[SerializeField]
	GameObject gameOverPanel;
	[SerializeField]
	TMPro.TMP_Text scoreText;

	float lastOrderTime;

	#region MONOBEHAVIOUR
	private void Start()
	{
		Time.timeScale = 1f;
		if (StaticHelper.firstPlay)
		{
			Time.timeScale = 0f;
			StaticHelper.firstPlay = false;
			firstPLayPanel.SetActive(true);
		}
	}

	private void OnEnable()
	{
		StaticHelper.scoreValueChanged += StaticHelper_scoreValueChanged;
		StaticHelper.scoreTriggerReached += StaticHelper_scoreTriggerReached;
		StaticHelper.onGameOver += StaticHelper_onGameOver;
	}

	private void OnDisable()
	{
		StaticHelper.scoreValueChanged -= StaticHelper_scoreValueChanged;
		StaticHelper.scoreTriggerReached -= StaticHelper_scoreTriggerReached;
		StaticHelper.onGameOver -= StaticHelper_onGameOver;
	}

	void Update()
	{
		if (Time.time > lastOrderTime + timeBetweenOrders)
		{
			List<CustomerAI> custs = customers.Where(x => x.isWaitingForOrder == false && x.isEating == false).ToList();
			if (custs.Count > 0)
			{
				custs[Random.Range(0, custs.Count)].Order(100);
				lastOrderTime = Time.time;
			}
			else
			{
				StaticHelper.Gameover = true;
			}
		}
	} 
	#endregion

	private void StaticHelper_onGameOver()
	{
		gameOverPanel.SetActive(true);
		Time.timeScale = 0f;
	}

	private void StaticHelper_scoreValueChanged(int value)
	{
		Debug.Log("NEW SCORE : " + value.ToString("000000"));
		scoreText.text = $"SCORE : {value.ToString("000000")}";
	}

	private void StaticHelper_scoreTriggerReached(int value)
	{
		timeBetweenOrders -= value * .3f;
	}

	public void ReloadScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void Play()
	{
		Time.timeScale = 1f;
	}
}
