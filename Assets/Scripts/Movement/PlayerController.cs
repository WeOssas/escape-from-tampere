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

        [SerializeField] private float movementLerpSpeed = 0.85F;

        [SerializeField] private float lookLerpSpeed = 0.1F;

        [SerializeField] private Rig aimRig;

        [SerializeField] private Transform camHolder;

        [SerializeField] private Transform aimingPos;

        [SerializeField] private Transform rightGunBone;
        
        [SerializeField] private Transform cam;
        
        [SerializeField] private CinemachineFreeLook camVirtualCam;

        [SerializeField] private Camera CamSetting;

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

        private Transform Gun;

        private Transform gunChild;

        private Animator anim;

        private bool holdingGun;

        private bool holdingHandGun;

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

        private WeaponArsenal weaponArsenal;

        private GunV2 currentWeapon;

        
        private const float walkSpeed = 4f;
        private const float runSpeed = 12f;
        private float turnSmoothVelocity = 0.1f;
        private float turnSmoothTime = 0.12f;
        
        

        private Vector2 currentVelocity;

        private void Start()
        {
            hasAnimator = TryGetComponent<Animator>(out anim);
            playerRb = GetComponent<Rigidbody>();
            inputManager = GetComponent<InputManager>();
            weaponArsenal = GetComponent<WeaponArsenal>();
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
            HandleGunSwitch();
            HandleShooting();
            //HandleAimAndMouse();
            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }
            if (Input.GetKeyDown(KeyCode.RightAlt))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        private void Move()
        {
            if (!hasAnimator) return;


            float speedMultiplier;
            if (inputManager.Run) speedMultiplier = runSpeed;
            else if (inputManager.Crouch) speedMultiplier = 1.5f;
            else if (inputManager.Aim) speedMultiplier = 2f;
            else speedMultiplier = walkSpeed;

            Quaternion cameraDirection = Quaternion.Euler(0f, cam.rotation.eulerAngles.y, 0f);
            
            Vector3 movementInput = new Vector3(inputManager.Move.x, 0f, inputManager.Move.y) * speedMultiplier;
            if (movementInput != Vector3.zero & grounded)
            {
                Vector3 rotatedMovement = cameraDirection * movementInput;
                rotatedMovement.y = 0f; // Lock movement to the XZ plane so the player can't start flying.
                playerRb.velocity = Vector3.Lerp(playerRb.velocity, rotatedMovement, movementLerpSpeed);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(rotatedMovement, transform.up), lookLerpSpeed);
            }

            if (inputManager.Aim & grounded)
            {
                transform.rotation = cameraDirection;
            }
            
            // Update animation
            Vector3 animationVelocity = Quaternion.Inverse(transform.rotation) * playerRb.velocity;
            anim.SetFloat(xVelHash, animationVelocity.x);
            anim.SetFloat(yVelHash, animationVelocity.z);
        }
        private void HandleCrouch() => anim.SetBool(crouchHash, inputManager.Crouch);
        private void HandleGunSwitch()
        {
            if (inputManager.Shotgun)
            {
                weaponArsenal.SetArsenal("Shotgun");
                anim.SetBool("HoldingBigGun", true);
                return;
            }
            if (inputManager.Rifle)
            {
                weaponArsenal.SetArsenal("Rifle");
                anim.SetBool("HoldingBigGun", true);
                return;
            }
                
            if (inputManager.Pistol)
            {
                weaponArsenal.SetArsenal("Pistol");
                anim.SetBool("HoldingBigGun", true);
                return;
            }
                

            if (inputManager.Holster)
            {
                Destroy(rightGunBone.GetChild(0).gameObject);
                anim.SetBool("HoldingBigGun", false);
                holdingGun = false;
                return;
            }
        }
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

        void HandleShooting()
        {
            Gun = rightGunBone.GetChild(0);
            gunChild = Gun.GetChild(0);
            currentWeapon = gunChild.GetComponent<GunV2>();
            if(Gun != null & inputManager.Shoot)
            {
                currentWeapon.Shoot();
            }
            if (inputManager.Reload && Gun != null)
            {
                currentWeapon.Reload();
            }
            
        }


        private void HandleAimAndMouse() 
        {
            
            anim.SetBool(aimHash, inputManager.Aim);
            if (inputManager.Aim)
            {
                camVirtualCam.enabled = false;
                CamSetting.enabled = false;
                //CamSetting2.enabled = true;

                aimRig.weight = 0.5f;
            }
            
            if (!inputManager.Aim)
            {
                //CamSetting2.enabled = false;
                CamSetting.enabled = true;
                camVirtualCam.enabled = true;
                
                aimRig.weight = 0f;
            }

        }
        

    }


}



