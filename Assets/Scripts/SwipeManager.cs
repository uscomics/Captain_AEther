using UnityEngine;
using System.Collections;

[System.Serializable]
public enum Swipe { None, Up, UpLeft, UpRight, Down, DownLeft, DownRight, Left, Right, Tap };
[System.Serializable]
public enum MouseButton { Left = 0, Right = 1, Middle = 2 };

// Detects swipes and taps for both touch and mouse.
public class SwipeManager : MonoBehaviour 
{
	public float minSwipeLength = 5.0f;
	public MouseButton mouseButton = MouseButton.Left;
	public bool use8PointCompass = true;
	public static Vector2 firstPosition;
	public static Vector2 secondPosition;
	public static Vector2 latestMovement;
	public static Vector2 swipe;
	public static Swipe swipeDirection;

	private Vector2 firstTouchPosition;
	private Vector2 firstMousePosition;
	private static bool touchSwipeStarted = false;
	private static bool mouseSwipeStarted = false;

	void Update()
	{
		DetectSwipe();
	} // Update
	
	public static bool GetSwipeStarted()
	{
		return (mouseSwipeStarted || touchSwipeStarted);
	} // GetSwipeStarted
	
	public static bool HasSwipe()
	{
		if ((Swipe.None == SwipeManager.swipeDirection)
	    || (Swipe.Tap == SwipeManager.swipeDirection))
		{
			return false;
		} // if
		return true;
	} // HasSwipe

	public static Vector2 GetSwipeAsVector()
	{
		if (!SwipeManager.HasSwipe())
		{
			return new Vector2(0.0f, 0.0f);
		} // if
		
		switch(SwipeManager.swipeDirection)
		{
		case Swipe.Up: 			return new Vector2( 0.0f,  1.0f);
		case Swipe.UpLeft: 		return new Vector2(-1.0f,  1.0f);
		case Swipe.UpRight: 	return new Vector2( 1.0f,  1.0f);
		case Swipe.Down: 		return new Vector2( 0.0f, -1.0f);
		case Swipe.DownLeft: 	return new Vector2(-1.0f, -1.0f);
		case Swipe.DownRight: 	return new Vector2( 1.0f, -1.0f);
		case Swipe.Left: 		return new Vector2(-1.0f,  0.0f);
		case Swipe.Right: 		return new Vector2( 1.0f,  0.0f);
		} // switch
		return new Vector2(0.0f, 0.0f);
	} // GetSwipeAsVector

	public void DetectSwipe()
	{
		swipeDirection = Swipe.None;
		if (Input.touches.Length > 0) 
		{
			Touch touch = Input.GetTouch(0);
			
			if (TouchPhase.Began == touch.phase) 
			{
				firstTouchPosition = new Vector2(touch.position.x, touch.position.y);
				touchSwipeStarted = true;
				mouseSwipeStarted = false;
			} // if
			else if (TouchPhase.Ended == touch.phase) 
			{
				CalculateSwipePosition(firstTouchPosition, touch.position);
				touchSwipeStarted = false;
			} // else if
			else if (TouchPhase.Moved == touch.phase)
			{
				latestMovement = touch.position;
			} // else if
		} //if
		else 
		{
			if (Input.GetMouseButtonDown((int)mouseButton)) 
			{
				firstMousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
				mouseSwipeStarted = true;
				touchSwipeStarted = false;
			} // if
			else if (Input.GetMouseButtonUp((int)mouseButton))
			{
				CalculateSwipePosition(firstMousePosition, Input.mousePosition);
				mouseSwipeStarted = false;
				latestMovement = Vector2.zero;
			} // else if
			else if (mouseSwipeStarted)
			{
				latestMovement = Input.mousePosition;
			} // else if
		} // else
	} // DetectSwipe

	void CalculateSwipePosition(Vector2 inFirstPosition, Vector2 inSecondPosition)
	{
		Vector2 currentSwipe = new Vector3(inSecondPosition.x - inFirstPosition.x, inSecondPosition.y - inFirstPosition.y);

		if (currentSwipe.magnitude < minSwipeLength) 
		{
			swipeDirection = Swipe.Tap;
			return;
		} // if

		if ((currentSwipe.normalized.y > 0)
		&& (currentSwipe.normalized.x > -0.5f)
		&& (currentSwipe.normalized.x < 0.5f))
		{
			SetSwipe(inFirstPosition, inSecondPosition, currentSwipe, Swipe.Up);
		} // if
		else if ((currentSwipe.normalized.y < 0)
		&& (currentSwipe.normalized.x > -0.5f)
		&& (currentSwipe.normalized.x < 0.5f))
		{
			SetSwipe(inFirstPosition, inSecondPosition, currentSwipe, Swipe.Down);
		} // else if
		else if ((currentSwipe.normalized.x < 0)
		&& (currentSwipe.normalized.y > -0.5f)
		&& (currentSwipe.normalized.y < 0.5f))
		{
			SetSwipe(inFirstPosition, inSecondPosition, currentSwipe, Swipe.Left);
		} // else if
		else if ((currentSwipe.normalized.x > 0)
		&& (currentSwipe.normalized.y > -0.5f)
		&& (currentSwipe.normalized.y < 0.5f)) 
		{
			SetSwipe(inFirstPosition, inSecondPosition, currentSwipe, Swipe.Right);
		} // else if
		else if (use8PointCompass)
		{
			if ((currentSwipe.normalized.y > 0)
			&& (currentSwipe.normalized.x < 0)) 
			{
				SetSwipe(inFirstPosition, inSecondPosition, currentSwipe, Swipe.UpLeft);
			} // if
			else if ((currentSwipe.normalized.y > 0)
			&& (currentSwipe.normalized.x > 0)) 
			{
				SetSwipe(inFirstPosition, inSecondPosition, currentSwipe, Swipe.UpRight);
			} // else if
			else if ((currentSwipe.normalized.y < 0)
			&& (currentSwipe.normalized.x < 0)) 
			{
				SetSwipe(inFirstPosition, inSecondPosition, currentSwipe, Swipe.DownLeft);
			} // else if
			else if ((currentSwipe.normalized.y < 0)
			&& (currentSwipe.normalized.x > 0))
			{
				SetSwipe(inFirstPosition, inSecondPosition, currentSwipe, Swipe.DownRight);
			} // else if
		} // else if
	} // CalculateSwipePosition

	void SetSwipe(Vector2 inFirstPosition, Vector2 inSecondPosition, Vector2 inSwipe, Swipe inSwipeDirection)
	{
		Debug.Log("SwipeManager::SetSwipe: inFirstPosition.x: " + inFirstPosition.x + ", inFirstPosition.y: " + inFirstPosition.y);
		Debug.Log("SwipeManager::SetSwipe: inSecondPosition.x: " + inSecondPosition.x + ", inSecondPosition.y: " + inSecondPosition.y);
		Debug.Log(string.Format("SwipeManager::SetSwipe: inSwipe.'{0}'.", inSwipe.ToString()));
		Debug.Log(string.Format("SwipeManager::SetSwipe: inSwipeDirection.'{0}'.", inSwipeDirection.ToString()));
		firstPosition = inFirstPosition;
		secondPosition = inSecondPosition;
		swipe = inSwipe;
		swipeDirection = inSwipeDirection;
	} // SetSwipe
} // class