using UnityEngine;
using System.Collections;

public class CreateBoard : MonoBehaviour 
{

	public int length;  // x-axis
	public int width;	// z-axis

	public GameObject block;

	// Use this for initialization
	void Start () 
	{
		Vector3 position = new Vector3(0.0f, 0.0f, 0.0f);
		Quaternion rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);

		for(int i = 0; i < width; i++)
		{
			position.z = i;
			for(int j = 0; j < length; j++)
			{
				if(i == 0 || i == width-1 || j == 0 || j == length -1)
				{
					position.y = 0.5f;
				}
				else
				{
					if(Random.value < 0.2)
					{
						position.y = 0.5f;
					}
					else
					{
						position.y = 0.0f;
					}
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
