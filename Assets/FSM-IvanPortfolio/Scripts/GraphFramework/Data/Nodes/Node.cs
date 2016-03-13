/////////////////////////////////////////////////////////////////////
//Copyright © 2013-2016 Ivan Perez
//Author: Ivan Perez
//Linkedin: http://es.linkedin.com/in/ivanperezduran
//Portfolio: http://binarycv.com
/////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BinaryLibrary.GraphCore
{
    /// <summary>
    /// Base class of a node.
    /// </summary>
    public abstract class Node
    {
        #region Variables
        public int id;

        /// <summary>
        /// Esta variable sirve para dar un id único a cada nodo
        /// </summary>
        public string nodeName;

        public Rect nodeRect;

        public Graph graph;

        [NonSerialized]
        public bool isSelected = false;

        public NodeType nodeType;

        public List<Linker> linkList = new List<Linker>();

        [NonSerialized]
        public Rect[] inputRects;
        [NonSerialized]
        public Rect[] outputRects;

        /// <summary>
        /// True if is currently actived in runtime.
        /// </summary>
        public bool currentlyActived = false;

#if UNITY_EDITOR
        protected Rect detailsPanelContainter;
#endif
        #endregion

        #region Main Methods
        public Node() { }

        public virtual void Initialize(Vector2 position, Graph nodeGraph)
        {
            this.graph = nodeGraph;

            id = nodeGraph.nextNodeId;
            nodeGraph.nextNodeId++;
        }

#if UNITY_EDITOR
        public virtual void InitializeGUI(){ }
        public virtual void UpdateGUI(Event e, Rect panelRect) { }
        public virtual void DrawGUI(Rect panelContainter, Event e, GUISkin skinPanel) { }
        public virtual void DrawDetails(Rect detailsPanelContainter)
        {
            this.detailsPanelContainter = detailsPanelContainter;
        }
#endif

        #endregion
    }
}//namespace BinaryLibrary.GraphCore
