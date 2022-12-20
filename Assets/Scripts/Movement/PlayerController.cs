using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Cinemachine;



namespace escapefromtampere.PlayerControl
{
    public class PlayerController : MonoBehaviour
    {

        [SerializeField] private float movementLerpSpeed = 0.85F;

        [SerializeField] private float lookLerpSpeed = 0.1F;

        [SerializeField] private Rig aimRig;

        [SerializeField] private MultiAimConstraint bodyAim;

        [SerializeField] private MultiAimConstraint handAim;

        [SerializeField] private RigBuilder aimBuilder;

        [SerializeField] private TwoBoneIKConstraint aimIKConstraint;

        [SerializeField] private Transform camHolder;

        [SerializeField] private Transform aimingPos;

        [SerializeField] private Transform rightGunBone;
        
        [SerializeField] private Transform cam;

        [SerializeField] private CinemachineVirtualCamera AimCam, MainCam;

        [SerializeField] private float upperLimit = -40f;
        [SerializeField] private float bottomLimit = 70f;

        [SerializeField] private float mouseSens = 21f;

        [SerializeField] private float jumpFactor = 250f;

        [SerializeField] private float disToGround = 10f;

        [SerializeField] private LayerMask groundCheck;
        public bool LockCameraPosition = false;

        public Collider footCollider;

        public Collider playerCollider;

        private Rigidbody playerRb;

        private Transform GunParent;

        private Transform Gun;

        private Transform GunGrapPoint;

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

        private bool readyToShoot;

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
            weaponArsenal = GetComponent<WeaponArsenal>();
            xVelHash = Animator.StringToHash("X_Velocity");
            yVelHash = Animator.StringToHash("Y_Velocity");
            jumpHash = Animator.StringToHash("Jump");
            groundHash = Animator.StringToHash("Grounded");
            fallingHash = Animator.StringToHash("Falling");
            zVelHash = Animator.StringToHash("Z_Velocity");
            crouchHash = Animator.StringToHash("Crouch");
            
            InitializeCursorLock();
        }

        private void Update()
        {
            HandleCrouch();
            HandleGunSwitch();
            HandleShooting();
            HandleAiming();
        }
        
        private void FixedUpdate()
        {
            SampleGround();
            Move(); // Moving needs the be in FixedUpdate so movement speed isn't dependent on framerate
        }

        private void Move()
        {
            if (!hasAnimator) return;

            Quaternion cameraDirection = Quaternion.Euler(0f, cam.rotation.eulerAngles.y, 0f);
            
            float speedMultiplier;
            if (Actions.ingame.Run.IsPressed()) speedMultiplier = runSpeed;
            else if (Actions.ingame.Crouch.IsPressed()) speedMultiplier = 1.5f;
            else if (Actions.ingame.Aim.IsPressed()) speedMultiplier = 2f;
            else speedMultiplier = walkSpeed;

            Vector3 movementInput = Actions.ingame.Move.ReadValue<Vector3>() * speedMultiplier;
            if (movementInput != Vector3.zero & grounded)
            {
                Vector3 rotatedMovement = cameraDirection * movementInput;
                rotatedMovement.y = 0f; // Lock movement to the XZ plane so the player can't start flying.
                playerRb.velocity = Vector3.Lerp(playerRb.velocity, rotatedMovement, movementLerpSpeed);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(rotatedMovement, transform.up), lookLerpSpeed);
                footCollider.enabled = false;
            }
            if(movementInput == Vector3.zero & grounded)
            {
                footCollider.enabled = true;
            }

            if (Actions.ingame.Aim.IsPressed() & grounded)
            {
                transform.rotation = cameraDirection;
            }
            
