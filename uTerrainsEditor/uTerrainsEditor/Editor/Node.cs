using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Zorlock.uTerrains.uEditor
{
    public class Node
    {
        public Rect windowRect;
        public string title;
        public Node nodeOut;
        public Node[] nodeIn = new Node[1];
        public NodeType nodetype = NodeType.Node;
        public ConnectionPoint[] inPoint = new ConnectionPoint[1];
        public ConnectionPoint outPoint;

        public enum NodeType
        {
            Node,Start,Noise,Perlin,Simplex,Blend,Terrace,Voroni,Billow,Curve,Final,Turbulence,HMF,HybridMF,MultiF,Texport,Pipe,Scale,ScaleBias,RigidMF,Invert
        }


        public virtual void Init()
        {

        }

        public void Drag(Vector2 delta)
        {
            windowRect.position += delta;
        }

        public virtual void SetNodeIn(Node nin)
        {
            nodeIn[0] = nin;
            //nodeOut = nout;
        }

        public virtual void SetNodeIn(Node nin,int index)
        {
            nodeIn[index] = nin;
            //nodeOut = nout;
        }

        public virtual void SetNodeOut(Node nout)
        {
            //nodeIn = nin;
            nodeOut = nout;
        }

        public virtual void SetConnectionPoints(Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint)
        {
            inPoint[0] = new ConnectionPoint(this, ConnectionPointType.In, OnClickInPoint);
            outPoint = new ConnectionPoint(this, ConnectionPointType.Out, OnClickOutPoint);
        }

        public virtual void DrawWindow()
        {

        }



        public virtual void DrawCurves()
        {
            if (inPoint.Length > 1)
            {
                for (int i = 0; i < inPoint.Length; i++)
                {
                    if (inPoint[i] != null)
                    {
                        inPoint[i].Draw(0,20*i);
                    }
                }
            }else
            {
                if (inPoint[0] != null)
                {
                    inPoint[0].Draw();
                }
            }

            if (outPoint != null)
            {
                outPoint.Draw();
            }
        }

        public virtual void UpdateWindow()
        {

        }


    }
}
