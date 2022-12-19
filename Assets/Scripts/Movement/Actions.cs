using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// A class for storing references to all actions that need to be referenced from code.
/// This provides more type-safety and performance than looking up the actions from the asset every time they need to
/// be accessed. This also brings the benefits of having hardcoded action names in one place instead of being scattered
/// across the project (as long as this script is used as intended).
/// Each subclass represents a different action map.
/// This script should be present once in every scene, otherwise it probably won't be loaded.
/// </summary>
/// <example>
/// Movement input as Vector2:
/// <code>Actions.instance.ingame.move.ReadValue<Vector2>()</code>
/// </example>
public class Actions : MonoBehaviour
{
    /// <summary>
    /// The Input Action Asset containing all action maps (and actions inside them).
    /// Please only use one Input Action Asset in the whole project to avoid issues.
    /// </summary>
    [SerializeField] protected InputActionAsset asset;
    
    public static InGameActions ingame;
    public static UIActions ui;

    private void Awake()
    {
        ingame = new InGameActions(asset);
        ui = new UIActions(asset);
    }

    public static InputAction[] GetAllActions()
    {
        return ingame.Actions.Concat(ui.Actions).ToArray();
    }

    private void OnEnable()
    {
        asset.Enable();
    }
    
    private void OnDisable()
    {
        asset.Disable();
    }

    public class InGameActions
    {
        internal InGameActions(InputActionAsset asset)
        {
            Actions = new[]
            {
                Move = asset.FindAction("In-Game/Move"),
                Look = asset.FindAction("In-Game/Look"),
                Run = asset.FindAction("In-Game/Run"),
                Jump = asset.FindAction("In-Game/Jump"),
                Crouch = asset.FindAction("In-Game/Crouch"),
                Aim = asset.FindAction("In-Game/Aim"),
                Rifle = asset.FindAction("In-Game/Rifle"),
                Shotgun = asset.FindAction("In-Game/Shotgun"),
                Pistol = asset.FindAction("In-Game/Pistol"),
                Holster = asset.FindAction("In-Game/Holster"),
                Interact = asset.FindAction("In-Game/Interact"),
                Shoot = asset.FindAction("In-Game/Shoot"),
                Reload = asset.FindAction("In-Game/Reload"),
                UnlockMouse = asset.FindAction("In-Game/UnlockMouse"),
                OpenInventory = asset.FindAction("In-Game/OpenInventory")
            };
        }

        public readonly InputAction[] Actions;

        public readonly InputAction Move;
        public readonly InputAction Look;
        public readonly InputAction Run;
        public readonly InputAction Jump;
        public readonly InputAction Crouch;
        public readonly InputAction Aim;
        public readonly InputAction Rifle;
        public readonly InputAction Shotgun;
        public readonly InputAction Pistol;
        public readonly InputAction Holster;
        public readonly InputAction Interact;
        public readonly InputAction Shoot;
        public readonly InputAction Reload;
        public readonly InputAction UnlockMouse;
        public readonly InputAction OpenInventory;
        
    }

    public class UIActions
    {
         // As of writing, no UI actions need to be referenced from code. Add any actions here if needed.
         
         internal UIActions(InputActionAsset asset)
         {
             Actions = new InputAction[]
             {
                 
                 // Example: Click = asset.FindAction("UI/Click")
                 
             };
         }

         public readonly InputAction[] Actions;
         
         // Example: public readonly InputAction Click;
    }
}
