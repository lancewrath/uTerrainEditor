using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zorlock.uTerrains.uEditor
{
    public class FinalNode : NoiseNode
    {


        public override void SetNodeOut(Node nout)
        {
            //does not output nodes

        }

        public override void DrawCurves()
        {
            //don't draw out point
            if (inPoint != null)
            {
                inPoint[0].Draw();
            }
        }

        public override void Init()
        {
            nodetype = NodeType.Final;
            noiseOperation = new NoiseOperation();
            noiseOperation.opType = NoiseOperation.OperationType.Final;
            noiseOperation.editorRect = windowRect;
            noiseOperation.OperationID = noiseOperation.GetUniqueID();

        }

        public override void Init(NoiseOperation n)
        {
            nodetype = NodeType.Final;
            noiseOperation = n;
            noiseOperation.opType = NoiseOperation.OperationType.Final;

        }
    }
}
