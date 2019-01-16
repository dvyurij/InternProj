using UnityEngine;
using UnityEngine.UI;

public class ButtonControlManager : MonoBehaviour {

	ButtonControlManager() {
		Btntype = ButtonType.NULL;
	}

	public static ButtonType Btntype { get; set; }

	// control button function by button text
	public void OnClick()
	{
		Debug.Log(this.gameObject.GetComponentInChildren<Text>().text + " Clicked!");
		switch (this.gameObject.GetComponentInChildren<Text>().text)
		{
			case "Swim":
				Btntype = ButtonType.Swim;
				break;
			case "Stand":
				Btntype = ButtonType.Stand;
				break;
			case "Jump":
				Btntype = ButtonType.Jump;
				break;
			case "Walk":
				Btntype = ButtonType.Walk;
				break;
			case "Attack":
				Btntype = ButtonType.Attack;
				break;
			case "Remove":
				Btntype = ButtonType.Remove;
				break;
			case "Screenshot":
				Btntype = ButtonType.Screenshot;
				break;
			default:
				break;
		}
	}
}


