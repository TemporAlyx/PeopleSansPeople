# PeopleSansPeopleVRPose: A Fork of PeopleSansPeople For Generating a 3D Pose Estimation Dataset For VR

[![license badge](https://img.shields.io/badge/license-Apache--2.0-green.svg)](LICENSE.md)
&nbsp;
<img src="https://img.shields.io/badge/unity-2020.3.20f1-green.svg?style=flat-square" alt="unity 2020.3.20f1">
  
  
Check out the base project for more info https://github.com/Unity-Technologies/PeopleSansPeople

## Summary
This repository contains a variant of PeopleSansPeople, a project based off of Unity's Perception system capable of generating synthetic data for a variety of tasks, specifically in this case 3D Pose estimation. This reposity implements several changes to the base project in order to gear the dataset towards 3d pose estimation for the purpose of testing and developing a webcam-based full-body tracking system for SteamVR. Synthetic dataset generation within a game engine offers several benefits, such as being able to easily capture from multiple virtual cameras in a scene.

In its current state, this project can be loaded up in Unity and generate a base dataset, however similar in the base repository, it only has a few sample character prefabs, animations, and textures. The sample dataset included in the images folder is ran with an additional 30 humanoid character prefabs from https://www.mixamo.com/#/, 108 animations also from mixamo, and additional wall and floor textures come from https://github.com/abin24/Textures-Dataset.

This is version zero of the dataset, allowing the focus to shift on to the modeling side of the project before I add extra features here and generate a full dataset.

All character prefabs are setup with additional keypoints for tracking approximate vr headset and controller locations, as well as a ground measurement keypoint to capture the height of the character.
The virtual scene has been reorganized to be a three walled room with several lights of varying intensity run to run, with object and camera placement edited to fit within this space.
KeypointLabel generaiton has been changed to track position of the keypoints in 3d world space as well as the 2d locations in the image.

## Data Notebook
Along with the Unity project, a python notebook is provided with the framework code to preprocess and load the data into tensorflow for modelling. 

## Generated Data and Labels
The /images folder contains a sample dataset of 100x5 instances, which is 100 different generated scenes with 5 frames simulated for each. This includes json files in the Dataset folder which contain all the inforation about the scene for each frame as well as the keypoints, the png images saved in the RGB folder, and the segmentation pngs in the SemanticSegmentation folder. Additionally, once running the python notebook on the sample data set, the keypoints.npy, rgbs.npy, and segs.npy files are generated, which contain the preprocessed keypoints and file references to the images.

Running in unity on my machine, at 512x512 resolution it can generate ~80 images a second.

keypoints.npy - numpy array of shape (length of dataset, 15 (number of keypoints), 6 (values per keypoint)
each keypoint is comprised of 6 values, the x and y position of the point on the rgb image (scaled between 0 and 1), a visibility score (0 for not present in image, 0.5 for occluded, and 1 for visible), and the x, y, and z coordinates, scaled by the height of the character, and with the origin at the approximate headset location.
rgbs and segs are 1 dimensional arrays the length of the dataset containing the corresponding file location string.

## Future Plans
* Implement data processing scripts that handle tracking multi-frame sequences
* Provide notebook and scripts for testing various contemporary pose estimation models on dataset
* Redo the lighting scripts to reduce/eliminate situations where the lighting is too dark or washed out
* Redo the camera script to always ensure the character is in frame at realistic angles (occlusion can be added as a pre-processing step in tensorflow)
* Streamline scripts for data pre-processing in the data notebook
* Continue testing/iterating on model concepts
* setup and test multiple camera angle instances


## License
PeopleSansPeople is licensed under the Apache License, Version 2.0. See [LICENSE](LICENSE.md) for the full license text.
