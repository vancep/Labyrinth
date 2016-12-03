using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ToMenuScript : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
		SceneManager.LoadSceneAsync("_Scenes/Menu");
	}

}
