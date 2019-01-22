using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{
	string targetScene = "HelloAR";

	public FadeController m_fader;

	AsyncOperation async;
	IEnumerator Start()
	{ 
		Debug.Log("Async start");
		async = SceneManager.LoadSceneAsync(targetScene);
		async.allowSceneActivation = false;
		yield return async;
	}

	void Update()
	{
		#region
		/*
		if (Input.GetMouseButtonDown(0))   //마우스 좌측 버튼을 누름.
		{
			async.allowSceneActivation = true;
			Debug.Log("Mouse Downed");
		}

		if (Input.touchCount > 0)
		{
			//Vector2 pos = Input.GetTouch(0).position;    // 터치한 위치
			//Vector3 theTouch = new Vector3(pos.x, pos.y, 0.0f);    // 변환 안하고 바로 Vector3로

			if (Input.GetTouch(0).phase == TouchPhase.Began)    // 터치 시작시
			{
				Debug.Log("Touch Began");
			}
			else if (Input.GetTouch(0).phase == TouchPhase.Moved)    // 터치하고 움직일시
			{
				Debug.Log("Touch Moved");
			}
			else if (Input.GetTouch(0).phase == TouchPhase.Ended)    // 터치 뗄시
			{
				async.allowSceneActivation = true;
				Debug.Log("Touch Ended");
			}
		}
		*/
		#endregion
		if (Application.platform == RuntimePlatform.Android)
		{
			if (Input.GetKey(KeyCode.Escape))
			{
				Application.Quit();
				return;
			}
		}
	}

	public void OnClick()
	{
		Debug.Log(this.gameObject.GetComponentInChildren<Text>().text + " Clicked!");
		switch (this.gameObject.GetComponentInChildren<Text>().text)
		{
			case "AR":
				StartCoroutine(Activate());

				break;
		}
	}

	IEnumerator Activate()
	{
		m_fader.FadeIn(2.0f, () => {
			UnityEngine.Debug.Log("Fade In");
			async.allowSceneActivation = true;
		});
		yield return null;
	}
}
