using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

[System.Serializable]
public enum GameDifficulty
{
    Easy,
    Medium,
    Hard
}; // enum

[System.Serializable]
public class GameSettings
{
    public GameSettings(GameDifficulty inDifficulty, float inHorizonalMouseSensitivity, float inVerticalMouseSensitivity)
    {
        difficulty = inDifficulty;
        horizonalMouseSensitivity = inHorizonalMouseSensitivity;
        verticalMouseSensitivity = inVerticalMouseSensitivity;
    }
    public GameDifficulty difficulty = GameDifficulty.Easy;
    public static float horizonalMouseSensitivity;
    public static float verticalMouseSensitivity;
}; // class

[System.Serializable]
public class Drop 
{ 
	public float percentage;
	public GameObject drop;
	public Rect dropRect;
}; // class

[System.Serializable]
public class Hazard 
{
	public GameObject hazardObject;
	public Vector3 hazardRotation = Vector3.zero;
	public Vector3 spawnBounds;
	public int spawnSize = 1;
} // class

[System.Serializable]
public enum LevelChaining
{
	RepeatLevel,
	LoadNextLevel,
	LastLevel,
	EndGame
}; // enum

[System.Serializable]
public class TimedLevel
{ 
	public LevelChaining levelChaining = LevelChaining.LoadNextLevel;
	public RawImage openingImage;
	public int openingImageDurationInSeconds = 5;
	public int levelDurationInSeconds;
	public AudioSource music;
	public Hazard[] hazards;
	public float dropChance = 0.5f;
	public Drop[] dropTable;
	public int spawnWait = 5;
	public bool spawnPlayer = true;
	public int endLevelWait = 0;
    public bool hasSettings = false;
    public bool hasStats = false;
    public bool hasRestart = false;
}; // class

public class GameController : MonoBehaviour 
{
	public TimedLevel[] levels;
	public int gameOverWonLevel;
	public int gameOverLostLevel;
	public Text scoreText;
    public Button statsButton;
    public Button settingsButton;
    public Button restartButton;
    public GameObject livesModel;
	public GameObject player;
	public GameObject bomb;
    public CanvasGroup settingsCanvasGroup;
    public CanvasGroup statsCanvasGroup;
    public TextMeshProUGUI difficultyText;
    public MessageManager messageManager;
    public Stats stats;
    public Stats previousStats;
    public GameSettings gameSettings;

    private bool playerDeath = false;
	private float startTime;
	private float endTime;
	private float nextLifeXLocation;
	private GameObject spawnedPlayer;
	private List<GameObject> dynamicallyAllocaredObjects = new List<GameObject>();
    private BadgeManager badgeManager;

    void Start()
	{
		Debug.Log("GameController::Start: Entering Start. GLOBALS.lives = " + GLOBALS.lives);
        badgeManager = GetComponent<BadgeManager>();
        for (int loop = 0; loop < levels.Length; loop++)
		{
			if (null != levels[loop].openingImage)
			{
				levels[loop].openingImage.enabled = false;
			} // if
			if (null != levels[loop].music)
			{
				levels[loop].music.Stop();
			} // if
		} // for
		if (0 < GLOBALS.lives)
		{
            startTime = Time.time;
			endTime = startTime + 125;
			nextLifeXLocation = -6.0f;
			RunLevel();
		} // if

        
        Stats combinedStats = stats.CombineStats(previousStats);
        stats.score = 0;
        stats.coin = combinedStats.coin;
        stats.lastDayPlayed = combinedStats.lastDayPlayed;
        stats.consecutiveDaysPlayingCount = combinedStats.consecutiveDaysPlayingCount;
        stats.killedSuperbrainCount = combinedStats.killedSuperbrainCount;
        stats.killedBySuperbrainCount = combinedStats.killedBySuperbrainCount;
        Debug.Log("previousStats.consecutiveDaysPlayingCount = " + previousStats.consecutiveDaysPlayingCount);
        Debug.Log("stats.consecutiveDaysPlayingCount = " + stats.consecutiveDaysPlayingCount);

        UpdateScore();
        UpdateLives();
        HideSettings();
		Debug.Log("GameController::Start: Exiting Start.");
	} // Start

