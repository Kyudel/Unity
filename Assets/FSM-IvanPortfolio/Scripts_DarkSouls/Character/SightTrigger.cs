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
    /// Trigger when find a specific target.
    /// </summary>
    public class SightTrigger : MonoBehaviour
    {
        public CharacterBlackboard blackboard;

        public string tagTarget;

        void OnTriggerEnter(Collider other)
        {
            if (other.tag == tagTarget)
            {
                blackboard.target = other.transform;
                blackboard.targetLocked = true;
            }
        }
    }
}