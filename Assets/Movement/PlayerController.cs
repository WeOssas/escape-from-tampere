using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using escapefromtampere.Manager;
using UnityEngine.Animations.Rigging;
using Cinemachine;


namespace escapefromtampere.PlayerControl
{
    public class PlayerController : MonoBehaviour
    {

        [SerializeField] private float animBlendSpeed;

        [SerializeField] private Rig aimRig;

        [SerializeField] private Transform camHolder;

        [SerializeField] private Transform aimingPos;
        
        [SerializeField] private Transform cam;
        [SerializeField] private Transform cam2;
        [SerializeField] private CinemachineFreeLook camVirtualCam;
        [SerializeField] private CinemachineFreeLook cam2VirtualCam;

        [SerializeField] private Transform Gun;

        [SerializeField] private Camera CamSetting;

        [SerializeField] private Camera CamSetting2;

        [SerializeField] private float upperLimit = -40f;
        [SerializeField] private float bottomLimit = 70f;

        [SerializeField] private float mouseSens = 21f;

        [SerializeField] private float jumpFactor = 250f;

        [SerializeField] private float disToGround = 10f;

        [SerializeField] private LayerMask groundCheck;
        public bool LockCameraPosition = false;

        private float _cinemachineTargetPitch;
        private float _cinemachineTargetYaw;

        public GameObject CinemachineCameraTarget;

        private Rigidbody playerRb;

        private InputManager inputManager;

        private Animator anim;

        private bool hasAnimator;

        private bool grounded;

        private int xVelHash;

        private int yVelHash;

        private int jumpHash;

        private int groundHash;

        private int fallingHash;

        private float xRotation;

        private int zVelHash;

        private int crouchHash;

        private int aimHash;

        private CharacterController characterController;
        
        private const float walkSpeed = 2f;
        private const float runSpeed = 6f;
        private float turnSmoothVelocity = 0.1f;
        private float turnSmoothTime = 0.12f;
        
        

        private Vector2 currentVelocity;

        private void Start()
        {
            hasAnimator = TryGetComponent<Animator>(out anim);
            playerRb = GetComponent<Rigidbody>();
            inputManager = GetComponent<InputManager>();
            xVelHash = Animator.StringToHash("X_Velocity");
            yVelHash = Animator.StringToHash("Y_Velocity");
            jumpHash = Animator.StringToHash("Jump");
            groundHash = Animator.StringToHash("Grounded");
            fallingHash = Animator.StringToHash("Falling");
            zVelHash = Animator.StringToHash("Z_Velocity");
            crouchHash = Animator.StringToHash("Crouch");
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;


        }

        private void FixedUpdate()
        {
            
            SampleGround();
            Move();
            HandleJump();
            HandleCrouch();
            //HandleAim();
            
        }
        

        private void Move()
        {
            if (!hasAnimator) return;

            
            float targetSpeed = inputManager.Run ? runSpeed : walkSpeed;
            if (inputManager.Crouch) targetSpeed = 1.5f;
            if(inputManager.Move == Vector2.zero) targetSpeed = 0.1f;
            if (inputManager.Aim)
            {
                targetSpeed = 2f;
            }

            currentVelocity.x = Mathf.Lerp(currentVelocity.x, inputManager.Move.x * targetSpeed, animBlendSpeed * Time.fixedDeltaTime);
            currentVelocity.y = Mathf.Lerp(currentVelocity.y, inputManager.Move.y * targetSpeed, animBlendSpeed * Time.fixedDeltaTime);

            var xVelDifference = currentVelocity.x - playerRb.velocity.x;
            var zVelDifference = currentVelocity.y - playerRb.velocity.z;

            Vector3 move = new Vector3(xVelDifference, 0, zVelDifference);

            if(move != Vector3.zero)
            {
                
                float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f,angle,0);
                playerRb.AddForce(transform.TransformVector(move), ForceMode.VelocityChange);
            }

            
            
            //float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
           
            //float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            
            //transform.rotation = Quaternion.Euler(0f, angle, 0f);

            
            
            anim.SetFloat(xVelHash, currentVelocity.x);
            anim.SetFloat(yVelHash, currentVelocity.y);
        }

      
        
        private void HandleCrouch() => anim.SetBool(crouchHash, inputManager.Crouch);
        

        
        private void HandleJump()
        {
            //Returnataan jos pelaajalle ei ole laitettu animatoria tai hän ei ole hypnnyt
            if (!hasAnimator) return;
            if (!inputManager.Jump) return;
            anim.SetTrigger(jumpHash);
            

        }

        public void JumpAddForce()
        {
            playerRb.AddForce(-playerRb.velocity.y * Vector3.up, ForceMode.VelocityChange);
            playerRb.AddForce(Vector3.up * jumpFactor, ForceMode.Impulse);
            anim.ResetTrigger(jumpHash);
           
        }

        private void SampleGround()
        {
            if (!hasAnimator) return;

            RaycastHit hitInfo;
            if(Physics.Raycast(playerRb.worldCenterOfMass, Vector3.down, out hitInfo, disToGround + 0.1f, groundCheck))
            {
                grounded = true;
                SetAnimationGrounding();
                return;
            }
            
            grounded = false;
            anim.SetFloat(zVelHash, playerRb.velocity.y);
            SetAnimationGrounding();
            return;

        }

        void SetAnimationGrounding()
        {
         
            anim.SetBool(fallingHash, !grounded);
            anim.SetBool(groundHash, grounded);
            
        }

        private void HandleAim() 
        {
            
            anim.SetBool(aimHash, inputManager.Aim);
            if (inputManager.Aim)
            {
                camVirtualCam.enabled = false;
                CamSetting.enabled = false;
                CamSetting2.enabled = true;

                aimRig.weight = 0.5f;




            }
            
            if (!inputManager.Aim)
            {
                CamSetting2.enabled = false;
                CamSetting.enabled = true;
                camVirtualCam.enabled = true;
                
                aimRig.weight = 0f;
            }

        }
        

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }




    }


}



