using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UltimateTerrains;
using LibNoise;

namespace Zorlock.uTerrains.uEditor
{
    public class PipeNode : NoiseNode
    {
        public override void Init()
        {
            nodetype = NodeType.Pipe;
            noiseOperation = new NoiseOperation();
            noiseOperation.opType = NoiseOperation.OperationType.Pipe;
            noiseOperation.editorRect = windowRect;
            noiseOperation.OperationID = noiseOperation.GetUniqueID();

        }

        public override void Init(NoiseOperation n)
        {
            nodetype = NodeType.Pipe;
            noiseOperation = n;
            noiseOperation.opType = NoiseOperation.OperationType.Pipe;

        }

        public override void DrawWindow()
        {


            noiseOperation.frequency = EditorGUILayout.FloatField("Frequency:", noiseOperation.frequency);
            //noiseOperation.scale = EditorGUILayout.FloatField("Scale:", noiseOperation.scale);
            noiseOperation.octaves = EditorGUILayout.IntField("Octaves:", noiseOperation.octaves);
            noiseOperation.lacunarity = EditorGUILayout.FloatField("Lacunarity:", noiseOperation.lacunarity);
            //noiseOperation.displacement = EditorGUILayout.FloatField("Displacement:", noiseOperation.displacement);
            noiseOperation.gain = EditorGUILayout.FloatField("Gain:", noiseOperation.gain);
            //po.quality = (NoiseQuality)EditorGUILayout.EnumPopup("Quality:", po.quality);
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

                EditorGUI.DrawPreviewTexture(new Rect(10, 150, windowRect.width - 20, windowRect.height - 170), texture);

            }
        }
    }
}
