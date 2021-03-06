# Volcanic Terrain Generator
Terrain generator built in Unity. For Boise State CS 497 - Advanced Computer Graphics.

A Trello task list can be found here: https://trello.com/b/A6eJpKhz/terrain-engine

# Overview
// TODO – will be adding video and pipeline explanation

# Running
To run this, simply download “Builds” directory (or download the entire repository), select a version, and run the TerrainGen(version) executable file.

# Log
### 5/15/2020 - Model Placement and v0.1 Release
![16](Images/16.PNG)
For the final release of this class project, I have finished implementing procedural model placement. Using information about the peak of each volcano, I’m able to place cloud prefabs in a stylized fashion. In addition, I used information from heightmaps and sub-biomes to add trees and rocks to the surface of blocks. 
This version also includes release v0.1, a playable version of the simulation! It’s still pretty buggy, but it provides a proof-of-concept needed for the deliverables of this class. In the future, I hope to continue to iterate on this project.

### 5/08/2020 (part 2) - Texture Mapping and Planet Curve Vertex Shader
![15](Images/15.PNG)
(Decided to do another update this week. What else am I supposed to do during this pandemic?)

I implemented the aforementioned texture mapping, which sample pixels from corresponding biome types during the texture generation process for each block. This replaces the old method that simply filled in a texture with a single RGB value. In addition, I noticed some performance issues when viewing large vistas of generated terrain. To remedy this, I reduced the culling distance in the player’s view frustrum, but now it became a bit jarring to see distant terrain disappear when it’s far away. To make it a little less jarring, I implemented a vertex shader that simulates a curved earth surface, which hide culled terrain from the player (thank you Animal Crossing: New Horizons for the inspiration).

### 5/08/2020 - Water Model and Volcanic Terrain
![14](Images/14.PNG)
With the deadline coming up, I decided to scale back the scope of this project and focus on a single objective rather than encapsulating all types of terrain. I decided that my procedural terrain generator would be well suited to making volcanic islands, so I modified heightmap generation to allow for this. I also implemented procedural placement of moving water/waves in place of the old, flat texture. In the next update, I will be focusing on getting prefab model placement working and improving the existing texture generation to sample Unity assets rather than flat colors. Time permitting, I will attempt to implement bump-mapping on the procedural materials.

### 5/01/2020 - Biome-Aided Heightmap Generation
![13](Images/13.PNG)
In earlier versions of this project, I stressed a lot of importance with the diamond-square algorithm for generating heightmaps. As the biome system was implemented, the algorithm became more and more obsolete, as the noise generated did not match the corresponding biomes very well. To remedy this, I created an alternative heightmap generation algorithm that would take a sub-biome as an input and interpolate from the peak down to the ocean biomes. In addition, different noise values would be applied depending on the biome that the algorithm would interpolate on. From here, I’m going to focus on polishing this algorithm, in addition to adding model placement and an improved texture generator.

### 4/24/2020 - Biome -> Texture and Heightmap Generation
![12](Images/12.PNG)
In this update, I refactored my material and heightmap generators to take in information from the biome. This was done by making an array with the same dimensions as the heightmap and filling it with a “sub-biome” (i.e., a small snippet of the biome that a block would reside in). Using this, I can generate a basic material using the colors of the biome, and I can use information from the biome to influence the heightmap. Both implementations are still rather simple, but I’ll eventually end up polishing both components to get better-looking blocks.

### 4/17/2020 - Advanced Biome Partitioning Agents
![11](Images/11.PNG)
In this update, I implemented a more organic-looking biome partitioning algorithm. This one works similarly to the previous implementation, but follow a more rigorous set of rules, including having one peak in every biome, circled by displaced mountain agents, then two sets of grass agents, then sand agents, and finally water agents. In addition, this generator takes in adjacent biomes as an argument and generates agents around the borders if one exists, which will make biomes seam with one another. For now, I’m satisfied with the results, and I’ll move on to generating this biomes on the fly and using them to influence displacement in heightmaps, in addition to the procedural texture generation algorithm.

### 4/10/2020 - Basic Biome Partition Algorithm
![10](Images/10.PNG)
In this update, I implemented a basic and somewhat naive approach to biome partitioning. It involves seeding a large canvas with agents that pseudo-randomly travel the canvas with their respective colors until the canvas is filled. Different colors represent different types of biomes, like grass fields, rocky mountains, and snowy peaks. Above is a sample output, which I’m not quite satisfied with as it exhibits the same fractal pattern in every generation. I have some displacement tricks that I’ll be implementing in order to make biomes look more organic in the next update.

### 4/03/2020 - Refactoring and Biome Partitioning Prep
![9](Images/9.jpg)
After a bit of a break, I have decided to refactor my entire generation pipeline in preparation for some of the more advanced features coming up. The pipeline now takes into account a cartesian coordinate system that queries both a biome map and a heightmap map, which takes care of some of the dependencies issues I would have had with the old implementation. In addition, I’ve opened up the pipeline to allow for bump/normal maps in addition to the regular texture generation. My next feature will be the biome generation algorithm.

