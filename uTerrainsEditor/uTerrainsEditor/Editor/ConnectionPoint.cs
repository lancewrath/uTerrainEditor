using System;
using UnityEngine;
using UnityEditor;

namespace Zorlock.uTerrains.uEditor
{
    public enum ConnectionPointType { In, Out }

    public class ConnectionPoint
    {
        public Rect rect;

        public ConnectionPointType type;

        public Node node;

        public GUIStyle style;

        public Action<ConnectionPoint> OnClickConnectionPoint;

        public ConnectionPoint(Node node, ConnectionPointType type, Action<ConnectionPoint> OnClickConnectionPoint)
        {
            this.node = node;
            this.type = type;
            //this.style = style;
            this.OnClickConnectionPoint = OnClickConnectionPoint;
            rect = new Rect(0, 0, 10f, 20f);
        }

        public void Draw()
        {
            rect.y = node.windowRect.y + (node.windowRect.height * 0.5f) - rect.height * 0.5f;

            switch (type)
            {
                case ConnectionPointType.In:
                    rect.x = node.windowRect.x - rect.width + 2f;
                    break;

                case ConnectionPointType.Out:
                    rect.x = node.windowRect.x + node.windowRect.width - 2f;
                    break;
            }

            if (GUI.Button(rect, ""))
            {
                if (OnClickConnectionPoint != null)
                {
                    OnClickConnectionPoint(this);
                }
            }
        }

        public void Draw(float offx,float offy)
        {
            rect.y = node.windowRect.y + (node.windowRect.height * 0.5f) - rect.height * 0.5f;
            rect.y += offy;
            switch (type)
            {
                case ConnectionPointType.In:
                    rect.x = node.windowRect.x - rect.width + 2f;
                    rect.x += offx;
                    break;

                case ConnectionPointType.Out:
                    rect.x = node.windowRect.x + node.windowRect.width - 2f;
                    rect.x += offx;
                    break;
            }

            if (GUI.Button(rect, ""))
            {
                if (OnClickConnectionPoint != null)
                {
                    OnClickConnectionPoint(this);
                }
            }
        }

    }
}