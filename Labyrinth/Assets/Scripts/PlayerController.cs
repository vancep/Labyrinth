using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed;

	private Rigidbody rb;

	private bool atEnd = false;

	private AudioSource audioSource;
	private float previousMag;
	private float mag;

	// Use this for initialization
	void Start () 
	{
		audioSource = GetComponent<AudioSource>();
		rb = GetComponent<Rigidbody>();

		previousMag = mag = rb.velocity.magnitude;
	}

	void FixedUpdate()
	{
		// used for sound when crashing
		previousMag = mag;
		mag = rb.velocity.magnitude;

		// for mouse controls
		if(Input.GetButton("Fire1"))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit rayHit;

			if(Physics.Raycast(ray, out rayHit, 150))
			{
				Vector3 rayVector = rayHit.point;

				Vector3 movement = rayVector - rb.position;
				movement.Normalize();

				rb.AddForce(movement * (speed /2));
			}
		}
		else
		{
			float moveHorizontal = Input.GetAxis("Horizontal");
			float moveVertical = Input.GetAxis("Vertical");

			Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

			rb.AddForce(movement * speed);
		}

	}

	void OnTriggerEnter(Collider other)
	{
		// marble reached the end of the maze
		if(other.gameObject.CompareTag("End"))
		{
			atEnd = true;
		}

	}

	void OnCollisionEnter(Collision other)
	{
		// for noise when marble hits a block
		// main issue with this is that i dont want the marble to keep making noise as it rolls along a wall.
		// because as it rolls along the wall, it causes new collisions since the wall is made of multiple blocks
		// and each time it reaches the next block, I dont want it to sound like it hit it instead of simply rolling along it.
		if(other.gameObject.CompareTag("Block"))
		{
			// for now, if the marble collides into a block at a specific height, play a sound.
			// since the floors and walls are the same objects, cant just play the sound when the marble collides with any block.
			if(other.gameObject.transform.position.y >= 0.4)
			{
				Vector3 vNorm = rb.velocity.normalized;
				Vector3 cbNorm = (rb.position + other.contacts[0].point).normalized;

				float angleBetween = Mathf.Abs( Vector3.Dot(vNorm, cbNorm));

				float f = Mathf.Cos(angleBetween) * previousMag;

				if(f > 2.0f)
				{
					audioSource.volume = Mathf.Min(1.2f, f/8);
					if(f > 0.2f)
					{
						audioSource.volume -= 0.2f;
					}
					audioSource.Play();
				}
			}
		}
	}

	public bool completedLevel()
	{
		return atEnd;
	}

	public void reset()
	{
		atEnd = false;
		rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
		rb.angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
	}
}
