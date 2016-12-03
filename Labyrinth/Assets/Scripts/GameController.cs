using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;

using System.Diagnostics;

[System.Serializable]
public struct coord
{
	public int x, z;

	public coord(int v1, int v2)
	{
		x = v1;
		z = v2;
	}
}

public class GameController : MonoBehaviour 
{
	public bool outsideBorder; // the outermost border

	public GameObject player;
	public GameObject block;
	public GameObject startCube;
	public GameObject endCube;
	public GameObject settings;

	public GameObject pausePanel;
	public Button resumeButton;
	public Button restartLevel;

	public Text timeBox;
	public Text scoreBox;

	private int length;  // x-axis
	private int width;	// z-axis

	private int score;

	private float chanceOfWall; // between 0.0 and 1.0! Values close to 1 will have a harder time to succeed. will need to increase number of attempts
	private int attemptsToCreate;

	private coord startCoord;
	private coord endCoord;

	private float[,] board;

	// the instantiated 2d array of blocks
	private GameObject[,] iBoard;

	private PlayerController playerController;

	private GameObject settingsObj;
	private SettingsInfo settingsInfo;

	private bool paused;

	private Stopwatch stopWatch;

	// Use this for initialization
	void Start () 
	{
		stopWatch = new Stopwatch();
		score = 0;
		UpdateScoreDisplay();
		PauseGame();

		// get access to settings
		getSettings();

		// get level size
		switch(settingsInfo.getLevelSize())
		{
		case 0:
			width = length = 15;
			endCoord = new coord(14, 14);
			break;
		case 1:
			width = length = 50;
			endCoord = new coord(49, 49);
			break;
		case 2:
			width = length = 100;
			endCoord = new coord(99, 99);
			break;
		case 3:
			width = length = 200;
			endCoord = new coord(199, 199);
			break;
		}
		startCoord = new coord(0,0);

		// get difficulty
		switch(settingsInfo.getDifficulty())
		{
		case 0:
			chanceOfWall = 0.2f;
			break;
		case 1:
			chanceOfWall = 0.3f;
			break;
		case 2:
			chanceOfWall = 0.4f;
			break;
		}
		attemptsToCreate = 1000;

		// setup width and length of board
		board = new float[width, length]; // just contains the y-vals of each block. used for testing stuff before moving real cubes
		iBoard = new GameObject[width, length]; // contains the actual cubes that show up on screen

		InstantiateBoard(); // only call this once! instantiates the real cubes, but they can be moved again later
		SetBoardToDefault(); // sets both board and iBoard to 0.0f for all values

		// create border walls
		if(outsideBorder)
		{
			CreateBorderWalls(); // also only call this once or else you get walls over walls. thats just silly.
		}

		CreateBoard();
		MovePlayerToStart();
		ResumeGame();

		playerController = Object.FindObjectOfType<PlayerController>();
		if(playerController != null)
		{
			
		}
		else
		{
			UnityEngine.Debug.Log("Could not find player.");
		}
	}

	void CreateBoard()
	{
		int attempts = 0;

		// create random boards until one is created with a path from start to finish
		do 
		{
			SetBoardToDefault();
			CreateRandomBoard(chanceOfWall);
			attempts++;
			UnityEngine.Debug.Log("Attempt: " + attempts);
		} while (!isPath() && attempts < attemptsToCreate);

		if(!isPath())
		{
			UnityEngine.Debug.Log("Too many attempts to create a board, try with a smaller amount of walls");
		}

		setIBoard();
	}

	// Update is called once per frame
	void Update () 
	{
		UpdateTimeDisplay();

		// reset level if r pressed
		if(Input.GetKeyUp(KeyCode.R))
		{
			Reset();
		}

		// open pause menu when 'esc' hit
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			if(!paused)
			{
				PauseGame();
				pausePanel.SetActive(true);
			}
			else
			{
				ResumeGame();
				pausePanel.SetActive(false);
			}
		}


