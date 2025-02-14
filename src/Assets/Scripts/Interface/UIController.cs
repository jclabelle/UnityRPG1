using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;


public class UIController : MonoBehaviour
{
    public enum EInput
    {
        ExplorationUIContext,   // Moving around the world.
        InGameMenu, // the IGM and sub-menus
        Popup, // UI overlaid on top of Explore or Battle
        BattleReactionsContext, // While reactions are available to the player.
        Disabled, // While waiting for anims or cutscenes to play
    }

    public enum EInterface
    {
        ExplorationUIContext,
        ExplorationCutscene,
        PopupMenu,
        InGameMenu,
        InventoryMenu,
        EquipmentMenu,
        AbilitiesMenu,
        ReactionsMenu,
        SettingsMenu,
        BattleSelectReaction,
        BattleSelectMenu,
        Disabled,
    }

    static Texture2D currentIGMBackground;

    [SerializeField]EInterface currentInterface;
    [SerializeField] EInput currentInput;
    Dictionary<string, IGameUserInterface> interfaces;
    PlayerInput pInput;

    public Dictionary<string, IGameUserInterface> Interfaces { get => interfaces; set => interfaces = value; }
    public EInterface CurrentInterface { get => currentInterface; set => currentInterface = value; }
    public EInput CurrentInput { get => currentInput; set => currentInput = value; }
    public PlayerInput PInput { get => pInput; set => pInput = value; }
    public static Texture2D CurrentIGMBackground { get => currentIGMBackground; set => currentIGMBackground = value; }

    public bool PlayerIsChoosingBattleAction =>
            currentInterface 
            is EInterface.BattleSelectMenu;

    private void Awake()
    {
        CurrentInterface = EInterface.ExplorationUIContext;
        CurrentInput = EInput.ExplorationUIContext;
        Interfaces = new Dictionary<string, IGameUserInterface>();

    }

    // Find Player Input and Create a dictionary of a Interfaces in the scene
    // Make all interfaces except Exploration transparent.
    void Start()
    {
        PInput = FindObjectOfType<PlayerInput>();

        UnityEngine.Assertions.Assert.IsTrue(pInput is PlayerInput, "Warning: InputController: PInput is not PlayerInput");
        UnityEngine.Assertions.Assert.IsTrue(pInput is not null, "Warning: InputController: PInput is null");

    }
    


    // Update is called once per frame
    void Update()
    {
        // Debug.Log($"Controls: {currentInput}, UI:{currentInterface}");
        //foreach(var ui in interfaces.Values)
        //{
        //    Debug.Log($"{ui.name}  opacity: {ui.rootVisualElement.style.opacity}");
        //}
    }


    public void Register(string interfaceName, IGameUserInterface gameInterface)
    {
        Interfaces.Add(interfaceName, gameInterface);
    }

    public void UnRegister(string interfaceName, IGameUserInterface gameInterface)
    {
        Interfaces.Remove(interfaceName);
    }


    private void HideAllInterfaces()
    {
        foreach (var ui in Interfaces)
            ui.Value.Hide();
    }

