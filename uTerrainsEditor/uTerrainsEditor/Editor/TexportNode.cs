using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UltimateTerrains;
using LibNoise;
using System.IO;

namespace Zorlock.uTerrains.uEditor
{
    public class TexportNode : NoiseNode
    {
        public string filename = "MyRender";
        public Vector2 texsize = new Vector2(256,256);
        public override void Init()
        {
            nodetype = NodeType.Texport;
            noiseOperation = new NoiseOperation();
            noiseOperation.opType = NoiseOperation.OperationType.Texport;
            noiseOperation.editorRect = windowRect;
            noiseOperation.OperationID = noiseOperation.GetUniqueID();

        }

        public override void Init(NoiseOperation n)
        {
            nodetype = NodeType.Texport;
            noiseOperation = n;
            noiseOperation.opType = NoiseOperation.OperationType.Texport;

        }

        public override void DrawWindow()
        {

            filename = EditorGUILayout.TextField("Filename:", filename);
            texsize = EditorGUILayout.Vector2Field("Texture Size", texsize);
            previewtex();
        }


        public void previewtex()
        {
            if (texture)
            {
                EditorGUI.PrefixLabel(new Rect(25, 100, 100, 15), 0, new GUIContent("Preview:"));
                if (GUI.Button(new Rect(10, 120, (windowRect.width/2) - 20, 20), "Update"))
                {

                    texture = noiseOperation.Preview(texture);
                }
                if (GUI.Button(new Rect(10+ (windowRect.width / 2), 120, (windowRect.width / 2) - 20, 20), "Export"))
                {
                    Texture2D tex = new Texture2D((int)texsize.x, (int)texsize.y);
                    tex = noiseOperation.Preview(tex);
                    byte[] bytes = tex.EncodeToPNG();
                    File.WriteAllBytes(Application.dataPath + "/../"+ filename + ".png", bytes);
                }
                EditorGUI.DrawPreviewTexture(new Rect(10, 150, windowRect.width - 20, windowRect.height - 160), texture);

            }
        }
    }
}
