/////////////////////////////////////////////////////////////////////
//Copyright © 2013-2015 Ivan Perez
//Author: Ivan Perez
//Linkedin: http://es.linkedin.com/in/ivanperezduran
//Portfolio: http://ivanportfolio.tk/
/////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BinaryLibrary.GraphCore;

namespace BinaryLibrary.FSM
{
    
    /// <summary>
    /// Finite State Machine. This class controls all states of an entity.
    /// </summary>
    public class FSMSystem : Graph
    {
        public List<FSMState> statesList;

        public int CurrentStateId;
        public FSMState CurrentState;


        [NonSerialized]
        public GameObject ownerGO;

        public FSMSystem()
        {
            statesList = new List<FSMState>();
#if UNITY_EDITOR
            //debugGraph
#endif
        }

        /// <summary>
        /// Adds a new state.
        /// </summary>
        public void AddState(FSMState newState)
        {

            //if new state is null
            if (newState == null)
            {
                Debug.LogError("FSM: Trying to add a null state");
            }
            else
            {
                //if the state is the first state.
                if (statesList.Count == 0)
                {
                    statesList.Add(newState);

                    CurrentState = newState;
                    CurrentStateId = newState.id;
                    newState.starterState = true;
                }
                else
                {
                    foreach (FSMState state in statesList)
                    {
                        //if new state already added
                        if (state.id == newState.id)
                        {
                            Debug.LogWarning("FSM : Cannot add the state " + newState.id + " because has already been added");
                            return;
                        }//if
                    }//foreach

                    statesList.Add(newState);
                }//else
            }
        }
       
        public void Initialize(GameObject _owner)
        {
            ownerGO = _owner;

            for (int i = 0; i < statesList.Count; i++)
            {
                if (statesList[i].starterState)
                {
                    CurrentState = statesList[i];
                    CurrentStateId = statesList[i].id;
                    break;
                }
            }

            for (int i = 0; i < statesList.Count; i++)
            {
                statesList[i].Initialize(this);
            }//foreach

            CurrentState.Enter();
        }

        public override void Update()
        {
            if (0 < statesList.Count)
            {
                CurrentState.Update();
            }
            CheckConditions();
        }

        private void CheckConditions()
        {
            if (CurrentState.conditionTrue && 0 < CurrentState.transitionsList.Count)
            {
                ChangeState(CurrentState.transitionsList[CurrentState.indexConditionTrue].targetId);
#if UNITY_EDITOR
                if (0 <= CurrentState.indexConditionTrue    )
                {
                    CurrentState.transitionsList[CurrentState.indexConditionTrue].ShowTransitionDebug();
                }
#endif 
            }
        }

        public override void CreateTransition(Node sourceNode, Node targetNode)
        {
            FSMState sourceState = (FSMState)sourceNode;
            FSMState targetState = (FSMState)targetNode;


            FSMTransition transition = new FSMTransition(sourceState, targetState);

            sourceState.AddTransition(transition);

			SetDirtyAndSave();
        }

        /// <summary>
        /// This method changes the state to other if is not already added.
        /// </summary>
        public void ChangeState(int newStateId)
        {
            if (CurrentStateId != newStateId)
            {
                CurrentStateId = newStateId;
                for (int i = 0; i < statesList.Count; i++)
                {
                    if (statesList[i].id == CurrentStateId)
                    {
                        CurrentState.Exit();
                        CurrentState = statesList[i];
                        CurrentState.Enter();
                        break;
                    }//if
                }
            }//if
        }
#if UNITY_EDITOR
        public override void UpdateGUI(Event e, Graph nodeGraph, Rect panelRect)
        {
            base.UpdateGUI(e, nodeGraph, panelRect);

            if (0 <= nodeToDelete)
            {
                DeleteNode(nodeToDelete);
                nodeToDelete = -1;
            }

            for (int i = 0; i < nodeList.Count; i++)
            {
                nodeList[i].UpdateGUI(e, panelRect);
            }
        }

        public override void DrawGUI(Event e, Graph nodeGraph, Rect panelRect, GUISkin skinPanel)
        {
            base.DrawGUI(e, nodeGraph, panelRect, skinPanel);
            for (int i = 0; i < nodeList.Count; i++)
            {
               nodeList[i].DrawGUI(panelRect, e, skinPanel);
            }
        }

        public override void DeleteNode(int id)
        {
            FSMState state = null;
            int index = -1;

            for (int i = 0; i < nodeList.Count; i++)
            {
                if (nodeList[i].id == id)
                {
                    state = (FSMState)nodeList[i];
                    index = i;
                    break;
                }
            }

            if (state != null)
            {
                nodeSelected.isSelected = false;
                nodeSelected = null;
                if (lastNodeSelected != null)
                {
                    lastNodeSelected.isSelected = false;
                    lastNodeSelected = null;
                }

                if (state.starterState)
                {
                    state.starterState = false;

                    for (int i = 0; i < nodeList.Count; i++)
                    {
                        if (nodeList[i].id != id)
                        {
                            ((FSMState)nodeList[i]).starterState = true;
                        }
                    }
                }

                state.Clear();
                nodeList.RemoveAt(index);

                SetDirtyAndSave();
            }
            
        }

        
#endif

    }
}//BinaryLibrary.FSM
