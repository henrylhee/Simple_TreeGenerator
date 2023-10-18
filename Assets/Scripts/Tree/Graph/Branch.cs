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
        public float phyllotaxisAngle { get; private set;}


        private float initLength;
        private float currentLength;
        private float endLength;

        private float currentThickness;

        private Internode currentInternode;

        public Branch(Internode initInternode, float initLength)
        {
            internodes = new List<Internode>();
            currentThickness = initInternode.thickness;

            phyllotaxisAngle = 0;
            this.initLength = initLength;
            currentLength = initLength;
            endLength = 0;

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
            float segmentLength = currentThickness * GraphSettings.Instance.ThicknessToSegmentLength;
            currentLength += segmentLength;
            currentInternode = currentInternode.FindNext(segmentLength, currentThickness);
            internodes.Add(currentInternode);

            if (currentLength >= GraphSettings.Instance.MaxLength || currentThickness <= GraphSettings.Instance.MinThicknessAbsolute)
            {
                endLength += segmentLength;

                if(endLength > GraphSettings.Instance.NoSplitEndLength)
                {
                    return;
                }
            }
            else if (Random.Range(0f, 1f) < GraphSettings.Instance.SplitChance && currentLength - initLength > GraphSettings.Instance.NoSplitStartLength)
            {
                float splitInitThickness = currentThickness * Mathf.Sqrt((1 - GraphSettings.Instance.ThicknessSplit));
                NewBranch?.Invoke(new Branch(currentInternode.GetSplitInternode(phyllotaxisAngle, splitInitThickness), currentLength));
                phyllotaxisAngle += GraphSettings.Instance.Phyllotaxis;
                currentThickness *= Mathf.Sqrt((GraphSettings.Instance.ThicknessSplit));
            }

            GenerateInternode();
        }
    }
}

