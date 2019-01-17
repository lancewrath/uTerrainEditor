using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LibNoise;
using LibNoise.Primitive;
using LibNoise.Utils;
using UltimateTerrains;


namespace Zorlock.uTerrains
{
    [Serializable]
    public class NoiseOperation 
    {

        public NoiseOperation[] opIn = new NoiseOperation[1];
        public NoiseOperation opOut;
        [SerializeField] public List<string> opIdIn = new List<string>();
        
        [SerializeField] public string opIdOut = "";
        [SerializeField] public Rect editorRect = new Rect(0, 0, 100, 100);
        [SerializeField] public OperationType opType = OperationType.Noise;
        [SerializeField] public string OperationID = "";
        [SerializeField] public Gradient gradient;
        #region noise fields
        [SerializeField] public float frequency = 1f;
        [SerializeField] public float scale = 1f;
        
        [SerializeField] public int seed;
        [SerializeField] public NoiseQuality quality = NoiseQuality.Standard;
        [SerializeField] public Operator blendop = Operator.add;
        [SerializeField] public int steps = 10;
        public float lightBrightness = 1;
        public float lightContrast = 10;
        [SerializeField] public int octaves = 2;
        [SerializeField] public float lacunarity = 2;
        [SerializeField] public float displacement = 10;
        [SerializeField] public bool bUseDistance = false;
        [SerializeField] public float gain = 1f;
        [SerializeField] public float power = 1f;
        [SerializeField] public float bias = -0.75f;
        [SerializeField] public AnimationCurve noisecurve = AnimationCurve.Linear(0, 0, 1, 1);
        #endregion

        public Texture2D previewtex;
        public LibNoise.IModule3D noisemodule;


        public enum Operator
        {
            add,subtract,multiply,max,min
        }

        public enum OperationType
        {
            Start,Noise,Perlin,Simplex,Terrace,Voronoi,Billow,Curve,Final,Turbulence,Blend,HMF,HybridMF,MultiF,Texport,Pipe,Scale,ScaleBias
        }

        


        public virtual void CreateNoise()
        {
            switch(opType)
            {

                case OperationType.ScaleBias:
                    CreateScaleBiasNoise();
                    break;

                case OperationType.Scale:
                    CreateScaleNoise();
                    break;

                case OperationType.Pipe:
                    CreatePipeNoise();
                    break;

                case OperationType.MultiF:
                    CreateMultiFNoise();
                    break;

                case OperationType.HybridMF:
                    CreateHybridMFNoise();
                    break;

                case OperationType.HMF:
                    CreateHMFNoise();
                    break;

                case OperationType.Perlin:
                    CreatePerlinNoise();
                    break;
                case OperationType.Simplex:
                    CreateSimplexNoise();
                    break;
                case OperationType.Terrace:
                    CreateTerraceNoise();
                    break;
                case OperationType.Voronoi:
                    CreateVoronoiNoise();
                    break;
                case OperationType.Billow:
                    CreateBillowNoise();
                    break;
                case OperationType.Curve:
                    CreateCurveNoise();
                    break;

                case OperationType.Turbulence:
                    CreateTurbulence();
                    break;

                case OperationType.Blend:
                    CreateBlendNoise();
                    break;

                case OperationType.Texport:
                    CreateFinalNoise();
                    break;

                case OperationType.Final:
                    CreateFinalNoise();
                    break;

            }
            
        }

        public void CreateScaleBiasNoise()
        {
            if (opIn[0] != null)
            {
                opIn[0].CreateNoise();
                LibNoise.Modifier.ScaleBias scaleb = new LibNoise.Modifier.ScaleBias(opIn[0].noisemodule);
                scaleb.Scale = scale;
                scaleb.Bias = bias;
                scaleb.SourceModule = opIn[0].noisemodule;


                noisemodule = scaleb;
            }
            else
            {
                //opIn[0].CreateNoise();
                LibNoise.Modifier.ScaleBias scaleb = new LibNoise.Modifier.ScaleBias(new LibNoise.Primitive.ImprovedPerlin(seed, quality));
                scaleb.Scale = scale;
                scaleb.Bias = bias;
                scaleb.SourceModule = opIn[0].noisemodule;


                noisemodule = scaleb;
            }



        }

