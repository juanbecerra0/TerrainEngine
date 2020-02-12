﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    int dimensions;

    public void GenerateMesh(int dimensions, Texture2D heightmap) {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape(dimensions, heightmap);
        UpdateMesh();
    }

    void CreateShape(int dimensions, Texture2D heightmap)
    {
        this.dimensions = dimensions + 1;
        vertices = new Vector3[(dimensions + 1) * (dimensions + 1)];
        float heightmapSize = (float) heightmap.width;

        for (int i = 0, z = 0; z <= dimensions; z++)
        {
            for (int x = 0; x <= dimensions; x++, i++)
            {
                Color pixelColor = heightmap.GetPixel(Mathf.FloorToInt(((float)x / (float)dimensions) * heightmapSize), Mathf.FloorToInt(((float)z / (float)dimensions) * heightmapSize));
                float y = ((pixelColor.r + pixelColor.g + pixelColor.b) / 3) * 10;//Mathf.PerlinNoise(x * 0.3f, z * 0.3f) * 3f;
                vertices[i] = new Vector3(x, y, z);
            }
        }

        triangles = new int[dimensions * dimensions * 6];
        int vert = 0, tris = 0;

        for (int z = 0; z < dimensions; z++)
        {
            for (int x = 0; x < dimensions; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + dimensions + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + dimensions + 1;
                triangles[tris + 5] = vert + dimensions + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }
    }

    /**
     * Returns the corresponding index of the 1 dimensional vertex array 
     * given an x and y coordinate
     */
    int GetIndex(int x, int y)
    {
        if (String.IsNullOrEmpty(dimensions.ToString()) || x > dimensions - 1 || x < 0 || y > dimensions - 1 || y < 0)
            return -1;
        else
            return ((dimensions * dimensions) - dimensions) + x - (y * dimensions);
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }
}