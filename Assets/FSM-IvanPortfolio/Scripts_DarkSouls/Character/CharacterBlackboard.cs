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
    /// Blackboard with common variables and animator functions to all tasks.
    /// </summary>
    public class CharacterBlackboard : MonoBehaviour
    {
        //////Animator variables///////
        public float forward;
        public float turn;
        public bool onGround;
        public bool targetLocked;
        public int attackType;
        public bool roll;
        public float sideways;
        public bool attack = false;

        //Root movement while rolling.
        public float curve_RollMovement;

        //////Control variables///////
        public Transform cameraTransform;
        public Transform target;
        public float loseTargetRange;
        public float attackRange = 2;
        public SwordController swordController;

    }
}//namespace BinaryLibrary.Showcase
