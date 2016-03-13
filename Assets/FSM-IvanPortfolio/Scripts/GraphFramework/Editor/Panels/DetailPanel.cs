/////////////////////////////////////////////////////////////////////
//Copyright © 2013-2016 Ivan Perez
//Author: Ivan Perez
//Linkedin: http://es.linkedin.com/in/ivanperezduran
//Portfolio: http://binarycv.com
/////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using UnityEditor;
using BinaryLibrary.GraphCore;

namespace BinaryLibrary.GraphEditor
{
    /// <summary>
    /// Shows the parameters and options of each node.
    /// </summary>
    public class DetailPanel : BasePanel
    {
        private Vector3 positionScrollView = Vector3.zero;

        public DetailPanel(GraphWindow graphWindow, GUISkin skin)
            : base(graphWindow, skin)
        {
            title = "Detail";
        }

        public override void DrawGUI(Rect panelRect, Event e, Graph nodeGraph)
        {
            base.DrawGUI(panelRect, e, nodeGraph);

            GUI.Box(panelRect, title, stylePanel);

            GUILayout.BeginArea(panelRect);
            positionScrollView = EditorGUILayout.BeginScrollView(positionScrollView,GUILayout.Width(panelRect.width), GUILayout.Height(panelRect.height - 8));

            GUILayout.Space(20);

            if (!Application.isPlaying)
            {
                if (nodeGraph != null && nodeGraph.nodeSelected != null)
                {
                    nodeGraph.nodeSelected.DrawDetails(panelRect);
                }
            }
            else
            {
                GUILayout.Space(10);
                GUILayout.BeginVertical(GUI.skin.box);
                GUILayout.Label("Not editable at runtime");
                GUILayout.EndVertical();

            }

            EditorGUILayout.EndScrollView();
            GUILayout.EndArea();
        }
    }
}