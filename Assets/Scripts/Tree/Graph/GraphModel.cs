using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Gen
{
    [ExecuteInEditMode]
    public class GraphModel : MonoBehaviour
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


        public int NoSplitBranchSegments;
        public float NoSplitEndLengthAbsolute;
        public float FirstSplitLengthAbsolute;
        public float MinThicknessAbsolute;


        //Mesh settings
        public float DeltaT;
        public float CurvatureFactor;
        public float MeshDetailLong;
        public int MeshDetailLat;


        private static GraphModel _instance;

        public static GraphModel Instance { get { return _instance; } }


        public GraphModel()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(_instance.gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        public void Initialize(GraphSettings settings)
        {
            Phyllotaxis = settings.Phyllotaxis;

            BranchingAngle = settings.BranchingAngle;

            StartThickness = settings.StartThickness;

            StartThickness = settings.StartThickness;

            StartDirection = settings.StartDirection;

            ThicknessSplit = settings.ThicknessSplit;

            ThicknessToSegmentLength = settings.ThicknessToSegmentLength;

            TerminalPerceptionAngle = settings.TerminalPerceptionAngle;

            RandomGrowthConeAngle = settings.RandomGrowthConeAngle;

            SplitChance = settings.SplitChance;

            SplitChanceIncreasePerSegment = settings.SplitChanceIncreasePerSegment;

            LastSegmentsWithoutSplitChance = settings.LastSegmentsWithoutSplitChance;

            FirstSegmentsWithoutSplitChance = settings.FirstSegmentsWithoutSplitChance;

            MaxLength = settings.MaxLength;

            NoSplitBranchSegments = settings.NoSplitBranchSegments;
            NoSplitEndLengthAbsolute = settings.MaxLength * settings.NoSplitEndLengthRelative;
            FirstSplitLengthAbsolute = settings.MaxLength * settings.FirstSplitLengthRelative;
            MinThicknessAbsolute = settings.StartThickness * settings.MinThicknessRelative;

            //Mesh settings
            DeltaT = settings.DeltaT;
            CurvatureFactor = settings.CurvatureFactor;
            MeshDetailLong = settings.MeshDetailLong;
            MeshDetailLat = settings.MeshDetailLat;
        }
    }
}

