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
    /// Attacks the player.
    /// </summary>
    public class EnemyAttackTask : CharacterControllerTask
    {
        public Vector3 velocity = Vector3.zero;
        public float speed = 4;
        public float turnSpeed = 10;
        public float minAngleToAttack = 5;

        private AnimatorEnemyController enemyAnimController;

        enum AttackType
        {
            AttackBackhand = 1,
            AttackDownward = 2
        }
        private AttackType attackType;

        private bool isNewAttack = false;  

        public EnemyAttackTask()
        {
            nameTask = "EnemyAttackTask";
        }

        /// <summary>
        /// When a state change FSMSystem calls this method from the current state before update the current state.
        /// </summary>
        public override void OnEnter()
        {
            base.OnEnter();

            enemyAnimController = fsmOwner.ownerGO.GetComponent<AnimatorEnemyController>();

            //Triggers new attack.
            isNewAttack = false;
            ownerBlackboard.attackType = Random.Range(1, 3);
            attackType = (AttackType)ownerBlackboard.attackType;
            ownerBlackboard.attack = true;

            //Active the sword controller.
            ownerBlackboard.swordController.ActiveSword();
        }

        /// <summary>
        /// Update the current state.
        /// </summary>
        public override void OnUpdate()
        {
            if (enemyAnimController.animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Looking"))
            {
                ownerBlackboard.swordController.DeactiveSword();

                HandleRotation();

                //Randomize new attack.
                if (!isNewAttack)
                {
                    //Triggers new attack.
                    isNewAttack = true;
                    ownerBlackboard.attackType = Random.Range(1, 3);
                    attackType = (AttackType)ownerBlackboard.attackType;
                    ownerBlackboard.swordController.DeactiveSword();
                }

                if (ownerBlackboard.attackRange < Vector3.Distance(ownerTransform.position, ownerBlackboard.target.position))
                {
                    taskFinished = true;
                }
            }
            //if it is playing an attack animation.
            else
            {
                if (isNewAttack)
                {
                    ownerBlackboard.swordController.ActiveSword();
                }
                isNewAttack = false;
            }
        }

        /// <summary>
        /// When a state changes, FSMSystem calls this method from the last state before change to new state.
        /// </summary>
        public override void OnExit()
        {
            ownerBlackboard.attackType = 0;
            ownerBlackboard.attack = false;
            ownerBlackboard.swordController.DeactiveSword();

        }

        private void HandleRotation()
        {
            //Rotation trying to face the player.
            Vector3 directionToTarget = ownerBlackboard.target.position - ownerTransform.position;
            directionToTarget = directionToTarget.normalized;
            Quaternion targetQuaternion = Quaternion.LookRotation(directionToTarget);

            if (minAngleToAttack < Quaternion.Angle(ownerTransform.rotation, targetQuaternion))
            {
                float angle = Quaternion.Angle(ownerTransform.rotation, targetQuaternion);
                //We identify the angle polarity with the cross product.
                if (Vector3.Cross(ownerTransform.forward, directionToTarget).y < 0)
                {
                    angle = -angle;
                }
                ownerBlackboard.turn = angle;

                Vector3 newDirection = Vector3.Lerp(ownerTransform.forward, directionToTarget, Time.deltaTime * turnSpeed / 10);
                ownerTransform.rotation = Quaternion.LookRotation(newDirection);
            }
            else
            {
                ownerBlackboard.turn = 0;
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
