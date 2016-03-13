/////////////////////////////////////////////////////////////////////
//Copyright © 2013-2016 Ivan Perez
//Author: Ivan Perez
//Linkedin: http://es.linkedin.com/in/ivanperezduran
//Portfolio: http://binarycv.com
/////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using BinaryLibrary.Utils;
using UnityEditor;
#endif

namespace BinaryLibrary.Showcase
{
    /// <summary>
    /// Controller of the movement on ground.
    /// </summary>
    public class GroundedTask : CharacterControllerTask
    {
        public Vector3 velocity = Vector3.zero;
        public float speed = 4;
        public float turnSpeed = 10;

        public GroundedTask()
        {
            nameTask = "GroundedTask";
        }

        /// <summary>
        /// When a state change FSMSystem calls this method from the current state before update the current state.
        /// </summary>
        public override void OnEnter()
        {
            base.OnEnter();
            ownerBlackboard.onGround = true;

            currentEulerRotation = ownerTransform.rotation.eulerAngles;
            targetEulerRotation = ownerTransform.rotation.eulerAngles;

            ownerBlackboard.target = null;
            ownerBlackboard.targetLocked = false;

            ownerCameraController.SetFreeMode();
        }

        /// <summary>
        /// Update the current state.
        /// </summary>
        public override void OnUpdate()
        {
            Vector2 axis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            currentEulerRotation = ownerTransform.rotation.eulerAngles;

            //Transforms the controller axis to face where the camera faces.
            if (axis != Vector2.zero)
            {
                Vector3 relativeAxis = ownerBlackboard.cameraTransform.forward * axis.y + ownerBlackboard.cameraTransform.right * axis.x;
                targetEulerRotation = Quaternion.LookRotation(relativeAxis).eulerAngles;
            }
            else
            {
                targetEulerRotation = ownerTransform.rotation.eulerAngles;
            }

            if (0.1f < Vector3.Distance(currentEulerRotation, targetEulerRotation))
            {
                currentEulerRotation = Quaternion.Lerp(Quaternion.Euler(currentEulerRotation), Quaternion.Euler(targetEulerRotation), turnSpeed * Time.smoothDeltaTime / 2).eulerAngles;
            }
            ownerTransform.rotation = Quaternion.Euler(currentEulerRotation);

            //Takes the largest axis input as the speed multiplier.
            float speedMultiplier = Mathf.Max(Mathf.Abs(axis.x), Mathf.Abs(axis.y));

            //Applies horizontal movement.
            Vector3 currentVelocity = ownerTransform.forward * speedMultiplier * speed;
            currentVelocity.y = ownerRigidBody.velocity.y;

            //Applies vertical movement.
            ownerRigidBody.velocity = currentVelocity;
            ownerRigidBody.AddForce((Physics.gravity * extraGravity) - Physics.gravity);

            //Fix the velocity to go where the character faces.
            Vector3 fixedVelocity = ownerTransform.InverseTransformDirection(ownerRigidBody.velocity);

            ownerBlackboard.forward = fixedVelocity.z;
            //characterBlackboard.turn = Input.GetAxis("Horizontal");
        }

        /// <summary>
        /// When a state changes, FSMSystem calls this method from the last state before change to new state.
        /// </summary>
        public override void OnExit() { }

#if UNITY_EDITOR
        public override void DrawDetails()
        {
            base.DrawDetails();

            speed = EditorGUILayout.FloatField("Speed", speed);
            turnSpeed = EditorGUILayout.FloatField("Turn Speed", turnSpeed);
        }
#endif
    }
}//namespace BinaryLibrary.Showcase