    public void RunLevel()
	{
		Debug.Log("GameController::RunLevel: Entering RunLevel. GLOBALS.currentLevel = " + GLOBALS.currentLevel + ".");
		if (levels.Length <= GLOBALS.currentLevel)
		{
			return;
		} // if
		StartCoroutine(RunLevelCoroutine());
		Debug.Log("GameController::RunLevel: Exiting RunLevel.");
	} // RunLevel

    IEnumerator RunLevelCoroutine()
    {
        Debug.Log("GameController::RunLevelCoroutine: Entering function.");
        int currentLevel = GLOBALS.currentLevel;

        UpdateButtons(currentLevel);

        if (levels.Length <= GLOBALS.currentLevel)
		{
			Debug.Log("GameController::RunLevelCoroutine: GLOBALS.currentLevel " + GLOBALS.currentLevel + " larger than levels.Length " + levels.Length + ". Exiting function.");
			yield break;
		} // if

		endTime = Time.time + levels[GLOBALS.currentLevel].levelDurationInSeconds;
		playerDeath = false;
		Debug.Log("GameController::RunLevelCoroutine: Starting level " + currentLevel + " with endTime " + endTime + ".");
		if (null != levels[currentLevel].music)
		{
			Debug.Log("GameController::RunLevelCoroutine: Playing music.");
			levels[currentLevel].music.enabled = true;
			levels[currentLevel].music.Play();
		} // if
		if (null != levels[currentLevel].openingImage)
		{
			Debug.Log("GameController::RunLevelCoroutine: Displaying opening image.");
			levels[currentLevel].openingImage.enabled = true;
			yield return new WaitForSeconds(levels[currentLevel].openingImageDurationInSeconds);
			Debug.Log("GameController::RunLevelCoroutine: Hiding opening image.");
			levels[currentLevel].openingImage.enabled = false;
		} // if
		if (levels[currentLevel].spawnPlayer)
		{
			Debug.Log("GameController::RunLevelCoroutine: Spawning player.");
			spawnedPlayer = Instantiate(player, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity) as GameObject;
			dynamicallyAllocaredObjects.Add(spawnedPlayer);
			if (null == spawnedPlayer)
			{
				Debug.Log("GameController::RunLevelCoroutine: Spawning player failed.");
			} // if
		} // if
		UpdateLives();
        ShowLevelMessages(currentLevel);

        while (Time.time < endTime)
		{
            int spawnIndex = 0;

			while (true)
			{
				bool didSpawn = false;
                int spawnMultiplier1 = 1;   // 3
                int spawnMultiplier2 = 1;   // 2
                if (GameDifficulty.Medium == gameSettings.difficulty)
                {
                    spawnMultiplier1 = 2;
                }
                else if (GameDifficulty.Hard == gameSettings.difficulty)
                {
                    spawnMultiplier1 = 3;
                    spawnMultiplier2 = 2;
                }

                Debug.Log("GameController::RunLevelCoroutine: Spawning enemies. Enemy count is " + levels[currentLevel].hazards.Length + ".");
                Debug.Log("GameController::RunLevelCoroutine: Spawn multiplier are: spawnMultiplier1: " + spawnMultiplier1 + ", spawnMultiplier2: " + spawnMultiplier2 + ".");
                for (int loop = 0; loop < levels[currentLevel].hazards.Length; loop++)
				{
					if (spawnIndex < (levels[currentLevel].hazards[loop].spawnSize * spawnMultiplier1))
					{
                        for (int loop2 = 0; loop2 < spawnMultiplier2; loop2++) {
                            Vector3 spawnBounds = levels[currentLevel].hazards[loop].spawnBounds;
                            Vector3 spawnPosition = new Vector3(Random.Range(-spawnBounds.x, spawnBounds.x), spawnBounds.y, spawnBounds.z);
                            Vector3 spawnRotation = levels[currentLevel].hazards[loop].hazardRotation;
                            Quaternion spawnQuaternion = Quaternion.Euler(spawnRotation.x, spawnRotation.y, spawnRotation.z);
                            GameObject hazard = Instantiate(levels[currentLevel].hazards[loop].hazardObject, spawnPosition, spawnQuaternion) as GameObject;

                            dynamicallyAllocaredObjects.Add(hazard);
                            didSpawn = true;
                        }
						yield return new WaitForSeconds(0.5f);
					} // if
				} // for
				if (!didSpawn)
				{
					break; // while
				} // if
				spawnIndex++;

            } // while
            Stats combinedStats = stats.CombineStats(previousStats);
            badgeManager.UpdateCustomerBadgesUsingStats(combinedStats);

            yield return new WaitForSeconds(levels[currentLevel].spawnWait);
		} // while

		if (0 <= levels[currentLevel].endLevelWait)
		{
			Debug.Log("GameController::RunLevelCoroutine: End level wait: " + levels[currentLevel].endLevelWait + ".");
			yield return new WaitForSeconds(levels[currentLevel].endLevelWait);
		} // if
		if (null != levels[currentLevel].music)
		{
			Debug.Log("GameController::RunLevelCoroutine: Stopping music.");
			levels[currentLevel].music.Stop();
		} // if
		Debug.Log("GameController::RunLevelCoroutine: Destroying dynamic objects.");
		DestroyDynamicObjects();

		if (LevelChaining.EndGame != levels[currentLevel].levelChaining)
	    {
		    if ((LevelChaining.LoadNextLevel == levels[currentLevel].levelChaining)
		    && (!playerDeath))
			{
				GLOBALS.currentLevel++;
				Debug.Log("GameController::RunLevelCoroutine: Incrimented level number to " + GLOBALS.currentLevel + ".");
			} // if
			if (playerDeath)
			{
				levels[GLOBALS.currentLevel].levelChaining = LevelChaining.LoadNextLevel;
				playerDeath = false;
				Debug.Log("GameController::RunLevelCoroutine: Level chaining and playerDeath reset to LevelChaining.LoadNextLevel after processing player death.");
			} // if
			if (LevelChaining.LastLevel != levels[currentLevel].levelChaining)
			{
				Debug.Log("GameController::RunLevelCoroutine: Loading level number " + GLOBALS.currentLevel + ".");
				RunLevel();
			} // if
			else if (LevelChaining.LastLevel == levels[currentLevel].levelChaining)
			{
				Debug.Log("GameController::RunLevelCoroutine: Calling GameOver().");
				GameOver();
			} // else if
		} // if

        Debug.Log("GameController::RunLevelCoroutine: Ending level " + currentLevel);
	} // RunLevelCoroutine
	
