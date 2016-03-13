/////////////////////////////////////////////////////////////////////
//Copyright © 2013-2016 Ivan Perez
//Author: Ivan Perez
//Linkedin: http://es.linkedin.com/in/ivanperezduran
//Portfolio: http://binarycv.com
/////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

namespace BinaryLibrary.Showcase
{
    /// <summary>
    /// Controls the parameters of the enemy's animator.
    /// </summary>
    public class AnimatorEnemyController : MonoBehaviour
    {
        [System.NonSerialized]
        public Animator animator;

        [System.NonSerialized]
        public CharacterBlackboard blackboard;

        void Awake()
        {
            animator = GetComponent<Animator>();
            blackboard = GetComponent<CharacterBlackboard>();
        }

        void Update()
        {
            animator.SetFloat("Forward", blackboard.forward);
            animator.SetFloat("Turn", blackboard.turn);
            animator.SetFloat("Sideways", blackboard.sideways);
            animator.SetBool("Attack", blackboard.attack);
            animator.SetInteger("AttackType", blackboard.attackType);
        }

    }
}