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
	private Dropdown difficultyDropdown;
	private Dropdown levelSizeDropdown;
	private Toggle movingWallsToggle;
	private Text scoresText;

	private List<ScoreList> allScores; 

	// Use this for initialization
	void Start () 
	{
		DontDestroyOnLoad(this);

		allScores = new List<ScoreList>();

		// initializes each score list for the variable allScores
		initScoresList();

		// get stored values if they exist or else add in defaults
		SetupStoredScores();
	}	

	public string getScores(int diff, int size, bool toggle)
	{
		int modePos = (6 * diff) + (2 * size) + (toggle? 1:0);

		string scoresText = "Times Completed: "  + getTimesCompleted(modePos) + "\n\n";
		scoresText += "Best Times:\n";

		for(int i = 0; i < 10; i++)
		{
			scoresText +=	i + ". " + getMinutes(modePos, i) + ":" + getSeconds(modePos, i) + " on " + getMonth(modePos, i) + "/" + getDay(modePos, i) + "/" + getYear(modePos, i) + "\n";
		}

		return scoresText;
	}

	/// <summary>
	/// Allows this class to access the UI elements that are holding the current gamemode combination.
	/// Needs to be called at some point after they exist and before this class tries to access them.
	/// </summary>
	private void InitAccess()
	{
		if(difficultyDropdown == null)
		{
			difficultyDropdown = UIHelp.getAccessTo<Dropdown>("HsDifficulty");
		}

		if(levelSizeDropdown == null)
		{
			levelSizeDropdown = UIHelp.getAccessTo<Dropdown>("HsSize");
		}

		if(movingWallsToggle == null)
		{
			movingWallsToggle = UIHelp.getAccessTo<Toggle>("HsMoving");
		}

		if(scoresText == null)
		{
			scoresText = UIHelp.getAccessTo<Text>("ScoresText");
		}
	}

	/// <summary>
	/// Updates the scoresText object text field with the most current highscores.
	/// </summary>
	public void UpdatePlayerScores()
	{
		InitAccess();

		Debug.Log("UpdatePlayerScores() " + difficultyDropdown.value + " " + levelSizeDropdown.value + " " + (movingWallsToggle.isOn? 1:0));

		scoresText.text = getScores(difficultyDropdown.value, levelSizeDropdown.value, movingWallsToggle.isOn);
	}

	// have to break this down because other files don't know about my special structs
	private int getMinutes(int modePos, int entryPos)
	{
		return allScores[modePos].entries[entryPos].minutes;
	}

	private int getSeconds(int modePos, int entryPos)
	{
		return allScores[modePos].entries[entryPos].seconds;
	}

	private int getDay(int modePos, int entryPos)
	{
		return allScores[modePos].entries[entryPos].day;
	}

	private int getMonth(int modePos, int entryPos)
	{
		return allScores[modePos].entries[entryPos].month;
	}

	private int getYear(int modePos, int entryPos)
	{
		return allScores[modePos].entries[entryPos].year;
	}

	private int getTimesCompleted(int modePos)
	{
		return allScores[modePos].timesCompleted;
	}

	private void incrementTimesCompleted(int modePos)
	{
		allScores[modePos].SetTimesCompleted(allScores[modePos].timesCompleted + 1);
	}

	/// <summary>
	/// Initializes the scores list to a list of 18 ScoreLists.
	/// One ScoreList for each possible gamemode combination.
	/// </summary>
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

	/// <summary>
	/// Adds the score to allScores which holds everything until it gets saved to PlayerPrefs.
	/// </summary>
	/// <param name="diff">Diff.</param>
	/// <param name="size">Size.</param>
	/// <param name="toggle">If set to <c>true</c> toggle.</param>
	/// <param name="time">Time.</param>
	public void AddScore(int diff, int size, bool toggle, System.TimeSpan time)
	{
		int modePos = (6 * diff) + (2 * size) + (toggle? 1:0); // Determines where in the list to insert new data
		int minutes = time.Minutes;
		int seconds = time.Seconds;

		// get current date to store with high score
		System.DateTime date;
		date = System.DateTime.Now;

		// updates the number of times the gamemode combination has been played
		incrementTimesCompleted(modePos);

		// find the right place to insert new score and shift other scores accordingly
		for(int i = 0; i < 10; i++)
		{
			if(minutes <= getMinutes(modePos, i))
			{
				if(seconds < getSeconds(modePos, i))
				{
					for(int j = 9; j > i; j--)
					{
						allScores[modePos].entries[j].minutes = allScores[modePos].entries[j - 1].minutes;
						allScores[modePos].entries[j].seconds = allScores[modePos].entries[j - 1].seconds;
						allScores[modePos].entries[j].month = allScores[modePos].entries[j - 1].month;
						allScores[modePos].entries[j].day = allScores[modePos].entries[j - 1].day;
						allScores[modePos].entries[j].year = allScores[modePos].entries[j - 1].year;

					}

					allScores[modePos].entries[i].minutes = minutes;
					allScores[modePos].entries[i].seconds = seconds;
					allScores[modePos].entries[i].month = date.Month;
					allScores[modePos].entries[i].day = date.Day;
					allScores[modePos].entries[i].year = date.Year;

					Debug.Log("Score Added: " + minutes + ":" + seconds);
					break;
				}
			}
		}
	}

	/// <summary>
	/// Saves the scores to PlayerPrefs.
	/// Note, still need to call PLayerPrefs.Save to or wait for OnApplicationQuit() to write prefs to disk
	/// </summary>
	public void SaveScores()
	{
		Debug.Log("Saving Scores");

		for(int i = 0; i < 18; i++)
		{
			// save times completed
			PlayerPrefs.SetInt(allScores[i].name + "Times Completed", allScores[i].timesCompleted);

			// save each entry
			for(int j = 0; j < 10; j++)
			{
				PlayerPrefs.SetInt(allScores[i].name + j + "minutes", allScores[i].entries[j].minutes);
				PlayerPrefs.SetInt(allScores[i].name + j + "seconds", allScores[i].entries[j].seconds);
				PlayerPrefs.SetInt(allScores[i].name + j + "day", allScores[i].entries[j].day);
				PlayerPrefs.SetInt(allScores[i].name + j + "month", allScores[i].entries[j].month);
				PlayerPrefs.SetInt(allScores[i].name + j + "year", allScores[i].entries[j].year);
			}
		}
	}

	/// <summary>
	/// Gets the stored values from PlayerPrefs that were stored via some keys.
	/// </summary>
	public void SetupStoredScores()
	{
		for(int i = 0; i < allScores.Count; i++)
		{
			// get times completed
			if(PlayerPrefs.HasKey(allScores[i].name + "Times Completed"))
			{
				allScores[i].SetTimesCompleted(PlayerPrefs.GetInt(allScores[i].name + "Times Completed"));
			}
			else
			{
				allScores[i].SetTimesCompleted(0); 
			}

			// get each entry
			for(int j = 0; j < 10; j++)
			{
				// get minutes
				allScores[i].entries[j].minutes = (PlayerPrefs.HasKey(allScores[i].name + j + "minutes")) ? PlayerPrefs.GetInt(allScores[i].name + j + "minutes") : 59;

				// get seconds
				allScores[i].entries[j].seconds = (PlayerPrefs.HasKey(allScores[i].name + j + "seconds")) ? PlayerPrefs.GetInt(allScores[i].name + j + "seconds") : 59;

				// get day
				allScores[i].entries[j].day = (PlayerPrefs.HasKey(allScores[i].name + j + "day")) ? PlayerPrefs.GetInt(allScores[i].name + j + "day") : 12;

				// get month
				allScores[i].entries[j].month = (PlayerPrefs.HasKey(allScores[i].name + j + "month")) ? PlayerPrefs.GetInt(allScores[i].name + j + "month") : 12;

				// get year
				allScores[i].entries[j].year = (PlayerPrefs.HasKey(allScores[i].name + j + "year")) ? PlayerPrefs.GetInt(allScores[i].name + j + "year") : 12;
			}
		}
	}
}
