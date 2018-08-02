using UnityEngine;
using System.Collections;

public class PickupDrop : MonoBehaviour 
{
	public GameObject shield;

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
			Debug.Log("PickupDrop::Start: Cannot find GameController");
		} // if
	} // Start
	
	void OnTriggerEnter(Collider inOther) 
	{
		if (("ExtraLife" != inOther.tag)
	    && ("Shield" != inOther.tag)
	    && ("Bomb" != inOther.tag)
	    && ("ComicBook" != inOther.tag)
		&& ("Dime" != inOther.tag))
		{
			return;
		} // if

		if ("ExtraLife" == inOther.tag)
		{
			gameController.AddLife();
		} // if
		else if ("Shield" == inOther.tag)
		{
			gameController.AddShield(shield);
		} // else if
		else if ("Bomb" == inOther.tag)
		{
			gameController.AddBomb();
		} // else if
		else if ("Dime" == inOther.tag)
		{
			gameController.AddDime();
		} // else if
		Destroy(inOther.gameObject);
	} // OnTriggerEnter
} // class

