using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Gen
{
    [ExecuteInEditMode]
    public class LeafModel
    {
        public float StemRelativeThickness { get; private set; }

        public AnimationCurve ContourCurve { get; private set; }

        public Color Color1 { get; private set; }
        public Color Color2 { get; private set; }
        public Color Color3 { get; private set; }

        public float LeafSizeX { get; private set; }

        public float LeafSizeY { get; private set; }

        public int ResolutionX { get; private set; }

        public int ResolutionY { get; private set; }

        public float TwigSpawnChance { get; private set; }

        public float TwigRandomGrowthConeAngle { get; private set; }

        public float ThicknessToTwigLength { get; private set; }

        public float LeafStepsRelative { get; private set; }


        private static LeafModel _instance;

        public static LeafModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LeafModel();
                }
                return _instance;
            }
        }

        public void Initialize(LeafSettings settings)
        {
            StemRelativeThickness = settings.StemRelativeThickness;
            ContourCurve = settings.ContourCurve;
            LeafSizeX = settings.LeafSizeX;
            LeafSizeY = (settings.ResolutionY*settings.LeafSizeX)/settings.ResolutionX;
            ResolutionX = settings.ResolutionX;
            ResolutionY = settings.ResolutionY;
            TwigSpawnChance = settings.TwigSpawnChance;
            TwigRandomGrowthConeAngle = settings.TwigRandomGrowthConeAngle;
            ThicknessToTwigLength = settings.ThicknessToTwigLength;
            LeafStepsRelative = settings.LeafStepsRelative;
        }
    }
}