            // Update animation
            Vector3 animationVelocity = Quaternion.Inverse(transform.rotation) * playerRb.velocity;
            anim.SetFloat(xVelHash, animationVelocity.x);
            anim.SetFloat(yVelHash, animationVelocity.z);
        }
        private void HandleCrouch() => anim.SetBool(crouchHash, Actions.ingame.Crouch.IsPressed());
        private void HandleGunSwitch()
        {
            if(rightGunBone.childCount <= 0)
            {
                aimRig.weight = 0f;
            }
            else
            {
                aimRig.weight = 1f;
            }
            
            
            if (Actions.ingame.Shotgun.WasPerformedThisFrame())
            {
                weaponArsenal.SetArsenal("Shotgun");
                anim.SetBool("HoldingBigGun", true);
                BuildAimingRig();
                return;
            }
            if (Actions.ingame.Rifle.WasPerformedThisFrame())
            {
                weaponArsenal.SetArsenal("Rifle");
                anim.SetBool("HoldingBigGun", true);
                BuildAimingRig();
                return;
            }
                
            if (Actions.ingame.Pistol.WasPerformedThisFrame())
            {
                weaponArsenal.SetArsenal("Pistol");
                anim.SetBool("HoldingBigGun", true);
                BuildAimingRig();
                return;
            } 

            if (Actions.ingame.Holster.WasPerformedThisFrame())
            {
                Destroy(rightGunBone.GetChild(0).gameObject);
                anim.SetBool("HoldingBigGun", false);
                return;
            }
        }
        private void HandleJump()
        {
            //Returnataan jos pelaajalle ei ole laitettu animatoria tai hän ei ole hypännyt
            if (!hasAnimator) return;
            if (!Actions.ingame.Jump.IsPressed()) return;
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
            Vector3 targetPosition;
            targetPosition = transform.position;

            RaycastHit hitInfo;
            if (Physics.Raycast(playerRb.worldCenterOfMass, Vector3.down, out hitInfo, disToGround + 0.1f, groundCheck))
            {
                playerCollider.enabled = true;
                Vector3 raycastHitPoint = hitInfo.point;
                targetPosition.y = raycastHitPoint.y;
                grounded = true;
                SetAnimationGrounding();

            }
            else
            {
                grounded = false;
                playerCollider.enabled = false;
                anim.SetFloat(zVelHash, playerRb.velocity.y);
                SetAnimationGrounding();
            }
            if (grounded && !Actions.ingame.Jump.WasPerformedThisFrame() && Actions.ingame.Move.ReadValue<Vector3>() != Vector3.zero)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / 0.01f);
            }
            else
            {
                transform.position = targetPosition;
            }
            
            return;



        }

        void SetAnimationGrounding()
        {
         
            anim.SetBool(fallingHash, !grounded);
            anim.SetBool(groundHash, grounded);
            
        }

        void HandleShooting()
        {
            if (Actions.ingame.Aim.IsPressed() & readyToShoot)
            {
                GunParent = rightGunBone.GetChild(0);
                Gun = GunParent.GetChild(0);
                currentWeapon = Gun.GetComponent<GunV2>();
                if (GunParent != null && Actions.ingame.Shoot.WasPerformedThisFrame())
                {
                    currentWeapon.Shoot();
                }
                if (Actions.ingame.Reload.WasPerformedThisFrame() && Gun != null)
                {
                    currentWeapon.Reload();
                }

            }

        }
        void HandleAiming()
        {
            if (Actions.ingame.Aim.IsPressed() && rightGunBone.childCount >= 1)
            {
                AimCam.gameObject.SetActive(true);
                MainCam.gameObject.SetActive(false);
                bodyAim.weight = 1f;
                handAim.weight = 1f;
                readyToShoot = true;
            }
            else
            {
                AimCam.gameObject.SetActive(false);
                MainCam.gameObject.SetActive(true);
                bodyAim.weight = 0f;
                handAim.weight = 0f;
                readyToShoot = false;
            }

        }

        void InitializeCursorLock()
        {
            Actions.ingame.UnlockMouse.started += _ =>
            {
                // Called when the player unlocks the mouse

                SetCursorLock(Cursor.visible);
            };
            
            // When the game starts, lock the mouse.
            SetCursorLock(true);
        }

        public static void SetCursorLock(bool locked)
        {
            Cursor.visible = !locked;
            Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.Confined;
        }
        
        void BuildAimingRig()
        {
            GunParent = rightGunBone.GetChild(0);
            Gun = GunParent.GetChild(0);
            GunGrapPoint = Gun.GetChild(2);
            aimIKConstraint.data.target = GunGrapPoint;
            aimBuilder.Build();

        }


    }


}



