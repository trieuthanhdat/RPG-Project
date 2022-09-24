using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class CameraRotation : MonoBehaviour
    {
        [SerializeField] private Transform followCam;

        [SerializeField] private Camera mainCam;
        private Vector3 previousPosition;

        private void Update()
        {
            if(Input.GetMouseButtonDown(2))
            {
                CalculatePreviousPosition();
            }

            if(Input.GetMouseButton(2))
            {
                RotateCamera();
            }

            
        }

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
