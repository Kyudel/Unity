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

namespace BinaryLibrary.FSM
{
    /// <summary>
    /// Simple tasks to move to a specified location.
    /// </summary>
    public class GoToTask : FSMTask
    {
        private Transform ownerTransform;
        public Vector3 destination = Vector3.zero;
        public float speed = 3;

        public GoToTask() { }

        /// <summary>
        /// When a state change FSMSystem calls this method from the current state before update the current state.
        /// </summary>
        public override void OnEnter()
        {
            ownerTransform = fsmOwner.ownerGO.transform;
        }

        /// <summary>
        /// Update the current state.
        /// </summary>
        public override void OnUpdate()
        {
            if (0.2f < Vector3.Distance(ownerTransform.position, destination))
            {
                Vector3 dir = Vector3.Normalize(destination - ownerTransform.position);
                ownerTransform.position += dir * speed * Time.deltaTime;
            }
            else
            {
                taskFinished = true;
            }
        }

        /// <summary>
        /// When a state changes, FSMSystem calls this method from the last state before change to new state.
        /// </summary>
        public override void OnExit() { }

#if UNITY_EDITOR
        public override void DrawDetails()
        {
            base.DrawDetails();

            destination = EditorGUILayout.Vector3Field("Destination", destination);
        }
#endif
    }
}