		// reset level if player reached the end
		if(playerController.completedLevel())
		{
			UnityEngine.Debug.Log("Player completed level!");
			IncrementScore();
			UpdateScoreDisplay();
			Reset();
		}
	}

	/// <summary>
	/// Creates the outermost border walls.
	/// </summary>
	private void CreateBorderWalls()
	{
		//Vector3 position = new Vector3(0.0f, 0.5f, 0.0f);
		Quaternion rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);

		// gets left and right borders
		for(int i = -1; i <= width; i++)
		{
			Instantiate(block, new Vector3(-1.0f, 0.5f, i), rotation).name = "Block (Border)";
			Instantiate(block, new Vector3(length, 0.5f, i), rotation).name = "Block (Border)";
		}

		// gets top and bottom borders
		for(int i = 0; i < length; i++)
		{
			Instantiate(block, new Vector3(i, 0.5f, -1.0f), rotation).name = "Block (Border)";
			Instantiate(block, new Vector3(i, 0.5f, width), rotation).name = "Block (Border)";
		}

	}

	/// <summary>
	/// Creates the random board.
	/// randomly sets each block within the border to either the lower or upper position.
	/// </summary>
	/// <param name="percent">Percent. Should be a float between 0.0 and 1.0.</param>
	private void CreateRandomBoard(float percent)
	{
		for(int i = 0; i < width; i++)
		{
			for(int j = 0; j < length; j++)
			{
				if(Random.value < percent)
				{
					board[i, j] = 0.5f;
				}
			}
		}

		// set start and end positions
		SetStartPos(startCoord.x, 1, startCoord.z);
		SetEndPos(endCoord.x, 1, endCoord.z);
	}

	private void InstantiateBoard()
	{
		Quaternion rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);

		for(int i = 0; i < width; i++)
		{
			for(int j = 0; j < length; j++)
			{
				iBoard[i,j] = (GameObject)Instantiate(block, new Vector3(i, 0.0f, j), rotation);
				iBoard[i,j].name = "Block (" + i + ", " + j + ")";
			}
		}
	}

	/// <summary>
	/// Sets the board to default height of 0.
	/// </summary>
	private void SetBoardToDefault()
	{
		for(int i = 0; i < width; i++)
		{
			for(int j = 0; j < length; j++)
			{
				board[i, j] = 0.0f;
				iBoard[i,j].transform.position = new Vector3(iBoard[i,j].transform.position.x, 0.0f, iBoard[i,j].transform.position.z);
			}
		}
	}

	private void setIBoard()
	{
		for(int i = 0; i < width; i++)
		{
			for(int j = 0; j < length; j++)
			{
				iBoard[i,j].transform.position = new Vector3(iBoard[i,j].transform.position.x, board[i,j], iBoard[i,j].transform.position.z);
			}
		}
	}

	private void SetStartPos(int x, int y, int z)
	{
		startCube.transform.position = new Vector3(x, y, z);
		board[x,z] = 0.0f;
	}

	private void SetEndPos(int x, int y, int z)
	{
		endCube.transform.position = new Vector3(x, y, z);
		board[x,z] = 0.0f;
	}

	private void Reset()
	{
		ResetPlayer();
		CreateBoard();
		ResetStopWatch();
		ResumeGame();
	}

	private void ResetPlayer()
	{
		playerController.reset();
		MovePlayerToStart();
	}

	private void MovePlayerToStart()
	{
		player.transform.position = startCube.transform.position;
	}

	private bool isPath()
	{

		Queue<coord> paths = new Queue<coord>();
		coord current = startCoord;
		coord toAdd;

		bool[,] marked = new bool[width, length];

		// set all spots to unmarked. leaving a trail.
		for(int i = 0; i < width; i++)
		{
			for(int j = 0; j < length; j++)
			{
				marked[i,j] = false;
			}
		}
			
		// add starting position to queue and mark it.
		paths.Enqueue(current);
		marked[current.x, current.z] = true;

		do {
			current = paths.Dequeue();

			// check if at the end
			if(current.x == endCoord.x && current.z == endCoord.z)
			{
				return true;
			}
			else
			{
				// check if can move up
				if(current.x > 0 && board[current.x - 1, current.z] == 0.0f && marked[current.x - 1, current.z] == false)
				{
					toAdd.x = current.x - 1;
					toAdd.z = current.z;
					marked[toAdd.x, toAdd.z] = true;
					paths.Enqueue(toAdd);
				}
				// check if can move right
				if(current.z < length - 1 && board[current.x, current.z + 1] == 0.0f && marked[current.x, current.z + 1] == false)
				{
					toAdd.x = current.x;
					toAdd.z = current.z + 1;
					marked[toAdd.x, toAdd.z] = true;
					paths.Enqueue(toAdd);
				}
				// check if can move down
				if(current.x < width - 1 && board[current.x + 1, current.z] == 0.0f && marked[current.x + 1, current.z] == false)
				{
					toAdd.x = current.x + 1;
					toAdd.z = current.z;
					marked[toAdd.x, toAdd.z] = true;
					paths.Enqueue(toAdd);
				}
				// check if can move left
				if(current.z > 0 && board[current.x, current.z - 1] == 0.0f && marked[current.x, current.z - 1] == false)
				{
					toAdd.x = current.x;
					toAdd.z = current.z - 1;
					marked[toAdd.x, toAdd.z] = true;
					paths.Enqueue(toAdd);
				}
			}

		} while (paths.Count > 0);
		
		return false;
	}

	/// <summary>
	/// Gets the settings that were picked in the previous menu.
	/// If the menu was skipped, picks default settings.
	/// </summary>
	private void getSettings()
	{
		if((settingsObj = GameObject.FindWithTag("Settings")) == null)
		{
			Instantiate(settings);
			settingsObj = GameObject.FindWithTag("Settings");
		}
			
		settingsInfo = settingsObj.GetComponent<SettingsInfo>();

		UnityEngine.Debug.Log("Level Size: " + settingsInfo.getLevelSize() + " Difficulty: " + settingsInfo.getDifficulty());
	}

	private void PauseGame()
	{
		stopWatch.Stop();
		paused = true;
	}

	private void ResumeGame()
	{
		stopWatch.Start();
		paused = false;
	}

	private void ResetStopWatch()
	{
		stopWatch.Reset();
	}

	private void UpdateScoreDisplay()
	{
		scoreBox.text = score + "";
	}

	private void UpdateTimeDisplay()
	{
		System.TimeSpan ts = stopWatch.Elapsed;
		timeBox.text = string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
	}

	private void IncrementScore()
	{
		if(score < 999999999)
		{
			score++;
		}
	}

}
