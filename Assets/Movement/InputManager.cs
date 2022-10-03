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
        
        private InputActionMap currentMap;

        private InputAction moveAction;

        private InputAction lookAction;

        private InputAction runAction;

        private InputAction jumpAction;

        private void Awake()
        {
            currentMap = playerInput.currentActionMap;
            moveAction = currentMap.FindAction("Move");
            lookAction = currentMap.FindAction("Look");
            runAction = currentMap.FindAction("Run");
            jumpAction = currentMap.FindAction("Jump");

            moveAction.performed += onMove;
            lookAction.performed += onLook;
            runAction.performed += onRun;
            jumpAction.performed += onJump;

            moveAction.canceled += onMove;
            lookAction.canceled += onLook;
            runAction.canceled += onRun;
            jumpAction.canceled += onJump;

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