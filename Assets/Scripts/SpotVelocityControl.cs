using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Geometry;
using RosMessageTypes.Nav;

using UnityEngine.UI;
using UnityEngine.XR.MagicLeap;
using UnityEngine.InputSystem;

public class SpotVelocityControl : MonoBehaviour
{
    // Unity data structures
    public GameObject SpotCurrentPositionObject; 
    // public GameObject SpotTargetPositionObject; 
    public float SpotTargetPositionLinearSpeed = 0.1f; 
    public float SpotTargetPositionAngularSpeed = 0.5f;

    private Vector3 SpotCurrentPosition;
    private Quaternion SpotCurrentOrientation;

    // Magic Leap 2 setup  
    private MagicLeapInputs mlInputs;
    private MagicLeapInputs.ControllerActions controllerActions;

    // ROS setup 
    ROSConnection ros;
    public string spotTwistTopic = "/spot/cmd_vel";
    private TwistMsg spotTwistMsg;
    public string spotOdometryTopic = "/spot/odometry_in_map";
    OdometryMsg spotOdometryMsg;
  
    public float publishMessageFrequency;   // Publish the cube's position and rotation every N seconds
    private float timeElapsed = 0;  // Used to determine how much time has elapsed since the last message was published

    // Start is called before the first frame update
    void Start()
    {
        // start the ROS connection
        ros = ROSConnection.GetOrCreateInstance();
        ros.Subscribe<RosMessageTypes.Nav.OdometryMsg>(spotOdometryTopic, spotOdometryCallback);
        ros.RegisterPublisher<TwistMsg>(spotTwistTopic);
        spotTwistMsg = new TwistMsg();

        // Controller inputs
        mlInputs = new MagicLeapInputs();
        mlInputs.Enable();
        controllerActions = new MagicLeapInputs.ControllerActions(mlInputs);

        spotOdometryMsg = new OdometryMsg();
    }

    // Update is called once per frame
    void Update()
    {
        // Go in x & y direction, touchpad velocity control
        if (controllerActions.Trigger.IsPressed())
        {
            Vector2 controlPos = controllerActions.TouchpadPosition.ReadValue<Vector2>();   // Go forward

            if (controlPos != Vector2.zero)
            {
                // float speedFactor = 0.01f; // This is the rate to slow down to match the point cloud map environment
                float linearSpeedX = SpotTargetPositionLinearSpeed * controlPos.x;
                float linearSpeedY = SpotTargetPositionLinearSpeed * controlPos.y;

                // Vector3 bodyTranslate = new Vector3(linearSpeedX, (float)0, linearSpeedY);
                // float objectRotation = SpotTargetPositionObject.transform.eulerAngles.y;

                // bodyTranslate = Quaternion.Euler(0, objectRotation, 0) * bodyTranslate;
                // SpotTargetPositionObject.transform.Translate(bodyTranslate);

                spotTwistMsg.linear.x = linearSpeedX;
                spotTwistMsg.linear.y = linearSpeedY;
            }
        }

        // Turn if bumper is pressed 
        if (controllerActions.Bumper.IsPressed())
        {
            // SpotTargetPositionObject.transform.Rotate(0, SpotTargetPositionAngularSpeed, 0);
            spotTwistMsg.angular.z = SpotTargetPositionAngularSpeed;
        }

        timeElapsed += Time.deltaTime;

        if (timeElapsed > publishMessageFrequency)
        {
            // Finally send the message to server_endpoint.py running in ROS
            ros.Publish(spotTwistTopic, spotTwistMsg);
            // Debug.Log("[PublisherExample] Sent TwistMsg" + spotTwistMsg.linear.x + ", " + spotTwistMsg.linear.y);
            timeElapsed = 0;
            spotTwistMsg = new TwistMsg();
        }
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
}
