using System.Collections;
using System.Collections.Generic;
using TreeGen;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;



namespace Gen
{
    public class Graph
    {
        public List<Branch> branches;


        public void Generate()
        {
            Debug.Log("Generate Graph");

            branches = new List<Branch>();

            branches.Add(new Branch(new Internode(GraphModel.Instance.StartPosition, GraphModel.Instance.StartDirection, GraphModel.Instance.StartThickness), 0, false));
            branches[0].NewBranch.AddListener(Split);
            branches[0].Generate();
        }

        public void Generate(Vector3 startPosition)
        {
            Debug.Log("Generate Graph");

            branches = new List<Branch>();

            branches.Add(new Branch(new Internode(startPosition, GraphModel.Instance.StartDirection, GraphModel.Instance.StartThickness), 0, false));
            branches[0].NewBranch.AddListener(Split);
            branches[0].Generate();
        }


        public void Split(Branch newBranch)
        {
            branches.Add(newBranch);
            branches[branches.Count-1].NewBranch.AddListener(Split);
            branches[branches.Count - 1].Generate();
        }
    }
}

