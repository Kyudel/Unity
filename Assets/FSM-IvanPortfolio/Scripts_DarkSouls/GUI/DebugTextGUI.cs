/////////////////////////////////////////////////////////////////////
//Copyright © 2013-2016 Ivan Perez
//Author: Ivan Perez
//Linkedin: http://es.linkedin.com/in/ivanperezduran
//Portfolio: http://binarycv.com
/////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace BinaryLibrary.Showcase
{
    /// <summary>
    /// Visual helper for the users.
    /// </summary>
    public class DebugTextGUI : MonoBehaviour
    {

        public Text debugText;

        private static string playerState;
        private static string enemyState;

        // Update is called once per frame
        void Update()
        {
            debugText.text = playerState + "\n" + enemyState;
        }

        public static void SetPlayerState(string state)
        {
            playerState = "Player current task: " + state;
        }

        public static void SetEnemyState(string state)
        {
            enemyState = "Enemy current task: " + state;
        }


    }
}//namespace BinaryLibrary.Showcase

