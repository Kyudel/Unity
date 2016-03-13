/////////////////////////////////////////////////////////////////////
//Copyright © 2013-2016 Ivan Perez
//Author: Ivan Perez
//Linkedin: http://es.linkedin.com/in/ivanperezduran
//Portfolio: http://binarycv.com
/////////////////////////////////////////////////////////////////////

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BinaryLibrary.Showcase
{
    /// <summary>
    /// This task makes that the owner moves around the target.
    /// </summary>
    public class TargetLockedTask : CharacterControllerTask
    {
        public Vector3 velocity = Vector3.zero;
        public float speed = 3;
        public float turnSpeed = 10;

        public TargetLockedTask()
        {
            nameTask = "TargetLockedTask";
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

            ownerCameraController.SetLockedMode();
        }

        /// <summary>
        /// Update the current state.
        /// </summary>
        public override void OnUpdate()
        {
            Vector2 axis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            currentEulerRotation = ownerTransform.rotation.eulerAngles;

            Vector3 directionToTarget = ownerBlackboard.target.position - ownerTransform.position;
            ownerTransform.rotation = Quaternion.LookRotation(directionToTarget);


            //Clamps the axis so if the Vector is (1,1) still has a max magnitude of 1 and don't move faster than (1,0).
            Vector3 fixedAxis = Vector3.ClampMagnitude(axis, 1);

            //Applies horizontal movement.
            Vector3 currentVelocity = (ownerTransform.forward * fixedAxis.y + ownerTransform.right * fixedAxis.x) * speed;
            currentVelocity.y = ownerRigidBody.velocity.y;

            //Applies vertical movement.
            ownerRigidBody.velocity = currentVelocity;
            ownerRigidBody.AddForce((Physics.gravity * extraGravity) - Physics.gravity);

            //Fix the velocity to go where the character faces.
            Vector3 fixedVelocity = ownerTransform.InverseTransformDirection(ownerRigidBody.velocity);

            ownerBlackboard.forward = axis.y;
            ownerBlackboard.sideways = axis.x;
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
