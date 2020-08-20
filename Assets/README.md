# VR Fitts Test

This unity project is part of a PhD research by Maxime Hébert-Lavoie. It is a VR adaptation of a [Fitts Test](https://www.yorku.ca/mack/FittsLawSoftware/) with graphic degradation.

## Requirements

* Unity 2019.4.0f1
* Vive Pro headset (other headset will require a few modifications to the scene and a different SDK for TobbiXR)
* SteamVR asset (https://assetstore.unity.com/packages/tools/integration/steamvr-plugin-32647)
* TobiiXR for the eye tracking feature (https://vr.tobii.com/sdk/develop/unity/getting-started/vive-pro-eye/)
* Post-processing package (from the package manager)

## How to run it

* Set up your SteamVR and your eye calibration
* Select the scene "Fitts_Test" from the scene folder
* Build and run the scene
	* Two screens will appear, one for the VR view and one for the menu
* Enter test parameters
* Enter degradation control 
	* If you don't want to degrade graphics, remove them with the toggle to the left of each one
	* If you want to degrade them manually during the test, uncheck the Auto toggle
* Click Start Test
	* At any time during the test, if you want to save the values of the degradation, press spacebar or the Save Sate button (it will stop the degradation)
	* Data about the Fitts test are exported automatically at the end of the test on the Desktop in the Fitts_Test_Data folder
	* If you don't want the eye tracking feature, disable the TobiiXR initializer

## Script

If you want to delve into the code start with TestParameter.cs, VRController.cs and FittsTest.cs.
