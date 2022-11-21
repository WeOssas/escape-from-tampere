using UnityEngine;
using UnityEngine.InputSystem;


namespace escapefromtampere.Manager
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private PlayerInput playerInput;

        public Vector2 Move { get; private set; }

        public Vector2 Look { get; private set; } 
        public bool Run { get; private set; }

        public bool Jump { get; private set; }

        public bool Crouch { get; private set; }

        public bool Aim { get; private set; }

        public bool Rifle { get; private set; }

        public bool Shotgun { get; private set; }

        public bool Pistol { get; private set; }

        public bool Holster { get; private set; }

        public bool Interact { get; private set; }



        private InputActionMap currentMap;

        private InputAction moveAction;

        private InputAction lookAction;

        private InputAction runAction;

        private InputAction jumpAction;

        private InputAction crouchAction;

        private InputAction aimAction;

        private InputAction rifleAction;

        private InputAction shotgunAction;

        private InputAction pistolAction;

        private InputAction holsterAction;

        private InputAction interAction;

        private void Awake()
        {
            HideCursor();
            currentMap = playerInput.currentActionMap;
            moveAction = currentMap.FindAction("Move");
            lookAction = currentMap.FindAction("Look");
            runAction = currentMap.FindAction("Run");
            jumpAction = currentMap.FindAction("Jump");
            crouchAction = currentMap.FindAction("Crouch");
            aimAction = currentMap.FindAction("Aim");
            rifleAction = currentMap.FindAction("Rifle");
            shotgunAction = currentMap.FindAction("Shotgun");
            pistolAction = currentMap.FindAction("Pistol");
            holsterAction = currentMap.FindAction("Holster");
            interAction = currentMap.FindAction("Interact");

            moveAction.performed += onMove;
            lookAction.performed += onLook;
            runAction.performed += onRun;
            jumpAction.performed += onJump;
            crouchAction.started += onCrouch;
            aimAction.started += onAim;
            rifleAction.performed += onRifle;
            shotgunAction.performed += onShotgun;
            pistolAction.performed += onPistol;
            holsterAction.performed += onHolster;
            interAction.performed += onInteract;


            moveAction.canceled += onMove;
            lookAction.canceled += onLook;
            runAction.canceled += onRun;
            jumpAction.canceled += onJump;
            crouchAction.canceled += onCrouch;
            aimAction.canceled += onAim;
            rifleAction.canceled += onRifle;
            shotgunAction.canceled += onShotgun;
            pistolAction.canceled += onPistol;
            holsterAction.canceled += onHolster;
            interAction.canceled += onInteract;
        }

        private void HideCursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        private void onMove(InputAction.CallbackContext context)
        {
            Move = context.ReadValue<Vector2>();
        }

        private void onLook(InputAction.CallbackContext context)
        {
            Look = context.ReadValue<Vector2>();
        }

        private void onRun(InputAction.CallbackContext context)
        {
            Run = context.ReadValueAsButton();
        }

        private void onJump(InputAction.CallbackContext context)
        {
            Jump = context.ReadValueAsButton();
        }

        private void onCrouch(InputAction.CallbackContext context)
        {
            Crouch = context.ReadValueAsButton();
        }

        private void onAim(InputAction.CallbackContext context)
        {
            Aim = context.ReadValueAsButton();
        }

        private void onRifle(InputAction.CallbackContext context)
        {
            Rifle = context.ReadValueAsButton();
        }

        private void onShotgun(InputAction.CallbackContext context)
        {
            Shotgun = context.ReadValueAsButton();
        }

        private void onPistol(InputAction.CallbackContext context)
        {
            Pistol = context.ReadValueAsButton();
        }

        private void onHolster(InputAction.CallbackContext context)
        {
            Holster = context.ReadValueAsButton();
        }

        private void onInteract(InputAction.CallbackContext context)
        {
            Interact = context.ReadValueAsButton();
        }
        private void OnEnable()
        {
            currentMap.Enable();
        }

        private void OnDisable()
        {
            currentMap.Disable();
        }


    }
   
}