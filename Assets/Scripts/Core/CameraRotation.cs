using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UIElements;
using static UnityEngine.ParticleSystem;
using Touch = UnityEngine.Touch;

namespace RPG.Core
{
    public class CameraRotation : MonoBehaviour
    {
        [SerializeField] private Transform followCam;

        [SerializeField] private Camera mainCam;
        private Vector3 previousPosition;
        bool OneFingerTap;
        float TimerOne;
        float timerInterval = 0.1f; //the more you decrease it the more accurate it gets 

        private void Update()
        {
#if UNITY_IOS || UNITY_ANDROID
            foreach (Touch touch in Input.touches)
            {
                if(Input.touchCount == 2)
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        HandlePositionCalculationOnTouch(touch.position);
                    }
                    if(touch.phase == TouchPhase.Moved)
                    {
                        HandleMobileRotation(touch.position);
                    }
                }
            }

#endif
            if (Input.GetMouseButtonDown(2))
            {
                CalculatePreviousPosition();
            }

            if (Input.GetMouseButton(2))
            {
                RotateCamera();
            }


        }
    
       //MOBILE ROTATION
        void HandlePositionCalculationOnTouch(Vector2 position)
        {
            previousPosition = mainCam.ScreenToViewportPoint(position);
        }

        void HandleMobileRotation(Vector2 position)
        {
            Vector3 direction = previousPosition - mainCam.ScreenToViewportPoint(position);

            followCam.transform.position = new Vector3();

            followCam.transform.Rotate(new Vector3(1, 0, 0), direction.y * 180);
            followCam.transform.Rotate(new Vector3(0, 1, 0), -direction.x * 180, Space.World);

            HandlePositionCalculationOnTouch(position);
        }


        //DEFAULT PC ROTATION
        private Vector3 CalculatePreviousPosition()
        {
            previousPosition = mainCam.ScreenToViewportPoint(Input.mousePosition);
            return previousPosition;
        }

        private void RotateCamera()
        {
            Vector3 direction = previousPosition - mainCam.ScreenToViewportPoint(Input.mousePosition);

            followCam.transform.position = new Vector3();

            followCam.transform.Rotate(new Vector3(1, 0, 0), direction.y * 180);
            followCam.transform.Rotate(new Vector3(0, 1, 0), -direction.x *180, Space.World);

            CalculatePreviousPosition();
        }
    }

}
