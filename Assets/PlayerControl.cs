//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/PlayerControl.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerControl: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControl()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControl"",
    ""maps"": [
        {
            ""name"": ""playerMove"",
            ""id"": ""0ce76398-f43c-4f6d-9d8e-24334aea2b73"",
            ""actions"": [
                {
                    ""name"": ""WASD"",
                    ""type"": ""Value"",
                    ""id"": ""3ec0c3ab-7967-4a9e-ab88-467bfd2ee8eb"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Fire"",
                    ""type"": ""Button"",
                    ""id"": ""027c5c04-48e2-4da6-8b62-60483060664e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Sprint"",
                    ""type"": ""Button"",
                    ""id"": ""55688457-b630-4d0f-9dee-e283f8c23f6d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""EscPause"",
                    ""type"": ""Button"",
                    ""id"": ""09e42433-5f7a-48c7-b79a-de2c72600009"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""295e77fc-7631-48c9-9755-65cd56a708f1"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WASD"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""af39eff8-8c9a-47e2-a8f2-28d024a3da0d"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WASD"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""53b999ae-82ae-4ea6-ab5b-63591e1ae69f"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WASD"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""2084ffe9-18e9-4e16-bbf8-384689a48601"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WASD"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""946b7466-0389-4e9e-918e-68116a6c86b6"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WASD"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""a75f039b-5455-43f9-81ff-0efd4c9bc6b3"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""93493f4c-6494-4247-89b4-7f037484887e"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""164d8a6f-39cd-4f13-9702-c7b52c80a313"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""EscPause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UiControl"",
            ""id"": ""7c216aae-b570-45f7-8661-f6cfef9c0b01"",
            ""actions"": [
                {
                    ""name"": ""EscContinue"",
                    ""type"": ""Button"",
                    ""id"": ""e28728ac-bb2f-4bed-b98e-edb2a175e6b3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""b2e9ca75-b52f-4c87-aaf0-2fe2995ac771"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""EscContinue"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // playerMove
        m_playerMove = asset.FindActionMap("playerMove", throwIfNotFound: true);
        m_playerMove_WASD = m_playerMove.FindAction("WASD", throwIfNotFound: true);
        m_playerMove_Fire = m_playerMove.FindAction("Fire", throwIfNotFound: true);
        m_playerMove_Sprint = m_playerMove.FindAction("Sprint", throwIfNotFound: true);
        m_playerMove_EscPause = m_playerMove.FindAction("EscPause", throwIfNotFound: true);
        // UiControl
        m_UiControl = asset.FindActionMap("UiControl", throwIfNotFound: true);
        m_UiControl_EscContinue = m_UiControl.FindAction("EscContinue", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // playerMove
    private readonly InputActionMap m_playerMove;
    private List<IPlayerMoveActions> m_PlayerMoveActionsCallbackInterfaces = new List<IPlayerMoveActions>();
    private readonly InputAction m_playerMove_WASD;
    private readonly InputAction m_playerMove_Fire;
    private readonly InputAction m_playerMove_Sprint;
    private readonly InputAction m_playerMove_EscPause;
    public struct PlayerMoveActions
    {
        private @PlayerControl m_Wrapper;
        public PlayerMoveActions(@PlayerControl wrapper) { m_Wrapper = wrapper; }
        public InputAction @WASD => m_Wrapper.m_playerMove_WASD;
        public InputAction @Fire => m_Wrapper.m_playerMove_Fire;
        public InputAction @Sprint => m_Wrapper.m_playerMove_Sprint;
        public InputAction @EscPause => m_Wrapper.m_playerMove_EscPause;
        public InputActionMap Get() { return m_Wrapper.m_playerMove; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerMoveActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerMoveActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerMoveActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerMoveActionsCallbackInterfaces.Add(instance);
            @WASD.started += instance.OnWASD;
            @WASD.performed += instance.OnWASD;
            @WASD.canceled += instance.OnWASD;
            @Fire.started += instance.OnFire;
            @Fire.performed += instance.OnFire;
            @Fire.canceled += instance.OnFire;
            @Sprint.started += instance.OnSprint;
            @Sprint.performed += instance.OnSprint;
            @Sprint.canceled += instance.OnSprint;
            @EscPause.started += instance.OnEscPause;
            @EscPause.performed += instance.OnEscPause;
            @EscPause.canceled += instance.OnEscPause;
        }

        private void UnregisterCallbacks(IPlayerMoveActions instance)
        {
            @WASD.started -= instance.OnWASD;
            @WASD.performed -= instance.OnWASD;
            @WASD.canceled -= instance.OnWASD;
            @Fire.started -= instance.OnFire;
            @Fire.performed -= instance.OnFire;
            @Fire.canceled -= instance.OnFire;
            @Sprint.started -= instance.OnSprint;
            @Sprint.performed -= instance.OnSprint;
            @Sprint.canceled -= instance.OnSprint;
            @EscPause.started -= instance.OnEscPause;
            @EscPause.performed -= instance.OnEscPause;
            @EscPause.canceled -= instance.OnEscPause;
        }

        public void RemoveCallbacks(IPlayerMoveActions instance)
        {
            if (m_Wrapper.m_PlayerMoveActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerMoveActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerMoveActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerMoveActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerMoveActions @playerMove => new PlayerMoveActions(this);

    // UiControl
    private readonly InputActionMap m_UiControl;
    private List<IUiControlActions> m_UiControlActionsCallbackInterfaces = new List<IUiControlActions>();
    private readonly InputAction m_UiControl_EscContinue;
    public struct UiControlActions
    {
        private @PlayerControl m_Wrapper;
        public UiControlActions(@PlayerControl wrapper) { m_Wrapper = wrapper; }
        public InputAction @EscContinue => m_Wrapper.m_UiControl_EscContinue;
        public InputActionMap Get() { return m_Wrapper.m_UiControl; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UiControlActions set) { return set.Get(); }
        public void AddCallbacks(IUiControlActions instance)
        {
            if (instance == null || m_Wrapper.m_UiControlActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_UiControlActionsCallbackInterfaces.Add(instance);
            @EscContinue.started += instance.OnEscContinue;
            @EscContinue.performed += instance.OnEscContinue;
            @EscContinue.canceled += instance.OnEscContinue;
        }

        private void UnregisterCallbacks(IUiControlActions instance)
        {
            @EscContinue.started -= instance.OnEscContinue;
            @EscContinue.performed -= instance.OnEscContinue;
            @EscContinue.canceled -= instance.OnEscContinue;
        }

        public void RemoveCallbacks(IUiControlActions instance)
        {
            if (m_Wrapper.m_UiControlActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IUiControlActions instance)
        {
            foreach (var item in m_Wrapper.m_UiControlActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_UiControlActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public UiControlActions @UiControl => new UiControlActions(this);
    public interface IPlayerMoveActions
    {
        void OnWASD(InputAction.CallbackContext context);
        void OnFire(InputAction.CallbackContext context);
        void OnSprint(InputAction.CallbackContext context);
        void OnEscPause(InputAction.CallbackContext context);
    }
    public interface IUiControlActions
    {
        void OnEscContinue(InputAction.CallbackContext context);
    }
}
