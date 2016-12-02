using UnityEngine;
using System.Collections;

public class SettingsInfo : MonoBehaviour 
{
	private int difficulty;
	private int levelSize;

	// Use this for initialization
	void Start () 
	{
		DontDestroyOnLoad(this);

		difficulty = 0;
		levelSize = 0;
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
		if(s >= 0 && s <= 3)
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
}
