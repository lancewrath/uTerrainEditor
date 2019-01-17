using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UltimateTerrains;
using LibNoise;

namespace Zorlock.uTerrains.uEditor
{
    public class CurveNode : NoiseNode
    {

        public override void Init()
        {
            nodetype = NodeType.Curve;
            noiseOperation = new NoiseOperation();
            noiseOperation.opType = NoiseOperation.OperationType.Curve;
            noiseOperation.editorRect = windowRect;
            noiseOperation.OperationID = noiseOperation.GetUniqueID();

        }

        public override void Init(NoiseOperation n)
        {
            nodetype = NodeType.Curve;
            noiseOperation = n;
            noiseOperation.opType = NoiseOperation.OperationType.Curve;

        }

        public override void DrawWindow()
        {
            /*
            //po.frequency = EditorGUILayout.FloatField("Frequency:", po.frequency);
            noiseOperation.scale = EditorGUILayout.FloatField("Scale:", noiseOperation.scale);
            noiseOperation.steps = EditorGUILayout.IntField("Steps:", noiseOperation.steps);
            //po.quality = (NoiseQuality)EditorGUILayout.EnumPopup("Quality:", po.quality);
            */
            noiseOperation.noisecurve = EditorGUILayout.CurveField("Curve", noiseOperation.noisecurve);
            previewtex();
        }


        public void previewtex()
        {
            if (texture)
            {
                EditorGUI.PrefixLabel(new Rect(25, 40, 100, 15), 0, new GUIContent("Preview:"));
                if (GUI.Button(new Rect(10, 60, windowRect.width - 20, 20), "Update"))
                {

                    texture = noiseOperation.Preview(texture);
                }

                EditorGUI.DrawPreviewTexture(new Rect(10, 150, windowRect.width - 20, windowRect.height - 160), texture);

            }
        }
    }
}
