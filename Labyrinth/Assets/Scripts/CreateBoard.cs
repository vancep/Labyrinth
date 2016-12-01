using UnityEngine;
using System.Collections;

public class CreateBoard : MonoBehaviour 
{
	public int length;  // x-axis
	public int width;	// z-axis

	public bool outsideBorder; // the outermost border

	public GameObject block;

	// Use this for initialization
	void Start () 
	{
		if(outsideBorder)
		{
			createBorderWalls();
		}

		createRandomBoard(0.2f);
	}

	/// <summary>
	/// Creates the outermost border walls.
	/// </summary>
	void createBorderWalls()
	{
		//Vector3 position = new Vector3(0.0f, 0.5f, 0.0f);
		Quaternion rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);

		// gets left and right borders
		for(int i = 0; i <= width; i++)
		{
			Instantiate(block, new Vector3(0.0f, 0.5f, i), rotation);
			Instantiate(block, new Vector3(length, 0.5f, i), rotation);
		}

		// gets top and bottom borders
		for(int i = 1; i < length; i++)
		{
			Instantiate(block, new Vector3(i, 0.5f, 0.0f), rotation);
			Instantiate(block, new Vector3(i, 0.5f, width), rotation);
		}

	}
		
	/// <summary>
	/// Creates the random board.
	/// randomly sets each block within the border to either the lower or upper position.
	/// </summary>
	/// <param name="percent">Percent. Should be a float between 0.0 and 1.0.</param>
	void createRandomBoard(float percent)
	{
		Vector3 position = new Vector3(0.0f, 0.0f, 0.0f);
		Quaternion rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);

		for(int i = 1; i < width - 1; i++)
		{
			position.z = i;
			for(int j = 1; j < length - 1; j++)
			{
				if(Random.value < percent)
				{
					position.y = 0.5f;
				}
				else
				{
					position.y = 0.0f;
				}

				position.x = j;
				Instantiate(block, position, rotation);
			}
		}
	}

	// Update is called once per frame
	void Update () 
	{
	
	}
}
