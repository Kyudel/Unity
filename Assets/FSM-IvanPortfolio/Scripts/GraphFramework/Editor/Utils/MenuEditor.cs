/////////////////////////////////////////////////////////////////////
//Copyright © 2013-2016 Ivan Perez
//Author: Ivan Perez
//Linkedin: http://es.linkedin.com/in/ivanperezduran
//Portfolio: http://binarycv.com
/////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEditor;
using System.Collections;

using BinaryLibrary.GraphEditor;

namespace BinaryLibrary.EditorUtils
{
    /// <summary>
    /// Shows the graph button in the top menu bar.
    /// </summary>
    public class MenuEditor : MonoBehaviour
    {
        [MenuItem("Example/Finite State Machine %g")]
        public static void ShowWindow()
        {
            GraphWindow.ShowWindow();
        }
    }
}//namespace BinaryLibrary.EditorUtils
