/////////////////////////////////////////////////////////////////////
//Copyright © 2013-2016 Ivan Perez
//Author: Ivan Perez
//Linkedin: http://es.linkedin.com/in/ivanperezduran
//Portfolio: http://binarycv.com
/////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

namespace BinaryLibrary.GraphCore
{
    /// <summary>
    /// Type nodes.
    /// For now, it only uses TASK_STATE.
    /// </summary>
    public enum NodeType
    {
        ANY_STATE,
        TASK_STATE,
        RECURRENT_STATE,
    }
}