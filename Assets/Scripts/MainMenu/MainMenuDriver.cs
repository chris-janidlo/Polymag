using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuDriver : MonoBehaviour 
{
	public Image FaderImage;
	public float FadeInTime, FadeOutTime;

	void Start () 
	{
		StartCoroutine(enterMainMenu());
	}

	public void StartButton ()
	{
		StartCoroutine(leaveMainMenu());
	}

	public void ExitButton ()
	{
		Application.Quit();
	}

	IEnumerator enterMainMenu () 
	{
		yield return new WaitForEndOfFrame();
		FaderImage.CrossFadeAlpha(0, FadeInTime, true);
	}

	IEnumerator leaveMainMenu () 
	{
		FaderImage.CrossFadeAlpha(1, FadeOutTime, true);
		yield return new WaitForSecondsRealtime(FadeOutTime);
		SceneManager.LoadScene("Endless");
	}
}
