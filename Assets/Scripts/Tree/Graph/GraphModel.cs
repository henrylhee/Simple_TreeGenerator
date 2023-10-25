using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Gen
{
    [ExecuteInEditMode]
    public class GraphModel
    {
        //------------Settings--------------
        public float Phyllotaxis { get; private set; }

        public float BranchingAngle { get; private set; }

        public float StartThickness { get; private set; }

        public Vector3 StartPosition { get; private set; }

        public Vector3 StartDirection { get; private set; }

        public float ThicknessSplit { get; private set; }

        public float ThicknessToSegmentLength { get; private set; }

        public float TerminalPerceptionAngle { get; private set; }

        public float RandomGrowthConeAngle { get; private set; }

        public float SplitChance { get; private set; }

        public float SplitChanceIncreasePerSegment { get; private set; }

        public float LastSegmentsWithoutSplitChance { get; private set; }

        public float FirstSegmentsWithoutSplitChance { get; private set; }


        public float MaxLength { get; private set; }


        public int NoSplitBranchSegments { get; private set; }
        public float NoSplitEndLengthAbsolute { get; private set; }
        public float FirstSplitLengthAbsolute { get; private set; }
        public float MinThicknessAbsolute { get; private set; }


        //Mesh settings
        public float DeltaT { get; private set; }
        public float CurvatureFactor { get; private set; }
        public float MeshDetailLong { get; private set; }
        public int MeshDetailLat { get; private set; }


        private static GraphModel _instance;

        public static GraphModel Instance
        { 
            get 
            { 
                if (_instance == null)
                {
                    _instance = new GraphModel();
                }
                return _instance; 
            } 
        }


        //public GraphModel()
        //{
        //    if (_instance != null && _instance != this)
        //    {
        //        _instance = null;
        //    }
        //    else
        //    {
        //        _instance = this;
        //    }
        //}

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

