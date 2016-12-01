using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartGame : MonoBehaviour 
{
	public string startScene;

	private Scene scene;
	//private AsyncOperation loadLevel; // might want in future

	// Use this for initialization
	void Start () 
	{
		Debug.Log("Scene Count:" + SceneManager.sceneCount);

		SceneManager.LoadSceneAsync(startScene); // could assign a variable to this if necessary

		scene = SceneManager.GetSceneByName(startScene);

		Debug.Log(scene.name);
	}
	
	// Update is called once per frame
	void Update () 
	{

	}
}
