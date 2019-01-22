using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayCanvasSceneButtonManager : MonoBehaviour
{

	// Use this for initialization
	void Start()
	{

	}

	public void OnClick()
	{
		Debug.Log(this.gameObject.GetComponentInChildren<Text>().text + " Clicked!");
		switch (this.gameObject.GetComponentInChildren<Text>().text)
		{
			case "AR":
				
				break;

		}
	}
}
