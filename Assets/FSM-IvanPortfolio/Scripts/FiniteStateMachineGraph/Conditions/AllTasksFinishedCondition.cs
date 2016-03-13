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
    /// Triggered when all tasks of a state are marked as finished.
    /// Condition by default in every transition.
    /// </summary>
	public class AllTasksFinishedCondition : FSMCondition
	{
		protected override void OnCheck()
		{
            if (fsmOwner.statesList[transitionOwner.sourceId].allTasksFinished)
            {
                condition = true;
            }
		}

		#if UNITY_EDITOR
		public override void DrawDetails(int indexCondition)
		{
			base.DrawDetails(indexCondition);

			GUILayout.Label("On All Tasks Finished");
		}
		#endif
	}
}
