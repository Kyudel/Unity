/////////////////////////////////////////////////////////////////////
//Copyright © 2013-2015 Ivan Perez
//Author: Ivan Perez
//Linkedin: http://es.linkedin.com/in/ivanperezduran
//Portfolio: http://ivanportfolio.tk/
/////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using BinaryLibrary.Utils;
using System.Reflection;
using System.Linq;
using System;
#endif

using BinaryLibrary.GraphCore;

namespace BinaryLibrary.FSM
{
    /// <summary>
    /// State of a Finite State Machine. Only one state can be updated to the time.
    /// </summary>
    public class FSMState : Node
    {
        #region Variables

        public bool starterState = false;

        protected FSMSystem fsmOwner;

        public string name = "";

        public bool conditionTrue = false;
        private bool isFinishedByTasks = false;
        public bool allTasksFinished = false;
        public int indexConditionTrue = -1;

        public List<FSMTransition> transitionsList = new List<FSMTransition>();
        public List<FSMTask> tasksList = new List<FSMTask>();
#if UNITY_EDITOR
        [System.NonSerialized]
        public bool isPlaying = false;

        private int conditionActivatedDebug = -1;
        private float startTimeConditionActivatedDebug = -1;

        private float delayTimeConditionActivatedDebug
        {
            get{ return 1; }
        }

        private bool checkedLayout = false;
#endif
        #endregion Variables

        #region Constructor & Initialization
        public FSMState(int id)
        {
            this.id = id;
            name = "State " + (id + 1);
        }

        /// <summary>
        /// On initialize the state.
        /// </summary>
        public virtual void Initialize(FSMSystem fsmOwner)
        {
            this.fsmOwner = fsmOwner;
            for (int i = 0; i < tasksList.Count; i++)
            {
                tasksList[i].Initialize(fsmOwner);
            }

            transitionsList.Clear();

            for (int i = 0; i < linkList.Count; i++)
            {
                FSMTransition t = (FSMTransition)linkList[i];
                t.Initialize(fsmOwner);
                transitionsList.Add(t);
            }

#if UNITY_EDITOR
            conditionActivatedDebug = -1;
            startTimeConditionActivatedDebug = -1;
#endif
        }

        //Poner en desuso o en el padre
        public void AddTransition(FSMTransition transition)
        {
			transition.AddCondition(new AllTasksFinishedCondition());
            linkList.Add(transition);
        }

        public void DeleteTransition(int transitionToDeleteIndex)
        {
            linkList.RemoveAt(transitionToDeleteIndex);
            graph.SetDirtyAndSave();
        }

        public void AddTask(FSMTask task)
        {
            tasksList.Add(task);
        }

        public void DeleteTask(int indexTaskToDelete)
        {
            tasksList.RemoveAt(indexTaskToDelete);
            graph.SetDirtyAndSave();
        }

        #endregion Constructor & Initialization

        /// <summary>
        /// When a state change FSMSystem calls this method from the current state before update the current state.
        /// </summary>
        public void Enter()
        {
#if UNITY_EDITOR
            isPlaying = true;
#endif
            for (int i = 0; i < tasksList.Count; i++)
            {
                tasksList[i].Enter();
            }
        }

        /// <summary>
        /// Update the current state.
        /// </summary>
        public void Update()
        {
            //Check if some condition is true and the state has to change.
            for (int i = 0; i < transitionsList.Count; i++)
            {
                //If is equal or greater than 0, one condition is true.
                int indexCondition = transitionsList[i].CheckCondition();
                if (0 <= indexCondition)
                {
#if UNITY_EDITOR
                    TimeSpan timeSpan = TimeSpan.FromSeconds(Time.realtimeSinceStartup);
                    string formatTime = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);

                    graph.AddRecordedAction("(" + formatTime + ")\n State " + name + "\n   Transition: " + indexConditionTrue + "\n    Condition:" + indexCondition);
#endif
                    conditionTrue = true;
                    indexConditionTrue = i;
                    break;
                }
            }

            if (!conditionTrue)
            {
                //Check if all the tasks are finished.
                allTasksFinished = true;
                for (int i = 0; i < tasksList.Count; i++)
                {
                    if (!tasksList[i].taskFinished)
                    {
                        allTasksFinished = false;
                        break;
                    }
                }
                if (allTasksFinished)
                {
                    OnFinishByTasks();
                }
            }

            if (!conditionTrue && !allTasksFinished)
            {
                for (int i = 0; i < tasksList.Count; i++)
                {
                    tasksList[i].Update();
                }
            }
        }

        public void Exit()
        {
#if UNITY_EDITOR
            isPlaying = false;
#endif
            conditionTrue = false;
            isFinishedByTasks = false;
            allTasksFinished = false;
            for (int i = 0; i < tasksList.Count; i++)
            {
                tasksList[i].Exit();
            }

            for (int i = 0; i < transitionsList.Count; i++)
            {
                transitionsList[i].Reset();
            }
        }

