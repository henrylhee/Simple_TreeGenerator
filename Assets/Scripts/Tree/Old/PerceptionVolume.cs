using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Gen
{
    public struct PerceptionVolume
    {
        public Vector3 position;
        public Vector3 direction;
        public float angle;

        public void SetAngle(float val)
        {
            angle = val;
        }
    }
}

