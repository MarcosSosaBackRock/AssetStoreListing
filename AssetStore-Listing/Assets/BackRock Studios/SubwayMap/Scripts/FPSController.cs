using UnityEngine;


namespace BackRockStudios
{

    [RequireComponent(typeof(CharacterController))]
    public class FPSController : MonoBehaviour
    {
        [Header("Movement Settings")]
        public float walkSpeed = 5f;
        public float runSpeed = 9f;
        public float jumpHeight = 2f;
        public float gravity = -9.81f;

        [Header("Mouse Settings")]
        public float mouseSensitivity = 2f;
        public Transform cameraTransform;

        [Header("FOV Settings")]
        public float walkFOV = 60f;
        public float runFOV = 75f;
        public float fovSmoothSpeed = 8f; // higher = faster transition

        private CharacterController controller;
        private Vector3 velocity;
        private float xRotation = 0f;
        private bool isGrounded;

        private Camera playerCamera;
        private float targetFOV;

        void Start()
        {
            controller = GetComponent<CharacterController>();
            playerCamera = cameraTransform.GetComponent<Camera>();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            targetFOV = walkFOV;
            playerCamera.fieldOfView = walkFOV;
        }

        void Update()
        {
            HandleMouseLook();
            HandleMovement();
            HandleFOV();
        }

        void HandleMouseLook()
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            transform.Rotate(Vector3.up * mouseX);

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }

        void HandleMovement()
        {
            isGrounded = controller.isGrounded;
            if (isGrounded && velocity.y < 0)
                velocity.y = -2f;

            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            float currentSpeed = isRunning ? runSpeed : walkSpeed;
            targetFOV = isRunning ? runFOV : walkFOV; // <-- update FOV target here

            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            Vector3 move = transform.right * moveX + transform.forward * moveZ;
            controller.Move(move * currentSpeed * Time.deltaTime);

            if (isGrounded && Input.GetButtonDown("Jump"))
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }

        void HandleFOV()
        {
            // Smooth FOV transition
            playerCamera.fieldOfView = Mathf.Lerp(
                playerCamera.fieldOfView,
                targetFOV,
                Time.deltaTime * fovSmoothSpeed
            );
        }
    }

}