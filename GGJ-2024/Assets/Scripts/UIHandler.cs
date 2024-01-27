using System.Collections;
using System.Collections.Generic;
using FishNet.Demo.AdditiveScenes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.Serialization;
using UnityEngine.UI;

enum ACTIVE_SCREEN
{
    START,
    CHARACTER,
    LOADING,
    IN_GAME,
    END_GAME
}

public class UIHandler : MonoBehaviour
{
    [Header("Menu Background")]
    [SerializeField] public GameObject menuBackground;
    
    [Header("Start Menu")]
    [SerializeField] public GameObject startMenu;
    
    [Header("Character Menu")]
    [SerializeField] public GameObject characterMenu;
    [Space]
    [SerializeField] public Button leftButton;
    [SerializeField] public Button rightButton;
    
    [Header("Loading Screen")]
    [SerializeField] public GameObject loadingScreen;
    
    [Header("In Game UI Objects")]
    [SerializeField] public GameObject inGameScreen;
    
    [Header("End Game Screen")]
    [SerializeField] public GameObject endGameScreen;

    [Header("Audio Sources")]
    [SerializeField] public AudioSource firstTrack;
    [SerializeField] public AudioSource secondTrack;
    [SerializeField] public AudioSource thirdTrack;
    [SerializeField] public AudioSource fourthTrack;
    [SerializeField] public AudioSource fifthTrack;

    private List<GameObject> _screens;
    private List<AudioSource> _tracks;
    
    private ACTIVE_SCREEN _currentActiveScreen = ACTIVE_SCREEN.START;
    
    private GameObject _currentScreenObject;
    private AudioSource _currentTrack;

    // Start is called before the first frame update
    void Start()
    {
        // Define lists for easily manipulating all objects at once (for muting, setting active ect..).
        _screens = new List<GameObject>() { startMenu, characterMenu, loadingScreen, inGameScreen, endGameScreen };
        _tracks = new List<AudioSource>() { firstTrack, secondTrack, thirdTrack, fourthTrack, fifthTrack };

        // Ensure all audio sources are looping & muted.
        foreach (var track in _tracks)
        {
            track.loop = true;
            track.mute = true;
        }
        
        // Ensure all screen objects are inactive
        foreach (var screen in _screens)
        {
            screen.SetActive(false);
        }

        menuBackground.SetActive(true);
        _currentScreenObject = startMenu;
        _currentTrack = firstTrack;
        
        // Start off with start screen.
        SwitchMenu(ACTIVE_SCREEN.START);
    }

    // Update is called once per frame
    void Update()
    {
        switch (_currentActiveScreen)
        {
            case (ACTIVE_SCREEN.START):
                StartMenu();
                break;
            case (ACTIVE_SCREEN.CHARACTER):
                CharacterMenu();
                break;
            case (ACTIVE_SCREEN.LOADING):
                break;
            case (ACTIVE_SCREEN.IN_GAME):
                break;
            case (ACTIVE_SCREEN.END_GAME):
                break;
        }
    }

    void StartMenu()
    {
        //Wait for player to hit a key to progress.
        if (Input.anyKeyDown)
        {
            SwitchMenu(ACTIVE_SCREEN.CHARACTER);
        }
    }

    private bool leftButtonIsClicked = false;
    private bool rightButtonIsClicked = false;

    void CharacterMenu()
    {
        if (leftButtonIsClicked && rightButtonIsClicked)
        {
            SwitchMenu(ACTIVE_SCREEN.LOADING);
        }
    }

    public void LeftButtonClick()
    {
        leftButtonIsClicked = true;
        ButtonClicked(leftButton);
    }

    public void RightButtonClick()
    {
        rightButtonIsClicked = true;
        ButtonClicked(rightButton);
    }

    void ButtonClicked(Button buttonClicked)
    {
        buttonClicked.interactable = false;
    }
    
    /// <summary>
    /// Call this to switch between UI in scene.
    /// </summary>
    void SwitchMenu(ACTIVE_SCREEN newScreen)
    {
        _currentActiveScreen = newScreen;
        
        switch (newScreen)
        {
            case (ACTIVE_SCREEN.START):
                SwitchUI(startMenu);
                SwitchTracks(firstTrack);
                break;
            case (ACTIVE_SCREEN.CHARACTER):
                SwitchUI(characterMenu);
                SwitchTracks(secondTrack);
                break;
            case (ACTIVE_SCREEN.LOADING):
                SwitchUI(loadingScreen);
                SwitchTracks(thirdTrack);
                break;
            case (ACTIVE_SCREEN.IN_GAME):
                SwitchUI(inGameScreen);
                SwitchTracks(fourthTrack);
                menuBackground.SetActive(false);
                break;
            case (ACTIVE_SCREEN.END_GAME):
                SwitchUI(endGameScreen);
                SwitchTracks(fifthTrack);
                menuBackground.SetActive(true);
                break;
        }
    }
    
    /// <summary>
    /// Toggles between UI objects and updates the _currentUIObject.
    /// </summary>
    /// <param name="activeUI"> UI object to be made active. </param>
    void SwitchUI(GameObject activeUI)
    {
        _currentScreenObject.SetActive(false);
        activeUI.SetActive(true);
        _currentScreenObject = activeUI;
    }

    /// <summary>
    /// Toggles between audio tracks and updates the _currentTrack.
    /// </summary>
    /// <param name="activeTrack"> Track object to be made active. </param>
    void SwitchTracks(AudioSource activeTrack)
    {
        _currentTrack.mute = true;
        activeTrack.mute = false;
        _currentTrack = activeTrack;
    }
}
