﻿using UnityEngine;

public class PenguinBehaviour : MonoBehaviour {

	public static int Pengnum = 0;

	private Animation anim;

	void Start()
	{
		anim = gameObject.GetComponent<Animation>();
		anim["swim"].layer = 1;
		anim["stand"].layer = 1;
		anim["jump"].layer = 1;
		anim["walk"].layer = 1;
		anim["attack"].layer = 1;

		anim.Play("stand");
		Pengnum++;
	}

	void Update()
	{
		if (ButtonControlManager.Btntype == ButtonType.NULL)
		{
			return;
		}


		Debug.Log(ButtonControlManager.Btntype.ToString());
		switch (ButtonControlManager.Btntype)
		{
			case ButtonType.Swim:
				anim.Play("swim");
				break;
			case ButtonType.Stand:
				anim.Play("stand");
				break;
			case ButtonType.Jump:
				anim.Play("jump");
				break;
			case ButtonType.Walk:
				anim.Play("walk");
				break;
			case ButtonType.Attack:
				anim.Play("attack");
				break;
			case ButtonType.Remove:
				Destroy(GameObject.Find("Anchor"));
				GameObject.Find("ButtonContainer").SetActive(false);
				break;
		}
		ButtonControlManager.Btntype = ButtonType.NULL;
	}

	private void OnDestroy()
	{
		Debug.Log("PenguinBehaviour script was destroyed");
		Pengnum--;
	}
}