    // PlayerAction callback set in the editor
    public void ChangeContext(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            switch (context.action.name)
            {
                case "OpenIGM":
                    {
                        PInput.SwitchCurrentActionMap(EInput.InGameMenu.ToString());
                        CurrentInput = EInput.InGameMenu;
                        ChangeContext(EInterface.InGameMenu);
                        break;
                    }
                case "CloseIGM":
                    {
                        PInput.SwitchCurrentActionMap(EInput.ExplorationUIContext.ToString());
                        CurrentInput = EInput.ExplorationUIContext;
                        ChangeContext(EInterface.ExplorationUIContext);
                        CurrentIGMBackground = null;    // Flush the BGM so we know to generate a new one the next time the IGM is opened.
                        break;
                    }
                case "Popup":
                    {
                        PInput.SwitchCurrentActionMap(EInput.Popup.ToString());
                        CurrentInput = EInput.Popup;
                        ChangeContext(EInterface.PopupMenu);

                        break;
                    }
                case "BattleReactionsContext":
                    {
                        PInput.SwitchCurrentActionMap(EInput.BattleReactionsContext.ToString());
                        CurrentInput = EInput.BattleReactionsContext;
                        ChangeContext(EInterface.BattleSelectReaction);
                        break;
                    }
                case "Wait":
                    {
                        PInput.SwitchCurrentActionMap(EInput.Disabled.ToString());
                        CurrentInput = EInput.Disabled;
                        ChangeContext(EInterface.Disabled);
                        break;
                    }
            }
        }
    }

    public void ChangeContext(EInterface intr)
    {
        switch (intr)
        {
            case EInterface.ExplorationUIContext:
            {
                    PInput.SwitchCurrentActionMap(EInput.ExplorationUIContext.ToString());
                    CurrentInput = EInput.ExplorationUIContext;
                    CurrentInterface = EInterface.ExplorationUIContext;
                    break;
            }
            case EInterface.PopupMenu:
                {
                    PInput.SwitchCurrentActionMap(EInput.Popup.ToString());
                    CurrentInput = EInput.Popup;
                    CurrentInterface = EInterface.PopupMenu;
                    break;
                }
            case EInterface.ExplorationCutscene:
                {
                    PInput.SwitchCurrentActionMap(EInput.Popup.ToString());
                    CurrentInput = EInput.Popup;
                    CurrentInterface = EInterface.ExplorationCutscene;
                    break;
                }
            case EInterface.InGameMenu:
                {
                    PInput.SwitchCurrentActionMap(EInput.InGameMenu.ToString());
                    CurrentInput = EInput.InGameMenu;
                    CurrentInterface = EInterface.InGameMenu;

                    break;
                }
            case EInterface.InventoryMenu:
                {
                    PInput.SwitchCurrentActionMap(EInput.InGameMenu.ToString());
                    CurrentInput = EInput.InGameMenu;
                    CurrentInterface = EInterface.InventoryMenu;

                    break;
                }
            case EInterface.EquipmentMenu:
                {
                    PInput.SwitchCurrentActionMap(EInput.InGameMenu.ToString());
                    CurrentInput = EInput.InGameMenu;
                    CurrentInterface = EInterface.EquipmentMenu;

                    break;
                }
            case EInterface.AbilitiesMenu:
                {
                    PInput.SwitchCurrentActionMap(EInput.InGameMenu.ToString());
                    CurrentInput = EInput.InGameMenu;
                    CurrentInterface = EInterface.AbilitiesMenu;

                    break;
                }
            case EInterface.ReactionsMenu:
                {
                    PInput.SwitchCurrentActionMap(EInput.InGameMenu.ToString());
                    CurrentInput = EInput.InGameMenu;
                    CurrentInterface = EInterface.ReactionsMenu;

                    break;
                }
            
            case EInterface.BattleSelectMenu:
            {
                PInput.SwitchCurrentActionMap(EInput.Popup.ToString());
                CurrentInput = EInput.Popup;
                CurrentInterface = EInterface.BattleSelectMenu;

                break;
            }
            case EInterface.BattleSelectReaction:
                {
                    PInput.SwitchCurrentActionMap(EInput.BattleReactionsContext.ToString());
                    CurrentInput = EInput.BattleReactionsContext;
                    CurrentInterface = EInterface.BattleSelectReaction;

                    break;
                }
            case EInterface.Disabled:
                {
                    PInput.SwitchCurrentActionMap(EInput.Disabled.ToString());
                    CurrentInput = EInput.Disabled;
                    CurrentInterface = EInterface.Disabled;
                    break;
                }
        }

        ShowCurrentInterface();
    }

    // todo: Add other UIs
    private void ShowCurrentInterface()
    {
        HideAllInterfaces();

        switch (currentInterface)
        {
            case EInterface.ExplorationUIContext:
                {
                    break;
                }
            case EInterface.PopupMenu:
                {
                    Interfaces[EInterface.PopupMenu.ToString()].Show();
                    break;
                }
            //    case EInterface.Explore_Cutscene:
            //        {
            //            Interfaces[EInterface.Explore_Cutscene.ToString()].rootVisualElement.style.opacity = 1;
            //            break;
            //        }
            case EInterface.InGameMenu:
                {
                    if(CurrentIGMBackground == null)
                        StartCoroutine(ShowIGM());
                    else
                        Interfaces[EInterface.InGameMenu.ToString()].Show();

                    break;
                }
            case EInterface.InventoryMenu:
                {
                    Interfaces[EInterface.InventoryMenu.ToString()].Show();
                    break;
                }
            case EInterface.EquipmentMenu:
                {
                    Interfaces[EInterface.EquipmentMenu.ToString()].Show();
                    break;
                }
            case EInterface.AbilitiesMenu:
                {
                    Interfaces[EInterface.AbilitiesMenu.ToString()].Show();
                    break;
                }
            case EInterface.ReactionsMenu:
                {
                    Interfaces[EInterface.ReactionsMenu.ToString()].Show();
                    break;
                }
            case EInterface.BattleSelectMenu:
            {
                Interfaces[EInterface.BattleSelectMenu.ToString()].Show();
                break;
            }
            case EInterface.BattleSelectReaction:
            {
                Interfaces[EInterface.BattleSelectReaction.ToString()].Show();
                break;
            }
        }
    }

    IEnumerator ShowIGM()
    {
        yield return new WaitForEndOfFrame();

        CurrentIGMBackground = CustomTools.GraphicsTools.GetScreenshotNow(false);
        //Tools.TGraphics.SaveImageToDisk(CurrentIGMBackground, false, "From UIController");
        Interfaces[EInterface.InGameMenu.ToString()].Show();
    }
}
