﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UltimateTerrains;
using LibNoise;

namespace Zorlock.uTerrains.uEditor
{
    public class TurbulenceNode : NoiseNode
    {

        public override void Init()
        {
            nodetype = NodeType.Turbulence;
            noiseOperation = new NoiseOperation();
            noiseOperation.opType = NoiseOperation.OperationType.Turbulence;
            noiseOperation.editorRect = windowRect;
            noiseOperation.OperationID = noiseOperation.GetUniqueID();

        }

        public override void Init(NoiseOperation n)
        {
            nodetype = NodeType.Turbulence;
            noiseOperation = n;
            noiseOperation.opType = NoiseOperation.OperationType.Turbulence;

        }

        public override void DrawWindow()
        {

            noiseOperation.power = EditorGUILayout.FloatField("Power:", noiseOperation.power);
            noiseOperation.scale = EditorGUILayout.FloatField("Scale:", noiseOperation.scale);
            noiseOperation.seed = EditorGUILayout.IntField("Seed:", noiseOperation.seed);
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
