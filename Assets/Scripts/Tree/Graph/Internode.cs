using Gen;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


namespace Gen
{
    public class Internode
    {
        public Vector3 position { get; private set; }
        public Vector3 direction { get; private set; }
        public float thickness { get; private set; }

        public Internode(Vector3 position, Vector3 initGrowDir, float thickness)
        {
            this.position = position;
            direction = Helper.getRandomVectorInCone(GraphSettings.Instance.RandomGrowthConeAngle, initGrowDir);
            this.thickness = thickness > GraphSettings.Instance.MinThicknessAbsolute ? thickness : GraphSettings.Instance.MinThicknessAbsolute;
            //this.thickness = thickness;
        }

        public Internode FindNext(float length, float thickness)
        {
            return new Internode(position + (direction * length), Helper.getRandomVectorInCone(GraphSettings.Instance.RandomGrowthConeAngle, direction), thickness);
        }

        public Internode GetSplitInternode(float phyllotaxis, float thickness)
        {
            return new Internode(position, GetSplitGrowDir(phyllotaxis), thickness);
        }

        public Vector3 GetSplitGrowDir(float phyllo)
        {
            Vector3 rightRotated = Quaternion.AngleAxis(Vector3.Angle(Vector3.up, direction), Vector3.Cross(Vector3.up, direction)) * Vector3.right;
            Vector3 resultPhyllo = Quaternion.AngleAxis(phyllo, direction) * rightRotated;
            return Quaternion.AngleAxis(GraphSettings.Instance.BranchingAngle, Vector3.Cross(direction, resultPhyllo)) * direction;
        }
    }
}

