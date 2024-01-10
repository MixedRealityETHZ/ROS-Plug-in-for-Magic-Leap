# ROS-Plug-in-for-Magic-Leap

# Spot Demo

## Packages
Install the following applications to setup the application:
- https://github.com/jiaqchen/ROS-TCP-Connector.git?path=/com.unity.robotics.visualizations
- https://github.com/jiaqchen/ROS-TCP-Connector.git?path=/com.unity.robotics.ros-tcp-connector 
- https://assetstore.unity.com/packages/tools/integration/magic-leap-setup-tool-194780 

## Setup
To setup the Magic Leap 2 with Unity, refer to the guide below: 
https://developer-docs.magicleap.cloud/docs/guides/unity/getting-started/unity-getting-started/

## Important Notes
Make sure that `tf` tracking is turned off or carefully selected in all objects (ex. ROSConnector, PointCloud2Visualizer). `tf` will write to the global frame of Unity rather than the local frame of the parent. 