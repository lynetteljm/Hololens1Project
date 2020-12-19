# Hololens1Project
An app that acts as an introduction to Mixed Reality Development on Hololens 1. It showcases different types of interactions such as gaze, air taps and drags. It utilises spatial mapping and processing, and has an option for network synchronisation as well.

## Gaze Interactions
Text appears upon gaze of 2 game objects - the capsule and the sofa. For the capsule, it turns from grey to red and a panel with text reading "Try Grabbing" appears. For the sofa, a text reading "Manipulate Sofa" appears upon gaze. The posters light up upon gaze as well.

## Air Taps
The 3 posters control the visualisation of the 3D model of the changeable object - upon tapping any of the posters (hololens, car, and engine), a 3D replica of that poster will appear. The capsule also turns yellow upon tapping. Air taps anywhere can also spawn cardboard boxes to the scene.

## Drags
The model object can be rotated and moved, with speech input source keywords as “rotation mode” and “movement mode”. The capsule can be rotated, moved and scaled, with speech input source keywords as “rotate mode”, “move mode”, and “rescale mode”. The sofa can be rotated, moved, and scaled, with speech input source keywords as “rotating”, “moving”, and “rescaling”.

## Spatial Mapping
The posters are positioned on the walls, the model on the table, the sofa on the floor, and the capsule on the ceiling. *These require prior spatial mapping of the environment to be done on the Hololens.*

## Networking
A Hololens running the same app is able to join the other HoloLens and their positions within the same world is synced. Syncing of manipulations of the capsule, the sofa and the model across network are in real time. The capsule and the sofa can be manipulated through rotations, movements and rescalings. The model can be manipulated through rotations and movements. These manipulations are updated in the network. 
