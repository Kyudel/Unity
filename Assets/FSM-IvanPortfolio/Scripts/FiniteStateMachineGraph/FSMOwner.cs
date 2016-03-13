/////////////////////////////////////////////////////////////////////
//Copyright © 2013-2016 Ivan Perez
//Author: Ivan Perez
//Linkedin: http://es.linkedin.com/in/ivanperezduran
//Portfolio: http://binarycv.com
/////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BinaryLibrary.FSM
{
    public class FSMOwner : MonoBehaviour
    {
        public FSMSystem fsm;

        public string serializedFSM = "";

        void Start()
        {
            if (!serializedFSM.Equals(""))
            {
                fsm = ParadoxNotion.Serialization.JSON.Deserialize<FSMSystem>(serializedFSM, null);
            }

            fsm.Initialize(gameObject);
        }

        // Update is called once per frame
        void Update()
        {
            if (fsm != null)
            {
                fsm.Update();
            }
        }
    }
}//namespace BinaryLibrary.FSM