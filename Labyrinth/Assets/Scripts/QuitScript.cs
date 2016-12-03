using UnityEngine;
using System.Collections;

public class QuitScript : MonoBehaviour 
{
	void Start()
	{
		Debug.Log("Quitting Application");
		Application.Quit();
	}
}
