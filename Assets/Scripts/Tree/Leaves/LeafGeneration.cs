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
    Color color;
    float leafSizeX;
    float leafSizeY;
    int resolutionX;
    int resolutionY;

    Texture2D texture;
    Material leafUIMaterial;
    Material leafMaterial;
    GameObject leaf;
    Mesh mesh;


    private void Initialize()
    {
        InitializeSettings();

        texture = new Texture2D(resolutionX, resolutionY);

        leafUIMaterial = leafPreview.GetComponent<RawImage>().material;
        leafMaterial = leaf.GetComponent<MeshRenderer>().material;
    }

    private void InitializeSettings()
    {
        stemRelativeThickness = LeafModel.Instance.StemRelativeThickness;
        contourCurve = LeafModel.Instance.ContourCurve;
        color = LeafModel.Instance.Color;
        leafSizeX = LeafModel.Instance.LeafSizeX;
        leafSizeY = LeafModel.Instance.LeafSizeY;
        resolutionX = LeafModel.Instance.ResolutionX;
        resolutionY = LeafModel.Instance.ResolutionY;
    }

    public GameObject Generate(GameObject leafPreView, GameObject leaf)
    {
        this.leafPreview = leafPreView;
        this.leaf = leaf;
        Initialize();
        GenerateTexture();
        GenerateMesh();
        return leaf;
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
        Debug.Log(leafPixelCount);
        Debug.Log(noLeafPixelCount);
        texture.Apply();

        leafUIMaterial.SetTexture("_DataTexture", texture);
        leafUIMaterial.SetColor("_Color", color);

        leaf.GetComponent<MeshRenderer>().material.SetTexture("_DataTexture", texture);
        leaf.GetComponent<MeshRenderer>().material.SetColor("_Color", color);
    }

    private void GenerateMesh()
    {
        mesh = new Mesh();

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

        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.SetUVs(0, uvs);

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.Optimize();

        leaf.GetComponent<MeshFilter>().sharedMesh = mesh;
    }
}
