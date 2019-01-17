﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Zorlock.uTerrains;

namespace Zorlock.uTerrains.uEditor
{
    public class GeneratorEditorWindow : EditorWindow
    {

        #region variables
        public Generator generator;
        static List<Node> windows = new List<Node>();
        static List<Connection> connections = new List<Connection>();
        Node selectedNode;
        ConnectionPoint selectedInPoint;
        ConnectionPoint selectedOutPoint;
        bool clickedWindow;
        Vector3 mousePosition;
        bool rebuild;
        public enum UserActions
        {
            AddNode,AddStart,AddNoise,AddPerlin,AddSimplex,AddTerrace,AddVoronoi,AddBillow,AddCurve,AddFinal,AddTurbulence,AddBlend,AddHMF,DeleteNode
        }

        #endregion

        #region Init

        #endregion

        #region GUI Methods
        private void OnGUI()
        {
            if(rebuild)
            {
                windows.Clear();
                connections.Clear();
                NoiseNode noiseNode;
                foreach (NoiseOperation n in generator.noiseOperations)
                {
                    switch(n.opType)
                    {

                        case NoiseOperation.OperationType.HMF:
                            noiseNode = new HMFNode();
                            noiseNode.title = "Heterogeneous MultiFractal";
                            break;

                        case NoiseOperation.OperationType.Blend:
                            noiseNode = new BlendNode();
                            noiseNode.title = "Blend";
                            break;

                        case NoiseOperation.OperationType.Curve:
                            noiseNode = new CurveNode();
                            noiseNode.title = "Curve";
                            break;
                            
                        case NoiseOperation.OperationType.Voronoi:
                            noiseNode = new VoronoiNode();
                            noiseNode.title = "Voronoi Noise";
                            break;

                        case NoiseOperation.OperationType.Billow:
                            noiseNode = new BillowNode();
                            noiseNode.title = "Billow Noise";
                            break;

                        case NoiseOperation.OperationType.Terrace:
                            noiseNode = new TerraceNode();
                            noiseNode.title = "Terrace Noise";
                            break;

                        case NoiseOperation.OperationType.Simplex:
                            noiseNode = new SimplexNode();
                            noiseNode.title = "Simplex Noise";
                            break;

                        case NoiseOperation.OperationType.Perlin:
                            noiseNode = new PerlinNode();
                            noiseNode.title = "Perlin Noise";
                            break;

                        case NoiseOperation.OperationType.Noise:
                            noiseNode = new NoiseNode();
                            noiseNode.title = "Noise";
                            break;

                        case NoiseOperation.OperationType.Start:
                            noiseNode = new StartNode();
                            noiseNode.title = "Start";
                            break;

                        case NoiseOperation.OperationType.Final:
                            noiseNode = new FinalNode();
                            noiseNode.title = "Final";
                            break;
                        case NoiseOperation.OperationType.Turbulence:
                            noiseNode = new TurbulenceNode();
                            noiseNode.title = "Turbulence";
                            break;
                        default:
                            noiseNode = new NoiseNode();
                            noiseNode.title = "Default";
                            break;
                    }
                    
                    noiseNode.Init(n);
                    noiseNode.windowRect = n.editorRect;
                    
                    noiseNode.Init(n);
                    noiseNode.SetConnectionPoints(OnClickInPoint, OnClickOutPoint);

                    

                    
                    windows.Add(noiseNode);
                }
                RebuildConnections();
                rebuild = false;
            }
            Event e = Event.current;
            mousePosition = e.mousePosition;
            UserInput(e);
            DrawWindows();
            DrawConnections();
        }

        private void RebuildConnections()
        {
            foreach (Node n in windows)
            {
                if(n is NoiseNode)
                {
                    NoiseNode noiseNode = (NoiseNode)n;

                    
                        if (noiseNode != null)
                        {
                            foreach (Node nn in windows)
                            {
                                if (nn is NoiseNode)
                                {
                                    NoiseNode nnoiseNode = (NoiseNode)nn;
                                    for (int i = 0; i < nnoiseNode.noiseOperation.opIdIn.Count; i++)
                                    {
                                        if (nnoiseNode.noiseOperation.opIdIn[i].Equals(noiseNode.noiseOperation.OperationID) && n != nn)
                                        {

                                            connections.Add(new Connection(nn.inPoint[i], n.outPoint, OnClickRemoveConnection));

                                        }
                                    }

                                    

                                }
                            }
                        }
                    

                }
            }
        }

        private void OnDisable()
        {
            UnityEditor.EditorUtility.SetDirty(generator);
        }
        private void OnEnable()
        {
            rebuild = true;

        }

        private void DrawWindows()
        {
            BeginWindows();
            foreach (Node n in windows)
            {
                n.DrawCurves();
                
            }
            for (int i = 0; i < windows.Count; i++)
            {
                windows[i].windowRect = GUI.Window(i, windows[i].windowRect, DrawNodeWindow, windows[i].title);
            }
            EndWindows();
        }

