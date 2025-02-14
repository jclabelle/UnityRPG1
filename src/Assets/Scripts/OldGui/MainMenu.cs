using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour, IDataManager
{


    int buttonWidth = (int)(Screen.width * 0.16);
    int buttonHeight = (int)(Screen.height * 0.05);

    int buttonSpacingY = 100;

    int x;
    int y;

    GUIStyle buttonStyle;
    [SerializeField] private AudioClip buttonPressSFX;
    [SerializeField] private AudioClip navigateBackSFX;
    [SerializeField] private AudioClip deleteSFX;
    [SerializeField] private AudioClip music;
    private AudioSource sfx;


    State state = State.MainScreen;

    string playerName = "DefaultName";

    enum State
    {
        MainScreen,
        NewGameScreen,
        WaitLoadingScreen,
        LoadGameScreen,
    }

    DataManager dataManager;
    public DataManager DataMngr { get => dataManager; set => dataManager = value; }

    public void SetDataManager()
    {
        dataManager = FindObjectOfType<DataManager>();
    }

    private void Awake()
    {
        SetDataManager();
    }

    // Start is called before the first frame update
    void Start()
    {

        state = State.MainScreen;
        sfx = gameObject.GetComponent<AudioSource>();

        UnityEngine.Assertions.Assert.IsTrue(sfx is not null
                                             && DataMngr is not null);

        sfx.clip = music;
        sfx.Play();

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnGUI()
    {
        buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize = 36;

        if (state == State.MainScreen)
        {

            x = (int)(((float)Screen.width / 2) - ( (float)buttonWidth / 2));
            y = (int)(((float)Screen.height / 2) - ((float)buttonHeight / 2));

            if (GUI.Button(new Rect(x, y, buttonWidth, buttonHeight), "New Game", buttonStyle))
            {
                state = State.NewGameScreen;
                sfx.PlayOneShot(buttonPressSFX, 0.5f);
            }

            if (GUI.Button(new Rect(x, y+buttonSpacingY, buttonWidth, buttonHeight), "Load Game", buttonStyle))
            {
                state = State.LoadGameScreen;
                sfx.PlayOneShot(buttonPressSFX, 0.5f);

            }
        }

        if(state == State.NewGameScreen)
        {
            x = (int)(((float)Screen.width / 2) - ((float)buttonWidth / 2));
            y = (int)(((float)Screen.height / 10) - ((float)buttonHeight / 2));

            var labelStyle = CustomTools.GUITools.CreateStyle(48, Color.black, true, TextAnchor.MiddleLeft);
            GUI.Label(new Rect(x-100, y, buttonWidth*2, buttonHeight), "Enter your name:", labelStyle);

            var textStyle = CustomTools.GUITools.GetDefaultStyle("textfield");
            textStyle.fontSize = 36;
            textStyle.alignment = TextAnchor.MiddleLeft;
            playerName = GUI.TextField(new Rect(x + buttonWidth, y, buttonWidth, buttonHeight), playerName, textStyle);

            int spacingIncrements = 1;

            
            if (GUI.Button(new Rect(x, y + buttonSpacingY * spacingIncrements, buttonWidth, buttonHeight), DataMngr.SaveSlotOne, buttonStyle))
            {
                state = State.WaitLoadingScreen;
                sfx.PlayOneShot(buttonPressSFX, 0.5f);
                DataMngr.NewGame(playerName, DataMngr.SaveSlotOne);
                UnityEngine.SceneManagement.SceneManager.LoadScene("Village");
            }

            if (GUI.Button(new Rect(x + x/2, y + buttonSpacingY * spacingIncrements, buttonWidth, buttonHeight), "UI Dev in SS1", buttonStyle))
            {
                state = State.WaitLoadingScreen;
                sfx.PlayOneShot(buttonPressSFX, 0.5f);
                DataMngr.NewGame(playerName, DataMngr.SaveSlotOne);
                UnityEngine.SceneManagement.SceneManager.LoadScene("UIDev");
            }

            spacingIncrements++;
            if (GUI.Button(new Rect(x, y + buttonSpacingY * spacingIncrements, buttonWidth, buttonHeight), DataMngr.SaveSlotTwo, buttonStyle))
            {
                state = State.WaitLoadingScreen;
                sfx.PlayOneShot(buttonPressSFX, 0.5f);
                DataMngr.NewGame(playerName, DataMngr.SaveSlotTwo);
                UnityEngine.SceneManagement.SceneManager.LoadScene("Village");
            }
            spacingIncrements++;

            if (GUI.Button(new Rect(x, y + buttonSpacingY * spacingIncrements, buttonWidth, buttonHeight), DataMngr.SaveSlotThree, buttonStyle))
            {
                state = State.WaitLoadingScreen;
                sfx.PlayOneShot(buttonPressSFX, 0.5f);
                DataMngr.NewGame(playerName, DataMngr.SaveSlotThree);
                UnityEngine.SceneManagement.SceneManager.LoadScene("Village");
            }
            spacingIncrements++;

            if (GUI.Button(new Rect(x, y + buttonSpacingY * spacingIncrements, buttonWidth, buttonHeight), DataMngr.SaveSlotFour, buttonStyle))
            {
                state = State.WaitLoadingScreen;
                sfx.PlayOneShot(buttonPressSFX, 0.5f);
                DataMngr.NewGame(playerName, DataMngr.SaveSlotFour);
                UnityEngine.SceneManagement.SceneManager.LoadScene("Village");
            }
            spacingIncrements++;

            if (GUI.Button(new Rect(x, y + buttonSpacingY * spacingIncrements, buttonWidth, buttonHeight), DataMngr.SaveSlotFive, buttonStyle))
            {
                state = State.WaitLoadingScreen;
                sfx.PlayOneShot(buttonPressSFX, 0.5f);
                DataMngr.NewGame(playerName, DataMngr.SaveSlotFive);
                UnityEngine.SceneManagement.SceneManager.LoadScene("Village");
            }
            spacingIncrements++;

            if (GUI.Button(new Rect(x, y + buttonSpacingY * spacingIncrements, buttonWidth, buttonHeight), DataMngr.SaveSlotSix, buttonStyle))
            {
                state = State.WaitLoadingScreen;
                sfx.PlayOneShot(buttonPressSFX, 0.5f);
                DataMngr.NewGame(playerName, DataMngr.SaveSlotSix);
                UnityEngine.SceneManagement.SceneManager.LoadScene("Village");
            }
            spacingIncrements++;

            if (GUI.Button(new Rect(x, y + buttonSpacingY * spacingIncrements, buttonWidth, buttonHeight), DataMngr.SaveSlotSeven, buttonStyle))
            {
                state = State.WaitLoadingScreen;
                sfx.PlayOneShot(buttonPressSFX, 0.5f);
                DataMngr.NewGame(playerName, DataMngr.SaveSlotSeven);
                UnityEngine.SceneManagement.SceneManager.LoadScene("Village");
            }
            spacingIncrements++;

            if (GUI.Button(new Rect(x, y + buttonSpacingY * spacingIncrements, buttonWidth, buttonHeight), DataMngr.SaveSlotEight, buttonStyle))
            {
                state = State.WaitLoadingScreen;
                sfx.PlayOneShot(buttonPressSFX, 0.5f);
                DataMngr.NewGame(playerName, DataMngr.SaveSlotEight);
                UnityEngine.SceneManagement.SceneManager.LoadScene("Village");
            }
                //todo: add safety check for null if devices not plugged in.
            if (Keyboard.current.escapeKey.wasPressedThisFrame || 
                ((Gamepad.current != null) && Gamepad.current.buttonSouth.wasPressedThisFrame))
            {
                state = State.MainScreen;
                sfx.PlayOneShot(navigateBackSFX, 0.5f);

            }
        }

        if (state == State.LoadGameScreen)
        {
            x = (int)(((float)Screen.width / 2) - ((float)buttonWidth / 2));
            y = (int)(((float)Screen.height / 10) - ((float)buttonHeight / 2));

            int spacingIncrements = 1;

            if (PlayerPrefs.HasKey(DataMngr.SaveSlotOne) && GUI.Button(new Rect(x, y + buttonSpacingY * spacingIncrements, buttonWidth, buttonHeight), dataManager.SaveSlotOne, buttonStyle))
            {
                state = State.WaitLoadingScreen;
                sfx.PlayOneShot(buttonPressSFX, 0.5f);
                DataMngr.LoadGame(dataManager.SaveSlotOne, dataManager);
                UnityEngine.SceneManagement.SceneManager.LoadScene(DataMngr.Player.CurrentWorldScene); 
            }
            spacingIncrements++;
            if (PlayerPrefs.HasKey(DataMngr.SaveSlotTwo) && GUI.Button(new Rect(x, y + buttonSpacingY * spacingIncrements, buttonWidth, buttonHeight), dataManager.SaveSlotTwo, buttonStyle))
            {
                state = State.WaitLoadingScreen;
                sfx.PlayOneShot(buttonPressSFX, 0.5f);
                DataMngr.LoadGame(dataManager.SaveSlotTwo, dataManager);
                UnityEngine.SceneManagement.SceneManager.LoadScene(DataMngr.Player.CurrentWorldScene);
            }
            spacingIncrements++;

            if (PlayerPrefs.HasKey(DataMngr.SaveSlotThree) && GUI.Button(new Rect(x, y + buttonSpacingY * spacingIncrements, buttonWidth, buttonHeight), dataManager.SaveSlotThree, buttonStyle))
            {
                state = State.WaitLoadingScreen;
                sfx.PlayOneShot(buttonPressSFX, 0.5f);
                DataMngr.LoadGame(dataManager.SaveSlotThree, dataManager);
                UnityEngine.SceneManagement.SceneManager.LoadScene(DataMngr.Player.CurrentWorldScene);
            }
            spacingIncrements++;

            if (PlayerPrefs.HasKey(DataMngr.SaveSlotFour) && GUI.Button(new Rect(x, y + buttonSpacingY * spacingIncrements, buttonWidth, buttonHeight), dataManager.SaveSlotFour, buttonStyle))
            {
                state = State.WaitLoadingScreen;
                sfx.PlayOneShot(buttonPressSFX, 0.5f);
                DataMngr.LoadGame(dataManager.SaveSlotFour, dataManager);
                UnityEngine.SceneManagement.SceneManager.LoadScene(DataMngr.Player.CurrentWorldScene);
            }
            spacingIncrements++;

            if (PlayerPrefs.HasKey(DataMngr.SaveSlotFive) && GUI.Button(new Rect(x, y + buttonSpacingY * spacingIncrements, buttonWidth, buttonHeight), dataManager.SaveSlotFive, buttonStyle))
            {
                state = State.WaitLoadingScreen;
                sfx.PlayOneShot(buttonPressSFX, 0.5f);
                DataMngr.LoadGame(dataManager.SaveSlotFive, dataManager);
                UnityEngine.SceneManagement.SceneManager.LoadScene(DataMngr.Player.CurrentWorldScene);
            }
            spacingIncrements++;

            if (PlayerPrefs.HasKey(DataMngr.SaveSlotSix) && GUI.Button(new Rect(x, y + buttonSpacingY * spacingIncrements, buttonWidth, buttonHeight), dataManager.SaveSlotSix, buttonStyle))
            {
                state = State.WaitLoadingScreen;
                sfx.PlayOneShot(buttonPressSFX, 0.5f);
                DataMngr.LoadGame(dataManager.SaveSlotSix, dataManager);
                UnityEngine.SceneManagement.SceneManager.LoadScene(DataMngr.Player.CurrentWorldScene);
            }
            spacingIncrements++;

            if (PlayerPrefs.HasKey(DataMngr.SaveSlotSeven) && GUI.Button(new Rect(x, y + buttonSpacingY * spacingIncrements, buttonWidth, buttonHeight), dataManager.SaveSlotSeven, buttonStyle))
            {
                state = State.WaitLoadingScreen;
                sfx.PlayOneShot(buttonPressSFX, 0.5f);
                DataMngr.LoadGame(dataManager.SaveSlotSeven, dataManager);
                UnityEngine.SceneManagement.SceneManager.LoadScene(DataMngr.Player.CurrentWorldScene);
            }
            spacingIncrements++;

            if (PlayerPrefs.HasKey(DataMngr.SaveSlotEight) && GUI.Button(new Rect(x, y + buttonSpacingY * spacingIncrements, buttonWidth, buttonHeight), dataManager.SaveSlotEight, buttonStyle))
            {
                state = State.WaitLoadingScreen;
                sfx.PlayOneShot(buttonPressSFX, 0.5f);
                DataMngr.LoadGame(dataManager.SaveSlotEight, dataManager);
                UnityEngine.SceneManagement.SceneManager.LoadScene(DataMngr.Player.CurrentWorldScene);
            }

            if(GUI.Button(new Rect(x + (buttonWidth * 2) , y + buttonSpacingY, buttonWidth, buttonHeight), "Wipe PlayerPrefs", buttonStyle))
            {
                PlayerPrefs.DeleteAll();
            }

            if (Keyboard.current.escapeKey.wasPressedThisFrame || Gamepad.current.buttonSouth.wasPressedThisFrame)
            {
                sfx.PlayOneShot(navigateBackSFX, 0.5f);
                state = State.MainScreen;
            }
        }
        if (state == State.WaitLoadingScreen)
        {

        }

    }

}