using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

// need to use datatypes that can be stored by PlayerPrefs
struct Entry
{
	public int minutes;
	public int seconds;

	public int day;
	public int month;
	public int year;
}

struct ScoreList
{
	public string name;
	public int timesCompleted;
	public Entry [] entries;

	public ScoreList(string n, int amount)
	{
		name = n;
		timesCompleted = amount;
		entries = new Entry[10];
	}

	public void SetTimesCompleted(int amount)
	{
		timesCompleted = amount;
	}
}

public class PlayerScoresScript : MonoBehaviour 
{
	private GameObject difficultyObj;
	private Dropdown difficultyDropdown;

	private GameObject levelSizeObj;
	private Dropdown levelSizeDropdown;

	private GameObject movingWallsObj;
	private Toggle movingWallsToggle;


	private List<ScoreList> allScores;

	// Use this for initialization
	void Start () 
	{
		DontDestroyOnLoad(this);

		allScores = new List<ScoreList>();

		// initializes each score list
		initScoresList();

		// get stored values if they exist or else add in defaults
		setupStoredScores();
	}	
	
	// Update is called once per frame
	void Update () 
	{

	}

	// have to break this down because other files don't know about my special structs
	public int getMinutes(int modePos, int entryPos)
	{
		return allScores[modePos].entries[entryPos].minutes;
	}

	public int getSeconds(int modePos, int entryPos)
	{
		return allScores[modePos].entries[entryPos].seconds;
	}

	public int getDay(int modePos, int entryPos)
	{
		return allScores[modePos].entries[entryPos].day;
	}

	public int getMonth(int modePos, int entryPos)
	{
		return allScores[modePos].entries[entryPos].month;
	}

	public int getYear(int modePos, int entryPos)
	{
		return allScores[modePos].entries[entryPos].year;
	}

	private void initScoresList()
	{
		foreach(string difficulty in new string[]{"Easy", "Normal", "Hard"})
		{
			foreach(string size in new string[]{"Small", "Medium", "Large"})
			{
				foreach(string moving in new string[]{"Moving", "Frozen"})
				{
					allScores.Add(new ScoreList(difficulty + " " + size + " " + moving, 0));
				}
			}
		}
	}

	private void setupStoredScores()
	{
		for(int i = 0; i < allScores.Count; i++)
		{
			// get times completed
			if(PlayerPrefs.HasKey(allScores[i].name + " Times Completed"))
			{
				allScores[i].SetTimesCompleted(PlayerPrefs.GetInt(allScores[i].name + " Times Completed"));
			}
			else
			{
				allScores[i].SetTimesCompleted(0); 
			}

			// get each entry
			for(int j = 0; j < 10; j++)
			{
				// get minutes
				allScores[i].entries[j].minutes = (PlayerPrefs.HasKey(allScores[i].name + j + "minutes")) ? allScores[i].entries[j].minutes = PlayerPrefs.GetInt(allScores[i].name + j + "minutes") : 0;

				// get seconds
				allScores[i].entries[j].seconds = (PlayerPrefs.HasKey(allScores[i].name + j + "seconds")) ? allScores[i].entries[j].seconds = PlayerPrefs.GetInt(allScores[i].name + j + "seconds") : 0;

				// get day
				allScores[i].entries[j].day = (PlayerPrefs.HasKey(allScores[i].name + j + "day")) ? allScores[i].entries[j].day = PlayerPrefs.GetInt(allScores[i].name + j + "day") : 0;

				// get month
				allScores[i].entries[j].month = (PlayerPrefs.HasKey(allScores[i].name + j + "month")) ? allScores[i].entries[j].month = PlayerPrefs.GetInt(allScores[i].name + j + "month") : 0;

				// get year
				allScores[i].entries[j].year = (PlayerPrefs.HasKey(allScores[i].name + j + "year")) ? allScores[i].entries[j].year = PlayerPrefs.GetInt(allScores[i].name + j + "year") : 0;
			}
		}
	}
}
