using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class ExplorationUIContext : MonoBehaviour, IGameUserInterface
{
    public IGameUserInterface.ChangeContext NotifyNav { get; set; }
    public UIDocument Menu { get; set; }
    public VisualElement Root { get; set; }

    // public InputAction GamepadRaycast { get; set; }
    public IMovement PlayerMovement { get; set; }
    public IPlayerPosition PlayerPosition { get; set; }
    public GameObject Player { get; set; }
    public UIController uiController;
    

// Start is called before the first frame update
    void Start()
    {
        // (this as IDataManager).SetDataManager();
        PlayerMovement = FindObjectOfType<PlayerControllerV2>() as IMovement;
        UnityEngine.Assertions.Assert.IsNotNull(PlayerMovement, "Warning: PlayerMovement in ExplorationUIContext is null");
        PlayerPosition = FindObjectOfType<PlayerControllerV2>() as IPlayerPosition;
        UnityEngine.Assertions.Assert.IsNotNull(PlayerPosition, "Warning: PlayerPosition in ExplorationUIContext is null");

        Player = GameObject.FindGameObjectWithTag("Player");
        UnityEngine.Assertions.Assert.IsNotNull(Player, "Warning: Player in ExplorationUIContext is null");

        Init();
        (this as IGameUserInterface).RegisterWithUIController();
        Hide();

        uiController = FindObjectOfType<UIController>();
        UnityEngine.Assertions.Assert.IsNotNull(uiController, "Warning: uiController in ExplorationUIContext is Null");
    }

    // Update is called once per frame
    void Update()
    {
        if (uiController.CurrentInterface == UIController.EInterface.ExplorationUIContext)
        {
            DoRaycastUnderPlayer();
        }
    }
    
    private void Init()
    {
        SetUIComponents();
        SetInterfaceContent();
        SetCallbacks();
        // ResetFocus();
    }
    
    protected  void SetCallbacks()
    {
        // GamepadRaycast. += DoRaycast;
    }
    protected  void SetUIComponents()
    {
        Menu = GetComponent<UIDocument>();
        Root = Menu.rootVisualElement;
        
        // GamepadRaycast = new InputAction(type: InputActionType.Button, binding: "<Gamepad>/buttonEast", interactions: "press(behavior=1)");
        // GamepadRaycast.Enable();
        // NavButtonRight = Root.Q<Button>("NavButtonRight");
        // NavButtonLeft = Root.Q<Button>("NavButtonLeft");

        // GamepadConfirm = new InputAction(type: InputActionType.Button, binding: "<Gamepad>/buttonEast", interactions: "press(behavior=1)");
        // GamepadNavRight = new InputAction(type: InputActionType.Button, binding: "<Gamepad>/rightShoulder", interactions: "press(behavior=1)");
        // GamepadNavLeft = new InputAction(type: InputActionType.Button, binding: "<Gamepad>/leftShoulder", interactions: "press(behavior=1)");
        // GamepadConfirm.Enable();
        // GamepadNavRight.Enable();
        // GamepadNavLeft.Enable();
    }

    protected void SetInterfaceContent()
    {
            
    }

    private void DoRaycast()
    {
        var direction = MapEventRaycast.GetRaycastDirection(PlayerMovement.EDirectionOfMovement);
        var hits = Physics2D.Raycast(PlayerPosition.PlayerPosition, PlayerMovement.PlayerFacing, 1.0f);
        if (hits.collider != null && hits.collider.name != "Player")
        {
            Debug.Log("got a hit." + hits.collider.name);
            MapEventRaycast.TriggerEvents(hits, Player);
        }
    }
    
    private void DoRaycastUnderPlayer()
    {
        var direction = MapEventRaycast.GetRaycastDirection(PlayerMovement.EDirectionOfMovement);
        var hits = Physics2D.Raycast(PlayerPosition.PlayerPosition, direction, 0.1f);
        if (hits.collider != null && hits.collider.name != "Player")
        {
            Debug.Log("got a hit." + hits.collider.name);
            MapEventRaycast.TriggerEventsUnderPlayer(hits, Player);
        }
    }

    public void CallbackRaycast(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && context.action.name == "MapEventRaycast")
        {
            Debug.Log("Raycast Triggered");
            DoRaycast();
        }
    }
    
    public void Show()
    {
    }

    public void Hide()
    {
    }
}