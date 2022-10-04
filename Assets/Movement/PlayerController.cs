using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using escapefromtampere.Manager;


namespace escapefromtampere.PlayerControl
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float animBlendSpeed;
        
        [SerializeField] private Transform camHolder;

        [SerializeField] private Transform cam;

        [SerializeField] private float upperLimit = -40f;
        [SerializeField] private float bottomLimit = 70f;

        [SerializeField] private float mouseSens = 21f;

        [SerializeField] private float jumpFactor = 250f;

        [SerializeField] private float disToGround = 10f;

        [SerializeField] private LayerMask groundCheck;


        private Rigidbody playerRb;

        private InputManager inputManagerr;

        private Animator anim;

        private bool hasAnimator;

        private bool grounded;

        private int xVelHash;

        private int yVelHash;

        private int jumpHash;

        private int groundHash;

        private int fallingHash;

        private float xRotation;

        private int _zVelHash;

        private const float walkSpeed = 2f;
        private const float runSpeed = 6f;

        private Vector2 currentVelocity;

        private void Start()
        {
            hasAnimator = TryGetComponent<Animator>(out anim);
            playerRb = GetComponent<Rigidbody>();
            inputManagerr = GetComponent<InputManager>();

            xVelHash = Animator.StringToHash("X_Velocity");
            yVelHash = Animator.StringToHash("Y_Velocity");
            jumpHash = Animator.StringToHash("Jump");
            groundHash = Animator.StringToHash("Grounded");
            fallingHash = Animator.StringToHash("Falling");
            _zVelHash = Animator.StringToHash("Z_Velocity");

        }

        private void FixedUpdate()
        {
            SampleGround();
            Move();
            HandleJump();
            
        }
        private void LateUpdate()
        {
            CameraMovement();
        }

        private void Move()
        {
            if (!hasAnimator) return;

            float targetSpeed = inputManagerr.Run ? runSpeed : walkSpeed;
            if(inputManagerr.Move == Vector2.zero) targetSpeed = 0.1f;

            currentVelocity.x = Mathf.Lerp(currentVelocity.x, inputManagerr.Move.x * targetSpeed, animBlendSpeed * Time.fixedDeltaTime);
            currentVelocity.y = Mathf.Lerp(currentVelocity.y, inputManagerr.Move.y * targetSpeed, animBlendSpeed * Time.fixedDeltaTime);

            var xVelDifference = currentVelocity.x - playerRb.velocity.x;
            var zVelDifference = currentVelocity.y - playerRb.velocity.z;


            playerRb.AddForce(transform.TransformVector(new Vector3(xVelDifference, 0, zVelDifference)), ForceMode.VelocityChange);
            
            anim.SetFloat(xVelHash, currentVelocity.x);
            anim.SetFloat(yVelHash, currentVelocity.y);
        }

        private void CameraMovement()
        {
            if(!hasAnimator) return;

            var Mouse_X = inputManagerr.Look.x;
            var Mouse_Y = inputManagerr.Look.y;
            cam.position = camHolder.position;

            xRotation -= Mouse_Y * mouseSens * Time.deltaTime;
            xRotation = Mathf.Clamp(xRotation, upperLimit, bottomLimit);

            cam.localRotation = Quaternion.Euler(xRotation,0,0);
            transform.Rotate(Vector3.up, Mouse_X * mouseSens * Time.deltaTime);
        }

        private void HandleJump()
        {
            //Returnataan jos pelaajalle ei ole laitettu animatoria tai hän ei ole hypnnyt
            if (!hasAnimator) return;
            if (!inputManagerr.Jump) return;
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
            anim.SetFloat(_zVelHash, playerRb.velocity.y);
            SetAnimationGrounding();
            return;

        }

        void SetAnimationGrounding()
        {
         
            anim.SetBool(fallingHash, !grounded);
            anim.SetBool(groundHash, grounded);
            
        }
    }


}



