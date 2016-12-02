﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct coord
{
	public int x, z;
}

public class GameController : MonoBehaviour 
{
	public coord startCoord;
	public coord endCoord;

	public int length;  // x-axis
	public int width;	// z-axis

	public float chanceOfWall; // between 0.0 and 1.0! Values close to 1 will have a harder time to succeed. will need to increase number of attempts
	public int attemptsToCreate;

	public bool outsideBorder; // the outermost border

	public GameObject player;
	public GameObject block;
	public GameObject startCube;
	public GameObject endCube;

	private float[,] board;

	// the instantiated 2d array of blocks
	private GameObject[,] iBoard;

	private PlayerController playerController;

	public GameObject settingsObj;
	private SettingsInfo settingsInfo;

	// Use this for initialization
	void Start () 
	{

		// get access to settings
		settingsObj = GameObject.FindWithTag("Settings");

		if(settingsObj == null)
		{
			settingsObj = Instantiate(settingsObj);
		}
		settingsInfo = settingsObj.GetComponent<SettingsInfo>();

		Debug.Log("Level Size: " + settingsInfo.getLevelSize() + " Difficulty: " + settingsInfo.getDifficulty());

		board = new float[width, length]; // just contains the y-vals of each block. used for testing stuff before moving real cubes
		iBoard = new GameObject[width, length]; // contains the actual cubes that show up on screen

		InstantiateBoard(); // only call this once! instantiates the real cubes, but they can be moved again later
		SetBoardToDefault(); // sets both board and iBoard to 0.0f for all values

		if(outsideBorder)
		{
			CreateBorderWalls(); // also only call this once or else you get walls over walls. thats just silly.
		}

		CreateBoard();

		playerController = Object.FindObjectOfType<PlayerController>();
		if(playerController != null)
		{
			
		}
		else
		{
			Debug.Log("Could not find player.");
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
			Debug.Log("Attempt: " + attempts);
		} while (!isPath() && attempts < attemptsToCreate);

		if(!isPath())
		{
			Debug.Log("Too many attempts to create a board, try with a smaller amount of walls");
		}

		setIBoard();

		MovePlayerToStart();
	}

	// Update is called once per frame
	void Update () 
	{
		// reset level if r pressed
		if(Input.GetKeyUp(KeyCode.R))
		{
			Reset();

		}

		// reset level if player reached the end
		if(playerController.completedLevel())
		{
			Debug.Log("Player completed level!");

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
				//iBoard[i,j].transform.position = new Vector3(iBoard[i,j].transform.position.x, 0.0f, iBoard[i,j].transform.position.z);
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
}
