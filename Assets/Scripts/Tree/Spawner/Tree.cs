using Gen;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor;
using UnityEngine;

public class Tree : MonoBehaviour
{
    Graph graph = new Graph();
    [SerializeField]
    private GameObject leaves;

    [SerializeField]
    GraphSettings graphSettings;

    [SerializeField]
    private LeafSettings leafSettings;
    [SerializeField]
    int spawnLeavesTimes;

    private LeafGeneration leafGeneration;
    private MeshGeneration meshGeneration;


    private void Start()
    {
        
    }

    public void Initialize()
    {
        meshGeneration = new MeshGeneration();
        leafGeneration = new LeafGeneration();

        GraphModel.Instance.Initialize(graphSettings);
        LeafModel.Instance.Initialize(leafSettings);

    }

    public void Spawn()
    {
        graph.Generate();

        GetComponent<MeshFilter>().mesh = meshGeneration.Generate(graph.branches, transform);
        leafGeneration.GenerateLeaf();

        for (int i = 0; i < spawnLeavesTimes; i++)
        {
            leafGeneration.SpawnLeaves(graph.branches, transform, leaves);
        }
    }
}
