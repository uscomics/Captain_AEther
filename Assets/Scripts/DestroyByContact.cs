using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour 
{
	public GameObject explosion;
	public GameObject playerExplosion;
	public int scoreValue = 0;

	private GameController gameController;

	void Start()
	{
		GameObject gameControllerObject = GameObject.FindWithTag("GameController");

		if (null != gameControllerObject)
		{
			gameController = gameControllerObject.GetComponent<GameController>();
		} // if
		if (null == gameController)
		{
			Debug.Log("DestroyByContact::Start: Cannot find GameController");
		} // if
	} // Start

	void OnTriggerEnter(Collider inOther) 
	{
		Debug.Log("DestroyByContact::OnTriggerEnter: Collision between " + this.tag + " and " + inOther.tag + ".");

		string tag = inOther.tag;

		if (("Boundary" == tag)
	    || ("LifeIcon" == tag)
	    || ("ExtraLife" == tag)
	    || ("Shield" == tag)
	    || ("Bomb" == tag)
	    || ("ComicBook" == tag)
	    || ("EnemyBolt" == tag)
		|| ("Dime" == tag))
		{
			return;
		} // if
		
		Instantiate(explosion, transform.position, transform.rotation);
		if ("ShipShield" != tag)
		{
			Destroy(inOther.gameObject);
		} // if
		Destroy(gameObject);

		if ("Player" == tag)
		{
			Debug.Log("DestroyByContact::OnTriggerEnter: Player died.");
			Instantiate(playerExplosion, inOther.transform.position, inOther.transform.rotation);
			gameController.PlayerDeath();
		} // if
		else
		{
			gameController.SpawnDrop(inOther.transform);
		} // else

		gameController.AddScore(scoreValue);
		Debug.Log("DestroyByContact::OnTriggerEnter: Exiting function.");
	} // OnTriggerEnter
} // class
