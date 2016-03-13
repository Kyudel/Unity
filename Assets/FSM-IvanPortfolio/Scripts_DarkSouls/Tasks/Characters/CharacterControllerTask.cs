/////////////////////////////////////////////////////////////////////
//Copyright © 2013-2016 Ivan Perez
//Author: Ivan Perez
//Linkedin: http://es.linkedin.com/in/ivanperezduran
//Portfolio: http://binarycv.com
/////////////////////////////////////////////////////////////////////

using UnityEngine;
using BinaryLibrary.FSM;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BinaryLibrary.Showcase
{
    /// <summary>
    /// Basic task with necessary elements to control the movement.
    /// </summary>
    public abstract class CharacterControllerTask : FSMTask
    {
        protected Transform ownerTransform;
        protected Rigidbody ownerRigidBody;
        protected CapsuleCollider ownerCollider;
        protected CameraController ownerCameraController;
        /// <summary>
        /// Blackboard with common variables and functions to all tasks.
        /// </summary>
        protected CharacterBlackboard ownerBlackboard;

        /// <summary>
        /// Animator controller
        /// </summary>
        protected AnimatorPlayerController ownerAnimController;


        /// <summary>
        /// Extra acceleration to have a jump more realistic.
        /// </summary>
        public float extraGravity = 2;

        protected Vector3 currentEulerRotation = Vector3.zero;
        protected Vector3 targetEulerRotation = Vector3.zero;

        public CharacterControllerTask() { }

        /// <summary>
        /// When a state change FSMSystem calls this method from the current state before update the current state.
        /// </summary>
        public override void OnEnter()
        {
            ownerTransform = fsmOwner.ownerGO.transform;
            ownerRigidBody = fsmOwner.ownerGO.GetComponent<Rigidbody>();
            ownerCollider = fsmOwner.ownerGO.GetComponent<CapsuleCollider>();
            ownerBlackboard = fsmOwner.ownerGO.GetComponent<CharacterBlackboard>();
            ownerAnimController = fsmOwner.ownerGO.GetComponent<AnimatorPlayerController>();

            ownerCameraController = ownerBlackboard.cameraTransform.GetComponent<CameraController>();

            currentEulerRotation = ownerTransform.rotation.eulerAngles;
            currentEulerRotation.y = 0;
#if UNITY_EDITOR
            if (fsmOwner.ownerGO.tag == "Player")
            {
                DebugTextGUI.SetPlayerState(nameTask);
            }
            else
            {
                DebugTextGUI.SetEnemyState(nameTask);
            }
#endif
        }
#if UNITY_EDITOR

        public override void DrawDetails()
        {
            base.DrawDetails();

            extraGravity = EditorGUILayout.FloatField("Extra Gravity", extraGravity);
        }
#endif
    }
}//namespace BinaryLibrary.Showcase