        public void CreateScaleNoise()
        {
            if (opIn[0] != null)
            {
                opIn[0].CreateNoise();
                LibNoise.Transformer.ScalePoint scalep = new LibNoise.Transformer.ScalePoint(opIn[0].noisemodule);
                scalep.XScale = scale;
                scalep.YScale = scale;
                scalep.ZScale = scale;
                scalep.SourceModule = opIn[0].noisemodule;


                noisemodule = scalep;
            }
            else
            {
                //opIn[0].CreateNoise();
                LibNoise.Transformer.ScalePoint scalep = new LibNoise.Transformer.ScalePoint(new LibNoise.Primitive.ImprovedPerlin(seed, quality));
                scalep.XScale = scale;
                scalep.YScale = scale;
                scalep.ZScale = scale;
                scalep.SourceModule = new LibNoise.Primitive.ImprovedPerlin(seed, quality);


                noisemodule = scalep;
            }



        }

        public void CreatePipeNoise()
        {
            if (opIn[0] != null)
            {
                opIn[0].CreateNoise();
                LibNoise.Filter.Pipe pipe = new LibNoise.Filter.Pipe();
                pipe.Frequency = frequency;
                pipe.Lacunarity = lacunarity;
                pipe.OctaveCount = octaves;
                pipe.Gain = gain;

                pipe.Primitive3D = opIn[0].noisemodule;

                noisemodule = pipe;
            }
            else
            {
                //opIn[0].CreateNoise();
                LibNoise.Filter.Pipe pipe = new LibNoise.Filter.Pipe();
                pipe.Frequency = frequency;
                pipe.Lacunarity = lacunarity;
                pipe.OctaveCount = octaves;
                pipe.Gain = gain;
                pipe.Primitive3D = new LibNoise.Primitive.ImprovedPerlin(seed, quality);
                noisemodule = pipe;
            }



        }

        public void CreateMultiFNoise()
        {
            if (opIn[0] != null)
            {
                opIn[0].CreateNoise();
                LibNoise.Filter.MultiFractal hmf = new LibNoise.Filter.MultiFractal();
                hmf.Frequency = frequency;
                hmf.Gain = gain;
                hmf.Lacunarity = lacunarity;
                hmf.OctaveCount = octaves;
                
                hmf.Primitive3D = opIn[0].noisemodule;

                noisemodule = hmf;
            }
            else
            {
                //opIn[0].CreateNoise();
                LibNoise.Filter.MultiFractal hmf = new LibNoise.Filter.MultiFractal();
                hmf.Frequency = frequency;
                hmf.Gain = gain;
                hmf.Lacunarity = lacunarity;
                hmf.OctaveCount = octaves;
                hmf.Primitive3D = new LibNoise.Primitive.ImprovedPerlin(seed, quality);
                noisemodule = hmf;
            }



        }

        public void CreateHybridMFNoise()
        {
            if (opIn[0] != null)
            {
                opIn[0].CreateNoise();
                LibNoise.Filter.HybridMultiFractal hmf = new LibNoise.Filter.HybridMultiFractal();
                hmf.Frequency = frequency;
                hmf.Gain = gain;
                hmf.Lacunarity = lacunarity;
                hmf.OctaveCount = octaves;
                
                hmf.Primitive3D = opIn[0].noisemodule;

                noisemodule = hmf;
            }
            else
            {
                //opIn[0].CreateNoise();
                LibNoise.Filter.HybridMultiFractal hmf = new LibNoise.Filter.HybridMultiFractal();
                hmf.Frequency = frequency;
                hmf.Gain = gain;
                hmf.Lacunarity = lacunarity;
                hmf.OctaveCount = octaves;
                hmf.Primitive3D = new LibNoise.Primitive.ImprovedPerlin(seed, quality);
                noisemodule = hmf;
            }



        }

        public void CreateHMFNoise()
        {
            if(opIn[0]!=null)
            {
                opIn[0].CreateNoise();
                LibNoise.Filter.HeterogeneousMultiFractal hmf = new LibNoise.Filter.HeterogeneousMultiFractal();
                hmf.Frequency = frequency;
                hmf.Gain = gain;
                hmf.Lacunarity = lacunarity;
                hmf.OctaveCount = octaves;
                hmf.Primitive3D = opIn[0].noisemodule;
                
                noisemodule = hmf;
            } else
            {
                //opIn[0].CreateNoise();
                LibNoise.Filter.HeterogeneousMultiFractal hmf = new LibNoise.Filter.HeterogeneousMultiFractal();
                hmf.Frequency = frequency;
                hmf.Gain = gain;
                hmf.Lacunarity = lacunarity;
                hmf.OctaveCount = octaves;
                hmf.Primitive3D = new LibNoise.Primitive.ImprovedPerlin(seed, quality);
                noisemodule = hmf;
            }

            

        }

