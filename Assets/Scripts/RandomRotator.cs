using UnityEngine;
using System.Collections;

public class RandomRotator : MonoBehaviour 
{
	public float tumble = 5.0f;

	void Start()
	{
		Rigidbody r = GetComponent<Rigidbody>();

		r.angularVelocity =  Random.insideUnitSphere * tumble;
	} // Start
} // clas
