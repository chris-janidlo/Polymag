using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

	public float IncreasePerSecond, Value;
	
	void Update () {
		// if (PlayerManager.Instance.Started && !PlayerManager.Instance.Dead)
		// 	Value += IncreasePerSecond * Time.deltaTime;
		// GetComponent<Text>().text = Mathf.Ceil(Value).ToString();
	}

}
