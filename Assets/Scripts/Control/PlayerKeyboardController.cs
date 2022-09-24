using System.Collections;
using System.Collections.Generic;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerKeyboardController : MonoBehaviour
    {
        [SerializeField] float turnSmoothTime = 0.1f;
        [SerializeField] Transform cam;
        float turnSmoothVelocity;
        float speed;
        CharacterController characterController; 
        PlayerController playerController;
        Mover mover;
        
        private void Start()
        {
            characterController = GetComponent<CharacterController>();
            playerController = GetComponent<PlayerController>();
            mover = GetComponent<Mover>();

            speed = mover.GetMovementSpeed(playerController.speedFraction);
        }

        private void Update()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(horizontal,0f, vertical);

            if(direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime, speed);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

                characterController.Move(moveDirection * speed * Time.deltaTime);
            }
        }

    }

}
