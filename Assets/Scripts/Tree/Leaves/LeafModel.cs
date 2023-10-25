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

        public Color Color { get; private set; }

        public float LeafSizeX { get; private set; }

        public float LeafSizeY { get; private set; }

        public int ResolutionX { get; private set; }

        public int ResolutionY { get; private set; }


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
            Color = settings.Color;
            LeafSizeX = settings.LeafSizeX;
            LeafSizeY = (settings.ResolutionY*settings.LeafSizeX)/settings.ResolutionX;
            ResolutionX = settings.ResolutionX;
            ResolutionY = settings.ResolutionY;
        }
    }
}

