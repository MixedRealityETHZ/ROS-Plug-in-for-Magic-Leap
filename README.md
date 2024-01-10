# ROS-Plug-in-for-Magic-Leap

# Spot Demo

## Packages
Install the following applications to setup the application:
- Unity Hub: https://unity.com/download
    - make sure to install the 2022.3.11f1 Unity Edito version

and additionally download following packages: 
- https://github.com/jiaqchen/ROS-TCP-Connector.git?path=/com.unity.robotics.visualizations
- https://github.com/jiaqchen/ROS-TCP-Connector.git?path=/com.unity.robotics.ros-tcp-connector

  
## Setup
To setup the Magic Leap 2 with Unity, refer to the guide below: 
https://developer-docs.magicleap.cloud/docs/guides/unity/getting-started/unity-getting-started/
login to your unity account and get the following asset to simplyfy the magic leap setup: 
    - https://assetstore.unity.com/packages/tools/integration/magic-leap-setup-tool-194780 


Select `Open` and navigate to the folder where this repo is installed. Once you find the folder with the repo name, click the Open button. Unity will then begin to open the folder (this could take several minutes).
![Setup on Unity. Note the colored circles.](https://github.com/ROS-Plugin-for-Magic-Leap-2/Unity/blob/feature/turtlesim/Images/Documentation/mixed_reality_turtlesim_test_unity_setup.png)

Once Unity launches, navigate to the `Hierarchy` tab and click on `ROSConnectionPrefab` (refer to the red circle in the image).

Afterwards, navigate to the `Inspector` tab. Take note of the 4 scripts.

The first script is for setting up the ROS-TCP connector. **Note that the `ROS IP Address` and `Ros Port` should match based on what was inputted during the roslaunch of the `ros_tcp_endpoint` (refer to the green circle).**

Once the necessary fields in the `Inspector` have been setup, press the play button to begin running the Unity game.
 - To control the turtlesim, press `W`, `S`, `A`, `D` to move the turtle    in the respective directions. This verifies that the `Twist Publisher` is working. 
	 - Note that from testing and if the ROS stuff is setup in WSL2, the commands are extremely slow. The turtlesim will not seem to be    responsive, but it is picking up the commanded messages.
 - To reset the turtlesim node, press `R` on your keyboard. This verifies that the `Reset Caller` works. 
 - To verify that the `Pose Subscriber` is working, go to the Console, and you should see messages being printed that show the Pose of the turtlesim.


    

## ROS Setup

In your Ubuntu 20.04 system (OS or WSL Ubuntu 20.04), open up 2 terminals.

First, in one terminal, run the following command. This brings up the ROS-TCP Connection and launches `roscore` underneath the hood, e.g.:

```
roslaunch ros_tcp_endpoint endpoint.launch tcp_ip:=127.0.0.1 tcp_port:=10000
```
![Launching the ROS-TCP Endpoint to communicate with Unity](https://github.com/ROS-Plugin-for-Magic-Leap-2/Unity/blob/feature/turtlesim/Images/Documentation/mixed_reality_ros-tcp-launch_setup.png)




## Important Notes
Make sure that `tf` tracking is turned off or carefully selected in all objects (ex. ROSConnector, PointCloud2Visualizer). `tf` will write to the global frame of Unity rather than the local frame of the parent. 

