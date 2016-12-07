using UnityEngine;
using System.Collections;

public class QuitScript : MonoBehaviour 
{
	void Start()
	{
		Debug.Log("Quitting Application");
		PlayerPrefs.Save();
		Application.Quit();
	}
}
