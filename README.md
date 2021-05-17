# End-to-End-In-Game-Style-Transfer-Tutorial
![unity-style-transfer-screenshot](https://raw.githubusercontent.com/cj-mills/End-to-End-In-Game-Style-Transfer-Tutorial/main/images/unity-style-transfer-screenshot.png)

**Note:** You might get an error like the one below in Unity, if you download the project from GitHub. 

`AssetImporter is referencing an asset from the previous import. This should not happen.`

You can fix this issue by rebuilding the Unit asset. 
1. Open the Kinematica folder in the Assets section. 
2. Double-click on the `Unit` asset.
3. Click `Build` in the pop-up window. 
4. Close the pop-up window once the build is complete.
5. Back in the `Assets` section, open the `Biped` scene in the `Scenes` folder.

The project should run normally now. However, there might be some stuttering the first time it is run.

## Tutorial Links

[Part 1](https://christianjmills.com/End-To-End-In-Game-Style-Transfer-Tutorial-1/): This tutorial series covers how to train your own style transfer model with PyTorch and implement it in Unity using the Barracuda library.

[Part 1.5](https://christianjmills.com/End-To-End-In-Game-Style-Transfer-Tutorial-1-5/): This post covers how to use the Unity Recorder tool to generate additional training data for our style transfer model.

[Part 2](https://christianjmills.com/End-To-End-In-Game-Style-Transfer-Tutorial-2/): This post covers how to train an artistic style transfer model with PyTorch in Google Colab.

[Part 3](https://christianjmills.com/End-To-End-In-Game-Style-Transfer-Tutorial-3/): This post covers how implement the style transfer model in Unity with the Barracuda library.

