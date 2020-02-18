﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class MeshPlacer : MonoBehaviour
{
    public int vertexCount = 20;        // Verticies in a MeshGen object
    public int blockSize = 100;         // The dimensions of a MeshGen object
    public int initialBlockRadius = 2;  // The initial radius from the origin of the scene
    public int heightmapBaseN = 7;      // The dimensions of generated heightmaps (2^(n)+1)

    public static float peakMin = 0.0f;
    public static float peakMax = 1.0f;
    public static float displacementMin = -0.6f;
    public static float displacementMax = 0.6f;

    private int heightmapDimensions;
    Dictionary<Tuple<int, int>, float[,]> NoiseMap = new Dictionary<Tuple<int, int>, float[,]>();

    // Start is called before the first frame update
    // Initialize the map with several meshes based on input radius
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
                    script.GenerateMesh(vertexCount, blockSize, GenerateHeightmap(xIndex, zIndex));
                }
            }
        }
    }

    // Generates and adds heightmap to dictionary 
    // based on initial cartesian coordinates
    private float[,] GenerateHeightmap(int x, int y)
    {
        // Create 2D array of noise values
        float[,] heightmap = new float[heightmapDimensions, heightmapDimensions];

        // Set random value to each of the four corners of the heightmap
        heightmap[0, 0] = UnityEngine.Random.Range(peakMin, peakMax);
        heightmap[heightmapDimensions - 1, 0] = UnityEngine.Random.Range(peakMin, peakMax);
        heightmap[0, heightmapDimensions - 1] = UnityEngine.Random.Range(peakMin, peakMax);
        heightmap[heightmapDimensions - 1, heightmapDimensions - 1] = UnityEngine.Random.Range(peakMin, peakMax);

        // Recursive diamond-square terrain generation algorithm
        DiamondSquareGen(heightmap, 0, heightmapDimensions - 1, 0, heightmapDimensions - 1);

        // TODO stitch adjacent heightmaps
        /*
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
        */

        NoiseMap.Add(new Tuple<int, int>(x, y), heightmap);
        return heightmap;
    }

    // Recursively performs the diamond-square algorithm to generate terrain
    private static void DiamondSquareGen(float[,] heightmap, int xMin, int xMax, int yMin, int yMax)
    {
        // Diamond step
        Tuple<int, int> diamondIndex = getDiamondIndex(xMin, xMax, yMin, yMax);
        float cornerAverage = (heightmap[xMin, yMin] + heightmap[xMax, yMin] + heightmap[xMin, yMax] + heightmap[xMax, yMax]) / 4;

        heightmap[diamondIndex.Item1, diamondIndex.Item2] = cornerAverage + UnityEngine.Random.Range(displacementMin, displacementMax);

        // Square step
        Tuple<Tuple<int, int>, Tuple<int, int>, Tuple<int, int>, Tuple<int, int>> squareIndicies = getSquareIndices(xMin, xMax, yMin, yMax);

        float topAverage = (heightmap[xMin, yMin] + heightmap[diamondIndex.Item1, diamondIndex.Item2] + heightmap[xMax, yMin]) / 3;
        float rightAverage = (heightmap[xMax, yMin] + heightmap[diamondIndex.Item1, diamondIndex.Item2] + heightmap[xMax, yMax]) / 3; ;
        float bottomAverage = (heightmap[xMin, yMax] + heightmap[diamondIndex.Item1, diamondIndex.Item2] + heightmap[xMax, yMax]) / 3; ;
        float leftAverage = (heightmap[xMin, yMin] + heightmap[diamondIndex.Item1, diamondIndex.Item2] + heightmap[xMin, yMax]) / 3; ;

        heightmap[squareIndicies.Item1.Item1, squareIndicies.Item1.Item2] = topAverage + UnityEngine.Random.Range(displacementMin, displacementMax);
        heightmap[squareIndicies.Item1.Item1, squareIndicies.Item1.Item2] = rightAverage + UnityEngine.Random.Range(displacementMin, displacementMax);
        heightmap[squareIndicies.Item1.Item1, squareIndicies.Item1.Item2] = bottomAverage + UnityEngine.Random.Range(displacementMin, displacementMax);
        heightmap[squareIndicies.Item1.Item1, squareIndicies.Item1.Item2] = leftAverage + UnityEngine.Random.Range(displacementMin, displacementMax);

        // Determine if recursive step is required
        if (xMax - xMin <= 2 && yMax - yMin <= 2)
        {
            return; // Base case
        } else
        {
            // Recursive calls on sub-problems
            DiamondSquareGen(heightmap, xMin, (xMax / 2) + (xMin / 2), yMin, (yMax / 2) + (yMin / 2));    // Top-left
            DiamondSquareGen(heightmap, (xMax / 2) + (xMin / 2), xMax, yMin, (yMax / 2) + (yMin / 2));    // Top-right
            DiamondSquareGen(heightmap, (xMax / 2) + (xMin / 2), xMax, (yMax / 2) + (yMin / 2), yMax);    // Bottom-right
            DiamondSquareGen(heightmap, xMin, (xMax / 2) + (xMin / 2), (yMax / 2) + (yMin / 2), yMax);    // Bottom-left
        }
    }

    // Returns the diamond index
    private static Tuple<int, int> getDiamondIndex(int xMin, int xMax, int yMin, int yMax)
    {
        return new Tuple<int, int>(
            (xMin + (xMax - xMin + 1) / 2),
            (yMin + (yMax - yMin + 1) / 2));
    }

    // Returns the square indicies
    private static Tuple<Tuple<int, int>, Tuple<int, int>, Tuple<int, int>, Tuple<int, int>> getSquareIndices(int xMin, int xMax, int yMin, int yMax)
    {
        // This is horrible. Everything is horrible.
        return new Tuple<Tuple<int, int>, Tuple<int, int>, Tuple<int, int>, Tuple<int, int>>(
            new Tuple<int, int> (
                (xMin + ((xMax - xMin + 1) / 2)),
                (yMin)),
            new Tuple<int, int> (
                (xMax),
                (yMin + ((yMax - yMin + 1) / 2))),
            new Tuple<int, int> (
                (xMin + ((xMax - xMin + 1) / 2)),
                (yMax)),
            new Tuple<int, int> (
                (xMin),
                (yMin + ((yMax - yMin + 1) / 2))));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
