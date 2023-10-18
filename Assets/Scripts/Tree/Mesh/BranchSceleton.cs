using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gen
{
    public class BranchSceleton
    {
        public List<Knot> knots { get; private set; }

        int currentInternode;
        Vector3 currentDirection;
        float curvature;


        public BranchSceleton()
        {
            knots = new List<Knot>();
            currentInternode = 0;
        }


        public void Generate(Branch branch)
        {
            List<Internode> internodes = branch.internodes;
            knots.Add(new Knot(internodes[0].position, internodes[0].direction, internodes[0].thickness));

            currentDirection = internodes[currentInternode].direction;

            foreach (Internode internode in internodes)
            {
                curvature = Vector3.Angle(currentDirection, internode.direction);

                if (curvature > GraphSettings.Instance.MeshDetailLong)
                {
                    currentDirection = internode.direction;
                    knots.Add(new Knot(internode.position, internode.direction, internode.thickness));
                }
            }
        }
    }
}