	public void SpawnDrop(Transform inLocation)
	{
		Debug.Log("GameController::SpawnDrop: Drop location is X: " + inLocation.position.x 
		          + ", Y: " + inLocation.position.y 
		          + ", Z: " + inLocation.position.z + ".");
		int currentLevel = GLOBALS.currentLevel;

		Debug.Log("GameController::SpawnDrop: Drop chance is " + levels[currentLevel].dropChance + ".");
		if (Random.value <= levels[currentLevel].dropChance)
		{
			float precentageRoll = Random.Range(0, 101);
			Vector2 dropVector = new Vector2(inLocation.position.x, inLocation.position.z);

			Debug.Log("GameController::SpawnDrop: Drop selector percentage is " + precentageRoll + ".");
			for (int loop = 0; loop < levels[currentLevel].dropTable.Length; loop++)
			{
				Debug.Log("GameController::SpawnDrop: Drop " + loop + " percentage is " + levels[currentLevel].dropTable[loop].percentage + ".");
				Debug.Log("GameController::SpawnDrop: Drop " + loop + " dropRect is X: " + levels[currentLevel].dropTable[loop].dropRect.x 
				          + ", Y: " + levels[currentLevel].dropTable[loop].dropRect.y
				          + ", Width: " + levels[currentLevel].dropTable[loop].dropRect.width
				          + ", Height: " + levels[currentLevel].dropTable[loop].dropRect.height + ".");

				if ((precentageRoll <= levels[currentLevel].dropTable[loop].percentage)
				&& (levels[currentLevel].dropTable[loop].dropRect.Contains(dropVector)))
				{
					GameObject drop = Instantiate(levels[currentLevel].dropTable[loop].drop, 
					                              inLocation.position, Quaternion.Euler(-90, -180, 0))
													as GameObject;

					dynamicallyAllocaredObjects.Add(drop);
					break; // for
				} // if
			} // for
		} // if
	} // SpawnDrop

	void DestroyDynamicObjects()
	{
		Debug.Log("GameController::DestroyDynamicObjects: Entering DestroyDynamicObjects. dynamicallyAllocaredObjects.Count = " + dynamicallyAllocaredObjects.Count);
		for (int loop = dynamicallyAllocaredObjects.Count - 1; loop >= 0; loop--)
		{
			if (null != dynamicallyAllocaredObjects[loop])
			{
				Debug.Log("GameController::DestroyDynamicObjects: Destroying object (tag = " + dynamicallyAllocaredObjects[loop].tag + ").");
				Destroy(dynamicallyAllocaredObjects[loop]);
				dynamicallyAllocaredObjects.RemoveAt(loop);
			} // if
		} // for
		Debug.Log("GameController::DestroyDynamicObjects: Exiting DestroyDynamicObjects. dynamicallyAllocaredObjects.");
	} // DestroyDrops

