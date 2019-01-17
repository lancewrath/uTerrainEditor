using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Zorlock.uTerrains;

namespace Zorlock.uTerrains.uEditor
{
    [CustomEditor(typeof(Generator))]
    public class GeneratorEditor : Editor
    {
        SerializedObject GetTarget;


        void OnEnable()
        {
            Generator myTarget = (Generator)target;
            GetTarget = new SerializedObject(myTarget);
        }
        public override void OnInspectorGUI()
        {
            GetTarget.Update();
            Generator myTarget = (Generator)target;
            //GetTarget = new SerializedObject(myTarget);
            //EditorGUILayout.PropertyField(GetTarget.FindProperty("noiseOperations"));
            if (GUILayout.Button("Show Editor"))
            {
                
                GeneratorEditorWindow window =
                            EditorWindow.GetWindow<GeneratorEditorWindow>();
                window.generator = (Generator)target;
                window.title = window.generator.name;
                window.minSize = new Vector2(600, 400);

            }
        }
    }
}
