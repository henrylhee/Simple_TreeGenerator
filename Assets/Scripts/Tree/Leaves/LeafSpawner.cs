using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Gen
{
    public class LeafSpawner
    {
        List<Branch> branches;
        List<LeafSpawnData> leafSpawnData;
        float thicknessLimitAbsolute;
        float spawnChance;
        float randomConeAngle;
        float thicknessToTwigLength;
        float leafStepRelative;


        public List<LeafSpawnData> GenerateData(List<Branch> branches)
        {
            thicknessLimitAbsolute = GraphModel.Instance.LeafSpawnThicknessLimit;
            spawnChance = LeafModel.Instance.TwigSpawnChance;
            randomConeAngle = LeafModel.Instance.TwigRandomGrowthConeAngle;
            leafStepRelative = LeafModel.Instance.LeafStepsRelative;
            thicknessToTwigLength = LeafModel.Instance.ThicknessToTwigLength;

            this.branches = branches;
            leafSpawnData = new List<LeafSpawnData>();

            GenerateLeafData();

            return leafSpawnData;
        }

        private void GenerateLeafData()
        {
            Vector3 twigPosition;
            Vector3 twigDirection;
            Vector3 helper;
            float leafStep;
            float twigLength;
            float currentTwigLength;

            foreach (Branch branch in branches)
            {
                foreach (TwigSpawnData twigSpawnData in branch.twigSpawnData)
                {
                    if(Random.Range(0f,1f) < spawnChance)
                    {
                        if (twigSpawnData.direction != new Vector3(-1, 1, 0))
                        {
                            helper = new Vector3(twigSpawnData.direction.z, twigSpawnData.direction.z, -twigSpawnData.direction.x - twigSpawnData.direction.y);
                        }
                        else
                        {
                            helper = new Vector3(twigSpawnData.direction.y - twigSpawnData.direction.z, twigSpawnData.direction.x, twigSpawnData.direction.x);
                        }

                        twigDirection = (Quaternion.AngleAxis(Random.Range(0f, 360f), twigSpawnData.direction) * helper).normalized;
                        twigPosition = twigSpawnData.position + twigDirection * twigSpawnData.thickness;
                        //still normalized?
                        twigDirection = Helper.getRandomVectorInCone(randomConeAngle, twigDirection);

                        leafStep = twigSpawnData.thickness * thicknessToTwigLength;
                        twigLength = twigSpawnData.thickness * thicknessToTwigLength;
                        currentTwigLength = 0;
                        while (currentTwigLength <= twigLength)
                        {
                            currentTwigLength += leafStep;

                            leafSpawnData.Add(new LeafSpawnData(twigPosition + twigDirection * currentTwigLength, Random.rotation));
                        }
                    }
                }
            }
        }
    }
}

