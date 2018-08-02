using UnityEngine;
using System.Collections;

public class FireAtPlayer : MonoBehaviour 
{
	public GameObject bolt;
	public AudioClip boltSound;

	void Start()
	{
		StartCoroutine(Fire());
	} // Start

	IEnumerator Fire()
	{
		int secondsToWait = Random.Range(3, 7);

		yield return new WaitForSeconds(secondsToWait);
		Instantiate(bolt, transform.position, Quaternion.Euler(0, 180, 0));
		AudioSource.PlayClipAtPoint(boltSound, transform.position);
	} // Fire

} // class
