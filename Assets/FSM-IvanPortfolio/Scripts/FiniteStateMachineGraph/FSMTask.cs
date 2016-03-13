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
    /// Specific behaviour inside a state.
    /// A State can have multiplies tasks.
    /// </summary>
    public class FSMTask
    {
        protected FSMSystem fsmOwner;

        public bool taskFinished = false;

        protected string nameTask = "";

        public void Initialize(FSMSystem fsmOwner)
        {
            this.fsmOwner = fsmOwner;
        }

        public void Enter()
        {
            taskFinished = false;
            OnEnter();
        }

        public void Update()
        {
            OnUpdate();
        }

        public void Exit()
        {
            OnExit();
        }

        /// <summary>
        /// When a state change FSMSystem calls this method from the current state before update the current state.
        /// </summary>
        public virtual void OnEnter() { }

        /// <summary>
        /// Update the current state.
        /// </summary>
        public virtual void OnUpdate() { }


        /// <summary>
        /// When a state changes, FSMSystem calls this method from the last state before change to new state.
        /// </summary>
        public virtual void OnExit() { }

        /// <summary>
        /// When the game is paused.
        /// </summary>
        public virtual void OnPause() { }

#if UNITY_EDITOR
        public virtual void DrawDetails() { }
#endif

    }
}