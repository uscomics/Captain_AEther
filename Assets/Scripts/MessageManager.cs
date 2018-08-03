using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Messages : MonoBehaviour
{
    public static string MSG_SUPERBRAINS_SHIPS_ARE_ATTACKING = "Superbrains ships are attacking!";
    public static string MSG_HOW_TO_MOVE_SHIP = "Use AWSD, cursor keys, or mouse to move.";
    public static string MSG_HOW_TO_FIRE = "Use left mouse button or space bar to attack.";
    public static string MSG_RECEIVED_STORE_CREDIT = "You received in-store credits!";
    public static string MSG_DIFFICULTY_SET_TO = "Difficulty set to ";
    public static string MSG_BADGE_SCORE_PARTICIPANT = "You got the Participant badge!";
    public static string MSG_BADGE_SCORE_ROOKIE = "You got the Rookie badge!";
    public static string MSG_BADGE_SCORE_COMPETITOR = "You got the Competitor badge!";
    public static string MSG_BADGE_SCORE_PLAYER = "You got the Player badge!";
    public static string MSG_BADGE_SCORE_MAJOR_PLAYER = "You got the Major Player badge!";
    public static string MSG_BADGE_SCORE_CHAMP = "You got the Champ badge!";
    public static string MSG_BADGE_COIN_BENJAMINS = "You got the Benjamins badge!";
    public static string MSG_BADGE_COIN_CASH_COW = "You got the Cash Cow badge!";
    public static string MSG_BADGE_COIN_LOOT = "You got the Loot badge!";
    public static string MSG_BADGE_COIN_WAD = "You got the Wad badge!";
    public static string MSG_BADGE_COIN_BIG_BANK_HANK = "You got the Big Bank Hank badge!";
    public static string MSG_BADGE_COIN_RICH = "You got the Rich badge!";
    public static string MSG_BADGE_COIN_MILLIONAIRE = "You got the Millionaire badge!";
    public static string MSG_BADGE_CONSECUTIVE_DAYS_TOURIST = "You got the Tourist badge!";
    public static string MSG_BADGE_CONSECUTIVE_DAYS_VISITOR = "You got the Visitor badge!";
    public static string MSG_BADGE_CONSECUTIVE_DAYS_DAY_TRIPPER = "You got the Day Tripper badge!";
    public static string MSG_BADGE_CONSECUTIVE_DAYS_HARD_CORE = "You got the Hard Core badge!";
    public static string MSG_BADGE_CONSECUTIVE_DAYS_ADDICTED = "You got the Addicted badge!";
    public static string MSG_BADGE_CONSECUTIVE_DAYS_FANATIC = "You got the Fanatic badge!";
    public static string MSG_BADGE_CONSECUTIVE_DAYS_DEVOTEE = "You got the Devotee badge!";
    public static string MSG_BADGE_CONSECUTIVE_DAYS_ZEALOT = "You got the Zealot badge!";
    public static string MSG_BADGE_CONSECUTIVE_DAYS_GET_A_LIFE = "You got the Get A Life badge!";
    public static string MSG_BADGE_DEFEATED_SUPERBRAIN_VICTOR = "You got the Victor badge!";
    public static string MSG_BADGE_DEFEATED_SUPERBRAIN_HERO = "You got the Hero badge!";
    public static string MSG_BADGE_DEFEATED_SUPERBRAIN_SUPER_HERO = "You got the Super Hero badge!";
    public static string MSG_BADGE_DEFEATED_SUPERBRAIN_BRAIN_BLASTER = "You got the Brain Blaster badge!";
    public static string MSG_BADGE_DEFEATED_BY_SUPERBRAIN_DEFEATED = "You got the Defeated badge!";
    public static string MSG_BADGE_DEFEATED_BY_SUPERBRAIN_LOSER = "You got the Loser badge!";
    public static string MSG_BADGE_DEFEATED_BY_SUPERBRAIN_BROWBEATEN = "You got the Browbeaten badge!";
    public static string MSG_BADGE_DEFEATED_BY_SUPERBRAIN_BRAIN_BLASTED = "You got the Brain Blasted badge!";
}

public class MessageManager : MonoBehaviour {
    public TextMeshProUGUI message;
    public CanvasGroup messageCanvasGroup;
    public int lifetime = 5; // seconds

    private Queue<string> messageQueue = new Queue<string>();
    private bool visible = false;
    private float startTime;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        if ((true == visible)
        && (Time.time >= startTime + lifetime))
        {
            HideMessage();
        } // if
    }

    public void ShowMessage(string inMessage)
    {
        Debug.Log("MessageManager::ShowMessage: Received message: inMessage: " + inMessage + ".");
        if (null == inMessage)
        {
            Debug.Log("MessageManager::ShowMessage: Invalide data: inMessage: " + inMessage + ".");
            return;
        }
        if (true == visible)
        {
            messageQueue.Enqueue(inMessage);
            return;
        }
        visible = true;
        startTime = Time.time;
        message.text = inMessage;
        messageCanvasGroup.alpha = 1;
        messageCanvasGroup.interactable = true;
        messageCanvasGroup.blocksRaycasts = true;
    }

    void HideMessage()
    {
        messageCanvasGroup.alpha = 0;
        messageCanvasGroup.interactable = false;
        messageCanvasGroup.blocksRaycasts = false;
        visible = false;
        startTime = 0;
        if (0 == messageQueue.Count)
        {
            return;
        }
        string msg = messageQueue.Dequeue();
        ShowMessage(msg);
    }
}
