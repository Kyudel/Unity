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
    /// Condition triggered when there a specified mouse event.
    /// </summary>
    public class MouseInputCondition : FSMCondition
    {

        public enum InputKey
        {
            LEFT_CLICK,
            RIGHT_CLICK,
            MIDDLE_CLICK,
        }
        public InputKey inputKey;

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
                    condition = Input.GetMouseButtonDown((int)inputKey);
                    break;
                case InputType.KEY_UP:
                    condition = Input.GetMouseButtonUp((int)inputKey);
                    break;
                case InputType.KEY_PRESSING:
                    condition = Input.GetMouseButton((int)inputKey);
                    break;
            }

        }

#if UNITY_EDITOR
        public override void DrawDetails(int indexCondition)
        {
            base.DrawDetails(indexCondition);
            inputType = (InputType)EditorGUILayout.EnumPopup("InputType:", inputType);
            inputKey = (InputKey)EditorGUILayout.EnumPopup("InputKey:", inputKey);
        }
#endif
    }
}