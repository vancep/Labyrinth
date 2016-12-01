using UnityEngine;
using System.Collections;

public class CreateBoard : MonoBehaviour 
{
	public int length;  // x-axis
	public int width;	// z-axis

	public bool outsideBorder; // the outermost border

	public GameObject block;

	private float[,] board;

	// the instantiated 2d array of blocks
	private Object[,] iBoard;

	// Use this for initialization
	void Start () 
	{
		board = new float[width, length];
		iBoard = new Object[width, length];

		setBoardToDefault();

		if(outsideBorder)
		{
			createBorderWalls();
		}

		createRandomBoard(0.2f);

		instantiateBoard();
	}

	/// <summary>
	/// Creates the outermost border walls.
	/// </summary>
	private void createBorderWalls()
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
	private void createRandomBoard(float percent)
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
	}

	private void instantiateBoard()
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
	private void setBoardToDefault()
	{
		for(int i = 0; i < width; i++)
		{
			for(int j = 0; j < length; j++)
			{
				board[i, j] = 0.0f;
			}
		}
	}

	private void setStart()
	{
		
	}
}
