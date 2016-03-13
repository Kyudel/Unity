/////////////////////////////////////////////////////////////////////
//Copyright © 2013-2016 Ivan Perez
//Author: Ivan Perez
//Linkedin: http://es.linkedin.com/in/ivanperezduran
//Portfolio: http://binarycv.com
/////////////////////////////////////////////////////////////////////

using UnityEngine;
using System;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BinaryLibrary.GraphCore
{
    /// <summary>
    /// Link between nodes.
    /// </summary>
    public class Linker
    {
        public int sourceId;
        public int targetId;

        public Linker(Node originNode, Node destinationNode)
        {
            sourceId = originNode.id;
            targetId = destinationNode.id;
        }

#if UNITY_EDITOR
        [NonSerialized]
        public Rect rectConnection = new Rect(0, 0, 50, 25);

        public Color DefaultColor
        {
            get { return new Color(1, 0.4f, 0.4f, 1); }
        }

        public Color ColorInTranstion
        {
            get { return new Color(0.4f, 1f, 0.4f, 1); }
        }

        public Color BaseColor
        {
            get { return new Color(1, 0.4f, 0.4f, 1); }
        }

        private Color currentColor;
        public Color CurrentColor
        {
            get { return currentColor; }
            set { currentColor = value; }
        }


        protected void InitializeColor()
        {
            currentColor = new Color(1, 0.4f, 0.4f, 1);
        }
        #endif
    };
}