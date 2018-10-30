using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class TextUI : MonoBehaviour
{
	void OnEnable ()
	{
		GetComponent<Toggle>().isOn = PlayerManager.SkipIntro;
	}

	public void SetSkipIntro (bool value)
	{
		PlayerManager.SkipIntro = value;
	}
}
