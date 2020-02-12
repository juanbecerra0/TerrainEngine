﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class MeshPlacer : MonoBehaviour
{
    public int vertexCount = 20;
    public int initialBlockRadius = 2;
    public int heightmapBaseN = 7;

    private int heightmapDimensions;
    Dictionary<Tuple<int, int>, float[,]> NoiseMap = new Dictionary<Tuple<int, int>, float[,]>();

    // Start is called before the first frame update
    void Start()
    {
        GameObject prefab = (GameObject) Resources.Load("Prefabs/MeshGenerator");

        // Error checking
        if (vertexCount < 2)
            vertexCount = 2;

        if (initialBlockRadius < 1)
            initialBlockRadius = 1;

        if (heightmapBaseN < 3)
            heightmapBaseN = 3;

        if (!prefab)
        {
            Debug.Log("Prefab could not be found");
            return;
        }

        // Calculate heightmap dimensions
        heightmapDimensions = (int) Mathf.Pow(2, heightmapBaseN) + 1;
        Debug.Log(heightmapDimensions);

        Quaternion orientation = Quaternion.identity;

        // Iterates through each sequence in the block radius
        for (int i = 1, sequenceLength = 1; i <= initialBlockRadius; i++, sequenceLength += 2)
        {
            // Iterates through each shell sequence
            for(int j = 0; j < 4; j++)
            {
                // Iterates through shell sequence lengths
                for (int k = 0; k < sequenceLength; k++)
                {
                    float xLocation = 0, zLocation = 0;
                    int xIndex = 0, zIndex = 0;

                    if (j == 0)  // Top-left shell
                    {
                        xLocation = -(vertexCount * ((sequenceLength / 2) + 1)) + (k * vertexCount) + (vertexCount / 2);
                        zLocation =  (vertexCount * ((sequenceLength / 2) + 1)) - (vertexCount / 2);
                        xIndex = -i + k;
                        zIndex =  i;
                    }
                    else if (j == 1)  // Top-right shell
                    {
                        xLocation =  (vertexCount * ((sequenceLength / 2) + 1)) - (vertexCount / 2);
                        zLocation =  (vertexCount * ((sequenceLength / 2) + 1)) - (k * vertexCount) - (vertexCount / 2);
                        xIndex = i - 1;
                        zIndex = i - k;
                    }
                    else if (j == 2)  // Bottom-right shell
                    {
                        xLocation =  (vertexCount * ((sequenceLength / 2) + 1)) - (k * vertexCount) - (vertexCount / 2);
                        zLocation = -(vertexCount * ((sequenceLength / 2) + 1)) + (vertexCount / 2);
                        xIndex =  i - 1 - k;
                        zIndex = -i + 1;
                    }
                    else if (j == 3)    // Bottom-left shell
                    {
                        xLocation = -(vertexCount * ((sequenceLength / 2) + 1)) + (vertexCount / 2);
                        zLocation = -(vertexCount * ((sequenceLength / 2) + 1)) + (k * vertexCount) + (vertexCount / 2);
                        xIndex = -i;
                        zIndex = -i + 1 + k;
                    }

                    // Calculate location
                    Vector3 position = new Vector3(xLocation, 0, zLocation);

                    // Generate instance of prefab
                    GameObject prefabInstance = (GameObject)GameObject.Instantiate(prefab, position, orientation);

                    // Generate mesh for instance
                    MeshGenerator script = prefabInstance.GetComponent<MeshGenerator>();
                    script.GenerateMesh(vertexCount, GenerateHeightmap(xIndex, zIndex));
                }
            }
        }
    }

    /**
     * Generates and adds heightmap to dictionary 
     * based on initial cartesian coordinates
     */
    private float[,] GenerateHeightmap(int x, int y)
    {
        // Create 2D array of noise values
        float[,] heightmap = new float[heightmapDimensions, heightmapDimensions];

        // Set random color to each of the four corners of the heightmap
        heightmap[0, 0] = UnityEngine.Random.Range(0.0f, 1.0f);
        heightmap[heightmapDimensions - 1, 0] = UnityEngine.Random.Range(0.0f, 1.0f);
        heightmap[0, heightmapDimensions - 1] = UnityEngine.Random.Range(0.0f, 1.0f);
        heightmap[heightmapDimensions - 1, heightmapDimensions - 1] = UnityEngine.Random.Range(0.0f, 1.0f);

        /*
        // Set random color to each RGB pixel
        for (int py = 0; py < heightmap.height; py++)
        {
            for (int px = 0; px < heightmap.width; px++)
            {
                float value = UnityEngine.Random.Range(0.0f, 1.0f);
                heightmap.SetPixel(px, py, new Color(value, value, value, 1.0f));
            }
        }
        */

        // TODO use algorithms to create heightmap usign adjacent heightmaps

        // Check above
        if (NoiseMap.ContainsKey(new Tuple<int, int>(x, y + 1)))
        {

        }

        // Check right
        if (NoiseMap.ContainsKey(new Tuple<int, int>(x + 1, y)))
        {

        }

        // Check bottom
        if (NoiseMap.ContainsKey(new Tuple<int, int>(x, y - 1)))
        {

        }

        // Check left
        if (NoiseMap.ContainsKey(new Tuple<int, int>(x - 1, y)))
        {

        }

        NoiseMap.Add(new Tuple<int, int>(x, y), heightmap);
        return heightmap;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
