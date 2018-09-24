using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleTextFade : MonoBehaviour 
{
	public float TransitionTime;

	private Text text;

	void Start () 
	{
		text = GetComponent<Text>();
		text.canvasRenderer.SetAlpha(1);
	}
	
	void Update () 
	{
		if (text.canvasRenderer.GetColor().a == 0)
			text.CrossFadeAlpha(.99f, TransitionTime, true);
		if (text.canvasRenderer.GetColor().a == .99f)
			text.CrossFadeAlpha(1, TransitionTime / 2, true);
		if (text.canvasRenderer.GetColor().a == 1)
			text.CrossFadeAlpha(.01f, TransitionTime, true);
		if (text.canvasRenderer.GetColor().a == .01f)
			text.CrossFadeAlpha(0, TransitionTime / 2, true);
	}
}
