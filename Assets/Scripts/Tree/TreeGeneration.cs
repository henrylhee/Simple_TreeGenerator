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
using Unity.VisualScripting;

namespace Gen
{
    public class TreeGeneration : MonoBehaviour
    {
        [Header("Graph")]
        Graph graph = new Graph();
        [SerializeField]
        private bool reloadSettings = false;
        [SerializeField]
        private bool generateGraph = false;
        [SerializeField]
        private bool generateGraphOnChange = false;
        [SerializeField]
        private bool drawGraph = true;
        [SerializeField]
        private bool generateMesh = false;
        [SerializeField]
        private bool deleteMesh = false;

        private bool scriptLoaded = false;

        
        public GraphSettings graphSettingsTemp;
        private GraphSettings graphSettings;

        [SerializeField]
        private Material material;


        void OnValidate()
        {
            if (!scriptLoaded)
            {
                Initialize();
                scriptLoaded = true;
            }
            else
            {
                if (reloadSettings)
                {
                    Initialize();
                    reloadSettings = false;
                }
                else if (generateGraph)
                {
                    UpdateSettings();
                    GenerateGraph();
                    generateGraph = false;
                }
                else if (generateMesh)
                {
                    if (graph.branches[0] != null)
                    {
                        GenerateMesh();
                    }
                    else
                    {
                        Debug.Log("Cant generate a tree mesh. Missing graph!");
                    }
                    generateMesh = false;
                }
                else if (deleteMesh)
                {
                    GetComponent<MeshFilter>().mesh = null;
                    deleteMesh = false;
                }
            }
        }

        private void Initialize()
        {
            Debug.Log("Initialize.");
            
            new GraphModel();
            GraphModel.Instance.Initialize(graphSettings);
            graphSettingsTemp = graphSettings;
            graphSettingsTemp.OnSettingsChanged.AddListener(SettingsChanged);
        }

        private void UpdateSettings()
        {
            Debug.Log("Update settings");

            GraphModel.Instance.Initialize(graphSettingsTemp);
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

        private void SettingsChanged()
        {
            UpdateSettings();
            if (generateGraphOnChange)
            {
                GenerateGraph();
            }
        }

        //Draw the Graph
        private void OnDrawGizmos()
        {
            if (drawGraph && graph.branches != null)
            {
                float thicknessToLength = GraphModel.Instance.ThicknessToSegmentLength;

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
    }
}
