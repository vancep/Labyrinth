using UnityEngine;
using System.Collections;

public class BlockController : MonoBehaviour 
{
	private bool canMove = false;

	private float correctHeight;
	private float randomHeight;
	private float distToMarble;

	private GameObject player;
	private float maxDist = 15;
	private float minDist = 2;
	private float b;
	private float m;
	private float y;

	// Use this for initialization
	void Start () 
	{
		
	}

	public void SetupForMoving()
	{
		canMove = true;

		correctHeight = this.gameObject.transform.position.y;

		randomHeight = Random.value * 4;

		m = ( randomHeight - correctHeight ) / ( maxDist - minDist );

		b = correctHeight - (m * minDist);

		player = GameObject.FindGameObjectWithTag("Player");
	}

	void FixedUpdate()
	{
		if(canMove)
		{
			distToMarble = Vector3.Distance(this.gameObject.transform.position, player.transform.position);

			if(distToMarble > maxDist)
			{
				y = randomHeight;
			}
			else if(distToMarble < minDist)
			{
				y = correctHeight;
			}
			else
			{
				y = m * distToMarble + b;
			}

			this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, y, this.gameObject.transform.position.z);
		}

	}

		
}
