using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gen
{
    public class MeshSceleton
    {
        public List<BranchSceleton> sceleton { get; private set; }

        public void Generate(List<Branch> branches)
        {
            sceleton = new List<BranchSceleton>();
            foreach (Branch branch in branches)
            {
                BranchSceleton branchSceleton = new BranchSceleton();
                branchSceleton.Generate(branch);
                sceleton.Add(branchSceleton);
            }
        }
    }
}