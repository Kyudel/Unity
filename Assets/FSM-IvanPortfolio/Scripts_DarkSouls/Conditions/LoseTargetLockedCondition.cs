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
    /// Condition triggered when the target is too far.
    /// </summary>
    public class LoseTargetLockedCondition : FSMCondition
    {
        private Transform ownerTransform;
        private CharacterBlackboard blackboard;

        public override void Initialize(FSMSystem _fsmOwner, FSMTransition _transitionOwner)
        {
            base.Initialize(_fsmOwner, _transitionOwner);
            blackboard = fsmOwner.ownerGO.GetComponent<CharacterBlackboard>();
            ownerTransform = fsmOwner.ownerGO.transform;
        }

        protected override void OnCheck()
        {
            base.OnCheck();

            if (blackboard.target)
            {
                float distance = Vector3.Distance(blackboard.target.position, ownerTransform.position);
                if (blackboard.loseTargetRange < distance)
                {
                    condition = true;
                }
            }
        }

#if UNITY_EDITOR
        public override void DrawDetails(int indexCondition)
        {
            base.DrawDetails(indexCondition);

            GUILayout.Label("Lose target locked");
        }
#endif
    }
}//namespace BinaryLibrary.FSM