        private void DrawConnections()
        {
            if (connections != null)
            {
                for (int i = 0; i < connections.Count; i++)
                {
                    connections[i].Draw();
                }
            }
        }

        private void DrawNodeWindow(int id)
        {
            windows[id].DrawWindow();
            GUI.DragWindow();
            windows[id].UpdateWindow();
        }

        private void UserInput(Event e)
        {
            if(e.button==1)
            {
                if(e.type==EventType.MouseDown)
                {
                    RightClick(e);
                }
            }
            if (e.button == 0)
            {
                if (e.type == EventType.MouseDown)
                {
                    LeftClick(e);
                }
            }
        }

        private void LeftClick(Event e)
        {

        }
        private void RightClick(Event e)
        {
            selectedNode = null;
            clickedWindow = false;
            for (int i = 0; i < windows.Count; i++)
            {
                if (windows[i].windowRect.Contains(mousePosition))
                {
                    selectedNode = windows[i];
                    clickedWindow = true;
                    break;
                }
            }
            if (!clickedWindow)
            {
                AddNode(e);
            }
            else
            {
                ModifyNode(e);
            }
        }

        void AddNode(Event e)
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Add Start"), false, ContextCallBack, UserActions.AddStart);
            menu.AddItem(new GUIContent("Add Perlin Noise"), false, ContextCallBack, UserActions.AddPerlin);
            menu.AddItem(new GUIContent("Add Simplex Noise"), false, ContextCallBack, UserActions.AddSimplex);
            menu.AddItem(new GUIContent("Add Terrace"), false, ContextCallBack, UserActions.AddTerrace);
            menu.AddItem(new GUIContent("Add Voronoi"), false, ContextCallBack, UserActions.AddVoronoi);
            menu.AddItem(new GUIContent("Add Billow"), false, ContextCallBack, UserActions.AddBillow);
            menu.AddItem(new GUIContent("Add Curve"), false, ContextCallBack, UserActions.AddCurve);
            menu.AddItem(new GUIContent("Add Turbulence"), false, ContextCallBack, UserActions.AddTurbulence);
            menu.AddItem(new GUIContent("Add Blend"), false, ContextCallBack, UserActions.AddBlend);
            menu.AddItem(new GUIContent("Add Heterogeneous MultiFractal"), false, ContextCallBack, UserActions.AddHMF);
            menu.AddItem(new GUIContent("Add Final"), false, ContextCallBack, UserActions.AddFinal);
            menu.ShowAsContext();
            e.Use();
        }

        void ModifyNode(Event e)
        {
            GenericMenu menu = new GenericMenu();


            switch(selectedNode.nodetype)
            {
                case Node.NodeType.Noise:
                    menu.AddItem(new GUIContent("Delete Noise"), false, ContextCallBack, UserActions.DeleteNode);
                    break;
                case Node.NodeType.Perlin:
                    menu.AddItem(new GUIContent("Delete Perlin"), false, ContextCallBack, UserActions.DeleteNode);
                    break;
                case Node.NodeType.Simplex:
                    menu.AddItem(new GUIContent("Delete Simplex"), false, ContextCallBack, UserActions.DeleteNode);
                    break;
                case Node.NodeType.Terrace:
                    menu.AddItem(new GUIContent("Delete Terrace"), false, ContextCallBack, UserActions.DeleteNode);
                    break;
                case Node.NodeType.Voroni:
                    menu.AddItem(new GUIContent("Delete Voronoi"), false, ContextCallBack, UserActions.DeleteNode);
                    break;
                case Node.NodeType.Billow:
                    menu.AddItem(new GUIContent("Delete Billow"), false, ContextCallBack, UserActions.DeleteNode);
                    break;
                case Node.NodeType.Curve:
                    menu.AddItem(new GUIContent("Delete Curve"), false, ContextCallBack, UserActions.DeleteNode);
                    break;
                case Node.NodeType.Turbulence:
                    menu.AddItem(new GUIContent("Delete Turbulence"), false, ContextCallBack, UserActions.DeleteNode);
                    break;
                case Node.NodeType.Blend:
                    menu.AddItem(new GUIContent("Delete Blend"), false, ContextCallBack, UserActions.DeleteNode);
                    break;
                case Node.NodeType.HMF:
                    menu.AddItem(new GUIContent("Delete Heterogeneous MultiFractal"), false, ContextCallBack, UserActions.DeleteNode);
                    break;
                case Node.NodeType.Final:
                    menu.AddItem(new GUIContent("Delete Final"), false, ContextCallBack, UserActions.DeleteNode);
                    break;
                case Node.NodeType.Start:
                    menu.AddItem(new GUIContent("Delete Start"), false, ContextCallBack, UserActions.DeleteNode);
                    break;
            }
                

                

            menu.ShowAsContext();
            e.Use();
        }

