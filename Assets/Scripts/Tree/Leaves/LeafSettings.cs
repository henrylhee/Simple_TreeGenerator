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
        [SerializeField]
        Color color;
        public Color Color { get => color; }
        [SerializeField, Range(0, 10.2f)]
        float leafSizeX;
        public float LeafSizeX { get => leafSizeX;}
        [SerializeField]
        int resolutionX;
        public int ResolutionX { get => resolutionX;}
        [SerializeField]
        int resolutionY;
        public int ResolutionY { get => resolutionY;}

        private void OnValidate()
        {
            OnSettingsChanged.Invoke();
        }
    }
}


