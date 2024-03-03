# Simple Tree Generator
![alt text](/Assets/Resources/Pictures/TreePreview.PNG?raw=true)
The Simple Tree Generator is a small project in Unity which you can generate trees with.
They can be generated in the editor or at runtime.

## Graph Generation
The first part of the tree generation is the growth of a graph. A graph is a set of nodes and each node has a length, a direction and a thickness. 
According to the tweakable options and randomness a new node grows from the end of the last node. There is also a chance for a split, creating a new branch.
Most trees that I have observed in nature don't decrease gradually in diameter but rather split their surface area between the split trunks.
As such this is how I choose to implement it as well. After a split, the node length is decreased as well.

![alt text](/Assets/Resources/Pictures/GraphExampleNew.PNG?raw=true)
![alt text](/Assets/Resources/Pictures/GraphSettings.PNG?raw=true)

## Mesh Generation
Each trunk generates a mesh by adding circular structures of vertices with nodes at its center.
In order to decrease the amount of generated vertices, a new set of vertices is only created when the curvature of the growth exceeds a certain value.
This, as well as the density of vertices for a circular structure, is adjustable in the settings.

![alt text](/Assets/Resources/Pictures/TreeMeshExample.png?raw=true)

## Leaf Generation
My main focus in this project was the mesh generation of the trunks as well as using no extra resources like textures.
Thats why I chose to simply generate a leaf with Unity`s AnimationCurve class. In the leaf settings you can set a few points to generate one half of the outline of the leaf.
This information is mirrored for the whole leaf as such a texture with adjustable resolution is generated. Each pixel is basically only holding information if its part of the leaf or not.
The mesh of the leaf is a set of 2 triangles each containing 3 vertices. All leaves are part of the same mesh with a shared material to optimize performance.


