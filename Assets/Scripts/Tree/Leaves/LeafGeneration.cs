using Gen;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class LeafGeneration
{
    GameObject leafPreview;
    float stemRelativeThickness;
    AnimationCurve contourCurve;
    float leafSizeX;
    float leafSizeY;
    int resolutionX;
    int resolutionY;
    int mimMapLevels;

    Texture2D texture;
    Material leafUIMaterial;
    Material leafMaterial;
    Mesh leafMesh;

    List<LeafSpawnData> leafSpawns;


    private void Initialize()
    {
        InitializeSettings();

        texture = new Texture2D(resolutionX, resolutionY);
    }

    private void InitializeSettings()
    {
        stemRelativeThickness = LeafModel.Instance.StemRelativeThickness;
        contourCurve = LeafModel.Instance.ContourCurve;
        leafSizeX = LeafModel.Instance.LeafSizeX;
        leafSizeY = LeafModel.Instance.LeafSizeY;
        resolutionX = LeafModel.Instance.ResolutionX;
        resolutionY = LeafModel.Instance.ResolutionY;
    }

    public GameObject GenerateLeaf(GameObject leafPreView)
    {
        Initialize();

        this.leafPreview = leafPreView;

        GenerateTexture();
        GenerateBaseMesh();

        leafPreview.GetComponent<RawImage>().texture = texture;

        return leafPreview;
    }

    private void GenerateTexture()
    {
        // faster with setpixeldata!!
        Color trueC = new Color(1,0,0,1);
        Color falseC = new Color(0,1,0,1);
        int halfHeight = texture.height / 2;
        int leafPixelCount = 0;
        int noLeafPixelCount = 0;
        for (int i = 0; i < halfHeight; i++) 
        {
            for (int j = 0; j < texture.width; j++)
            {
                if (1 - contourCurve.Evaluate((float)j/texture.width) < (float)i / halfHeight)
                {
                    texture.SetPixel(j, i, trueC);
                    leafPixelCount++;
                }
                else
                {
                    texture.SetPixel(j, i, falseC);
                    noLeafPixelCount++;
                }
            }
        }
        for (int i = 0; i < halfHeight; i++)
        {
            for (int j = 0; j < texture.width; j++)
            {
                if (contourCurve.Evaluate((float)j / texture.width) > (float)i / halfHeight)
                {
                    texture.SetPixel(j, i + halfHeight, trueC);
                    leafPixelCount++;
                }
                else
                {
                    texture.SetPixel(j, i + halfHeight, falseC);
                    noLeafPixelCount++;
                }
            }
        }

        texture.Apply();

        leafPreview.GetComponent<RawImage>().material.SetTexture("_DataTexture", texture);
    }

    private void GenerateBaseMesh()
    {
        leafMesh = new Mesh();

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        vertices.Add(new Vector3(0, leafSizeY / 2, 0));
        vertices.Add(new Vector3(leafSizeX, leafSizeY / 2, 0));
        vertices.Add(new Vector3(leafSizeX, -leafSizeY / 2, 0));
        vertices.Add(new Vector3(0, -leafSizeY / 2, 0));
        uvs.Add(new Vector2(0,1));
        uvs.Add(new Vector2(1,1));
        uvs.Add(new Vector2(1,0));
        uvs.Add(new Vector2(0,0));

        triangles.Add(0);
        triangles.Add(1);
        triangles.Add(2);
        triangles.Add(0);
        triangles.Add(2);
        triangles.Add(3);

        leafMesh.SetVertices(vertices);
        leafMesh.SetTriangles(triangles, 0);
        leafMesh.SetUVs(0, uvs);

        leafMesh.RecalculateBounds();
        leafMesh.RecalculateNormals();
        leafMesh.Optimize();
    }

    public void SpawnLeaves(List<Branch> branches, Transform treeTransform, GameObject leaves)
    {
        LeafSpawner leafSpawner = new LeafSpawner();
        leafSpawns = leafSpawner.GenerateData(branches);

        GameObject t = new GameObject();
        CombineInstance[] combine = new CombineInstance[leafSpawns.Count];
        for(int i = 0; i < leafSpawns.Count; i++)
        {
            CombineInstance combineInstance = new CombineInstance();
            combineInstance.mesh = leafMesh;

            t.transform.position = leafSpawns[i].position;
            t.transform.rotation = leafSpawns[i].rotation;
            combineInstance.transform = t.transform.localToWorldMatrix;
            combine[i] = combineInstance;
        }
        GameObject.DestroyImmediate(t);

        //leaves.GetComponent<MeshRenderer>().sharedMaterial = leafMat;
        leaves.GetComponent<MeshRenderer>().sharedMaterial.SetTexture("_DataTexture", texture);

        leaves.GetComponent<MeshFilter>().sharedMesh = new Mesh();
        leaves.GetComponent<MeshFilter>().sharedMesh.CombineMeshes(combine,true,true);
        leaves.GetComponent<MeshFilter>().sharedMesh.RecalculateNormals();
        leaves.GetComponent<MeshFilter>().sharedMesh.Optimize();

        GameObject.Instantiate(leaves, treeTransform, true);
    }
}