	GameObject SpawnLifeIcon()
	{
		Vector3 position = new Vector3(nextLifeXLocation, 1.0f, 13.5f);
		GameObject livesIcon = Instantiate(livesModel, position, Quaternion.Euler(-90, -180, 0)) as GameObject;

		dynamicallyAllocaredObjects.Add (livesIcon);
		nextLifeXLocation += 1.0f;
		return livesIcon;
	} // SpawnLifeIcon

	void UpdateScore()
	{
        scoreText.text = "Score: " + stats.score;
	} // UpdateScore

	void UpdateLives()
	{
		nextLifeXLocation = -6.0f;
		for (int loop2 = 0; loop2 < GLOBALS.lives - 1; loop2++)
		{
			dynamicallyAllocaredObjects.Add(SpawnLifeIcon());
		} // for
	} // UpdateLives

	public void AddScore(int inNewScoreAmount)
	{
		stats.score += inNewScoreAmount;
		UpdateScore();
	} // AddScore
	
	public void AddLife()
	{
		if (GLOBALS.lives < GLOBALS.maxLives)
		{
			GLOBALS.lives++;
			SpawnLifeIcon();
		} // if
	} // AddLife
	
	public void AddShield(GameObject inShield)
	{
		Debug.Log("GameController::AddShield: Entering AddShield. Shield tag is " + inShield.tag);
		DeactivateByTime deactivateByTime = inShield.GetComponent<DeactivateByTime>();

		if (null == deactivateByTime)
		{
			Debug.Log("GameController::AddShield: DeactivateByTime script is null.");
		} // if

		deactivateByTime.ResetTime();	
		deactivateByTime.ResetObjectsActivateState();	
		Debug.Log("GameController::AddShield: Exiting AddShield. Shield's active state is " + inShield.activeSelf + " and its activeInHierarchy state is " + inShield.activeInHierarchy + ".");
	} // AddShield
	
	public void AddBomb()
	{
		for (int loop = 0; loop < 4; loop++)
		{
			int x = Mathf.RoundToInt(Random.Range(-6, 7));
			int z = Mathf.RoundToInt(Random.Range(3, 13));
			GameObject newBomb = Instantiate(bomb, new Vector3(x, 0.0f, z), Quaternion.identity) as GameObject;

			dynamicallyAllocaredObjects.Add(newBomb);
		} // for
	} // AddBomb
	

	public void AddDime()
	{
		stats.coin += 10;
        ShowMessage(Messages.MSG_RECEIVED_STORE_CREDIT);
	} // AddDime

	public void PlayerDeath()
	{
		GLOBALS.lives--;
		Debug.Log("GameController::PlayerDeath: Decrimented lives count to " + GLOBALS.lives);

		if (0 >= GLOBALS.lives)
		{
			Debug.Log("GameController::PlayerDeath: Player has no more lives. Ending game.");
			levels[GLOBALS.currentLevel].levelChaining = LevelChaining.EndGame;
			playerDeath = true;
			GameOver();
		} // if
		else
		{
			Debug.Log("GameController::PlayerDeath: Player has more lives remaining. Resetting current level.");
			endTime = Time.time;
			playerDeath = true;
		} // else
	} // PlayerDeath
	
	void GameOver()
	{
        if (playerDeath)
		{
			Debug.Log("GameController::GameOver: Setting level to gameOverLostLevel (" + gameOverLostLevel + ").");
			GLOBALS.currentLevel = gameOverLostLevel;
			stats.killedBySuperbrainCount++;
		} // if
		else
		{
			Debug.Log("GameController::GameOver: Setting level to gameOverWonLevel (" + gameOverWonLevel + ").");
			GLOBALS.currentLevel = gameOverWonLevel;
			stats.killedSuperbrainCount++;
		} // else
		Debug.Log("GameController::GameOver: Running next level.");
        previousStats.score = stats.score + previousStats.score;
        previousStats.coin = stats.coin;
        previousStats.lastDayPlayed = stats.lastDayPlayed;
        previousStats.consecutiveDaysPlayingCount = stats.consecutiveDaysPlayingCount;
        previousStats.killedSuperbrainCount = stats.killedSuperbrainCount;
        previousStats.killedBySuperbrainCount = stats.killedBySuperbrainCount;
        RunLevel();
	} // GameOver

