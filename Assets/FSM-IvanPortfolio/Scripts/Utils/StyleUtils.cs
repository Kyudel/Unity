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

namespace BinaryLibrary.Utils
{
    #if UNITY_EDITOR
    /// <summary>
    /// Library with styles utilities.
    /// </summary>
    public class StyleUtils
    {
        public static GUIStyle titleTextStyle
        {
            get
            {
                GUIStyle style = new GUIStyle("label");
                style.alignment = TextAnchor.MiddleCenter;
                style.fontStyle = FontStyle.Bold;
                style.fixedHeight = 30;
                return style;
            }
        }

        public static GUIStyle centeredTextStyle
        {
            get
            {
                GUIStyle style = new GUIStyle("label");
                style = new GUIStyle("label");
                style.alignment = TextAnchor.MiddleCenter;

                return style;
            }
        }

        public static GUIStyle commentTextStyle
        {
            get
            {
                GUIStyle style = new GUIStyle("label");
                style = new GUIStyle("label");
                style.alignment = TextAnchor.MiddleCenter;
                style.fontStyle = FontStyle.Italic;
                return style;
            }
        }

        public static GUIStyle correctAnswerStyle
        {
            get
            {
                GUIStyle style = new GUIStyle("label");
                style = new GUIStyle(GUI.skin.textArea.name);
                style.normal.textColor = Color.green;
                return style;
            }
        }
        public static GUIStyle wrongAnswerStyle
        {
            get
            {
                GUIStyle style = new GUIStyle("label");
                style = new GUIStyle(GUI.skin.textArea.name);
                style.normal.textColor = Color.red;
                return style;
            }
        }

        public static void DrawSeparator()
        {
            Rect rect = GUILayoutUtility.GetLastRect();
            Handles.color = Color.grey;
            Handles.DrawLine(new Vector3(rect.x, rect.y + 25), new Vector3(rect.width, rect.y + 25));
            Handles.DrawLine(new Vector3(rect.x, rect.y + 1 + 25), new Vector3(rect.width, rect.y + 1 + 25));
            Handles.DrawLine(new Vector3(rect.x, rect.y + 2 + 25), new Vector3(rect.width, rect.y + 2 + 25));
        }
    }
    #endif
}