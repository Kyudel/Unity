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

namespace BinaryLibrary.FSM
{
    /// <summary>
    /// Determines if a state has to make a transition. It can be multiples conditions in a transition.
    /// </summary>
    public class FSMCondition
	{
	    protected bool condition = false;
	    public bool Condition { get { return condition; } }

		public FSMTransition transitionOwner;

        protected FSMSystem fsmOwner;
        public FSMCondition() { }

        public virtual void Initialize(FSMSystem _fsmOwner, FSMTransition _transitionOwner)
        {
            this.fsmOwner = _fsmOwner;
            this.transitionOwner = _transitionOwner;
        }

	    public bool Check()
	    {
	        OnCheck();

	        return condition;
	    }

        protected virtual void OnCheck() { }

        public void Reset()
        {
            condition = false;
            OnReset();
        }

        protected virtual void OnReset() { }

		#if UNITY_EDITOR
		public virtual void DrawDetails(int indexCondition)
		{
			GUILayout.Label("* Condition " + indexCondition + ":");
		}
		#endif
	}
}