        public void CreateBlendNoise()
        {
            if (opIn[0] != null && opIn[1]!=null)
            {
                opIn[0].CreateNoise();
                opIn[1].CreateNoise();
                //LibNoise.Modifier.Blend blend = new LibNoise.Modifier.Blend();
                //blend.RightModule = opIn.noisemodule;
                //LibNoise.IModule boperation = null;

                Debug.Log("Have both Nodes!");
                
                switch (blendop)
                {
                    case Operator.add:
                        LibNoise.Combiner.Add add = new LibNoise.Combiner.Add(opIn[0].noisemodule, opIn[1].noisemodule);
                        noisemodule = add;
                        break;

                    case Operator.subtract:
                        LibNoise.Combiner.Substract sub = new LibNoise.Combiner.Substract(opIn[0].noisemodule, opIn[1].noisemodule);
                        noisemodule = sub;
                        break;

                    case Operator.multiply:
                        LibNoise.Combiner.Multiply mult = new LibNoise.Combiner.Multiply(opIn[0].noisemodule, opIn[1].noisemodule);
                        noisemodule = mult;
                        break;

                    case Operator.max:
                        LibNoise.Combiner.Min max = new LibNoise.Combiner.Min(opIn[0].noisemodule, opIn[1].noisemodule);
                        noisemodule = max;
                        break;

                    case Operator.min:
                        LibNoise.Combiner.Min min = new LibNoise.Combiner.Min(opIn[0].noisemodule, opIn[1].noisemodule);
                        noisemodule = min;
                        break;

                    default:
                        noisemodule = new ImprovedPerlin(seed, quality);
                        break;
                } 
                
                Debug.Log("created Blend noise");
            }
            else
            {
                noisemodule = new ImprovedPerlin(seed, quality);

            }
        }

        public void CreateFinalNoise()
        {
            //basically just gets the previous node and generates
            if (opIn[0] != null)
            {
                opIn[0].CreateNoise();
                noisemodule = opIn[0].noisemodule;
                Debug.Log("created final noise");
            }
            else
            {
                noisemodule = new ImprovedPerlin(seed, quality);

            }
        }

        public void CreateTurbulence()
        {
            if(opIdIn!=null)
            {
                opIn[0].CreateNoise();
                LibNoise.Transformer.Turbulence turbulence = new LibNoise.Transformer.Turbulence(opIn[0].noisemodule);
                turbulence.Power = power;
                
                turbulence.XDistortModule = new LibNoise.Primitive.ImprovedPerlin(seed, quality);
                turbulence.YDistortModule = new LibNoise.Primitive.ImprovedPerlin(seed, quality);
                turbulence.ZDistortModule = new LibNoise.Primitive.ImprovedPerlin(seed, quality);
                noisemodule = turbulence;
                
            } else
            {
                LibNoise.Transformer.Turbulence turbulence = new LibNoise.Transformer.Turbulence(new LibNoise.Primitive.ImprovedPerlin(seed, quality));
                turbulence.Power = power;

                turbulence.XDistortModule = new LibNoise.Primitive.ImprovedPerlin(seed, quality);
                turbulence.YDistortModule = new LibNoise.Primitive.ImprovedPerlin(seed, quality);
                turbulence.ZDistortModule = new LibNoise.Primitive.ImprovedPerlin(seed, quality);
                noisemodule = turbulence;
            }
        }

        public void CreateCurveNoise()
        {
            if (opIn != null)
            {
                opIn[0].CreateNoise();
                LibNoise.Modifier.Curve curve = new LibNoise.Modifier.Curve(opIn[0].noisemodule);
                curve.ClearControlPoints();
                for (int i = 0; i < noisecurve.length; i++)
                {
                    curve.AddControlPoint(noisecurve[i].value, noisecurve[i].time);
                }
                noisemodule = curve;


            }
            else
            {
                noisemodule = new LibNoise.Modifier.Curve(new ImprovedPerlin(seed, quality));

            }

        }

        public void CreateBillowNoise()
        {
            if (opIdIn != null)
            {
                opIn[0].CreateNoise();
                LibNoise.Filter.Billow billow = new LibNoise.Filter.Billow();
                billow.Frequency = frequency;
                billow.Gain = gain;
                billow.Lacunarity = lacunarity;
                billow.OctaveCount = octaves;
                billow.Scale = scale;
                billow.Bias = bias;
                
                billow.Primitive3D = opIn[0].noisemodule;
                noisemodule = billow;
            } else
            {
                //opIn[0].CreateNoise();
                LibNoise.Filter.Billow billow = new LibNoise.Filter.Billow();
                billow.Frequency = frequency;
                billow.Gain = gain;
                billow.Lacunarity = lacunarity;
                billow.OctaveCount = octaves;
                billow.Scale = scale;
                billow.Primitive3D = new SimplexPerlin(seed, quality);
                noisemodule = billow;
            }
        }

