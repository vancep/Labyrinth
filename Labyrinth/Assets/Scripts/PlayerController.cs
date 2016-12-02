using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed;

	private Rigidbody rb;

	private bool atEnd = false;

	// Use this for initialization
	void Start () 
	{
		rb = GetComponent<Rigidbody>();
	}

	void FixedUpdate()
	{

		if(Input.GetButton("Fire1"))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit rayHit;

			if(Physics.Raycast(ray, out rayHit, 150))
			{
				Vector3 rayVector = rayHit.point;

				Vector3 movement = rayVector - rb.position;
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
		if(other.gameObject.CompareTag("End"))
		{
			atEnd = true;
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
