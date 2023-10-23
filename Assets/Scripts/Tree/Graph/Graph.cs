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

            if (branches.Count == 0)
            {
                Debug.Log("max length: " + GraphModel.Instance.MaxLength);
                Debug.Log("min thickness absolute: " + GraphModel.Instance.MinThicknessAbsolute);
                Debug.Log("Generate tree from position: " + GraphModel.Instance.StartPosition);
            }
            else
            {
                branches = new List<Branch>();
                Debug.Log("Generating new tree from position: " + GraphModel.Instance.StartPosition);
            }
            branches.Add(new Branch(new Internode(GraphModel.Instance.StartPosition, GraphModel.Instance.StartDirection, GraphModel.Instance.StartThickness), 0, false));
            branches[0].NewBranch.AddListener(Split);
            branches[0].Generate();
        }


        public void Split(Branch newBranch)
        {
            //Debug.Log("-->Split occured! Generate new Branch. Branch count: " + branches.Count + 1 + ". Branch thickness: " + newBranch.thickness);

            branches.Add(newBranch);
            branches[branches.Count-1].NewBranch.AddListener(Split);
            branches[branches.Count - 1].Generate();
        }
    }
}

