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
    /// Condition triggered when there a specified event key happens.
    /// </summary>
    public class KeyInputCondition : FSMCondition
    {
        public KeyCode keyCode;

        public enum InputType
        { 
            KEY_DOWN,
            KEY_UP,
            KEY_PRESSING,
        }
        public InputType inputType = InputType.KEY_DOWN;

        protected override void OnCheck()
        {
            base.OnCheck();

            switch (inputType)
            {
                case InputType.KEY_DOWN:
                    condition = Input.GetKeyDown(keyCode);
                    break;
                case InputType.KEY_UP:
                    condition = Input.GetKeyUp(keyCode);
                    break;
                case InputType.KEY_PRESSING:
                    condition = Input.GetKey(keyCode);
                    break;
            }
            
        }

        #if UNITY_EDITOR
        public override void DrawDetails(int indexCondition)
        {
            base.DrawDetails(indexCondition);
            inputType = (InputType)EditorGUILayout.EnumPopup("InputType:", inputType);
            keyCode = (KeyCode)EditorGUILayout.EnumPopup("KeyCode:", keyCode);
        }
        #endif
    }
}