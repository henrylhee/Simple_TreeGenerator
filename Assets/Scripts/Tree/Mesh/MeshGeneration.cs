using System.Collections.Generic;
using UnityEngine;


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
            Debug.Log("start polygons: " + GetPolygonCount(meshSceleton.sceleton[0].knots[0].thickness));

            //foreach (BranchSceleton branchSceleton in meshSceleton.sceleton)
            foreach (BranchSceleton branchSceleton in meshSceleton.sceleton)
            {
                List<Vector3> vertices = new List<Vector3>();
                List<int> triangles = new List<int>();
                List<Vector2> uvs = new List<Vector2>();

                Vector3 currentPosition = branchSceleton.knots[0].position;
                int knotCount = 0;
                int vertexCount = 0;

                
                int polygonCount = GetPolygonCount(branchSceleton.knots[0].thickness);
                int polygonCountOld;
                float thickness;
                float length = 0;
 

                // ###Generate new mesh Knot###
                foreach (Knot knot in branchSceleton.knots)
                {                  
                    thickness = knot.thickness;
                    polygonCountOld = polygonCount;
                    polygonCount = GetPolygonCount(knot.thickness);

                    float angleStep = PI2 / polygonCount;
                    float angleStepOld = PI2 / polygonCountOld;
                    Quaternion rotation = Quaternion.FromToRotation(Vector3.up, knot.direction);

                    currentPosition = knot.position;

                    // generate in graph
                    if(knotCount > 0)
                    {
                        length += Vector3.Distance(currentPosition, branchSceleton.knots[knotCount-1].position);
                    }


                    for (int j = 0; j < polygonCount; j++)
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
                        if(polygonCount == polygonCountOld)
                        {
                            for (int j = 0; j < polygonCount; j++)
                            {
                                triangles.Add(vertexCount - polygonCountOld + j);
                                triangles.Add(vertexCount + j);
                                triangles.Add(vertexCount - polygonCountOld + (j + 1) % polygonCount);
                                triangles.Add(vertexCount - polygonCountOld + (j + 1) % polygonCount);
                                triangles.Add(vertexCount + j);
                                triangles.Add(vertexCount + (j + 1) % polygonCount);
                            }
                        }
                        // Generate Triangles; generate helper knot - Polygon decrease
                        else
                        {
                            int polygonOldIndex = 1;
                            for (int polygonIndex = 0; polygonIndex < polygonCount - 1; polygonIndex++)
                            {
                                if (polygonIndex * angleStep < polygonOldIndex * angleStepOld)
                                {
                                    triangles.Add(vertexCount + polygonIndex);
                                    triangles.Add(vertexCount - polygonCountOld + polygonOldIndex);
                                    triangles.Add(vertexCount - polygonCountOld + polygonOldIndex - 1);

                                    triangles.Add(vertexCount + polygonIndex);
                                    triangles.Add(vertexCount + polygonIndex + 1);
                                    triangles.Add(vertexCount - polygonCountOld + polygonOldIndex);
                                }
                                else
                                {
                                    polygonOldIndex++;

                                    triangles.Add(vertexCount + polygonIndex);
                                    triangles.Add(vertexCount - polygonCountOld + polygonOldIndex - 1);
                                    triangles.Add(vertexCount - polygonCountOld + polygonOldIndex - 2);

                                    triangles.Add(vertexCount + polygonIndex);
                                    triangles.Add(vertexCount - polygonCountOld + polygonOldIndex);
                                    triangles.Add(vertexCount - polygonCountOld + polygonOldIndex - 1);

                                    triangles.Add(vertexCount + polygonIndex);
                                    triangles.Add(vertexCount + polygonIndex + 1);
                                    triangles.Add(vertexCount - polygonCountOld + polygonOldIndex);

                                }
                                polygonOldIndex++;
                            }

                            triangles.Add(vertexCount + polygonCount - 1);
                            triangles.Add(vertexCount - 1);
                            triangles.Add(vertexCount - 2);

                            triangles.Add(vertexCount + polygonCount - 1);
                            triangles.Add(vertexCount - polygonCountOld);
                            triangles.Add(vertexCount - 1);

                            triangles.Add(vertexCount + polygonCount - 1);
                            triangles.Add(vertexCount);
                            triangles.Add(vertexCount - polygonCountOld);
                        }
                    }
                    vertexCount += polygonCount;
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

        private int GetPolygonCount(float thickness)
        {
            return Mathf.Max(3, Mathf.CeilToInt((thickness * meshDetailLat) / GraphModel.Instance.StartThickness));
        }
    }
}