        private void OnFinishByTasks()
        {    
            if (!isFinishedByTasks)
	        {
                isFinishedByTasks = true;
                //If is less than 0, means that the current state finishes the state.
                //Looking the first transition with on finish as a condition.
                for (int i = 0; i < transitionsList.Count; i++)
                {
                    
                    for (int j = 0; j < transitionsList[i].conditionsList.Count; j++)
                    {
                        if (transitionsList[i].conditionsList[j] is AllTasksFinishedCondition)
                        {
                            indexConditionTrue = i;

#if UNITY_EDITOR
                            TimeSpan timeSpan = TimeSpan.FromSeconds(Time.realtimeSinceStartup);
                            string formatTime = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);

                            graph.AddRecordedAction("(" + formatTime + ")\n State " + id + "\n   Transition by AllTasksFinishedCondition: " + indexConditionTrue + "\n    Condition:" + indexConditionTrue);
#endif
                            //conditionTrue = true;
                            break;
                        }
                    }//for
                }//for
            }
        }

#if UNITY_EDITOR
         public void Clear()
        {
            tasksList.Clear();

            for (int i = 0; i < transitionsList.Count; i++)
            {
                transitionsList[i].Clear();
            }

            //Deletes dependencies
            for (int i = 0; i < graph.nodeList.Count; i++)
            {
                //If it is not this node
                if (graph.nodeList[i].id != id)
                {
                    for (int j = 0; j < graph.nodeList[i].linkList.Count; j++)
                    {
                        if (graph.nodeList[i].linkList[j].targetId == id)
                        {
                            graph.nodeList[i].linkList.RemoveAt(j);
                            j--;
                            if (j < 0)
                            {
                                j = 0;
                            }
                            break;
                        }
                    }
                }
            }

            transitionsList.Clear();
        }

        public void ActiveConditionDebug(int stateId)
        {
            conditionActivatedDebug = stateId;
            checkedLayout = false;
        }

        public override void Initialize(Vector2 position, Graph nodeGraphContainter)
        {
            base.Initialize(position, nodeGraphContainter);
            nodeName = "State";
            nodeType = NodeType.TASK_STATE;

            nodeRect = new Rect(position.x, position.y, 150f, 80f);
        }

        public override void UpdateGUI(Event e, Rect panelRect)
        {
            base.UpdateGUI(e, panelRect);
        }

        public override void InitializeGUI()
        {
            inputRects = new Rect[2];
            outputRects = new Rect[2];

            for (int i = 0; i < inputRects.Length; i++)
            {
                inputRects[i] = new Rect();
                outputRects[i] = new Rect();
            }

            //left
            inputRects[0] = new Rect(0, 10, 15, 4);
            //right
            inputRects[1] = new Rect(nodeRect.width - 15, 10, 15, 4);

            //left
            outputRects[0] = new Rect(0, nodeRect.height - 15 - 10, 15, 15);
            //right
            outputRects[1] = new Rect(nodeRect.width - 15, nodeRect.height - 15 - 10, 15, 15);
        }

        public override void DrawGUI(Rect panelContainter, Event e, GUISkin skinPanel)
        {
            base.DrawGUI(panelContainter, e, skinPanel);

            InitializeGUI();

            if (isSelected)
            {
                GUI.Box(new Rect(nodeRect.x - 3, nodeRect.y - 3, nodeRect.width + 6, nodeRect.height + 6), "");
            }

            if (fsmOwner != null && id == fsmOwner.CurrentStateId)
            {
                GUI.Box(nodeRect, nodeName + " PLAYING");
            }
            else
            {
                GUI.Box(nodeRect, nodeName);
            }

            //If is the start node
            if (starterState)
            {
                GUI.Box(new Rect(nodeRect.x + nodeRect.width / 4, nodeRect.y - 16, nodeRect.width / 2, 20), "");
                GUILayout.BeginArea(new Rect(nodeRect.x, nodeRect.y - 15, nodeRect.width, nodeRect.height));
                GUILayout.Label("Start Node", StyleUtils.centeredTextStyle);
                GUILayout.EndArea();
            }
            GUILayout.BeginArea(nodeRect);
            GUILayout.Space(20);

            GUILayout.Label(name, StyleUtils.centeredTextStyle);

            if (linkList != null)
            {
                for (int i = 0; i < inputRects.Length; i++)
                {
                    GUI.Box(inputRects[i], "");
                }

                for (int i = 0; i < outputRects.Length; i++)
                {
                    GUI.Box(outputRects[i], "");
                }
                if (currentlyActived)
                {
                    GUI.Box(new Rect(nodeRect.width / 2 - 16, nodeRect.height / 2 - 16 + 16, 36, 36), "", skinPanel.GetStyle("playIcon"));
                }
            }
            GUILayout.EndArea();

            //DEBUG. There was a condition activated
            if (Application.isPlaying && 0 <= conditionActivatedDebug)
            {
                if (startTimeConditionActivatedDebug < 0)
                {
                    startTimeConditionActivatedDebug = Time.time;
                }
                else
                {
                    if ( delayTimeConditionActivatedDebug < Time.time - startTimeConditionActivatedDebug)
                    {
                        startTimeConditionActivatedDebug = -1;
                        conditionActivatedDebug = -1;
                    }
                    else
                    {
                        //If there are a change in the GUI it has to be draw first in a EventType.Layout
                        if (!checkedLayout && Event.current.type == EventType.Layout)
                        {
                            DrawConditionDebug();
                            checkedLayout = true;
                        }
                        else if (checkedLayout)
                        {
                            DrawConditionDebug();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Shows a visual feedback in the bottom of the current node.
        /// </summary>
        private void DrawConditionDebug()
        {
            GUILayout.BeginArea(new Rect(nodeRect.x, nodeRect.y + nodeRect.height, nodeRect.width, nodeRect.height));
            GUI.Box(new Rect(5, 0, nodeRect.width - 10, 30), "");
            GUILayout.Space(5);
            GUILayout.Label("Condition " + conditionActivatedDebug + " true", StyleUtils.centeredTextStyle);

            GUILayout.EndArea();

            GUILayout.Label("Condition", StyleUtils.centeredTextStyle);
        }

        public override void DrawDetails(Rect detailsPanelContainter)
        {
            base.DrawDetails(detailsPanelContainter);

            bool deletingState = false;
            GUILayout.Space(5);

            GUILayout.BeginVertical("State", "window", GUILayout.Width(detailsPanelContainter.width - 4));

            name = EditorGUILayout.TextField("State name: ", name);

            if (GUILayout.Button("Delete State"))
            {
                graph.DeleteStatePullAction(id);
                deletingState = true;
            }
            GUILayout.EndVertical();
            GUILayout.BeginVertical();

            if (!deletingState)
            {
                DrawTaskDetails();

                GUILayout.Space(5);

                DrawTaskTransitions();
            }

            GUILayout.EndVertical();
        }
        
        private void DrawTaskDetails()
        {
            GenericMenu menuTasks = new GenericMenu();

            System.Type[] typesTasks = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
            System.Type[] possibleTasks = (from System.Type type in typesTasks where type.IsSubclassOf(typeof(FSMTask)) select type).ToArray();

            GUILayout.Space(5);

            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical("Tasks", GUI.skin.window, GUILayout.Width(detailsPanelContainter.width - 4));

            for (int k = 0; k < possibleTasks.Length; k++)
            {
                menuTasks.AddItem(new GUIContent(possibleTasks[k].Name), false, CreateTask, possibleTasks[k]);
            }

            if (GUILayout.Button("Add task"))
            {
                menuTasks.ShowAsContext();
            }

            for (int i = 0; i < tasksList.Count; i++)
            {
                GUILayout.BeginVertical(GUI.skin.box);

                GUILayout.Label("*" + tasksList[i].GetType().Name);
                tasksList[i].DrawDetails();

                if (GUILayout.Button("Delete task"))
                {
                    DeleteTask(i);
                    break;
                }

                GUILayout.EndVertical();
            }

            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
        }

        private void DrawTaskTransitions()
        {
            GUILayout.BeginVertical("Transitions", "window", GUILayout.Width(detailsPanelContainter.width - 4));

            GUILayout.Space(10);

            if (linkList.Count == 0)
            {
                GUILayout.Label("To make a transition click on a \"link box\"\nand drag to another node.");
            }
            for (int i = 0; i < linkList.Count; i++)
            {
                GUILayout.BeginVertical(GUI.skin.box);

                FSMState sourceState = (FSMState)graph.nodeList[graph.GetNodePositionById(linkList[i].sourceId)];
                FSMState targetState = (FSMState)graph.nodeList[graph.GetNodePositionById(linkList[i].targetId)];
                GUILayout.Label("* Transition with " + sourceState.name + " - " + targetState.name);

                if (GUILayout.Button("Delete transition"))
                {
                    DeleteTransition(i);
                    break;
                }
                else
                {
                    FSMTransition transition = (FSMTransition)linkList[i];
                    transition.DrawDetails(graph);
                }

                GUILayout.Space(4);

                GUILayout.EndVertical();
            }

        }

        public void CreateTask(object _type)
        {
            Type type = _type as Type;
            tasksList.Add( (FSMTask) Activator.CreateInstance(type) );

            graph.SetDirtyAndSave();
        }
#endif// UNITY_EDITOR
    }//FSMState

}//BinaryLibrary.FSM
