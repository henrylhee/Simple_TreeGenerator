using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Gen
{
    [CreateAssetMenu(fileName = "GraphModel", menuName = "ScriptableObjects/GraphModel")]
    public class GraphModel : ScriptableObject
    {
        //------------Settings--------------
        [SerializeField]
        private float phyllotaxis = 120;
        public float Phyllotaxis { get => phyllotaxis; }

        [SerializeField]
        private float branchingAngle = 80;
        public float BranchingAngle { get => branchingAngle; }

        [SerializeField]
        private float startThickness = 1;
        public float StartThickness { get => startThickness; }

        [SerializeField]
        private Vector3 startPosition = new Vector3(0, 0, 0);
        public Vector3 StartPosition { get => startPosition; }

        [SerializeField]
        private Vector3 startDirection = new Vector3(0, 1, 0);
        public Vector3 StartDirection { get => startDirection; }

        [SerializeField]
        private float thicknessSplit = 0.7f;
        public float ThicknessSplit { get => thicknessSplit; }

        [SerializeField]
        private float thicknessToSegmentLength = 0.05f;
        public float ThicknessToSegmentLength { get => thicknessToSegmentLength; }

        [SerializeField]
        private float terminalPerceptionAngle;
        public float TerminalPerceptionAngle { get => terminalPerceptionAngle; }

        [SerializeField]
        private float randomGrowthConeAngle = 15;
        public float RandomGrowthConeAngle { get => randomGrowthConeAngle; }

        [SerializeField]
        private float splitChance = 0.15f;
        public float SplitChance { get => splitChance; }

        [SerializeField]
        private float splitChanceIncreasePerSegment = 0.01f;
        public float SplitChanceIncreasePerSegment { get => splitChanceIncreasePerSegment; }

        [SerializeField]
        private float lastSegmentsWithoutSplitChance = 2;
        public float LastSegmentsWithoutSplitChance { get => lastSegmentsWithoutSplitChance; }

        [SerializeField]
        private float firstSegmentsWithoutSplitChance = 5;
        public float FirstSegmentsWithoutSplitChance { get => firstSegmentsWithoutSplitChance; }

        [SerializeField]
        private float maxLength = 10;
        public float MaxLength { get => maxLength; }



        //percentage based
        [Header("Percentage based values")]
        [SerializeField]
        private float noSplitStart = 0.03f;
        public float NoSplitStart { get => noSplitStart; }

        [SerializeField]
        private float noSplitEnd = 0.03f;
        public float NoSplitEnd { get => noSplitEnd; }

        [SerializeField]
        private float firstSplit = 0.2f;
        public float FirstSplit { get => firstSplit; }

        [SerializeField]
        private float minThicknessRelative = 0.04f;
        public float MinThicknessRelative { get => minThicknessRelative; }

        //Gizmo settings
        //[SerializeField]
        //private float s = 0.2f;
        //public float S { get => s; }


        //Mesh settings
        [Header("Mesh settings")]
        [SerializeField, Range(0f, 1f)]
        private float deltaT = 0.1f;
        public float DeltaT { get => deltaT; }

        //[SerializeField, Range(0f, 1f)]
        private float curvatureFactor = 0.2f;
        public float CurvatureFactor { get => curvatureFactor; }

        [SerializeField, Range(0f, 30f),Tooltip("Angle of the difference in direction of the longitudinal growing mesh at whom a new segment is created.")]
        private float meshDetailLong = 5f;
        public float MeshDetailLong { get => meshDetailLong; }

        [SerializeField, Range(0f, 30f),Tooltip("Amount of vertices in a mesh plane at maximum thickness.")]
        private int meshDetailLat = 30;
        public int MeshDetailLat { get => meshDetailLat; }
    }
}


