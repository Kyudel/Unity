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

using BinaryLibrary.GraphCore;

namespace BinaryLibrary.GraphEditor
{
    /// <summary>
    /// Base class for all panels.
    /// </summary>
    public abstract class BasePanel
    {
        #region Variables
        protected Rect panelRect;

        protected string title;

        protected GraphWindow graphWindow;

        protected GUIStyle stylePanel;
        protected GUISkin skinPanel;
        #endregion

        #region MainMethods
        public BasePanel(GraphWindow graphWindow, GUISkin skin)
        {
            this.graphWindow = graphWindow;
            skinPanel = skin;

            SetupStyle();
        }

        public virtual void UpdateGUI(Rect panelRect, Event e, Graph nodeGraph) { }

        public virtual void DrawGUI(Rect panelRect, Event e, Graph nodeGraph)
        {
            this.panelRect = panelRect;
        }
        #endregion

        #region UtilsMethods
        private void SetupStyle()
        {
            stylePanel = new GUIStyle();

            //skinPanel = new GUIStyle("label");
            stylePanel.normal.textColor = new Color(200, 200, 200);
            stylePanel.alignment = TextAnchor.MiddleCenter;
            stylePanel.fontStyle = FontStyle.Bold;
            stylePanel.fixedHeight = 30;
            stylePanel.border.top = 4;
        }
        #endregion
    }
}