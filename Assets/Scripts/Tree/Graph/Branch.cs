using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Gen
{
    public class Branch
    {
        public UnityEvent<Branch> NewBranch = new UnityEvent<Branch>();

        public List<Internode> internodes {get; private set;}

        public List<TwigSpawnData> twigSpawnData { get; private set;}

        public float phyllotaxisAngle { get; private set;}


        private float initLength;
        private float currentLength;
        private float endLength;
        private int firstSplitInternode;
        private bool hasSplit;

        private float currentThickness;

        private Internode currentInternode;

        public Branch(Internode initInternode, float initLength, bool hasSplit)
        {
            internodes = new List<Internode>();
            twigSpawnData = new List<TwigSpawnData>();
            currentThickness = initInternode.thickness;

            phyllotaxisAngle = 0;
            this.initLength = initLength;
            currentLength = initLength;
            endLength = 0;
            this.hasSplit = hasSplit;
            if (!hasSplit) 
            {
                firstSplitInternode = Mathf.RoundToInt(GraphModel.Instance.FirstSplitLengthAbsolute / 
                                      (initInternode.thickness * GraphModel.Instance.ThicknessToSegmentLength));
            }
            currentInternode = initInternode;
            internodes.Add(currentInternode);
        }

        public void Generate()
        {
            //Debug.Log("Generate new branch from position: " + startBud.position);
            GenerateInternode();
        }

        public void GenerateInternode()
        {
            float segmentLength = currentThickness * GraphModel.Instance.ThicknessToSegmentLength;
            currentLength += segmentLength;
            currentInternode = currentInternode.FindNext(segmentLength, currentThickness);
            internodes.Add(currentInternode);
            bool _hastSplit = hasSplit;

            if (currentThickness <= GraphModel.Instance.LeafSpawnThicknessLimit)
            {
                twigSpawnData.Add(new TwigSpawnData(currentInternode.position, currentInternode.direction, currentThickness));
            }

            if (hasSplit == false)
            {
                if (firstSplitInternode == internodes.Count-1)
                {
                    Debug.Log("First split internode: " + firstSplitInternode);
                    BranchSplit();
                    hasSplit = true;
                }
            }
            else if (currentLength >= GraphModel.Instance.MaxLength || currentThickness <= GraphModel.Instance.MinThicknessAbsolute)
            {
                endLength += segmentLength;

                if(endLength > GraphModel.Instance.NoSplitEndLengthAbsolute)
                {
                    return;
                }
            }
            else if (internodes.Count > GraphModel.Instance.NoSplitBranchSegments)
            {
                if (Random.Range(0f, 1f) < GraphModel.Instance.SplitChance)
                {
                    BranchSplit();
                }
            }
            
            GenerateInternode();
        }

        private void BranchSplit()
        {
            float splitInitThickness = currentThickness * Mathf.Sqrt((1 - GraphModel.Instance.ThicknessSplit));
            NewBranch?.Invoke(new Branch(currentInternode.GetSplitInternode(phyllotaxisAngle, splitInitThickness), currentLength, true));
            phyllotaxisAngle += GraphModel.Instance.Phyllotaxis;
            currentThickness *= Mathf.Sqrt((GraphModel.Instance.ThicknessSplit));
        }
    }
}

