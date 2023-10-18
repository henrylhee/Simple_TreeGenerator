using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using Random = UnityEngine.Random;
//using Unity.Burst.CompilerServices;
using static UnityEditor.PlayerSettings;
using System.Security.Cryptography;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;


namespace Gen
{
    public class TreeGeneration : MonoBehaviour
    {
        [Header("Graph")]
        Graph graph = new Graph();
        [SerializeField]
        private bool generateNewGraph = false;
        [SerializeField]
        private bool drawGraph = true;
        [SerializeField]
        private bool generateMesh = false;
        [SerializeField]
        private bool deleteMesh = false;

        [HideInInspector]
        public GraphSettings settings;
        [SerializeField]
        private GraphModel graphModel;

        [SerializeField]
        private Material material;




        void OnValidate()
        {
            Initialize();
            if (generateNewGraph)
            {
                GenerateGraph();
                generateNewGraph = false;
            }

            if (generateMesh)
            {
                if (graph.branches[0] != null)
                {
                    GenerateMesh();
                }
                else
                {
                    Debug.Log("No graph to generate a mesh!");
                }
                generateMesh = false;
            }

            if (deleteMesh)
            {
                GetComponent<MeshFilter>().mesh = null;
                deleteMesh = false;
            }

            enabled = true;
        }

        private void Initialize()
        {
            Debug.Log("Initialize.");
            InitializeSettings();
        }

        private void GenerateGraph()
        {
            graph.Generate();
        }

        private void GenerateMesh()
        {
            MeshGenerator meshGenerator = new MeshGenerator();
            GetComponent<MeshFilter>().mesh = meshGenerator.Generate(graph.branches, transform);
            GetComponent<MeshRenderer>().material = material;
        }

        //Draw the Graph
        private void OnDrawGizmos()
        {
            if (drawGraph && graph.branches != null)
            {
                float thicknessToLength = GraphSettings.Instance.ThicknessToSegmentLength;

                foreach (Branch branch in graph.branches)
                {
                    foreach (Internode internode in branch.internodes)
                    {
                        if (internode.thickness > 0.001f)
                        {
                            Gizmos.color = Color.red;
                            Gizmos.DrawSphere(internode.position, internode.thickness);
                        }

                        Gizmos.color = Color.yellow;
                        Gizmos.DrawLine(internode.position, internode.position + internode.direction*internode.thickness*thicknessToLength);
                    }
                }
            }
        }

        private void InitializeSettings()
        {
            settings = new GraphSettings();
            GraphSettings.Instance.Initialize(graphModel);
        }
    }
}
