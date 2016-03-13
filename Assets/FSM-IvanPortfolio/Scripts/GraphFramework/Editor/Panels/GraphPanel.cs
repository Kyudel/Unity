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
using BinaryLibrary.EditorUtils;

namespace BinaryLibrary.GraphEditor
{
    /// <summary>
    /// Shows the nodes and connections in a ScrollView.
    /// </summary>
    public class GraphPanel : BasePanel
    {
        #region Variables
        /// <summary>
        /// Position inside the scrollView. Centered by default.
        /// </summary>
        private Vector2 positionScrollView = new Vector2(4950, 4950);

        /// <summary>
        /// max size of the scrollView.
        /// </summary>
        private Vector2 maxSize = new Vector2(10000, 10000);

        private PopupMenu contextMenu;

        private bool scrollingPanel = false;
        private bool linkerDragging = false;
        private Node originNode = null;
        private Node draggingNode;

        private Vector3 originBezier = Vector3.zero;
        private Vector2 positionOffset;

        private bool isRightOutside = true;

        /// <summary>
        /// States of the panel.
        /// </summary>
        public enum State
        {
            NONE,
            SWIPE_PANEL,
            DRAGGING_NODE,
            DRAGGING_LINK,
        }
        public State currentState = State.NONE;

        #endregion

        #region Methods
        public GraphPanel(GraphWindow graphWindow, GUISkin skin)
            : base(graphWindow, skin)
        {
            title = "Node Panel";
            contextMenu = new PopupMenu();
        }

        public override void UpdateGUI(Rect panelRect, Event e, Graph nodeGraph)
        {
            base.UpdateGUI(panelRect, e, nodeGraph);
            if (nodeGraph != null)
            {
                nodeGraph.UpdateGUI(e, nodeGraph, panelRect);
            }
        }

        public override void DrawGUI(Rect panelRect, Event e, Graph nodeGraph)
        {
            base.DrawGUI(panelRect, e, nodeGraph);

            if (nodeGraph != null)
            {
                DrawGUI(panelRect, e, nodeGraph, skinPanel);
                ProcessEvent(e, nodeGraph);
            }
            else
            {
                Rect rect = new Rect(panelRect.x, panelRect.y + panelRect.height / 2 - 30, panelRect.width, panelRect.height / 8);
                GUILayout.BeginArea(rect);

                GUILayout.Label("Select a FSMOwner", skinPanel.GetStyle("playText"));
                GUILayout.EndArea();
            }
            
        }
        
        #region GUI
        protected void DrawGUI(Rect panelRect, Event e, Graph nodeGraph, GUISkin skinPanel)
        {
            positionOffset = positionScrollView - new Vector2(panelRect.x, panelRect.y);

            GUI.Box(panelRect, title, stylePanel);

            GUILayout.BeginArea(panelRect);
            //Begin scrollView.
            positionScrollView = EditorGUILayout.BeginScrollView(positionScrollView, GUILayout.Width(panelRect.width), GUILayout.Height(panelRect.height - 8));
            GUILayout.BeginHorizontal(GUILayout.Width(maxSize.x), GUILayout.Height(maxSize.y));

            //It is necessary draw unless an element so the scrollView works properly.
            GUILayout.Space(0);

            //Draw the grid.
            DrawGrid();

            //Note. The proper order of these this function calls is important!
            InitializeNodes(nodeGraph);
            DrawLinkerConnections(e, nodeGraph);
            nodeGraph.DrawGUI(e, nodeGraph, panelRect, skinPanel);

            if (linkerDragging)
            {
                DrawDraggingLinkerConnection(e);
            }

            GUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();
            GUILayout.EndArea();
        }

        public void InitializeNodes(Graph nodeGraph)
        {
            for (int i = 0; i < nodeGraph.nodeList.Count; i++)
            {
                nodeGraph.nodeList[i].InitializeGUI();
            }
        }

