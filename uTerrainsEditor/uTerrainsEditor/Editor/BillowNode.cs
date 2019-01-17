using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UltimateTerrains;
using LibNoise;

namespace Zorlock.uTerrains.uEditor
{
    public class BillowNode : NoiseNode
    {

        public override void Init()
        {
            nodetype = NodeType.Billow;
            noiseOperation = new NoiseOperation();
            noiseOperation.opType = NoiseOperation.OperationType.Billow;
            noiseOperation.editorRect = windowRect;
            noiseOperation.OperationID = noiseOperation.GetUniqueID();

        }

        public override void Init(NoiseOperation n)
        {
            nodetype = NodeType.Billow;
            noiseOperation = n;
            noiseOperation.opType = NoiseOperation.OperationType.Billow;

        }

        public override void DrawWindow()
        {


            noiseOperation.frequency = EditorGUILayout.FloatField("Frequency:", noiseOperation.frequency);
            noiseOperation.scale = EditorGUILayout.FloatField("Scale:", noiseOperation.scale);
            noiseOperation.octaves = EditorGUILayout.IntField("Octaves:", noiseOperation.octaves);
            noiseOperation.lacunarity = EditorGUILayout.FloatField("Lacunarity:", noiseOperation.lacunarity);
            noiseOperation.bias = EditorGUILayout.FloatField("Bias:", noiseOperation.bias);
            //noiseOperation.gain = EditorGUILayout.FloatField("Gain:", noiseOperation.gain);
            //po.quality = (NoiseQuality)EditorGUILayout.EnumPopup("Quality:", po.quality);
            previewtex();
        }


        public void previewtex()
        {
            if (texture)
            {
                EditorGUI.PrefixLabel(new Rect(25, 140, 100, 15), 0, new GUIContent("Preview:"));
                if (GUI.Button(new Rect(10, 160, windowRect.width - 20, 20), "Update"))
                {

                    texture = noiseOperation.Preview(texture);
                }

                EditorGUI.DrawPreviewTexture(new Rect(10, 190, windowRect.width - 20, windowRect.height - 200), texture);

            }
        }
    }
}
