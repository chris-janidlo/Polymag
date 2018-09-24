using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ReplicaText : MonoBehaviour
{

	public Text Primary;

	Text text;

	void Start ()
	{
		text = GetComponent<Text>();
	}
	
	void Update ()
	{
		text.text = Primary.text;
	}
}
