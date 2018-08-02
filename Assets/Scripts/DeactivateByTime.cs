using UnityEngine;
using System.Collections;

public class DeactivateByTime : MonoBehaviour 
{
	public float lifetime = 1.0f;
	public bool activate = false;
	public bool flipAndRepeat = false;

	private float startTime;

	void Start()
	{
		startTime = Time.time;
	} // Start

	void Update()
	{
		if (Time.time >= startTime + lifetime)
		{
			Debug.Log("DeactivateByTime::Update: Setting the active state of object " + gameObject.tag + " to " + activate.ToString() + " at time " + Time.time + ".");
			gameObject.SetActive(activate);
		} // if
		if (flipAndRepeat)
		{
			activate = !activate;
			ResetTime();
		} // if
	} // Update
	
	public void ResetTime()
	{
		startTime = Time.time;
		Debug.Log("DeactivateByTime::ResetTime: Time reset to " + startTime + ".");
	} // ResetTime
	
	public void ResetObjectsActivateState()
	{
		gameObject.SetActive(!activate);
		Debug.Log("DeactivateByTime::ResetObjectsActivateState: Object's active state set to " + activate.ToString() + ".");
	} // ResetObjectsActivateState
} // class
