using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using FishNet.Object;
using FishNet.Connection;
using FishNet.Object.Synchronizing;

enum ACTIVE_SCREEN
{
    START,
    CHARACTER,
    LOADING,
    IN_GAME,
    END_GAME
}

public class UIHandler : NetworkBehaviour
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
    [SerializeField] public float waitTime = 3f;
    
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
    [SerializeField] public AudioSource sixthTrack;

    private List<GameObject> _screens;
    private List<AudioSource> _tracks;
    
    private ACTIVE_SCREEN _currentActiveScreen = ACTIVE_SCREEN.START;
    
    private GameObject _currentScreenObject;
    private AudioSource _currentTrack;

    public GameObject player;

    [SyncVar] public GameObject player1;
    [SyncVar] public GameObject player2;

    [SyncVar] public bool isPlayer1;
    [SyncVar] public bool isPlayer2;


    public override void OnStartClient()
    {
        base.OnStartClient();

        //players = GameObject.FindGameObjectsWithTag("Player");

    }


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
                LoadingScreen();
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

    private bool _leftButtonIsClicked = false;
    private bool _rightButtonIsClicked = false;

    void CharacterMenu()
    {
        if (_leftButtonIsClicked && _rightButtonIsClicked)
        {
            SwitchMenu(ACTIVE_SCREEN.LOADING);
        }
    }

    public void LeftButtonClick()
    {
        _leftButtonIsClicked = true;

        // turns blue on locally
        player.transform.GetChild(1).gameObject.SetActive(true);
        if (isPlayer1)
        {
            RPCUpdatePlayer2(1);
        }
        else if (isPlayer2)
        {
            RPCUpdatePlayer1(1);
        }

        RPCUpdateLButtonClicked();

    }

    public void RightButtonClick()
    {
        _rightButtonIsClicked = true;

        // turns red on locally
        player.transform.GetChild(0).gameObject.SetActive(true);
        if (isPlayer1)
        {
            RPCUpdatePlayer2(0);
        }
        else if (isPlayer2)
        {
            RPCUpdatePlayer1(0);
        }

        RPCUpdateRButtonClicked();

    }

    [ServerRpc(RequireOwnership = false)] private void RPCUpdateLButtonClicked() { LButtonClicked(); }

    [ObserversRpc]
    void LButtonClicked() // red
    {
        player.transform.GetChild(1).gameObject.SetActive(true);
        leftButton.interactable = false;
    }

    [ServerRpc(RequireOwnership = false)] private void RPCUpdateRButtonClicked() { RButtonClicked(); }

    [ObserversRpc]
    void RButtonClicked() // blue
    {
        player.transform.GetChild(0).gameObject.SetActive(true);
        rightButton.interactable = false;
    }

    [ServerRpc(RequireOwnership = false)] private void RPCUpdatePlayer1(int num) { UpdatePlayer1(num); }

    [ObserversRpc]
    void UpdatePlayer1(int num) // red
    {
        player1.transform.GetChild(num).gameObject.SetActive(true);
        
    }

    [ServerRpc(RequireOwnership = false)] private void RPCUpdatePlayer2(int num) { UpdatePlayer2(num); }

    [ObserversRpc]
    void UpdatePlayer2(int num) // blue
    {
        player2.transform.GetChild(num).gameObject.SetActive(true);
    }







    private float _elapsedTime = 0f;
    void LoadingScreen()
    {
        if (_elapsedTime >= waitTime)
        {
            SwitchMenu(ACTIVE_SCREEN.IN_GAME);
        }
        else
        {
            _elapsedTime += Time.deltaTime;
        }
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
                SwitchTracks(sixthTrack);
                menuBackground.SetActive(true);
                break;
            default:
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
