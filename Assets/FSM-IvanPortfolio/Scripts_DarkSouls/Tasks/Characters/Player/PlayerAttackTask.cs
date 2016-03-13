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
    /// Player attack task.
    /// </summary>
    public class PlayerAttackTask : CharacterControllerTask
    {
        public Vector3 velocity = Vector3.zero;
        public float speed = 4;

        enum AttackType
        {
            Attack360Low = 1,
            Attack360High = 2,
            AttackBackhand = 3,
            AttackDownward = 4
        }
        private AttackType attackType;


        public PlayerAttackTask()
        {
            nameTask = "PlayerAttackTask";
        }

        /// <summary>
        /// When a state change FSMSystem calls this method from the current state before update the current state.
        /// </summary>
        public override void OnEnter()
        {
            base.OnEnter();

            Vector3 worldVelocity = ownerRigidBody.velocity;
            
            //The relative velocity to the player direction.
            Vector3 relativeVelocity = ownerTransform.InverseTransformDirection(worldVelocity);
            
            //if the forward speed is bigger than the half of the max speed.
            if (speed/2 < relativeVelocity.z)
            {   
                ownerBlackboard.attackType = Random.Range(1, 3) + 2;
            }
            else
            {
                ownerBlackboard.attackType = Random.Range(1, 3);
            }

            attackType = (AttackType)ownerBlackboard.attackType;
            //Active Sword.
            ownerBlackboard.swordController.ActiveSword();

            ownerRigidBody.velocity = Vector3.zero;
        }

        /// <summary>
        /// Update the current state.
        /// </summary>
        public override void OnUpdate()
        {
            //if (animController.animator.GetAnimatorTransitionInfo(0).IsName("Base Layer."+attackType.ToString() + " -> Base Layer.TargetLockedMovement"))
            if (ownerAnimController.animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer."+attackType.ToString())
                && 0.75f < ownerAnimController.animator.GetCurrentAnimatorStateInfo(0).normalizedTime)
            {
                taskFinished = true;
            }
        }

        /// <summary>
        /// When a state changes, FSMSystem calls this method from the last state before change to new state.
        /// </summary>
        public override void OnExit()
        {
            ownerBlackboard.attackType = 0;
            ownerBlackboard.swordController.DeactiveSword();
        }

#if UNITY_EDITOR
        public override void DrawDetails()
        {
            base.DrawDetails();

            speed = EditorGUILayout.FloatField("Speed", speed);
        }
#endif
    }
}//namespace BinaryLibrary.Showcase
