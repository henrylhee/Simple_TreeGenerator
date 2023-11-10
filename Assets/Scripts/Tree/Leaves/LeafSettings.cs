using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Gen
{
    [CreateAssetMenu(fileName = "LeafSettings01", menuName = "ScriptableObjects/TreeGen/LeafSettings")]
    public class LeafSettings : ScriptableObject
    {
        [HideInInspector]
        public UnityEvent OnSettingsChanged;

        //------------Settings--------------
        [Header("Leaf")]
        [SerializeField, Range(0f, 0.2f)]
        float stemRelativeThickness;
        public float StemRelativeThickness { get =>  stemRelativeThickness;}
        [SerializeField]
        AnimationCurve contourCurve;
        public AnimationCurve ContourCurve { get => contourCurve;}
        [SerializeField, Range(0, 10.2f)]
        float leafSizeX;
        public float LeafSizeX { get => leafSizeX;}
        [SerializeField]
        int resolutionX;
        public int ResolutionX { get => resolutionX;}
        [SerializeField]
        int resolutionY;
        public int ResolutionY { get => resolutionY;}
        [SerializeField,Range(0f,1f)]
        float twigSpawnChance;
        public float TwigSpawnChance { get => twigSpawnChance;}
        [SerializeField, Range(0f, 30f)]
        float twigRandomGrowthConeAngle;
        public float TwigRandomGrowthConeAngle { get => twigRandomGrowthConeAngle; }
        [SerializeField, Range(0f, 50f)]
        float thicknessToTwigLength;
        public float ThicknessToTwigLength { get => thicknessToTwigLength; }
        [SerializeField, Range(0f, 1f)]
        float leafStepsRelative;
        public float LeafStepsRelative { get => leafStepsRelative; }

        private void OnValidate()
        {
            OnSettingsChanged.Invoke();
        }
    }
}


