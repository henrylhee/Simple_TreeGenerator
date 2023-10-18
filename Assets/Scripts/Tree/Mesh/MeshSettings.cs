using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MeshSettings : MonoBehaviour
{
    //------------Settings------------
    [SerializeField, Range(0.8f, 1f)]
    private float lateralDensity = 0.9f;
    public float LateralDensity { get => LateralDensity; }

    


    //percentage based
    [Header("Percentage based values")]
    [SerializeField]
    private float noSplitStart = 0.03f;

   
    public float MinThicknessAbsolute { get { return 1f; } }



    private static MeshSettings _instance;

    public static MeshSettings Instance { get { return _instance; } }


    public MeshSettings()
    {
        _instance = this;
    }
}