        protected void DrawLinkerConnections(Event e, Graph nodeGraph)
        {
            for (int i = 0; i < nodeGraph.nodeList.Count; i++)
            {
                for (int j = 0; j < nodeGraph.nodeList[i].linkList.Count; j++)
                {
                    int sourceNodeID = nodeGraph.nodeList[i].linkList[j].sourceId;
                    int targetNodeID = nodeGraph.nodeList[i].linkList[j].targetId;

                    Node sourceNode = nodeGraph.nodeList[nodeGraph.GetNodePositionById(sourceNodeID)];
                    Node targetNode = nodeGraph.nodeList[nodeGraph.GetNodePositionById(targetNodeID)];

                    Vector3 posOriginNode = new Vector3(sourceNode.nodeRect.x + sourceNode.nodeRect.width / 2,
                                                        sourceNode.nodeRect.y + sourceNode.nodeRect.height / 2,
                                                        0);
                    Vector3 posDestinationNode = new Vector3(targetNode.nodeRect.x + targetNode.nodeRect.width / 2,
                                                        targetNode.nodeRect.y + targetNode.nodeRect.height / 2,
                                                        0);

                    Color color;
                    //If is in runtime, it can have a color effects if there is a transition between states.
                    if (Application.isPlaying)
                    {
                        color = nodeGraph.nodeList[i].linkList[j].CurrentColor;
                    }
                    else
                    {
                        color = nodeGraph.nodeList[i].linkList[j].DefaultColor;
                    }

                    if (posOriginNode.x < posDestinationNode.x)
                    {
                        Vector3 positionA = new Vector3(sourceNode.nodeRect.x + sourceNode.nodeRect.width,
                                                        sourceNode.nodeRect.y + sourceNode.outputRects[1].y + sourceNode.outputRects[1].height / 2,
                                                        0);

                        Vector3 positionB = new Vector3(targetNode.nodeRect.x + targetNode.inputRects[0].x,
                                                        targetNode.nodeRect.y + targetNode.inputRects[0].y + targetNode.inputRects[0].height / 2,
                                                        0);
                        UtilsNodeGraph.DrawNodeBezier(positionA, positionB, color);

                        GUI.Box(new Rect(positionB.x + 4 - 18, positionB.y - 9, 18, 18), "", skinPanel.GetStyle("arrowRight"));
                    }
                    else
                    {
                        Vector3 positionA = new Vector3(targetNode.nodeRect.x + targetNode.nodeRect.width,
                                                        targetNode.nodeRect.y + targetNode.inputRects[1].y + targetNode.inputRects[1].height / 2,
                                                        0);

                        Vector3 positionB = new Vector3(sourceNode.nodeRect.x + sourceNode.outputRects[0].x,
                                                        sourceNode.nodeRect.y + sourceNode.outputRects[0].y + sourceNode.outputRects[0].height / 2,
                                                        0);

                        UtilsNodeGraph.DrawNodeBezier(positionA, positionB, color);

                        GUI.Box(new Rect(positionA.x - 4, positionA.y - 9, 18, 18), "", skinPanel.GetStyle("arrowLeft"));
                    }
                }
            }
        }

        protected void DrawDraggingLinkerConnection(Event e)
        {
            //If the linker box is the right one
            if (isRightOutside)
            {
                UtilsNodeGraph.DrawNodeBezier(new Vector3(positionOffset.x, positionOffset.y) + originBezier, new Vector3(e.mousePosition.x, e.mousePosition.y), new Color(1, 0.4f, 0.4f));
            }
            //Left one.
            else
            {
                UtilsNodeGraph.DrawNodeBezier(new Vector3(e.mousePosition.x, e.mousePosition.y), new Vector3(positionOffset.x, positionOffset.y) + originBezier, new Color(1, 0.4f, 0.4f));
            }
        }

