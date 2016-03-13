/////////////////////////////////////////////////////////////////////
//Copyright © 2013-2016 Ivan Perez
//Author: Ivan Perez
//Linkedin: http://es.linkedin.com/in/ivanperezduran
//Portfolio: http://binarycv.com
/////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEditor;

namespace BinaryLibrary.EditorUtils
{
    /// <summary>
    /// Library of utils.
    /// </summary>
    public static class UtilsNodeGraph
    {
        public static void DrawLine(Rect panelRect, Vector2 startPosition, Vector2 endPosition, float opacity, Color color)
        {

            Handles.BeginGUI();
            Handles.color = new Color(color.r, color.g, color.b, opacity);

            Handles.DrawLine(new Vector3(startPosition.x, startPosition.y, 0f), new Vector3(endPosition.x, endPosition.y, 0f));


            Handles.color = Color.white;
            Handles.EndGUI();
        }

        public static void DrawGrid(Rect panelRect, float gridSpacing, float gridOpacity, Color gridColor)
        {
            int widthDivs = Mathf.CeilToInt(panelRect.width / gridSpacing);
            int heightDivs = Mathf.CeilToInt(panelRect.height / gridSpacing);

            Handles.BeginGUI();
            Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

            for (int x = 0; x < widthDivs; x++)
            {
                Handles.DrawLine(new Vector3(gridSpacing * x, 0f, 0f), new Vector3(gridSpacing * x, panelRect.height, 0f));
            }

            for (int y = 0; y < heightDivs; y++)
            {
                Handles.DrawLine(new Vector3(0f, gridSpacing * y, 0f), new Vector3(panelRect.width, gridSpacing * y, 0f));
            }

            Handles.color = Color.white;
            Handles.EndGUI();
        }

        public static void DrawNodeBezier(Rect start, Rect end, Color color)
        {
            Vector3 startPos = new Vector3(start.x + start.width, start.y + start.height / 2, 0);
            Vector3 endPos = new Vector3(end.x, end.y + end.height / 2, 0);
            Vector3 startTan = startPos + Vector3.right * 50;
            Vector3 endTan = endPos + Vector3.left * 50;
            Color shadowCol = new Color(color.r, color.g, color.b, 0.06f);

            for (int i = 0; i < 3; i++)
            {
                // Draw a shadow
                Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);
            }

            Handles.DrawBezier(startPos, endPos, startTan, endTan, color, null, 2);
        }

        public static void DrawNodeBezier(Vector3 posA, Vector3 posB, Color color)
        {
            Vector3 startPos = new Vector3(posA.x, posA.y, 0);
            Vector3 endPos = new Vector3(posB.x, posB.y, 0);
            Vector3 startTan = startPos + Vector3.right * 50;
            Vector3 endTan = endPos + Vector3.left * 50;
            Color shadowCol = new Color(color.r, color.g, color.b, 0.06f);

            for (int i = 0; i < 3; i++)
            {
                // Draw a shadow
                Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);
            }

            Handles.DrawBezier(startPos, endPos, startTan, endTan, color, null, 2);
        }
    }
}//namespace BinaryLibrary.EditorUtils
