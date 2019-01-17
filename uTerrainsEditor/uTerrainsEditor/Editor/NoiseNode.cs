using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Zorlock.uTerrains;

namespace Zorlock.uTerrains.uEditor
{
    public class NoiseNode : Node
    {
        public NoiseOperation noiseOperation;
        public Texture2D texture = new Texture2D(256, 256);



        public override void Init()
        {
            nodetype = NodeType.Noise;
            noiseOperation = new NoiseOperation();
            noiseOperation.editorRect = windowRect;
            noiseOperation.OperationID = noiseOperation.GetUniqueID();

        }


        public virtual void Init(NoiseOperation n)
        {
            nodetype = NodeType.Noise;
            noiseOperation = n;

        }


        public override void SetNodeIn(Node nin, int index)
        {
            base.SetNodeIn(nin,index);
            if (nin is NoiseNode)
            {
                NoiseNode nn = (NoiseNode)nin;
                noiseOperation.opIn[index] = nn.noiseOperation;
                if (noiseOperation.opIdIn.Count - 1 < index)
                {
                    
                    noiseOperation.opIdIn.Add(nn.noiseOperation.OperationID);
                }
                else
                {
                    noiseOperation.opIdIn[index] = nn.noiseOperation.OperationID;
                }
            }

        }

        public override void SetNodeIn(Node nin)
        {
            base.SetNodeIn(nin);
            if(nin is NoiseNode)
            {
                NoiseNode nn = (NoiseNode)nin;
                noiseOperation.opIn[0] = nn.noiseOperation;
                if (noiseOperation.opIdIn.Count - 1 < 0)
                {

                    noiseOperation.opIdIn.Add(nn.noiseOperation.OperationID);
                }
                else
                {
                    noiseOperation.opIdIn[0] = nn.noiseOperation.OperationID;
                }
                //noiseOperation.opIdIn[0] = nn.noiseOperation.OperationID;
            }
            
        }

        public override void SetNodeOut(Node nout)
        {
            base.SetNodeOut(nout);
            if (nout is NoiseNode)
            {
                NoiseNode nn = (NoiseNode)nout;
                noiseOperation.opOut = nn.noiseOperation;
                noiseOperation.opIdOut = nn.noiseOperation.OperationID;
            }
        }



        public override void UpdateWindow()
        {
            noiseOperation.editorRect = windowRect;
        }

    }
}
