using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gen
{
    public class BranchSceleton
    {
        public List<Knot> knots { get; private set; }

        public float length { get; private set; } = 0;

        int currentInternode;
        Vector3 currentDirection;
        float currentCurvature;


        public BranchSceleton()
        {
            knots = new List<Knot>();
            currentInternode = 0;
        }


        public void Generate(Branch branch)
        {
            List<Internode> internodes = branch.internodes;
            knots.Add(new Knot(internodes[0].position, internodes[0].direction, internodes[0].thickness, 0));

            currentDirection = internodes[currentInternode].direction;

            foreach (Internode internode in internodes)
            {
                currentCurvature = Vector3.Angle(currentDirection, internode.direction);

                if (currentCurvature > GraphModel.Instance.MeshDetailLong)
                {
                    float segmentLength = (knots.LastOrDefault().position - internode.position).magnitude;
                    length += segmentLength;

                    currentDirection = internode.direction;
                    knots.Add(new Knot(internode.position, internode.direction, internode.thickness, length));
                }
            }
        }
    }
}