        protected void DrawGrid()
        {
            Rect areaPanel = new Rect(positionScrollView.x, positionScrollView.y, maxSize.x, maxSize.y);

            //Grid
            UtilsNodeGraph.DrawGrid(areaPanel, 80f, 1, new Color(0.1f, 0.1f, 0.1f));
            UtilsNodeGraph.DrawGrid(areaPanel, 10f, 0.05f, Color.white);

            //Center lines
            UtilsNodeGraph.DrawLine(areaPanel, new Vector2(0, maxSize.y / 2), new Vector2(maxSize.x, maxSize.y / 2), 3f, new Color(0.0f, 0.5f, 0.5f));
            UtilsNodeGraph.DrawLine(areaPanel, new Vector2(maxSize.x / 2, 0), new Vector2(maxSize.x / 2, maxSize.y), 3f, new Color(0.0f, 0.5f, 0.5f));

            //Margin lines
            UtilsNodeGraph.DrawLine(areaPanel, new Vector2(0, 10), new Vector2(maxSize.x, 10), 1f, Color.red);
            UtilsNodeGraph.DrawLine(areaPanel, new Vector2(0, maxSize.y - 10), new Vector2(maxSize.x, maxSize.y - 10), 1f, Color.red);

            UtilsNodeGraph.DrawLine(areaPanel, new Vector2(10, 0), new Vector2(10, maxSize.y), 1f, Color.red);
            UtilsNodeGraph.DrawLine(areaPanel, new Vector2(maxSize.x - 10, 0), new Vector2(maxSize.x - 10, maxSize.y), 1f, Color.red);
        }

        #endregion

        #region ProcessEvents

        protected void ProcessEvent(Event e, Graph nodeGraph)
        {
            //Lose focus of the graph window for other from unity.
            if (EditorWindow.focusedWindow != graphWindow)
            {
                //Stop all actions
                draggingNode = null;
                scrollingPanel = false;
                linkerDragging = false;

                if (originNode != null)
                {
                    originNode.isSelected = false;
                    originNode = null;
                }
            }

            if (e.type == EventType.MouseDrag || e.type == EventType.MouseDown)
            {
                //Dragging a node
                if (draggingNode != null && e.type == EventType.MouseDrag && e.button == 0)
                {
                    draggingNode.nodeRect.x += e.delta.x;
                    draggingNode.nodeRect.y += e.delta.y;
                }
                else if (!linkerDragging)
                {
                    //Start to drag a node
                    if (e.type == EventType.MouseDrag && e.button == 0)
                    {
                        ProcessDragAction(e, nodeGraph);
                    }
                    //Drag panel
                    else if (e.type == EventType.MouseDrag && e.button == 2)
                    {
                        if (panelRect.Contains(e.mousePosition))
                        {
                            positionScrollView -= e.delta;
                            scrollingPanel = true;
                        }
                    }
                }//else if (!linkerDragging)
            }
            else if (e.type == EventType.MouseUp)
            {
                //Show context menu
                if (e.button == 1)
                {
                    //If it was dragging dont show contextMenu IPD
                    if (scrollingPanel)
                    {
                        scrollingPanel = false;
                    }
                    else
                    {
                        contextMenu.ShowMenuNodePanel(e, positionScrollView - new Vector2(panelRect.x, panelRect.y));
                    }
                }
                else if (e.button == 0)
                {
                    //It iss dragging a linker
                    if (originNode != null)
                    {
                        FinishNodeAction(e, nodeGraph);
                    }
                    else if (!scrollingPanel)
                    {
                        //Select a node
                        ClickHoverNodeAction(e, nodeGraph);
                    }

                }

                //Stop all actions
                draggingNode = null;
                scrollingPanel = false;
                linkerDragging = false;

                if (originNode != null)
                {
                    originNode.isSelected = false;
                    originNode = null;
                }

                nodeGraph.SetDirtyAndSave();
            }

        }

        protected void ClickHoverNodeAction(Event e, Graph nodeGraph)
        {
            //The old node selected
            if (nodeGraph.nodeSelected != null)
            {
                nodeGraph.nodeSelected.isSelected = false;
                nodeGraph.lastNodeSelected = nodeGraph.nodeSelected;
            }

            Vector2 fixedMousePosition = positionOffset + e.mousePosition;

            for (int i = 0; i < nodeGraph.nodeList.Count; i++)
            {
                if (nodeGraph.nodeList[i].nodeRect.Contains(fixedMousePosition))
                {
                    nodeGraph.nodeSelected = nodeGraph.nodeList[i];
                    
                    
                    break;
                }
            }
            if (nodeGraph.lastNodeSelected != null)
            {
                nodeGraph.lastNodeSelected.isSelected = false;
                nodeGraph.nodeSelected.isSelected = true;
            }
        }

