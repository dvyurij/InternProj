using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour {

	Animator animator;
	int atkhash;

	private void OnMouseDown()
	{
		animator.SetTrigger(atkhash);
		//Debug.Log("Clicked!");
		//animator.ResetTrigger(atkhash);
	}
	
	// Use this for initialization
	void Start () {
		animator = GameObject.Find("Dinosaurus_attack").GetComponent<Animator>();
		atkhash = Animator.StringToHash("Attack");
	}

	// Update is called once per frame
	/*
	void Update () {
		
	}
	*/
}
