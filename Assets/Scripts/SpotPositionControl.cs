using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Std; 
using RosMessageTypes.Geometry;
using RosMessageTypes.Nav;
using RosMessageTypes.BuiltinInterfaces;

using UnityEngine.UI;
using UnityEngine.XR.MagicLeap;
using UnityEngine.InputSystem;
using static MagicLeapInputs;

public class SpotPositionControl : MonoBehaviour
{
    // ROS structures
    ROSConnection ros; 
    public string spotOdometryTopic = "/spot/odometry_in_map";
    public string spotPoseStampedTopic = "/spot/map_goal_pose"; // In Spot, topic name is /spot/map_goal_pose

    OdometryMsg spotOdometryMsg; 
    PoseStampedMsg spotPoseStampedMsg; 

    // Unity data structures
    public GameObject SpotCurrentPositionObject; 
    public GameObject SpotTargetPositionObject; 

    Vector3 SpotCurrentPosition;
    Quaternion SpotCurrentOrientation;
    uint sequenceNumber = 0; 

    // Magic Leap 2 structures 
    private MagicLeapInputs mlInputs;
    private MagicLeapInputs.ControllerActions controllerActions;

    // Start is called before the first frame update
    void Start()
    {
        // Magic Leap 2 setup 
        mlInputs = new MagicLeapInputs();
        mlInputs.Enable();
        controllerActions = new MagicLeapInputs.ControllerActions(mlInputs);

        // ROS setup 
        ros = ROSConnection.GetOrCreateInstance();
        ros.Subscribe<RosMessageTypes.Nav.OdometryMsg>(spotOdometryTopic, spotOdometryCallback);
        ros.RegisterPublisher<RosMessageTypes.Geometry.PoseStampedMsg>(spotPoseStampedTopic);

        spotOdometryMsg = new OdometryMsg();
        spotPoseStampedMsg = new PoseStampedMsg(); 

        Debug.Log(SpotTargetPositionObject.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate Wolf position and put constraints for rotation and z axis 
        Vector3 spotTargetPosition = SpotTargetPositionObject.transform.localPosition;
        Quaternion spotTargetRotation = SpotTargetPositionObject.transform.localRotation;
        spotTargetPosition.y = 0;   // Want to prevent robot from moving up (assuming flat area)
        spotTargetRotation.x = 0;
        spotTargetRotation.z = 0;
        SpotTargetPositionObject.transform.localPosition = spotTargetPosition;
        SpotTargetPositionObject.transform.localRotation = spotTargetRotation;
        
        if (controllerActions.Trigger.WasReleasedThisFrame())  // send message only on release of trigger 
        {
            PublishSpotPoseStampedMsg(spotTargetPosition, spotTargetRotation);
        }

        sequenceNumber++; 
    }

    void spotOdometryCallback(RosMessageTypes.Nav.OdometryMsg msg)
    {
        // Transforming from ROS to Unity coordinates 
        SpotCurrentPosition = ConvertRosPointMsgToUnityVector3(msg.pose.pose.position);
        SpotCurrentPositionObject.transform.localPosition = SpotCurrentPosition;

        SpotCurrentOrientation = new Quaternion(-(float)msg.pose.pose.orientation.x, -(float)msg.pose.pose.orientation.z, -(float)msg.pose.pose.orientation.y, (float)msg.pose.pose.orientation.w);
        SpotCurrentPositionObject.transform.localRotation = SpotCurrentOrientation;
    }

    /* 
        Transforms coordinates from ROS PointMsg to Unity Vector3.   

        Source: https://github.com/siemens/ros-sharp/wiki/Dev_ROSUnityCoordinateSystemConversion
    */
    Vector3 ConvertRosPointMsgToUnityVector3(PointMsg RosPointMsg)
    {
        return new Vector3(
            -1 * (float)RosPointMsg.y, 
            (float)RosPointMsg.z,
            (float)RosPointMsg.x
        );
    }


    /* 
        Transforms coordinates from Unity Vector3 to ROS PointMsg.   

        Source: https://github.com/siemens/ros-sharp/wiki/Dev_ROSUnityCoordinateSystemConversion
    */
    RosMessageTypes.Geometry.PointMsg ConvertUnityVector3ToRosPointMsg(Vector3 unityVector3)
    {
        RosMessageTypes.Geometry.PointMsg rosPointMsg = new PointMsg(); 
        rosPointMsg.x = (double) unityVector3.z;
        rosPointMsg.y = -1 * (double) unityVector3.x;
        rosPointMsg.z = (double) unityVector3.y;

        return rosPointMsg; 
    }

    void PublishSpotPoseStampedMsg(Vector3 spotTargetPosition, Quaternion spotTargetRotation)
    {
        spotPoseStampedMsg.header.seq = sequenceNumber;
        spotPoseStampedMsg.header.stamp = GetRosTimeNow();
        spotPoseStampedMsg.header.frame_id = "map";
        
        spotPoseStampedMsg.pose.position = ConvertUnityVector3ToRosPointMsg(spotTargetPosition);
        spotPoseStampedMsg.pose.position.z = (double)SpotCurrentPosition.y; // Want the z position of the robot to match the robot 
 
        spotPoseStampedMsg.pose.orientation.x = -(double)spotTargetRotation.x;
        spotPoseStampedMsg.pose.orientation.y = -(double)spotTargetRotation.z;
        spotPoseStampedMsg.pose.orientation.z = -(double)spotTargetRotation.y;
        spotPoseStampedMsg.pose.orientation.w = (double)spotTargetRotation.w;

        ros.Publish(spotPoseStampedTopic, spotPoseStampedMsg); 
        Debug.Log("Sent position: " + spotPoseStampedMsg.pose.position + "sent orientation: "+ spotPoseStampedMsg.pose.orientation);
    }

    TimeMsg GetRosTimeNow()
    {
        // Get the current time and convert it to ROS Time format
        double unixTime = (System.DateTime.UtcNow - new System.DateTime(1970, 1, 1)).TotalSeconds;
        uint secs = (uint)unixTime;
        uint nsecs = (uint)((unixTime - secs) * 1e9);
        TimeMsg rosTime = new TimeMsg(secs, nsecs); // Create a TimeMsg
        return rosTime;
    }
}