	public void GameRestart()
	{
		Debug.Log("GameController::GameRestart: Entering function.");
		GLOBALS.lives = 3;
		GLOBALS.currentLevel = 3;
        stats.score = 0;
        Application.LoadLevel(Application.loadedLevelName);
		Debug.Log("GameController::GameRestart: Exiting function.");
	} // GameRestart

    public void ShowSettings()
    {
        settingsCanvasGroup.alpha = 1;
        settingsCanvasGroup.interactable = true;
        settingsCanvasGroup.blocksRaycasts = true;
        Time.timeScale = 0;
        if (null != levels[GLOBALS.currentLevel].music)
        {
            levels[GLOBALS.currentLevel].music.Pause();
        }
    }

    public void HideSettings()
    {
        settingsCanvasGroup.alpha = 0;
        settingsCanvasGroup.interactable = false;
        settingsCanvasGroup.blocksRaycasts = false;
        Time.timeScale = 1;
        if (null != levels[GLOBALS.currentLevel].music)
        {
            levels[GLOBALS.currentLevel].music.Play();
        }
    }

    public void UpdateSettings()
    {
        Debug.Log("GameController::UpdateSettings: difficulty: " + difficultyText.text);
        if ("Hard" == difficultyText.text)
        {
            gameSettings.difficulty = GameDifficulty.Hard;
        }
        else if ("Medium" == difficultyText.text)
        {
            gameSettings.difficulty = GameDifficulty.Medium;
        }
        else
        {
            gameSettings.difficulty = GameDifficulty.Easy;
        }
        HideSettings();
        ShowMessage(Messages.MSG_GAME_SETTINGS_UPDATED);
    }

    public void ShowMessage(string inMessage)
    {
        if (null == messageManager)
        {
            Debug.Log("GameController::ShowMessage: MessageManager is null.");
            return;
        }
        messageManager.ShowMessage(inMessage);
    }

    public void ShowLevelMessages(int inLevel)
    {
        if (2 == inLevel)
        {
            ShowMessage(Messages.MSG_HOW_TO_MOVE_SHIP);
            ShowMessage(Messages.MSG_HOW_TO_FIRE);
        }
    }

    public void ShowStats()
    {
        statsCanvasGroup.alpha = 1;
        statsCanvasGroup.interactable = true;
        statsCanvasGroup.blocksRaycasts = true;
        Time.timeScale = 0;
        levels[GLOBALS.currentLevel].music.Pause();
    }

    public void HideStats()
    {
        statsCanvasGroup.alpha = 0;
        statsCanvasGroup.interactable = false;
        statsCanvasGroup.blocksRaycasts = false;
        Time.timeScale = 1;
        levels[GLOBALS.currentLevel].music.Play();
    }

    public void UpdateButtons(int currentLevel)
    {
        if (null != settingsButton)
        {
            if (false == levels[currentLevel].hasSettings)
            {
                settingsButton.enabled = false;
                settingsButton.image.enabled = false;
                settingsButton.GetComponentInChildren<Text>().text = "";
            }
            else
            {
                settingsButton.enabled = true;
                settingsButton.image.enabled = true;
                settingsButton.GetComponentInChildren<Text>().text = "Settings";
            }
        }
        if (null != restartButton)
        {
            if (false == levels[currentLevel].hasRestart)
            {
                restartButton.enabled = false;
                restartButton.image.enabled = false;
                restartButton.GetComponentInChildren<Text>().text = "";
            }
            else
            {
                restartButton.enabled = true;
                restartButton.image.enabled = true;
                restartButton.GetComponentInChildren<Text>().text = "Restart";
            }
        }
        if (null != statsButton)
        {
            if (false == levels[currentLevel].hasStats)
            {
                statsButton.enabled = false;
                statsButton.image.enabled = false;
                statsButton.GetComponentInChildren<Text>().text = "";
            }
            else
            {
                statsButton.enabled = true;
                statsButton.image.enabled = true;
                statsButton.GetComponentInChildren<Text>().text = "Stats";
            }
        }
    }
} // class
