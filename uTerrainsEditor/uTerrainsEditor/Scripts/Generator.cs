using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zorlock.uTerrains
{
    [CreateAssetMenu]
    [Serializable]
    public class Generator : ScriptableObject
    {

        [SerializeField] public List<NoiseOperation> noiseOperations = new List<NoiseOperation>();
        [SerializeField]  public string name = "Generator";
        NoiseOperation finalnoise = null;


        public void ReconnectNodes()
        {
            foreach (NoiseOperation n in noiseOperations)
            {
                foreach (NoiseOperation nn in noiseOperations)
                {
                    for (int i = 0; i < nn.opIdIn.Count; i++)
                    {
                        if (nn.opIdIn[i].Equals(n.OperationID))
                        {
                            nn.opIn[i] = n;
                            n.opOut = nn;
                            Debug.Log("Connected Nodes");
                        }
                    }

                }
            }
        }

        public void GetFinalOperation()
        {
            foreach (NoiseOperation n in noiseOperations)
            {
                if(n.opType==NoiseOperation.OperationType.Final)
                {
                    finalnoise = n;
                    finalnoise.CreateNoise();
                    Debug.Log("Have final operation, building noise");
                }
            }
        }

        public double GetValue(double x,double y,double z)
        {
            if(finalnoise==null)
            {
                GetFinalOperation();
                if (finalnoise == null)
                    return 0;
            }
            if(finalnoise.noisemodule==null)
            {
                Debug.Log("noise module null");
            }
            return finalnoise.noisemodule.GetValue(x, y, z);

        }
        

    }
}
