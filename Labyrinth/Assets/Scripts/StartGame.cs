using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class StartGame : MonoBehaviour 
{
	public string startScene;

	private Scene scene;

	private GameObject difficultyObj;
	private Dropdown difficultyDropdown;

	private GameObject levelSizeObj;
	private Dropdown levelSizeDropdown;

	private GameObject movingWallsObj;
	private Toggle movingWallsToggle;

	private SettingsInfo settingsInfo;

	// Use this for initialization
	void Start () 
	{

		// get access to value for difficulty
		difficultyObj = GameObject.FindWithTag("Difficulty");

		if(difficultyObj != null)
		{
			difficultyDropdown = difficultyObj.GetComponent<Dropdown>();
		}

		// get access to value for level size
		levelSizeObj = GameObject.FindWithTag("LevelSize");

		if(levelSizeObj != null)
		{
			levelSizeDropdown = levelSizeObj.GetComponent<Dropdown>();
		}

		// get access to value for moving walls
		movingWallsObj = GameObject.FindWithTag("MovingWalls");

		if(movingWallsObj != null)
		{
			movingWallsToggle = movingWallsObj.GetComponent<Toggle>();
		}

		// get access to object that holds the settings
		settingsInfo = Object.FindObjectOfType<SettingsInfo>();

		// store difficulty settings
		if(difficultyDropdown != null && settingsInfo != null)
		{
			settingsInfo.setDifficulty(difficultyDropdown.value);
		}
		else
		{
			Debug.Log("Could Not Find Either Difficulty Dropdown Object Or The Settings Info Object");
			settingsInfo.setDifficulty(0);
		}

		// store level size settings
		if(levelSizeDropdown != null && settingsInfo != null)
		{
			settingsInfo.setLevelSize(levelSizeDropdown.value);
		}
		else
		{
			Debug.Log("Could Not Find Either Level Size Dropdown Object Or The Settings Info Object");
			settingsInfo.setLevelSize(0);
		}

		// store moving wall settings
		if(movingWallsToggle != null && settingsInfo != null)
		{
			settingsInfo.setMovingWalls(movingWallsToggle.isOn);
		}
		else
		{
			Debug.Log("Could Not Find Either Moving Walls Object Or The Settings Info Object");
			settingsInfo.setMovingWalls(false);
		}


		SceneManager.LoadSceneAsync(startScene); // could assign a variable to this if necessary

		scene = SceneManager.GetSceneByName(startScene);

		Debug.Log(scene.name);
	}
	
	// Update is called once per frame
	void Update () 
	{

	}
}
