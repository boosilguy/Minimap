using UnityEngine;

namespace minimap.sample
{
    public class PlayerInputManager : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotationSpeed = 1.5f;
        [SerializeField] private Transform cameraTransform;

        private CharacterController characterController;
        private Vector3 moveDirection = Vector3.zero;
        private float mouseXInput;

        private void Start()
        {
            characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            GetMouseInput();
            MoveCharacter();
            RotateCharacterWithMouse();
        }

        private void GetMouseInput()
        {
            mouseXInput = Input.GetAxis("Mouse X");
        }

        private void MoveCharacter()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            Vector3 forward = cameraTransform.forward; forward.y = 0f;
            Vector3 right = cameraTransform.right; right.y = 0f;

            forward.Normalize();
            right.Normalize();

            // 움직이는 방향 설정
            Vector3 targetDirection = forward * verticalInput + right * horizontalInput;
            moveDirection = Vector3.Lerp(moveDirection, targetDirection, moveSpeed);

            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

        }

        private void RotateCharacterWithMouse()
        {
            if (Input.GetMouseButton(1))
            {
                // 캐릭터 좌우 회전
                float targetAngle = transform.eulerAngles.y + mouseXInput * rotationSpeed;
                transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
            }
        }
    }
}