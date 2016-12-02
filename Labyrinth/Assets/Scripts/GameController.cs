using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour 
{
	public int length;  // x-axis
	public int width;	// z-axis

	public bool outsideBorder; // the outermost border

	public GameObject player;
	public GameObject block;
	public GameObject startCube;
	public GameObject endCube;

	private float[,] board;

	// the instantiated 2d array of blocks
	private Object[,] iBoard;

	private PlayerController playerController;

	// Use this for initialization
	void Start () 
	{
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
		board = new float[width, length];
		iBoard = new Object[width, length];

		SetBoardToDefault();

		if(outsideBorder)
		{
			CreateBorderWalls();
		}

		CreateRandomBoard(0.2f);

		InstantiateBoard();

		MovePlayerToStart();
	}

	// Update is called once per frame
	void Update () 
	{
		if(playerController.completedLevel())
		{
			Debug.Log("Player completed level!");

			ResetPlayer();

			ResetBoard();
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
		for(int i = 0; i <= width; i++)
		{
			Instantiate(block, new Vector3(0.0f, 0.5f, i), rotation).name = "Block (Border)";
			Instantiate(block, new Vector3(length, 0.5f, i), rotation).name = "Block (Border)";
		}

		// gets top and bottom borders
		for(int i = 1; i < length; i++)
		{
			Instantiate(block, new Vector3(i, 0.5f, 0.0f), rotation).name = "Block (Border)";
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
		for(int i = 1; i < width - 1; i++)
		{
			for(int j = 1; j < length - 1; j++)
			{
				if(Random.value < percent)
				{
					board[i, j] = 0.5f;
				}
			}
		}

		// set start and end positions
		SetStartPos(1, 0.5f, 1);
		SetEndPos(width - 1, 0.5f, length - 1);

	}

	private void InstantiateBoard()
	{
		Quaternion rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);

		for(int i = 0; i < width; i++)
		{
			for(int j = 0; j < length; j++)
			{
				iBoard[i,j] = Instantiate(block, new Vector3(i + 1, board[i, j], j + 1), rotation);
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
			}
		}
	}

	private void SetStartPos(int x, float y, int z)
	{
		startCube.transform.position = new Vector3(x, y, z);
		board[x,z] = 0.0f;
	}

	private void SetEndPos(int x, float y, int z)
	{
		endCube.transform.position = new Vector3(x, y, z);
		board[x,z] = 0.0f;
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

	private void ResetBoard()
	{
		DestroyBoard();
		CreateBoard();
	}

	private void DestroyBoard()
	{
		for(int i = 0; i < width; i++)
		{
			for(int j = 0; j < length; j++)
			{
				DestroyImmediate(iBoard[i, j]);
			}
		}
	}
}
