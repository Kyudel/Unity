/////////////////////////////////////////////////////////////////////
//Copyright © 2013-2016 Ivan Perez
//Author: Ivan Perez
//Linkedin: http://es.linkedin.com/in/ivanperezduran
//Portfolio: http://binarycv.com
/////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;
using BinaryLibrary.GraphCore;
using BinaryLibrary.FSM;

namespace BinaryLibrary.GraphEditor
{
    /// <summary>
    /// Panel with a consoles with info in runtime about the different operations inside the graph.
    /// </summary>
    public class ConsolePanel : BasePanel
    {
        Vector2 positionScrollView = Vector2.zero;

        public ConsolePanel(GraphWindow graphWindow, GUISkin skin)
            : base(graphWindow, skin)
        {
            title = "Console";
        }

        public override void DrawGUI(Rect panelRect, Event e, Graph nodeGraph)
        {
            base.DrawGUI(panelRect, e, nodeGraph);

            GUI.Box(panelRect, title, stylePanel);
            GUILayout.BeginArea(panelRect);
            GUILayout.Space(30);

            positionScrollView = EditorGUILayout.BeginScrollView(positionScrollView, GUILayout.Width(panelRect.width), GUILayout.Height(panelRect.height - 30));
            if (Application.isPlaying && nodeGraph != null)
            {
                for (int i = 0; i < nodeGraph.GetRecordedActionList().Count; i++)
                {
                    GUILayout.BeginVertical(GUI.skin.box);
                    GUILayout.Label(nodeGraph.GetRecordedActionList()[i]);
                    GUILayout.EndVertical();
                }
            }
            EditorGUILayout.EndScrollView();

            GUILayout.EndArea();
        }
    }
}