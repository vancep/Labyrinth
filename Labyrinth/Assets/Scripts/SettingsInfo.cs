using UnityEngine;
using System.Collections;

public class SettingsInfo : MonoBehaviour 
{
	private int difficulty;
	private int levelSize;
	private bool movingWalls;

	public GameObject playersScoresObj;

	private PlayerScoresScript pss;

	// Use this for initialization
	void Start () 
	{
		DontDestroyOnLoad(this);

		pss = UIHelp.getAccessTo<PlayerScoresScript>("PlayerScores");
		if(pss == null)
		{
			Instantiate(playersScoresObj);
			pss = UIHelp.getAccessTo<PlayerScoresScript>("PlayerScores");
			//pss = gameObject.AddComponent<PlayerScoresScript>();
		}

		difficulty = 0;
		levelSize = 0;
		movingWalls = false;
	}

	public void AddScore(System.TimeSpan time)
	{
		pss.AddScore(difficulty, levelSize, movingWalls, time);
	}

	// Update is called once per frame
	void Update () 
	{
		
	}

	/// <summary>
	/// Sets the difficulty.
	/// </summary>
	/// <param name="d">D.</param>
	public void setDifficulty(int d)
	{
		if(d >= 0 && d <= 2)
		{
			difficulty = d;
		}
		else
		{
			Debug.Log("Bad Input For Difficulty");
			difficulty = 0;
		}
	}

	public int getDifficulty()
	{
		return difficulty;
	}

	/// <summary>
	/// Sets the size of the level.
	/// </summary>
	/// <param name="s">S.</param>
	public void setLevelSize(int s)
	{
		if(s >= 0 && s <= 2)
		{
			levelSize = s;
		}
		else
		{
			Debug.Log("Bad Input For Level Size");
			levelSize = 0;
		}
	}

	public int getLevelSize()
	{
		return levelSize; 
	}

	public void setMovingWalls(bool enabled)
	{
		movingWalls = enabled;
	}

	public bool getMovingWalls()
	{
		return movingWalls;
	}


}
