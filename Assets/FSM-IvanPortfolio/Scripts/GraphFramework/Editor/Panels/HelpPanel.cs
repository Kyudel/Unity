/////////////////////////////////////////////////////////////////////
//Copyright © 2013-2016 Ivan Perez
//Author: Ivan Perez
//Linkedin: http://es.linkedin.com/in/ivanperezduran
//Portfolio: http://binarycv.com
/////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

using BinaryLibrary.GraphCore;

namespace BinaryLibrary.GraphEditor
{
    /// <summary>
    /// Panel with info about the graph.
    /// </summary>
    public class HelpPanel : BasePanel
    {
        public HelpPanel(GraphWindow graphWindow, GUISkin skin)
            : base(graphWindow, skin)
        {
            title = "Help";
        }

        public override void DrawGUI(Rect panelRect, Event e, Graph nodeGraph)
        {
            base.DrawGUI(panelRect, e, nodeGraph);

            GUI.Box(panelRect, title, stylePanel);

            GUILayout.BeginArea(panelRect);
            GUILayout.Space(30);
            GUILayout.Label("- Right Click on panel to create nodes!");
            GUILayout.Space(10);

            GUILayout.Label("- Scroll the panel with middle button.");
            GUILayout.Label("- Left click on a node to select it.");
            GUILayout.Label("-To make a transition click on any \"link box\"\nand drag to another node.");
            GUILayout.EndArea();
        }
    }
}//namespace BinaryLibrary.EditorGraph

