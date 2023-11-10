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

        [Header("Mesh")]
        [SerializeField]
        private bool generateMesh = false;
        [SerializeField]
        private bool deleteMesh = false;
        [Header("Leaves")]
        [SerializeField]
        private bool generateLeaf = false;
        [SerializeField]
        private bool spawnLeaves = false;
        [SerializeField]
        private bool removeLeaves = false;
        [SerializeField]
        private GameObject leafPreview;
        [SerializeField]
        private GameObject leaves;

        GameObject leaveContainer;

        private bool scriptLoaded = false;

        
        public GraphSettings graphSettingsTemp;
        private GraphSettings graphSettings;

        public LeafSettings leafSettingsTemp;
        private LeafSettings leafSettings;

        private LeafGeneration leafGeneration;
        private MeshGeneration meshGeneration;
        private LeafSpawner leafSpawner;
        


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
                    GenerateGraph();
                    generateGraph = false;
                }
                else if (generateMesh)
                {
                    if (graph.branches[0] != null)
                    {
                        GetComponent<MeshFilter>().mesh = null;
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
                else if (generateLeaf)
                {
                    if (graph.branches[0] != null)
                    {
                        GenerateLeaf();
                        generateLeaf = false;
                    }
                    else
                    {
                        Debug.Log("Cant generate a tree mesh. Missing graph!");
                    }
                }
                else if (spawnLeaves)
                {
                    spawnLeaves = false;
                    UnityEditor.EditorApplication.delayCall += () =>
                    {
                        //RemoveLeaves();
                        SpawnLeaves();
                    };   
                }
                else if (removeLeaves)
                {
                    removeLeaves = false;
                    UnityEditor.EditorApplication.delayCall += () =>
                    {
                        RemoveLeaves();
                    };
                }
            }
        }

        private void Initialize()
        {
            Debug.Log("Initialize.");

            meshGeneration = new MeshGeneration();
            leafGeneration = new LeafGeneration();

            graphSettings = Resources.Load<GraphSettings>("Settings/Graph/GraphSettings");
            GraphModel.Instance.Initialize(graphSettings);
            graphSettingsTemp = Instantiate(graphSettings);
            graphSettingsTemp.OnSettingsChanged.AddListener(GraphSettingsChanged);

            leafSettings = Resources.Load<LeafSettings>("Settings/Leaves/LeafSettings");
            LeafModel.Instance.Initialize(leafSettings);
            leafSettingsTemp = Instantiate(leafSettings);
            leafSettingsTemp.OnSettingsChanged.AddListener(LeafSettingsChanged);
        }


        private void GenerateGraph()
        {
            graph.Generate();
        }

        private void GenerateMesh()
        {
            GetComponent<MeshFilter>().mesh = meshGeneration.Generate(graph.branches, transform);
        }

        private void GenerateLeaf()
        {
            leafPreview = leafGeneration.GenerateLeaf(leafPreview);
        }

        private void SpawnLeaves()
        {
            leafGeneration.SpawnLeaves(graph.branches, transform, leaves);
        }

        private void RemoveLeaves()
        {
            if(this.transform.childCount != 0)
            {
                DestroyImmediate(this.transform.GetChild(0).gameObject);
            }
        }

        private void GraphSettingsChanged()
        {
            Debug.Log("graph settings changed");
            GraphModel.Instance.Initialize(graphSettingsTemp);
            if (generateGraphOnChange)
            {
                GenerateGraph();
            }
        }

        private void LeafSettingsChanged()
        {
            Debug.Log("leaf settings changed");
            LeafModel.Instance.Initialize(leafSettingsTemp);
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
