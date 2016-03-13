/////////////////////////////////////////////////////////////////////
//Copyright © 2013-2016 Ivan Perez
//Author: Ivan Perez
//Linkedin: http://es.linkedin.com/in/ivanperezduran
//Portfolio: http://binarycv.com
/////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

using BinaryLibrary.Showcase;
namespace BinaryLibrary.FSM
{
    /// <summary>
    /// Condition which is true if has found a target.
    /// </summary>
    public class TargetLockedCondition : FSMCondition
    {
        private CharacterBlackboard blackboard;

        public override void Initialize(FSMSystem _fsmOwner, FSMTransition _transitionOwner)
        {
            base.Initialize(_fsmOwner, _transitionOwner);
            blackboard = fsmOwner.ownerGO.GetComponent<CharacterBlackboard>();
        }

        protected override void OnCheck()
        {
            base.OnCheck();

            if (blackboard.target)
            {
                condition = true;
            }
        }

#if UNITY_EDITOR
        public override void DrawDetails(int indexCondition)
        {
            base.DrawDetails(indexCondition);
            GUILayout.Label("Target locked");
        }
#endif
    }
}//namespace BinaryLibrary.FSM
