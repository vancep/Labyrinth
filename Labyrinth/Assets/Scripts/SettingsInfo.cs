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
		DontDestroyOnLoad(this); // want to keep this between scenes

		pss = UIHelp.getAccessTo<PlayerScoresScript>("PlayerScores");
		if(pss == null)
		{
			Instantiate(playersScoresObj);
			Debug.Log("Instantiated PlayerScoresObj");
			pss = UIHelp.getAccessTo<PlayerScoresScript>("PlayerScores");
		}

		// set defaults
		difficulty = 0;
		levelSize = 0;
		movingWalls = false;
	}

	/// <summary>
	/// Adds the score based on current game configuration and the time it took to complete level
	/// </summary>
	/// <param name="time">Time.</param>
	public void AddScore(System.TimeSpan time)
	{
		Debug.Log("Wanting To Add Score With Time: " + time.Minutes + ":" + time.Seconds);
		pss.AddScore(difficulty, levelSize, movingWalls, time);
	}

	/// <summary>
	/// Tells the PlayerScoresScript to save the scores to PlayerPrefs.
	/// Dont confuse this with calling PlayerPrefs.Save()
	/// </summary>
	public void SaveScores()
	{
		pss.SaveScores();
	}
		
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
