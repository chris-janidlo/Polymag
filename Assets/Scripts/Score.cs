using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
	public float IncreasePerSecond, Value;

	Text text;

	void Start ()
	{
		text = GetComponent<Text>();
	}
	
	void Update ()
	{
		if (PlayerManager.Instance.Started && !PlayerManager.Instance.Dead)
			Value += IncreasePerSecond * Time.deltaTime;
		text.text = Mathf.Ceil(Value).ToString();
	}
}
