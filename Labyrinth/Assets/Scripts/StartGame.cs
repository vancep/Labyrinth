using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartGame : MonoBehaviour 
{
	public string startScene;

	private Scene scene;

	// Use this for initialization
	void Start () 
	{
		Debug.Log("Scene Count:" + SceneManager.sceneCount);

		SceneManager.LoadScene(startScene);

		scene = SceneManager.GetSceneByName(startScene);
		Debug.Log(scene.name);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
