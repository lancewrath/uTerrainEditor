using LibNoise;
using LibNoise.Primitive;
using UltimateTerrains;
using Zorlock.uTerrains;
using UnityEngine;

public class NodeBased2DGenerator : I2DGenerator
{
    private readonly Generator generator;
    private readonly double frequency;
    private readonly double scale;

    public NodeBased2DGenerator(double frequency, double scale,Generator gen)
    {
        this.generator = gen;
        Debug.Log("reconnecting nodes");
        this.generator.ReconnectNodes();
        this.generator.GetFinalOperation();
        this.frequency = frequency;
        this.scale = scale;

    }

    public double GetValue(double x, double z)
    {
        return this.generator.GetValue(x * frequency, 0, z * frequency) *scale;
    }

}
