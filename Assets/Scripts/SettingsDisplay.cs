using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsDisplay : MonoBehaviour {
    public GameSettings settings;
    public TMP_Dropdown difficultyDropdown;
    public Slider mouseHorizontalSlider;
    public Slider mouseVerticalSlider;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if ((null == settings)
        || (null == difficultyDropdown)
        || (null == mouseHorizontalSlider)
        || (null == mouseVerticalSlider))
        {
            return;
        }
        if (0 == mouseHorizontalSlider.value) settings.difficulty = GameDifficulty.Easy;
        else if (1 == mouseHorizontalSlider.value) settings.difficulty = GameDifficulty.Medium;
        else if (2 == mouseHorizontalSlider.value) settings.difficulty = GameDifficulty.Hard;
        GameSettings.horizonalMouseSensitivity = mouseHorizontalSlider.value;
        GameSettings.verticalMouseSensitivity = mouseVerticalSlider.value;
    }
}
