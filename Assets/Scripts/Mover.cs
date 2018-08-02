using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour
{
	public float speed = 1.0f;
	public Vector3 direction = Vector3.zero;

	void Start()
	{
		Rigidbody r = GetComponent<Rigidbody>();

		if (Vector3.zero == direction)
		{
			direction = transform.forward;
		} // if
		r.velocity = direction * speed;
	} // Start
} // class
