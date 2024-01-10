# ROS-Plug-in-for-Magic-Leap

For a detailed description of the project please check out following paper: 
https://github.com/MixedRealityETHZ/ROS-Plug-in-for-Magic-Leap/blob/main/ROS%20Plug%20in%20for%20Magic%20Leap%20Final%20Report.pdf

and for a video demo: 
https://drive.google.com/file/d/1U-3vY0MfHQ0c8sJFA6zQiUBgGqsD_S83/view 


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


Open the Scene ml2_deployment_1 under Asstes/Scenes 

Once Unity launches, navigate to the `Hierarchy` tab and click on `ROSConnectionPrefab` (refer to the red circle in the image).

Afterwards, navigate to the `Inspector` tab. Take note of the 4 scripts.

The first script is for setting up the ROS-TCP connector. **Note that the `ROS IP Address` and `Ros Port` should match based on what was inputted during the roslaunch of the `ros_tcp_endpoint` (refer to the green circle).**




    

## ROS Setup

In your Ubuntu 20.04 system (OS or WSL Ubuntu 20.04), open up 2 terminals.

First, in one terminal, run the following command. This brings up the ROS-TCP Connection and launches `roscore` underneath the hood, e.g.:

```
roslaunch ros_tcp_endpoint endpoint.launch tcp_ip:=127.0.0.1 tcp_port:=10000
```
![Launching the ROS-TCP Endpoint to communicate with Unity](https://github.com/ROS-Plugin-for-Magic-Leap-2/Unity/blob/feature/turtlesim/Images/Documentation/mixed_reality_ros-tcp-launch_setup.png)




## Important Notes
Make sure that `tf` tracking is turned off or carefully selected in all objects (ex. ROSConnector, PointCloud2Visualizer). `tf` will write to the global frame of Unity rather than the local frame of the parent. 

