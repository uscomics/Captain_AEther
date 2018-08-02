using UnityEngine;
using System.Collections;

public class MoveTowardPlayer : MonoBehaviour 
{
	public float speed = 1.0f;

	private GameObject player;

	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
	} // Start

	void Update()
	{
		Vector3 currentPosition = transform.position;
		Vector3 playerPosition = player.transform.position;

		transform.position = Vector3.MoveTowards(currentPosition, playerPosition, speed * Time.deltaTime);
	} // Start
} // class
