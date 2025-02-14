using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public abstract class MenuBase : MonoBehaviour, IDataManager, IGameUserInterface
{
    public Texture2D BackgroundTexture { set; get; }
    public UIDocument Menu { set; get; }
    public VisualElement Root { set; get; }
    public DataManager DataMngr { get; set; }
    public IGameUserInterface.ChangeContext NotifyNav { set; get; }

    public Button NavButtonRight { set; get; }
    public Button NavButtonLeft { set; get; }
    public InputAction GamepadNavRight { set; get; }
    public InputAction GamepadNavLeft { set; get; }

    public InputAction GamepadConfirm { set; get; }

    public delegate void DelegateNavRight(InputAction.CallbackContext obj);



    protected virtual void Start()
    {
        (this as IDataManager).SetDataManager();
        Init();
        (this as IGameUserInterface).RegisterWithUIController();
        Hide();
    }

    private void Init()
    {
        SetUIComponents();
        SetInterfaceContent();
        SetCallbacks();
        ResetFocus();
    }

    protected virtual void SetUIComponents()
    {
        Menu = GetComponent<UIDocument>();
        Root = Menu.rootVisualElement;

        NavButtonRight = Root.Q<Button>("NavButtonRight");
        NavButtonLeft = Root.Q<Button>("NavButtonLeft");

        GamepadConfirm = new InputAction(type: InputActionType.Button, binding: "<Gamepad>/buttonEast", interactions: "press(behavior=1)");
        GamepadNavRight = new InputAction(type: InputActionType.Button, binding: "<Gamepad>/rightShoulder", interactions: "press(behavior=1)");
        GamepadNavLeft = new InputAction(type: InputActionType.Button, binding: "<Gamepad>/leftShoulder", interactions: "press(behavior=1)");
        GamepadConfirm.Enable();
        GamepadNavRight.Enable();
        GamepadNavLeft.Enable();
    }

    protected abstract void SetInterfaceContent();


    protected virtual void SetCallbacks()
    {
        if(NavButtonRight is not null)
            NavButtonRight.clicked += NavRight;
        if(NavButtonLeft is not null)
            NavButtonLeft.clicked += NavLeft;
    }

    protected abstract void ResetFocus();
    protected abstract void NavRight();
    protected abstract void NavLeft();
    protected abstract void DoConfirm();

    public void Show()
    {
        Root.style.display = UnityEngine.UIElements.DisplayStyle.Flex;
        SetInterfaceContent();
        SetBackground(UIController.CurrentIGMBackground);
        Root.BringToFront();
        ResetFocus();
    }

    public void Hide()
    {
        Root.style.display = UnityEngine.UIElements.DisplayStyle.None;
        Root.SendToBack();
    }

    private void SetBackground(Texture2D tex)
    {
        Root.style.backgroundImage = tex;
    }

    protected virtual void Update()
    {
        if (GamepadNavRight.triggered)
            StartCoroutine(DoNavRight());

        if (GamepadNavLeft.triggered)
            StartCoroutine(DoNavLeft());

        if (GamepadConfirm.triggered)
            DoConfirm();
    }

    public IEnumerator DoNavRight()
    {
        yield return new WaitForEndOfFrame();
        NavRight();
    }

    public IEnumerator DoNavLeft()
    {
        yield return new WaitForEndOfFrame();
        NavLeft();
    }


}