using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zorlock.uTerrains.uEditor
{
    public class StartNode : NoiseNode
    {
        public override void SetNodeIn(Node nin)
        {
            //does not recieve nodes

        }

        public override void DrawCurves()
        {
            //don't draw in point
            if (outPoint != null)
            {
                outPoint.Draw();
            }
        }

        public override void Init()
        {
            nodetype = NodeType.Start;
            noiseOperation = new NoiseOperation();
            noiseOperation.opType = NoiseOperation.OperationType.Start;
            noiseOperation.editorRect = windowRect;
            noiseOperation.OperationID = noiseOperation.GetUniqueID();

        }

        public override void Init(NoiseOperation n)
        {
            nodetype = NodeType.Start;
            noiseOperation = n;
            noiseOperation.opType = NoiseOperation.OperationType.Start;

        }

    }
}
