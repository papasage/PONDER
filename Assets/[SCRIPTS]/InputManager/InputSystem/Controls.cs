//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/[SCRIPTS]/InputManager/InputSystem/Controls.inputactions
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

public partial class @Controls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""Rumble"",
            ""id"": ""3f7eac7e-2bf0-48cc-a7ed-dc98218ffbfc"",
            ""actions"": [
                {
                    ""name"": ""RumbleAction"",
                    ""type"": ""PassThrough"",
                    ""id"": ""4116ce07-b20a-4dcd-886a-bf7213465314"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""8e10f45e-d0be-4b42-ad3f-d3617025bce9"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""RumbleAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""508637f4-4b13-47f1-8db5-462ef74b5513"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""RumbleAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": []
        },
        {
            ""name"": ""Controller"",
            ""bindingGroup"": ""Controller"",
            ""devices"": []
        }
    ]
}");
        // Rumble
        m_Rumble = asset.FindActionMap("Rumble", throwIfNotFound: true);
        m_Rumble_RumbleAction = m_Rumble.FindAction("RumbleAction", throwIfNotFound: true);
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

    // Rumble
    private readonly InputActionMap m_Rumble;
    private List<IRumbleActions> m_RumbleActionsCallbackInterfaces = new List<IRumbleActions>();
    private readonly InputAction m_Rumble_RumbleAction;
    public struct RumbleActions
    {
        private @Controls m_Wrapper;
        public RumbleActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @RumbleAction => m_Wrapper.m_Rumble_RumbleAction;
        public InputActionMap Get() { return m_Wrapper.m_Rumble; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(RumbleActions set) { return set.Get(); }
        public void AddCallbacks(IRumbleActions instance)
        {
            if (instance == null || m_Wrapper.m_RumbleActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_RumbleActionsCallbackInterfaces.Add(instance);
            @RumbleAction.started += instance.OnRumbleAction;
            @RumbleAction.performed += instance.OnRumbleAction;
            @RumbleAction.canceled += instance.OnRumbleAction;
        }

        private void UnregisterCallbacks(IRumbleActions instance)
        {
            @RumbleAction.started -= instance.OnRumbleAction;
            @RumbleAction.performed -= instance.OnRumbleAction;
            @RumbleAction.canceled -= instance.OnRumbleAction;
        }

        public void RemoveCallbacks(IRumbleActions instance)
        {
            if (m_Wrapper.m_RumbleActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IRumbleActions instance)
        {
            foreach (var item in m_Wrapper.m_RumbleActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_RumbleActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public RumbleActions @Rumble => new RumbleActions(this);
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    private int m_ControllerSchemeIndex = -1;
    public InputControlScheme ControllerScheme
    {
        get
        {
            if (m_ControllerSchemeIndex == -1) m_ControllerSchemeIndex = asset.FindControlSchemeIndex("Controller");
            return asset.controlSchemes[m_ControllerSchemeIndex];
        }
    }
    public interface IRumbleActions
    {
        void OnRumbleAction(InputAction.CallbackContext context);
    }
}
