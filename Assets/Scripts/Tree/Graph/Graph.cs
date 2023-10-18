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
                Debug.Log("max length: " + GraphSettings.Instance.MaxLength);
                Debug.Log("min thickness absolute: " + GraphSettings.Instance.MinThicknessAbsolute);
                Debug.Log("Generate tree from position: " + GraphSettings.Instance.StartPosition);
            }
            else
            {
                branches = new List<Branch>();
                Debug.Log("Generating new tree from position: " + GraphSettings.Instance.StartPosition);
            }
            branches.Add(new Branch(new Internode(GraphSettings.Instance.StartPosition, GraphSettings.Instance.StartDirection, GraphSettings.Instance.StartThickness), 0));
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

