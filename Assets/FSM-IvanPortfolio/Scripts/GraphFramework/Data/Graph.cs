/////////////////////////////////////////////////////////////////////
//Copyright © 2013-2016 Ivan Perez
//Author: Ivan Perez
//Linkedin: http://es.linkedin.com/in/ivanperezduran
//Portfolio: http://binarycv.com
/////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

using BinaryLibrary.FSM;
        
namespace BinaryLibrary.GraphCore
{
    /// <summary>
    /// Base class container.
    /// </summary>
    public abstract class Graph
    {
        #region Variables
        public List<Node> nodeList = new List<Node>();

        [NonSerialized]
        public Node nodeSelected;
        [NonSerialized]
        public Node lastNodeSelected;

        [NonSerialized]
        public bool isDirty = false;

        public int nextNodeId = 0;
        #endregion

        #region Main Methods
        public virtual void Update() { }
        #endregion

#if UNITY_EDITOR
        /// <summary>
        /// Recorded actions so they can be show in the console panel.
        /// </summary>
        private List<string> recordedActionsList;

        protected int nodeToDelete = -1;

#endif
        #region Utils Methods
        public void AddNode(NodeType nodeType, Vector2 mousePosition)
        {
            Node baseNode = null;

            switch (nodeType)
            {
                case NodeType.ANY_STATE:
                    break;
                case NodeType.TASK_STATE:
                    baseNode = new FSMState(nextNodeId);
                    break;
                case NodeType.RECURRENT_STATE:
                    break;
                default:
                    break;
            }

            baseNode.Initialize(mousePosition, this);

            if (nodeList.Count < 1)
            {
                ((FSMState)baseNode).starterState = true;
            }

            nodeList.Add(baseNode);

            if (((FSMSystem)this) != null)
            {
                ((FSMSystem)this).AddState(((FSMState)baseNode));
            }
        }

        public virtual void CreateTransition(Node sourceNode, Node targetNode) { }

        /// <summary>
        /// If a value changes, set as dirty so the graph can be serialized and saved.
        /// </summary>
        public void SetDirtyAndSave()
        {
            isDirty = true;
        }

#if UNITY_EDITOR
        public void DeleteStatePullAction(int nodeToDelete)
        {
            this.nodeToDelete = nodeToDelete;
        }

        public virtual void DeleteNode(int id) { }

        public int GetNodePositionById(int id)
        {
            for (int i = 0; i < nodeList.Count; i++)
            {
                if (nodeList[i].id == id)
                {
                    return i;
                }
            }
            return -1;
        }

        public virtual void UpdateGUI(Event e, Graph nodeGraph, Rect panelRect){ }

        public virtual void DrawGUI(Event e, Graph nodeGraph, Rect panelRect, GUISkin skinPanel) { }

        public void AddRecordedAction(string newAction)
        {
            if (recordedActionsList == null)
            {
                recordedActionsList = new List<string>();
            }
            recordedActionsList.Insert(0, newAction);

            if (30 < recordedActionsList.Count)
            {
                recordedActionsList.RemoveAt(recordedActionsList.Count - 1);
            }
        }

        public void CleanRecordedActionsList()
        {
            if (Application.isPlaying)
            {
                if (recordedActionsList != null)
                {
                    recordedActionsList.Clear();
                }
            }
        }

        public List<string> GetRecordedActionList()
        {
            if (recordedActionsList == null)
            {
                recordedActionsList = new List<string>();
            }
            return recordedActionsList;
        }
#endif

        #endregion
    }
}