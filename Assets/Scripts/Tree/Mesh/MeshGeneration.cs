﻿using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;


namespace Gen
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class MeshGeneration
    {
        readonly int meshDetailLat = GraphModel.Instance.MeshDetailLat;
        readonly float PI2 = Mathf.PI * 2f;


        public Mesh Generate(List<Branch> branches, Transform treeTransform)
        {
            MeshSceleton meshSceleton = new MeshSceleton();
            meshSceleton.Generate(branches);
            return GenerateMesh(meshSceleton, treeTransform);
        }

        private Mesh GenerateMesh(MeshSceleton meshSceleton, Transform treeTransform)
        {
            Mesh treeMesh = new Mesh();
            treeMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            CombineInstance[] combine = new CombineInstance[meshSceleton.sceleton.Count];

            int branchCount = 0;

            Debug.Log("start thickness: " + meshSceleton.sceleton[0].knots[0].thickness);
            Debug.Log("start polygons: " + GetVertexCount(meshSceleton.sceleton[0].knots[0].thickness));

            //foreach (BranchSceleton branchSceleton in meshSceleton.sceleton)
            foreach (BranchSceleton branchSceleton in meshSceleton.sceleton)
            {
                List<Vector3> vertices = new List<Vector3>();
                List<int> triangles = new List<int>();
                List<Vector2> uvs = new List<Vector2>();

                Vector3 currentPosition = branchSceleton.knots[0].position;
                int knotCount = 0;
                int vertexCount = 0;

                
                int vertexCountLevel = GetVertexCount(branchSceleton.knots[0].thickness);
                int vertexCountLastLevel;
                float thickness;
                float length = 0;
 

                // ###Generate new mesh Knot###
                foreach (Knot knot in branchSceleton.knots)
                {                  
                    thickness = knot.thickness;
                    vertexCountLastLevel = vertexCountLevel;
                    vertexCountLevel = GetVertexCount(knot.thickness);

                    float angleStep = PI2 / (vertexCountLevel - 1);
                    float angleStepOld = PI2 / (vertexCountLastLevel - 1);
                    Quaternion rotation = Quaternion.FromToRotation(Vector3.up, knot.direction);

                    currentPosition = knot.position;

                    // generate in graph
                    if(knotCount > 0)
                    {
                        length += Vector3.Distance(currentPosition, branchSceleton.knots[knotCount-1].position);
                    }

                    // <= because extra vertex for uv mapping
                    for (int j = 0; j < vertexCountLevel; j++)
                    {
                        Vector3 pos = new Vector3(Mathf.Cos(j * angleStep /*+ angleStart*/), 0f, Mathf.Sin(j * angleStep /*+ angleStart*/));
                        pos *= thickness;
                        pos = rotation * pos;
                        pos += currentPosition;
                        vertices.Add(pos);
                        uvs.Add(new Vector2((j * angleStep)/PI2 /*+ angleStart*/, knot.lengthFromStart/branchSceleton.length));
                    }

                    //#########Start vertex to triangle calculation
                    if (knotCount > 0)
                    {
                        // Generate Triangles - No polygon decrease
                        if(vertexCountLevel == vertexCountLastLevel)
                        {
                            for (int j = 0; j < vertexCountLevel - 2; j++)
                            {
                                triangles.Add(vertexCount - vertexCountLevel + j);
                                triangles.Add(vertexCount + j);
                                triangles.Add(vertexCount - vertexCountLevel + j + 1);
                                triangles.Add(vertexCount - vertexCountLevel + j + 1);
                                triangles.Add(vertexCount + j);
                                triangles.Add(vertexCount + j + 1);
                            }

                            //add extra quad for uv mapping
                            triangles.Add(vertexCount - 2);
                            triangles.Add(vertexCount + vertexCountLevel - 2);
                            triangles.Add(vertexCount - 1);
                            triangles.Add(vertexCount - 1);
                            triangles.Add(vertexCount + vertexCountLevel - 2);
                            triangles.Add(vertexCount + vertexCountLevel - 1);
                        }
                        // Generate Triangles; generate helper knot - Polygon decrease
                        else
                        {
                            int vertexOldIndex = 1;
                            for (int vertexIndex = 0; vertexIndex < vertexCountLevel - 2; vertexIndex++)
                            {
                                if (vertexIndex * angleStep < vertexOldIndex * angleStepOld)
                                {
                                    triangles.Add(vertexCount + vertexIndex);
                                    triangles.Add(vertexCount - vertexCountLastLevel + vertexOldIndex);
                                    triangles.Add(vertexCount - vertexCountLastLevel + vertexOldIndex - 1);

                                    triangles.Add(vertexCount + vertexIndex);
                                    triangles.Add(vertexCount + vertexIndex + 1);
                                    triangles.Add(vertexCount - vertexCountLastLevel + vertexOldIndex);
                                }
                                else
                                {
                                    Debug.Log("++++++ vertexCountLevel: "+ vertexCountLevel);
                                    Debug.Log("vertexCountLevelOld: " + vertexCountLastLevel);
                                    Debug.Log("angleStep: " + angleStep);
                                    Debug.Log("angleStepOld: " + angleStepOld);
                                    Debug.Log("vertexIndex: " + vertexIndex);
                                    vertexOldIndex++;

                                    triangles.Add(vertexCount + vertexIndex);
                                    triangles.Add(vertexCount - vertexCountLastLevel + vertexOldIndex - 1);
                                    triangles.Add(vertexCount - vertexCountLastLevel + vertexOldIndex - 2);

                                    triangles.Add(vertexCount + vertexIndex);
                                    triangles.Add(vertexCount - vertexCountLastLevel + vertexOldIndex);
                                    triangles.Add(vertexCount - vertexCountLastLevel + vertexOldIndex - 1);

                                    triangles.Add(vertexCount + vertexIndex);
                                    triangles.Add(vertexCount + vertexIndex + 1);
                                    triangles.Add(vertexCount - vertexCountLastLevel + vertexOldIndex);

                                }
                                vertexOldIndex++;
                            }

                            triangles.Add(vertexCount + vertexCountLevel - 2);
                            triangles.Add(vertexCount - 3);
                            triangles.Add(vertexCount - 2);

                            triangles.Add(vertexCount + vertexCountLevel - 2);
                            triangles.Add(vertexCount - 2);
                            triangles.Add(vertexCount - 1);

                            triangles.Add(vertexCount + vertexCountLevel - 1);
                            triangles.Add(vertexCount + vertexCountLevel - 2);
                            triangles.Add(vertexCount);
                        }
                    }
                    vertexCount += vertexCountLevel;
                    knotCount++;
                }

                Mesh subMesh = new Mesh();
                subMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

                subMesh.SetVertices(vertices);
                subMesh.SetUVs(0, uvs);
                subMesh.SetTriangles(triangles, 0);

                subMesh.RecalculateNormals();

                CombineInstance combineInstance = new CombineInstance();
                combineInstance.mesh = subMesh;
                combineInstance.transform = treeTransform.localToWorldMatrix;
                combine[branchCount] = combineInstance;

                branchCount++;
            }
            treeMesh.CombineMeshes(combine);
            treeMesh.RecalculateNormals();
            treeMesh.Optimize();
            return treeMesh;
        }

        private int GetVertexCount(float thickness)
        {
            int polygonCount = Mathf.Max(3, Mathf.CeilToInt((thickness * meshDetailLat) / GraphModel.Instance.StartThickness));
            return polygonCount + 1;
        }
    }
}