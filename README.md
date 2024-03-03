# Simple Tree Generator
![alt text](/Assets/Resources/Pictures/TreePreview.PNG?raw=true)
The Simple Tree Generator is a small project with whom you can generate trees in Unity.
They can be generated in the editor or at runtime.
## Graph Generation
The first part of the tree generation is the growth of a graph. A graph is a set of nodes. Each node has a length, a direction and a thickness. 
According to tweakable options and randomness a new node grows from the end of the last node. There is also a chance for a split, creating a new branch.
Most trees I observed in nature dont decrease gradually in diameter but split their surface area between the split trunks.
Thats how I implemented it as well. After a split the node length is decreased as well.

![alt text](/Assets/Resources/Pictures/GraphExampleNew.PNG?raw=true)
![alt text](/Assets/Resources/Pictures/GraphSettings.PNG?raw=true)

## Mesh Generation
Each trunk generates a mesh by adding circular structures of vertices with certain nodes at its center.
To decrease the amount of generated vertices a new set of vertices is only created when the curvature of the growth exceeds a certain value.
This, as well as the density of vertices for a circular structure, is adjustable in the settings.

![alt text](/Assets/Resources/Pictures/TreeMeshExample.PNG?raw=true)


