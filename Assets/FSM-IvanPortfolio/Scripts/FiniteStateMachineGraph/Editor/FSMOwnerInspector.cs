/////////////////////////////////////////////////////////////////////
//Copyright © 2013-2016 Ivan Perez
//Author: Ivan Perez
//Linkedin: http://es.linkedin.com/in/ivanperezduran
//Portfolio: http://binarycv.com
/////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEditor;
using System.Collections;

namespace BinaryLibrary.FSM
{
    /// <summary>
    /// Component container of a fsm graph.
    /// </summary>
    [CustomEditor(typeof(FSMOwner))]
    public class FSMOwnerInspector : Editor {

        FSMOwner fsmOwner;
        public override void OnInspectorGUI()
        {
            if (fsmOwner == null)
            {
                fsmOwner = (FSMOwner)target;
            }

            DrawDefaultInspector();

            if (GUILayout.Button("Delete fsm"))
            {
                fsmOwner.serializedFSM = "";
                fsmOwner.fsm = null;
            }
        } 
    }
}