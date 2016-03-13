/////////////////////////////////////////////////////////////////////
//Copyright © 2013-2016 Ivan Perez
//Author: Ivan Perez
//Linkedin: http://es.linkedin.com/in/ivanperezduran
//Portfolio: http://binarycv.com
/////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

namespace BinaryLibrary.Showcase
{
    /// <summary>
    /// Simple class to control the camera.
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        private Transform ownerTransform;

        public Transform target;
        public CharacterBlackboard targetBlackboard;

        private float turnSpeed = 45;

        private bool isDragging = false;
        private Vector3 lastMousePosition = Vector3.zero;

        /// <summary>
        /// Camera modes.
        /// </summary>
        enum ControlMode
        {
            FreeRotation,
            ControlRotation
        }
        private ControlMode controlMode = ControlMode.FreeRotation;

        void Awake()
        {
            ownerTransform = transform;
            targetBlackboard = target.GetComponent<CharacterBlackboard>();
        }

        // Update is called once per frame
        void Update()
        {
            switch (controlMode)
            {
                case ControlMode.FreeRotation:
                    HandleFreeMode();
                    break;
                case ControlMode.ControlRotation:
                    HandleLockedMode();
                    break;
            }
        }

        private void HandleFreeMode()
        {
            ownerTransform.position = target.position;

            //If starts to drag
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                lastMousePosition = Input.mousePosition;
                isDragging = true;
            }

            //If is dragging
            if (isDragging)
            {
                float deltaRotationY = Vector3.Magnitude((Input.mousePosition - lastMousePosition) * 1024 / Screen.width);

                int dir = 1;
                if (Input.mousePosition.x < lastMousePosition.x)
                {
                    dir = -1;
                }
                lastMousePosition = Input.mousePosition;

                ownerTransform.Rotate(0, dir * deltaRotationY * Time.deltaTime * turnSpeed, 0);
            }

            //If stops dragging.
            if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
            {
                isDragging = false;
                lastMousePosition = Vector2.zero;
            }
        }

        private void HandleLockedMode()
        {
            ownerTransform.position = target.position;
         
            Vector3 vectorDirectionToEnemy = targetBlackboard.target.position - target.position;
            Quaternion quaternionTarget = Quaternion.LookRotation(vectorDirectionToEnemy.normalized);

           
            ownerTransform.rotation = Quaternion.Slerp(ownerTransform.rotation, quaternionTarget, Time.deltaTime * Quaternion.Angle(ownerTransform.rotation, quaternionTarget) / 10);
        }

        /// <summary>
        /// Camera mode that always faces the target locked.
        /// </summary>
        public void SetLockedMode()
        {
            controlMode = ControlMode.ControlRotation;
        }

        /// <summary>
        /// Camera mode that orbits around the player.
        /// </summary>
        public void SetFreeMode()
        {
            controlMode = ControlMode.FreeRotation;
        }
    }
}//namespace BinaryLibrary.Showcase

