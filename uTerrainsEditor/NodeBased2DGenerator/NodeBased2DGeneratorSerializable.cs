using System;
using UnityEngine;
using UltimateTerrains;
using LibNoise;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Zorlock.uTerrains;

public class NodeBased2DGeneratorSerializable : Serializable2DGenerator {

    [SerializeField] Generator generator;
    [SerializeField] float frequency = 1f / 90f;
    [SerializeField] float scale = 1f;
    public override void OnEditorGUI(UltimateTerrain uTerrain)
    {
#if UNITY_EDITOR
        frequency = EditorGUILayout.FloatField("Frequency:", frequency);
        scale = EditorGUILayout.FloatField("Scale:", scale);
        generator = (Generator)EditorGUILayout.ObjectField(generator,typeof(Generator), false);

#endif
    }

    public override I2DGenerator CreateModule(UltimateTerrain uTerrain)
    {
        return new NodeBased2DGenerator(frequency, scale, generator);
    }

}
