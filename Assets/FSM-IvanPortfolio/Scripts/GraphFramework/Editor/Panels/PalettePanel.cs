/////////////////////////////////////////////////////////////////////
//Copyright © 2013-2016 Ivan Perez
//Author: Ivan Perez
//Linkedin: http://es.linkedin.com/in/ivanperezduran
//Portfolio: http://binarycv.com
/////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

using BinaryLibrary.GraphCore;

using BinaryLibrary.FSM;

namespace BinaryLibrary.GraphEditor
{
    /// <summary>
    /// Panel with graph functionalities.
    /// </summary>
    public class ActionPanel : BasePanel
    {
        public ActionPanel(GraphWindow graphWindow, GUISkin skin)
            : base(graphWindow, skin)
        {
            title = "Palette";
        }

        public override void DrawGUI(Rect panelRect, Event e, Graph nodeGraph)
        {
            base.DrawGUI(panelRect, e, nodeGraph);

            GUI.Box(panelRect, title, stylePanel);
            GUILayout.BeginArea(panelRect);
            GUILayout.Space(30);

            if (GUILayout.Button("Reset Node Graph"))
            {
                graphWindow.ResetNodeGraph();
            }

            if (GUILayout.Button("Clean console"))
            {
               nodeGraph.CleanRecordedActionsList();
            }
            GUILayout.EndArea();
        }
    }
}