        protected void ProcessDragAction(Event e, Graph nodeGraph)
        {
            Vector2 fixedMousePosition = positionOffset + e.mousePosition;

            for (int i = 0; i < nodeGraph.nodeList.Count; i++)
            {
                if (nodeGraph.nodeList[i].nodeRect.Contains(fixedMousePosition))
                {
                    originNode = HoverConnectionNodes(e, nodeGraph, i, fixedMousePosition);
                    if (originNode != null)
                    {
                        originNode.isSelected = true;
                    }
                   
                    //Is hover a node.
                    if (!linkerDragging)
                    {
                        draggingNode = nodeGraph.nodeList[i];
                    }
                    break;
                }
            }
        }

        protected Node HoverConnectionNodes(Event e, Graph nodeGraph, int indexNode, Vector2 fixedMousePosition)
        {
            Node originNode = null;
            for (int j = 0; j < nodeGraph.nodeList[indexNode].outputRects.Length; j++)
            {
                Vector2 nodeRecPosition = new Vector2(nodeGraph.nodeList[indexNode].nodeRect.x, nodeGraph.nodeList[indexNode].nodeRect.y);
                if (nodeGraph.nodeList[indexNode].outputRects[j].Contains(fixedMousePosition - nodeRecPosition))
                {
                    originNode = nodeGraph.nodeList[indexNode];
                    originNode.isSelected = true;
                    linkerDragging = true;
                    
                    //Left input.
                    if (j == 0)
                    {
                        originBezier = new Vector3(panelRect.x - positionScrollView.x + nodeRecPosition.x,
                                                panelRect.y - positionScrollView.y + nodeRecPosition.y + nodeGraph.nodeList[indexNode].outputRects[j].y + nodeGraph.nodeList[indexNode].outputRects[j].height / 2);
                        isRightOutside = false;
                    }
                    //Right input.
                    else
	                {
                        originBezier = new Vector3(panelRect.x - positionScrollView.x + nodeRecPosition.x + nodeGraph.nodeList[indexNode].nodeRect.width,
                                                panelRect.y - positionScrollView.y + nodeRecPosition.y + nodeGraph.nodeList[indexNode].outputRects[j].y + nodeGraph.nodeList[indexNode].outputRects[j].height / 2);
                        isRightOutside = true;
	                }

                    break;
                }
             
            }
            return originNode;
        }

        protected void FinishNodeAction(Event e, Graph nodeGraph)
        {
           Vector2 fixedMousePosition = positionOffset + e.mousePosition;

            for (int i = 0; i < nodeGraph.nodeList.Count; i++)
            {
                //Check if the cursor is over a node.
                if (nodeGraph.nodeList[i] != originNode && nodeGraph.nodeList[i].nodeRect.Contains(fixedMousePosition))
                {
                    bool triggered = false;
                    //Check if is a valid node.
                    for (int j = 0; j < nodeGraph.nodeList.Count; j++)
                    {
                        for (int k = 0; k < nodeGraph.nodeList[j].linkList.Count; k++)
                        {
                            int sourceNodeID = nodeGraph.nodeList[j].linkList[k].sourceId;
                            int targetNodeID = nodeGraph.nodeList[j].linkList[k].targetId;

                            if (originNode == nodeGraph.nodeList[nodeGraph.GetNodePositionById(sourceNodeID)] && nodeGraph.nodeList[i] == nodeGraph.nodeList[nodeGraph.GetNodePositionById(targetNodeID)])
                            {
                                triggered = true;
                                break;
                            }
                        }
                    }
                    //If is a valid connection, links both nodes.
                    if (!triggered)
                    {
                        nodeGraph.CreateTransition(originNode, nodeGraph.nodeList[i]);
                        break;
                    }
                }
            }
        }


        #endregion
        
        #endregion

    }
}//namespace BinaryLibrary.EditorGraph
