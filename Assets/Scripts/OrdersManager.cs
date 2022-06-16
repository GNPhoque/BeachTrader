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
	CustomerPatience[] originalCustomerPatiences;
	[SerializeField]
	float timeBetweenOrders;

	[SerializeField]
	GameObject firstPLayPanel;
	[SerializeField]
	GameObject gameOverPanel;
	[SerializeField]
	TMPro.TMP_Text scoreText;

	float lastOrderTime;
	[SerializeField]
	Queue<CustomerPatience> availableCustomerPatiences;

	#region MONOBEHAVIOUR
	private void Start()
	{
		availableCustomerPatiences = new Queue<CustomerPatience>();
		RefillQueue();
		Time.timeScale = 1f;
		if (StaticHelper.firstPlay)
		{
			Time.timeScale = 0f;
			StaticHelper.firstPlay = false;
			firstPLayPanel.SetActive(true);
		}
		customers.AddRange(FindObjectsOfType<CustomerAI>());
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
				Debug.Log(string.Join(",", availableCustomerPatiences));
				custs[Random.Range(0, custs.Count)].Order(100, GetOrderTime(availableCustomerPatiences.Dequeue()));
				lastOrderTime = Time.time;
				if (availableCustomerPatiences.Count == 0) 
					RefillQueue();
			}
			//else
			//{
			//	StaticHelper.Gameover = true;
			//}
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
		timeBetweenOrders -= .3f;
		Debug.Log($"TimeBetweenChange : {timeBetweenOrders}");
	}

	void RefillQueue()
	{
		List<CustomerPatience> tmpCustomerPatiences = new List<CustomerPatience>();
		tmpCustomerPatiences.AddRange(originalCustomerPatiences);

		for (int i = 0; i < originalCustomerPatiences.Length; i++)
		{
			int rng = Random.Range(0, tmpCustomerPatiences.Count - 1);
			availableCustomerPatiences.Enqueue(tmpCustomerPatiences[rng]);
			tmpCustomerPatiences.RemoveAt(rng);
		}
	}

	float GetOrderTime(CustomerPatience patience)
	{
		switch (patience)
		{
			case CustomerPatience.Impatient:
				return 6f;
			case CustomerPatience.Normal:
				return 9f;
			case CustomerPatience.Patient:
				return 12f;
			default:
				return 0f;
		}
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
