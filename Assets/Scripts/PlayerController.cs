using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary
{
	public float xMin = 0.0f;
	public float xMax = 0.0f;
	public float zMin = 0.0f;
	public float zMax = 0.0f;
} // class

public class PlayerController : MonoBehaviour
{
	public float speed = 5.0f;
	public float swipeSpeed = 5.0f;
	public float tilt = 0.0f;
	public Boundary boundary;
	public GameObject shield;
	public GameObject shot;
	public Transform shotSpawn;
	public float fireRate = 0.5F;

	private float nextFire = 0.0F;

    void Start()
	{
		shield.SetActive(false);
	} // Start

	void FixedUpdate()
	{
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");
        Debug.Log("PlayerController::FixedUpdate: h = " + h);
        Debug.Log("PlayerController::FixedUpdate: v = " + v);

        Rigidbody r = GetComponent<Rigidbody>();

        if ((0 == h) && (0 == v))
        {
            h = Input.GetAxis("MovementX");
            v = Input.GetAxis("MovementY");
            if (1 < h) h = 1f;
            else if (-1 > h) h = -1f;
            if (1 < v) v = 1f;
            else if (-1 > v) v = -1f;
        } // if

        Vector3 m = new Vector3(h, 0.0f, v) * speed;
		float x = Mathf.Clamp(r.position.x, boundary.xMin, boundary.xMax);
		float z = Mathf.Clamp(r.position.z, boundary.zMin, boundary.zMax);
		
		r.velocity = m;
		r.position = new Vector3(x, 0.0f, z);
	} // FixedUpdate

	void Update()
	{
		bool didFire = false;

		didFire = Input.GetButton("Fire1");

		if (!didFire)
		{
			didFire = (Swipe.Tap == SwipeManager.swipeDirection);
		} // if

		if ((didFire)
		&& (Time.time > nextFire)) 
		{
			nextFire = Time.time + fireRate;
			Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
			GetComponent<AudioSource>().Play();
		} // if
	} // Update

//	private Vector3 screenPoint, offset;
//	
//	void OnMouseDown() 
//	{ //Select object
//		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
//		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
//		                                                                                    Input.mousePosition.y, screenPoint.z));
//	}
//	void OnMouseDrag() 
//	{
//		Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
//		Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset - new Vector3(0,0, - 0.8561556f);
//		transform.parent.position = curPosition;
//	}
} // class
