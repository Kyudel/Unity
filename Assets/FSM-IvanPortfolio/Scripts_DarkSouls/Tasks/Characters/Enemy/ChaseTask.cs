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
    /// Chase the player task.
    /// </summary>
    public class ChaseTask : CharacterControllerTask
    {
        public Vector3 velocity = Vector3.zero;
        public float speed = 1.5f;
        public float turnSpeed = 10;

        /// <summary>
        /// Min angle between the player and the direction of the enemy that is facing.
        /// </summary>
        public float minAngleToAttack = 5;

        public ChaseTask()
        {
            nameTask = "ChaseTask";
        }

        /// <summary>
        /// When a state change FSMSystem calls this method from the current state before update the current state.
        /// </summary>
        public override void OnEnter()
        {
            base.OnEnter();
            ownerBlackboard.onGround = true;

            targetEulerRotation = ownerTransform.rotation.eulerAngles;

            ownerBlackboard.targetLocked = true;

            ownerCameraController.SetFreeMode();
        }

        /// <summary>
        /// Update the current state.
        /// </summary>
        public override void OnUpdate()
        {
            //Rotation trying to face the player.
            Vector3 directionToTarget = ownerBlackboard.target.position - ownerTransform.position;
            directionToTarget = directionToTarget.normalized;
            Quaternion targetQuaternion = Quaternion.LookRotation(directionToTarget);
            HandleRotation(directionToTarget, targetQuaternion);


            //Applies horizontal movement.
            Vector3 currentVelocity = ownerTransform.forward * speed;
            currentVelocity.y = ownerRigidBody.velocity.y;

            //Applies vertical movement.
            ownerRigidBody.velocity = currentVelocity;
            ownerRigidBody.AddForce((Physics.gravity * extraGravity) - Physics.gravity);

            //Fix the velocity to go where the character faces.
            Vector3 fixedVelocity = ownerTransform.InverseTransformDirection(ownerRigidBody.velocity);
            ownerBlackboard.forward = fixedVelocity.z;


            //If the conditions to attack are right, the task is finished and it will go to attack state.
            if (Vector3.Distance(ownerTransform.position, ownerBlackboard.target.position) < ownerBlackboard.attackRange
                && Quaternion.Angle(ownerTransform.rotation, targetQuaternion) < minAngleToAttack)
            {
                taskFinished = true;
            }
        }

        /// <summary>
        /// When a state changes, FSMSystem calls this method from the last state before change to new state.
        /// </summary>
        public override void OnExit() { }

        private void HandleRotation(Vector3 directionToTarget, Quaternion targetQuaternion)
        {
            if (minAngleToAttack < Quaternion.Angle(ownerTransform.rotation, targetQuaternion))
            {
                float angle = Quaternion.Angle(ownerTransform.rotation, targetQuaternion);
                //We identify the angle polarity with the cross product.
                if (Vector3.Cross(ownerTransform.forward, directionToTarget).y < 0)
                {
                    angle = -angle;
                }
                ownerBlackboard.sideways = angle / 90;

                Vector3 newDirection = Vector3.Lerp(ownerTransform.forward, directionToTarget, Time.deltaTime * turnSpeed / 5);
                ownerTransform.rotation = Quaternion.LookRotation(newDirection);
            }
            else
            {
                ownerBlackboard.sideways = 0;
            }
        }
#if UNITY_EDITOR
        public override void DrawDetails()
        {
            base.DrawDetails();

            speed = EditorGUILayout.FloatField("Speed", speed);
            turnSpeed = EditorGUILayout.FloatField("Turn Speed", turnSpeed);
            minAngleToAttack = EditorGUILayout.FloatField("Min Angle To Attack", minAngleToAttack);
        }
#endif
    }
}//namespace BinaryLibrary.Showcase
