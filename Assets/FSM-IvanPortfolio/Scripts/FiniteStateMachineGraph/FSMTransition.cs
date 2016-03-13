/////////////////////////////////////////////////////////////////////
//Copyright © 2013-2016 Ivan Perez
//Author: Ivan Perez
//Linkedin: http://es.linkedin.com/in/ivanperezduran
//Portfolio: http://binarycv.com
/////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using System.Reflection;
using System.Linq;
using System;
#endif

using BinaryLibrary.GraphCore;

namespace BinaryLibrary.FSM
{
    /// <summary>
    /// Transition between states.
    /// </summary>
    public class FSMTransition : Linker
    {
        #region Attributes
        private bool condition = false;
        public bool Condition { get {return condition;} }
        protected FSMSystem fsmOwner;
        
        [System.NonSerialized]
        /** Owner graph. It uses to serialize the fsm after add a condition. */
        private Graph graph;
        /// <summary>
        /// Conditions which determine if a state has to make a transition to another.
        /// </summary>
        public List<FSMCondition> conditionsList = new List<FSMCondition>();
        #endregion Attributes

        #region Construct

        public FSMTransition(FSMState _sourceState, FSMState _targetState)
            : base(_sourceState, _targetState)
        {
            
        }

        public void Initialize(FSMSystem _fsmOwner)
        {
            this.fsmOwner = _fsmOwner;
            for (int i = 0; i < conditionsList.Count; i++)
            {
                conditionsList[i].Initialize(_fsmOwner, this);
            }

#if UNITY_EDITOR
            InitializeColor();
#endif
        }
        #endregion Construct


        #region MainMethods
        public void AddCondition(FSMCondition condition)
        {
            conditionsList.Add(condition);
        }

        public void DeleteCondition(int indexConditionToDelete)
        {
            conditionsList.RemoveAt(indexConditionToDelete);
            graph.SetDirtyAndSave();
        }

        public int CheckCondition()
        {
            int conditionTriggered = -1;
            for (int i = 0; i < conditionsList.Count; i++)
            {
                if (conditionsList[i].Check())
                {
#if UNITY_EDITOR
                    fsmOwner.CurrentState.ActiveConditionDebug(i);
#endif
                    conditionTriggered = i;
                    break;
                }
            }
#if UNITY_EDITOR
           UpdateColorLink();
#endif
           return conditionTriggered;
        }

        public virtual void OnCheck()
        {
        }

        public void Reset()
        {
#if UNITY_EDITOR
            ResetColor();
#endif
            condition = false;  
            for (int i = 0; i < conditionsList.Count; i++)
            {
                conditionsList[i].Reset();
            }
            OnReset();
        }

        protected virtual void OnReset()
        { 
        
        }

        #endregion

#if UNITY_EDITOR
        public void Clear()
        {
            conditionsList.Clear();
        }

        /// <summary>
        /// Shows visual feedback when a transition is made.
        /// </summary>
        public void ShowTransitionDebug()
        {
            CurrentColor = ColorInTranstion;
        }

        private void UpdateColorLink()
        {
            if (Application.isPlaying)
            {
                CurrentColor = Color.Lerp(CurrentColor, BaseColor, Time.deltaTime);
            }
            else
            {
                CurrentColor = BaseColor;
            }
        }

        private void ResetColor()
        {
            CurrentColor = BaseColor;
        }

        public virtual void DrawDetails(Graph _graph)
        {
            graph = _graph;
            GenericMenu menu = new GenericMenu();

            //Finds all conditions.
            System.Type[] types = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
            System.Type[] possible = (from System.Type type in types where type.IsSubclassOf(typeof(FSMCondition)) select type).ToArray();

            for (int k = 0; k < possible.Length; k++)
            {
                menu.AddItem(new GUIContent(possible[k].Name), false, CreateCondition, possible[k]);
            }

			GUILayout.Label("Conditions:");
			if (GUILayout.Button("Add condition"))
			{
				menu.ShowAsContext();
			}

			GUILayout.BeginHorizontal();
			GUILayout.BeginVertical(GUILayout.Width(25));
			GUILayout.Label("");
			GUILayout.EndVertical();
			GUILayout.BeginVertical();
			
            //Shows details of all the conditions.
            for (int j = 0; j < conditionsList.Count; j++)
            {
                GUILayout.BeginVertical(GUI.skin.box);

                conditionsList[j].DrawDetails(j);

                if (1 < conditionsList.Count && GUILayout.Button("Delete condition"))
				{
                    DeleteCondition(j);
					break;
				}

                GUILayout.EndVertical();
            }
            GUILayout.EndVertical();
			

			GUILayout.EndHorizontal();
                    
        }

        public void CreateCondition(object _type)
        {
            Type type = _type as Type;

            AddCondition((FSMCondition)Activator.CreateInstance(type));
            graph.SetDirtyAndSave();
        }
#endif

    }
}//BinaryLibrary.FSM
