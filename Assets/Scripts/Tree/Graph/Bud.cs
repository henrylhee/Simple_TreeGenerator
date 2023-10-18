using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Gen
{
    public class Bud
    {
        public Vector3 position;
        public Vector3 initGrowDir;
        public Vector3 finalGrowDir;
        

        public Bud(Vector3 pos, Vector3 dir)
        {
            position = pos;
            initGrowDir = dir;
            finalGrowDir = Helper.getRandomVectorInCone(GraphSettings.Instance.RandomGrowthConeAngle, initGrowDir);
        }


        public Bud GetSplitBud(float length, float phylloAngle)
        {
            return new Bud(position + (finalGrowDir * length), GetSplitGrowDir(initGrowDir, phylloAngle));
        }

        public Vector3 GetSplitGrowDir(Vector3 startDir, float phyllo)
        {
            Vector3 rightRotated = Quaternion.AngleAxis(Vector3.Angle(Vector3.up, startDir), Vector3.Cross(Vector3.up, startDir)) * Vector3.right;
            Vector3 resultPhyllo = Quaternion.AngleAxis(phyllo, startDir) * rightRotated;
            return Quaternion.AngleAxis(GraphSettings.Instance.BranchingAngle, Vector3.Cross(startDir, resultPhyllo)) * startDir;
        }
    }
}


