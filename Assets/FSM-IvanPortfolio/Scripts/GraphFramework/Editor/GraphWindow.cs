/////////////////////////////////////////////////////////////////////
//Copyright © 2013-2016 Ivan Perez
//Author: Ivan Perez
//Linkedin: http://es.linkedin.com/in/ivanperezduran
//Portfolio: http://binarycv.com
/////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEditor;
using System.Collections;

using BinaryLibrary.GraphCore;
using BinaryLibrary.EditorUtils;

using BinaryLibrary.FSM;
namespace BinaryLibrary.GraphEditor
{
    public class GraphWindow : EditorWindow
    {
        #region Variables
        public static GraphWindow currentWindow;

        private DetailPanel detailPanel;
        private GraphPanel graphPanel;
        private ActionPanel actionPanel;
        private HelpPanel helpPanel;
        private ConsolePanel consolePanel;

        private Rect windowRect;

        private Rect rectDetailPanel;
        private Rect rectNodePanel;
        private Rect rectActionPanel;
        private Rect rectHelpPanel;
        private Rect rectConsolePanel;

        private float percentColumn01 = 0.3f;
        private float percentColumn02 = 0.5f;
        private float percentColumn03 = 0.2f;

        public static FSMSystem currentGraphNode;
        private GUISkin skin;

        public bool createGraphExample = true;

        public GameObject lastGOSelected;
        #endregion

        #region MainMethods
        public static void ShowWindow()
        {
            currentWindow = EditorWindow.GetWindow<GraphWindow>();
        }

        /// <summary>
        /// Updates the logic of all panels
        /// </summary>
        void Update()
        {
            Event e = Event.current;

            if (graphPanel != null)
            {
                detailPanel.UpdateGUI(rectDetailPanel, e, currentGraphNode);
                graphPanel.UpdateGUI(rectNodePanel, e, currentGraphNode);
                actionPanel.UpdateGUI(rectActionPanel, e, currentGraphNode);
                helpPanel.UpdateGUI(rectHelpPanel, e, currentGraphNode);
                consolePanel.UpdateGUI(rectConsolePanel, e, currentGraphNode);
            }
        }

        /// <summary>
        /// Update the gui side of all panels
        /// </summary>
        void OnGUI()
        {
            //If the is not graphPanels, initializes all panels and panel rects.
            if (graphPanel == null)
            {
                skin = Resources.Load<GUISkin>("Skins/NodeGraphSkin");

                detailPanel = new DetailPanel(this, skin);
                graphPanel = new GraphPanel(this, skin);
                actionPanel = new ActionPanel(this, skin);
                helpPanel = new HelpPanel(this, skin);
                consolePanel = new ConsolePanel(this, skin);

                windowRect = new Rect();
                rectDetailPanel = new Rect();
                rectNodePanel = new Rect();
                rectActionPanel = new Rect();
                rectHelpPanel = new Rect();
                rectConsolePanel = new Rect();
            }

            GetGraphPlayer();

            Event e = Event.current;

            UpdateWindowRect();
            
            detailPanel.DrawGUI(rectDetailPanel, e, currentGraphNode);
            graphPanel.DrawGUI(rectNodePanel, e, currentGraphNode);
            actionPanel.DrawGUI(rectActionPanel, e, currentGraphNode);
            helpPanel.DrawGUI(rectHelpPanel, e, currentGraphNode);
            consolePanel.DrawGUI(rectConsolePanel, e, currentGraphNode);
            
            //Repaint everyframe with changes or not.
            Repaint();
        }

        #endregion

        #region UtilsMethods
        
        /// <summary>
        /// Updates the size of all panels
        /// </summary>
        private void UpdateWindowRect()
        {
            windowRect = new Rect(0, 0, position.width, position.height);
            rectHelpPanel = new Rect(0, 0, windowRect.width * percentColumn01, windowRect.height * 0.2f);
            rectDetailPanel = new Rect(0, windowRect.height * 0.2f, windowRect.width * percentColumn01, windowRect.height * 0.8f);
            rectNodePanel = new Rect(rectDetailPanel.width, 0, windowRect.width * percentColumn02, windowRect.height);
            rectActionPanel = new Rect(rectDetailPanel.width + rectNodePanel.width, 0, windowRect.width * percentColumn03, windowRect.height * 0.2f);
            rectConsolePanel = new Rect(rectDetailPanel.width + rectNodePanel.width, windowRect.height * 0.2f, windowRect.width * percentColumn03, windowRect.height * 0.8f);

        }

        /// <summary>
        /// Check if the selected gameobject has a fsmOwner script component attached.
        /// </summary>
        private void GetGraphPlayer()
        {
            //If there is a gameObject selected and it has a fsmOwner component attached.
            if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<FSMOwner>() != null)
            {
                //If application running
                if (Application.isPlaying)
                {
                    //Check if the selected gameobject changed.
                    if (lastGOSelected == null || lastGOSelected != null && Selection.activeGameObject != lastGOSelected)
                    {
                        currentGraphNode = null;
                        lastGOSelected = Selection.activeGameObject;
                    }

                    FSMOwner fsmOwner = Selection.activeGameObject.GetComponent<FSMOwner>();

                    currentGraphNode = fsmOwner.fsm;
                }
                //If application not running
                else
                {
                    if (lastGOSelected == null || lastGOSelected != null && Selection.activeGameObject != lastGOSelected)
                    {
                        currentGraphNode = null;
                        lastGOSelected = Selection.activeGameObject;
                    }

                    FSMOwner fsmOwner = Selection.activeGameObject.GetComponent<FSMOwner>();

                    if (fsmOwner.serializedFSM.Equals(""))
                    {
                        fsmOwner.serializedFSM = ParadoxNotion.Serialization.JSON.Serialize(typeof(FSMSystem), new FSMSystem(), true, null);
                        currentGraphNode = ParadoxNotion.Serialization.JSON.Deserialize<FSMSystem>(fsmOwner.serializedFSM, null);
                    }
                    else if (currentGraphNode == null)
                    {
                        currentGraphNode = ParadoxNotion.Serialization.JSON.Deserialize<FSMSystem>(fsmOwner.serializedFSM, null);
                    }
                    else if (currentGraphNode != null && currentGraphNode.isDirty)
                    {
                        currentGraphNode.isDirty = false;
                        fsmOwner.serializedFSM = ParadoxNotion.Serialization.JSON.Serialize(typeof(FSMSystem), currentGraphNode, true, null);
                        Debug.Log(fsmOwner.serializedFSM);
                    }
                }//else
            }
            else
            {
                currentGraphNode = null;
            }
        }

        /// <summary>
        /// Reset the nodeGraph
        /// </summary>
        public void ResetNodeGraph()
        {
            if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<FSMOwner>() != null)
            {
                Selection.activeGameObject.GetComponent<FSMOwner>().fsm = new FSMSystem();
                Selection.activeGameObject = null;
            }
            Repaint();
        }

        #endregion
    }
}//namespace BinaryLibrary.EditorGraph
