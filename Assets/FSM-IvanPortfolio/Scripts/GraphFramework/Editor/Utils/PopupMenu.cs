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
using BinaryLibrary.GraphCore;

using BinaryLibrary.FSM;
namespace BinaryLibrary.EditorUtils
{
    /// <summary>
    /// Context menu which shows the different nodes that can be used.
    /// </summary>
    public class PopupMenu
    {
        public Vector2 currentPositionScrollView;
        public Vector2 currentMousePosition;

        /// <summary>
        /// Show the node context menu.
        /// </summary>
        /// <param name="e">Event.</param>
        /// <param name="positionScrollView">Position of the cursor.</param>
        public void ShowMenuNodePanel(Event e, Vector2 positionScrollView)
        {
            GenericMenu menu = new GenericMenu();

            currentPositionScrollView = positionScrollView;
            currentMousePosition = e.mousePosition;
            menu.AddItem(new GUIContent("Add Task State"), false, CreateNode, NodeType.TASK_STATE);

            menu.ShowAsContext();
            e.Use();
        }
        
        /// <summary>
        /// Create the specified node.
        /// </summary>
        /// <param name="_nodeType"></param>
        private void CreateNode(object _nodeType)
        {
            NodeType nodeType = (NodeType)_nodeType;

            GraphWindow.currentGraphNode.AddNode(nodeType, currentPositionScrollView + currentMousePosition);
        }
    }
}//namespace BinaryLibrary.EditorUtils
