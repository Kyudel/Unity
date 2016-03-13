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
    /// Controls the parameters of the player's animator.
    /// </summary>
    public class AnimatorPlayerController : MonoBehaviour
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
            animator.SetBool("TargetLocked", blackboard.targetLocked);
            animator.SetInteger("AttackType", blackboard.attackType);

            blackboard.curve_RollMovement = animator.GetFloat("Curve_RollMovement");
        }

        public void SetRollTrigger()
        {
            animator.SetTrigger("Roll");
        }
    }
}