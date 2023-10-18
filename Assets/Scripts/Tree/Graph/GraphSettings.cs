using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gen
{
    public class GraphSettings
    {
        //------------Settings--------------
        public float Phyllotaxis;

        public float BranchingAngle;

        public float StartThickness;

        public Vector3 StartPosition;

        public Vector3 StartDirection;

        public float ThicknessSplit;

        public float ThicknessToSegmentLength;

        public float TerminalPerceptionAngle;

        public float RandomGrowthConeAngle;

        public float SplitChance;

        public float SplitChanceIncreasePerSegment;

        public float LastSegmentsWithoutSplitChance;

        public float FirstSegmentsWithoutSplitChance;


        public float MaxLength;


        public float NoSplitStartLength;
        public float NoSplitEndLength;
        public float FirstSplitLength;
        public float MinThicknessAbsolute;


        //Mesh settings
        public float DeltaT;
        public float CurvatureFactor;
        public float MeshDetailLong;
        public int MeshDetailLat;


        private static GraphSettings _instance;

        public static GraphSettings Instance { get { return _instance; } }


        public GraphSettings()
        {
            if (_instance != null && _instance != this)
            {
                return;
            }
            else
            {
                _instance = this;
            }
        }


        public void Initialize(GraphModel model)
        {
            Phyllotaxis = model.Phyllotaxis;

            BranchingAngle = model.BranchingAngle;

            StartThickness = model.StartThickness;

            StartThickness = model.StartThickness;

            StartDirection = model.StartDirection;

            ThicknessSplit = model.ThicknessSplit;

            ThicknessToSegmentLength = model.ThicknessToSegmentLength;

            TerminalPerceptionAngle = model.TerminalPerceptionAngle;

            RandomGrowthConeAngle = model.RandomGrowthConeAngle;

            SplitChance = model.SplitChance;

            SplitChanceIncreasePerSegment = model.SplitChanceIncreasePerSegment;

            LastSegmentsWithoutSplitChance = model.LastSegmentsWithoutSplitChance;

            FirstSegmentsWithoutSplitChance = model.FirstSegmentsWithoutSplitChance;

            MaxLength = model.MaxLength;


            NoSplitStartLength = model.MaxLength * model.NoSplitStart;
            NoSplitEndLength = model.MaxLength * model.NoSplitEnd;
            FirstSplitLength = model.MaxLength * model.FirstSplit;
            MinThicknessAbsolute = model.StartThickness * model.MinThicknessRelative;

            //Mesh settings
            DeltaT = model.DeltaT;
            CurvatureFactor = model.CurvatureFactor;
            MeshDetailLong = model.MeshDetailLong;
            MeshDetailLat = model.MeshDetailLat;
        }
    }
}

