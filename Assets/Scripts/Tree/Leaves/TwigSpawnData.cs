using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TwigSpawnData
{
    public Vector3 position { get; private set; }
    public Vector3 direction { get; private set; }
    public float thickness { get; private set; }

    public TwigSpawnData(Vector3 position, Vector3 direction, float thickness)
    {
        this.position = position;
        this.direction = direction;
        this.thickness = thickness;
    }
}
