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
    /// Roll the owner character.
    /// </summary>
    public class RollTask : CharacterControllerTask
    {
        public Vector3 velocity = Vector3.zero;
        public float speed = 4;

        public RollTask()
        {
            nameTask = "RollTask";
        }

        /// <summary>
        /// When a state change FSMSystem calls this method from the current state before update the current state.
        /// </summary>
        public override void OnEnter()
        {
            base.OnEnter();

            ownerAnimController.SetRollTrigger();

            Vector2 axis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            if (axis != Vector2.zero)
            {
                axis = axis.normalized;

                //Relative to the direction the owner is facing.
                Vector3 relativeAxis = ownerTransform.forward * axis.y + ownerTransform.right * axis.x;

                ownerTransform.rotation = Quaternion.LookRotation(relativeAxis);
                
                //Clamps the axis so if the Vector is (1,1) still has a max magnitude of 1 and don't move faster than (1,0).
                Vector3 fixedAxis = Vector3.ClampMagnitude(relativeAxis, 1);
                velocity = fixedAxis * speed;
            }
            else
            {
                velocity = ownerTransform.forward * speed;
            }
        }

        /// <summary>
        /// Update the current state.
        /// </summary>
        public override void OnUpdate()
        {

            //f you have a curve with the same name as one of the parameters in the Animator Controller, then that
            //parameter will take its value automatically from the curve at each point in the timeline.
            //See: http://docs.unity3d.com/Manual/AnimationCurvesOnImportedClips.html
            float curve_RollMovement = ownerBlackboard.curve_RollMovement;

            //if (animController.animator.GetAnimatorTransitionInfo(0).IsName("Base Layer."+attackType.ToString() + " -> Base Layer.TargetLockedMovement"))
            if (ownerAnimController.animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Roll")
                && 0.9f < ownerAnimController.animator.GetCurrentAnimatorStateInfo(0).normalizedTime)
            {
                taskFinished = true;
            }
            else
            {
                ownerRigidBody.velocity = velocity * curve_RollMovement;
                ownerRigidBody.AddForce((Physics.gravity * extraGravity) - Physics.gravity);
            }
        }

        /// <summary>
        /// When a state changes, FSMSystem calls this method from the last state before change to new state.
        /// </summary>
        public override void OnExit()
        {
            ownerBlackboard.attackType = 0;
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
