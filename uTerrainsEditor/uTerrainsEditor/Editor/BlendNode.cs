using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Zorlock.uTerrains.uEditor
{
    public class BlendNode : NoiseNode
    {

        public ConnectionPoint inPointB;
        

        public override void SetConnectionPoints(Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint)
        {
            inPoint[0] = new ConnectionPoint(this, ConnectionPointType.In, OnClickInPoint);
            inPoint[1] = new ConnectionPoint(this, ConnectionPointType.In, OnClickInPoint);
            //inPointB = new ConnectionPoint(this, ConnectionPointType.In, OnClickInPoint);
            outPoint = new ConnectionPoint(this, ConnectionPointType.Out, OnClickOutPoint);
        }



        public override void Init()
        {
            nodetype = NodeType.Blend;
            noiseOperation = new NoiseOperation();
            noiseOperation.opIn = new NoiseOperation[2];
            noiseOperation.opType = NoiseOperation.OperationType.Blend;
            noiseOperation.editorRect = windowRect;
            noiseOperation.OperationID = noiseOperation.GetUniqueID();
            nodeIn = new Node[2];
            inPoint = new ConnectionPoint[2];
        }

        public override void Init(NoiseOperation n)
        {
            nodetype = NodeType.Blend;
            noiseOperation = n;
            noiseOperation.opIn = new NoiseOperation[2];
            noiseOperation.opType = NoiseOperation.OperationType.Blend;
            nodeIn = new Node[2];
            inPoint = new ConnectionPoint[2];
        }

        public override void DrawWindow()
        {


            noiseOperation.blendop = (NoiseOperation.Operator)EditorGUILayout.EnumPopup("Operation:", noiseOperation.blendop);
            previewtex();
        }


        public void previewtex()
        {
            if (texture)
            {
                EditorGUI.PrefixLabel(new Rect(25, 100, 100, 15), 0, new GUIContent("Preview:"));
                if (GUI.Button(new Rect(10, 120, windowRect.width - 20, 20), "Update"))
                {

                    texture = noiseOperation.Preview(texture);
                }

                EditorGUI.DrawPreviewTexture(new Rect(10, 150, windowRect.width - 20, windowRect.height - 160), texture);

            }
        }

    }
}