        void ContextCallBack(object o)
        {
            NoiseNode noiseNode = null;
            UserActions a = (UserActions)o;
            switch(a)
            {
                case UserActions.AddNode:
                    break;


                case UserActions.AddHMF:
                    noiseNode = new HMFNode();
                    noiseNode.windowRect = new Rect(mousePosition.x, mousePosition.y, 200, 300);
                    noiseNode.title = "Heterogeneous MultiFractal";

                    break;

                case UserActions.AddBlend:
                    noiseNode = new BlendNode();
                    noiseNode.windowRect = new Rect(mousePosition.x, mousePosition.y, 200, 300);
                    noiseNode.title = "Blend";

                    break;

                case UserActions.AddTurbulence:
                    noiseNode = new TurbulenceNode();
                    noiseNode.windowRect = new Rect(mousePosition.x, mousePosition.y, 200, 400);
                    noiseNode.title = "Turbulence";

                    break;

                case UserActions.AddCurve:
                    noiseNode = new CurveNode();
                    noiseNode.windowRect = new Rect(mousePosition.x, mousePosition.y, 200, 400);
                    noiseNode.title = "Curve";

                    break;

                case UserActions.AddBillow:
                    noiseNode = new BillowNode();
                    noiseNode.windowRect = new Rect(mousePosition.x, mousePosition.y, 200, 400);
                    noiseNode.title = "Billow";

                    break;

                case UserActions.AddVoronoi:
                    noiseNode = new VoronoiNode();
                    noiseNode.windowRect = new Rect(mousePosition.x, mousePosition.y, 200, 400);
                    noiseNode.title = "Voronoi";

                    break;

                case UserActions.AddTerrace:
                    noiseNode = new TerraceNode();
                    noiseNode.windowRect = new Rect(mousePosition.x, mousePosition.y, 200, 300);
                    noiseNode.title = "Terrace";

                    break;

                case UserActions.AddSimplex:
                    noiseNode = new SimplexNode();
                    noiseNode.windowRect = new Rect(mousePosition.x, mousePosition.y, 200, 300);
                    noiseNode.title = "Simplex Noise";

                    break;

                case UserActions.AddFinal:
                    noiseNode = new FinalNode();
                    noiseNode.windowRect = new Rect(mousePosition.x, mousePosition.y, 200, 100);
                    noiseNode.title = "Final";

                    break;

                case UserActions.AddStart:
                    noiseNode = new StartNode();
                    noiseNode.windowRect = new Rect(mousePosition.x, mousePosition.y, 200, 100);
                    noiseNode.title = "Start";
                    break;

                case UserActions.AddNoise:
                    noiseNode = new NoiseNode();
                    noiseNode.windowRect = new Rect(mousePosition.x,mousePosition.y,200,300);
                    noiseNode.title = "Noise";

                    break;

                case UserActions.AddPerlin:
                    noiseNode = new PerlinNode();
                    noiseNode.windowRect = new Rect(mousePosition.x, mousePosition.y, 200, 300);
                    noiseNode.title = "Perlin Noise";

                    break;

                case UserActions.DeleteNode:
                    if(selectedNode!=null)
                    {
                        windows.Remove(selectedNode);
                        if(selectedNode is NoiseNode)
                        {

                            NoiseNode n = (NoiseNode)selectedNode;
                            generator.noiseOperations.Remove(n.noiseOperation);
                        }
                        foreach (Connection c in connections)
                        {
                            if(c.inPoint.node==selectedNode)
                            {
                                connections.Remove(c);
                            }

                            if(c.outPoint.node==selectedNode)
                            {
                                connections.Remove(c);
                            }
                        }   
                    }
                    break;

                default:
                    break;
            }
            if(noiseNode!=null)
            {
                noiseNode.Init();
                noiseNode.SetConnectionPoints(OnClickInPoint, OnClickOutPoint);
                windows.Add(noiseNode);
                generator.noiseOperations.Add(noiseNode.noiseOperation);
            }
        }


        #endregion

        #region Helper Methods

        private void OnClickInPoint(ConnectionPoint inPoint)
        {
            selectedInPoint = inPoint;

            if (selectedOutPoint != null)
            {
                if (selectedOutPoint.node != selectedInPoint.node)
                {
                    CreateConnection();
                    ClearConnectionSelection();
                }
                else
                {
                    ClearConnectionSelection();
                }
            }
        }

        private void OnClickOutPoint(ConnectionPoint outPoint)
        {
            selectedOutPoint = outPoint;

            if (selectedInPoint != null)
            {
                if (selectedOutPoint.node != selectedInPoint.node)
                {
                    CreateConnection();
                    ClearConnectionSelection();
                }
                else
                {
                    ClearConnectionSelection();
                }
            }
        }

        private void OnClickRemoveConnection(Connection connection)
        {
            connections.Remove(connection);
        }

        private void CreateConnection()
        {
            if (connections == null)
            {
                connections = new List<Connection>();
            }

            connections.Add(new Connection(selectedInPoint, selectedOutPoint, OnClickRemoveConnection));
        }

        private void ClearConnectionSelection()
        {
            selectedInPoint = null;
            selectedOutPoint = null;
        }

        #endregion

    }
}