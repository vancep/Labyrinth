using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ToMenuScript : MonoBehaviour 
{
	private GameObject settings;

	// Use this for initialization
	void Start () 
	{
		settings = GameObject.FindWithTag("Settings");
		Destroy(settings);

		SceneManager.LoadSceneAsync("_Scenes/Menu");
	}

}