        public void CreatePerlinNoise()
        {

            noisemodule = new ImprovedPerlin(seed, quality);

        }

        public void CreateSimplexNoise()
        {
            
            noisemodule = new SimplexPerlin(seed, quality);

        }


        public void CreateTerraceNoise()
        {
            if (opIn != null)
            {
                opIn[0].CreateNoise();
                LibNoise.Modifier.Terrace terrace = new LibNoise.Modifier.Terrace(opIn[0].noisemodule);
                terrace.ClearControlPoints();
                float inc = 2.0f / steps;
                float control = -1f;
                for (int i = 0; i < steps; i++)
                {
                    terrace.AddControlPoint(control);
                    control += inc;
                }
                noisemodule = terrace;

                

            }
            else
            {
                noisemodule = new LibNoise.Modifier.Terrace(new ImprovedPerlin(seed, quality));

            }



        }

        public void CreateVoronoiNoise()
        {
            if (opIn != null)
            {
                opIn[0].CreateNoise();
                LibNoise.Filter.Voronoi voronoi = new LibNoise.Filter.Voronoi();
                voronoi.OctaveCount = octaves;
                voronoi.Lacunarity = lacunarity;
                voronoi.Displacement = displacement;
                voronoi.Distance = bUseDistance;
                voronoi.Frequency = frequency;
                voronoi.Primitive3D = opIn[0].noisemodule;
                voronoi.Gain = gain;
                noisemodule = voronoi;


            }
            else
            {
                
                LibNoise.Filter.Voronoi voronoi = new LibNoise.Filter.Voronoi();
                voronoi.OctaveCount = octaves;
                voronoi.Lacunarity = lacunarity;
                voronoi.Displacement = displacement;
                voronoi.Distance = bUseDistance;
                voronoi.Frequency = frequency;
                voronoi.Primitive3D = new ImprovedPerlin(seed, quality);
                voronoi.Gain = gain;
                noisemodule = voronoi;
            }



        }

    


        public virtual Texture2D Preview(Texture2D tex)
        {

            return RenderTexture(tex);
        }





        public Texture2D RenderTexture(Texture2D tex)
        {
            CreateNoise();

            LibNoise.Builder.NoiseMap nm = new LibNoise.Builder.NoiseMap();

            LibNoise.Builder.NoiseMapBuilderPlane noisemap = new LibNoise.Builder.NoiseMapBuilderPlane(0, (double)scale, 0, (double)scale, true);
            noisemap.SetSize(tex.width, tex.height);
            noisemap.NoiseMap = nm;

            
            noisemap.SourceModule = noisemodule;

            noisemap.SetSize(tex.width, tex.height);
            noisemap.Build();

            LibNoise.Renderer.ImageRenderer img = new LibNoise.Renderer.ImageRenderer();
            //img.BackgroundImage = new LibNoise.Renderer.Image(tex.width, tex.height);
            img.Image = new LibNoise.Demo.Ext.Dotnet.BitmapAdaptater(tex);
            img.LightBrightness = lightBrightness;
            img.LightContrast = lightContrast;



            img.Gradient = LibNoise.Renderer.GradientColors.Grayscale;
            img.NoiseMap = noisemap.NoiseMap;
            img.Render();
            for (int y = 0; y < tex.height; y++)
            {
                for (int x = 0; x < tex.width; x++)
                {


                    //double colamnt = perlin.GetValue(x * frequency, y * frequency) * scale;
                    //tex.SetPixel(x, y, new Color((float)colamnt/256, (float)colamnt/256, (float)colamnt/256,1));
                    Color32 c = new Color32(img.Image.GetValue(x, y).Red, img.Image.GetValue(x, y).Green, img.Image.GetValue(x, y).Blue, img.Image.GetValue(x, y).Alpha);
                    tex.SetPixel(x, y, c);
                    //Debug.Log("C:"+ img.Image.GetValue(x, y).Red + " "+ img.Image.GetValue(x, y).Green + " "+ img.Image.GetValue(x, y).Blue+ " " + img.Image.GetValue(x, y).Alpha);
                }
                tex.Apply();
            }

            tex.Apply();
            Debug.Log("Generated Preview: ");
            previewtex = tex;
            return tex;
        }


        public string GetUniqueID()
        {
            string[] split = System.DateTime.Now.TimeOfDay.ToString().Split(new Char[] { ':', '.' });
            string id = "";
            for (int i = 0; i < split.Length; i++)
            {
                id += split[i];
            }
            return id+UnityEngine.Random.Range(1,99999);
        }

    }
}