### 3/27/2020 - Procedural Texture Generation
![8](Images/8.PNG)
In this update, I utilized the UV mappings of individual blocks of geometry to experiment with procedural material generation. I want to eventually implement all sorts of great material shader techniques, like bump and specular maps, but I wanted to mainly focus on color at first. I made a rudimentary algorithm for generating textures based off of the height of a pixel’s heightmap value. I then interpolated between green, brown, and white colors to simulate grass, mountain, and snow respectively. This is still pretty basic as it does not read off of a master biome map, but it should give me a basic approach for crossing off future goals.

### 3/20/2020 - Procedural Block Generation
![7](Images/7.PNG)

In this update, I implemented procedural block generation. This is accomplished using two different components:
-	The player’s view frustum
-	The player’s position/vicinity

Using a few view raycasts, circle intersection points, local-to-world point calculations, and cartesian-coordinate block querying, we are now able to generate new blocks of terrain that didn’t previously exist. This is significant as this modular approach sets this project up for procedural biome partitioning and material/shader generation (per block). 

### 3/13/2020 - Procedural UV Mapping Setup and HM Placement Setup
![6](Images/6.PNG)

In this update, I set up procedural UV mapping for generated blocks of terrain. This will allow me to utilize Unity's shaders to create procedural materials (textures, normal maps, etc…). This depends on the biome mapping, which I will focus on in a future update. Currently, the generated terrain is covered using a default diffuse tile texture. In addition, I’ve implemented frame-by-frame calculations for rays casted by the player’s frustum and a circle generated around the player’s position. Using these components, I’ll be able to set up procedural placement.

### 3/06/2020 - Off-Week
<!---
![5](Images/5.PNG)
--->

Went to Vegas this week for the Mountain West Basketball Tournament. Didn't have much time to sit down and implement features.

Go Broncos!

### 2/28/2020 - Feature Planning
<!---
![4](Images/4.PNG)
--->

In this update, I pushed a bit of a work, but I was primarily focused on planning out how I would develop the rest of the project beyond my initial brainstorming. I created a slideshow that I presented to the BSU Visual Lab which showed off some of my currently implemented ideas, as well as some ideas that I wanted to eventually implement.

The slides can be found here: https://docs.google.com/presentation/d/1KS8lYTnNKSGSGAwerPso5T7KUpoCsoR7gVtVWvVw958/edit?usp=sharing

In summary, I want to build off of my current work and implement some popular terrain generation algorithms/techniques. These techniques include:

- Material Interpolation
- Procedural Block Placement
- Heightmap Displacement
- Hydraulic Erosion Bumpmap Generation
- Biome Mapping/Partitioning
- Procedural Material Application
- Procedural Model Placement

### 2/21/2020 - Diamond Square and Heightmap Stitching
![3](Images/3.PNG)

In this update, I implemented the diamond-square algorithm for generating heightmaps of size (2n+1) by (2n+1). In addition, I developed a data structure for storing heightmap information in a cartesian coordinate system. This way, adjacent heightmaps can be “stitched” so that newly generated MeshGenerator objects seamlessly blend into other adjacent objects. I have effectively generated the base terrain required for the rest of the project, which should put me in a good place for procedural generation. There is still work to be done on heightmap generation for extra generation layers that add extra detail and remove diamond-square artifacts. In addition, I will begin adding user interaction so that I can begin procedural generation using the player vicinity and camera frustum.

### 2/14/2020 - Setup for Mesh Placement + Heightmap Generation
![2](Images/2.PNG)

In this update, I created a new prefab object that generates MeshGenerator objects in a grid formation. In addition, the MeshPlacer generates indexed heightmaps that the MeshGenerators read in order to generate their geometry. Currently, only random RGB values are generated for the corners of the ((2^n) + 1)-sized heightmaps. However, in future updates, I will be using the diamond-square algorithm and other permutations to generate smooth noise fields for each MeshGenerator object that connect to other MeshGenerator object heightmaps. In addition, I will set up the MeshPlacer script to add new MeshGenerator objects based on vicinity and the viewport of the player character. 

### 2/7/2020 - Basic Terrain Grid
![1](Images/1.PNG)

In this update, I created a new 3D Unity project with a single empty object called a "MeshGenerator". It contains a Mesh Filter and Mesh Renderer subcomponent, in addition to a Mesh Generator C# script, which is where vertices and triangles are actually generated. This script creates a mesh and assigns it to the MeshFilter subcomponent of the object, and then calls a CreateShape() method. This method first creates a Vector3 array of size "(x + 1) * (y + 1)" called "vertices", then programmatically fills it with Vectors x and z values aligned in a grid, while y values are assigned using a Perlin Noise function. Another integer array called "triangles" keeps track of vertex indices generated in the "vertices" array. This array programmatically assigns the correct vertices together to create triangles with back-face culling. Once both arrays are generated, the mesh data is cleared, assigned with new vertices and triangles, and the RecalculateNormals() method is called to properly light the mesh.
