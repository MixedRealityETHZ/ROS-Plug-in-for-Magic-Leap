using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Geometry;
using RosMessageTypes.Sensor;
using RosMessageTypes.BuiltinInterfaces;
using RosMessageTypes.Std;
using RosMessageTypes.Nav;

#if ENABLE_WINMD_SUPPORT
using HL2UnityPlugin;
#endif

using Unity.Robotics.Visualizations;

// Script that renders the point cloud using VisualizationTopicsTabEntry
public class PointCloudRenderer : MonoBehaviour
{
    ROSConnection ros;
    public GameObject visTopicsTabGameObject;

    public string PointCloud2Topic = "/surfacepoints";

    // public ConfigReader configReader;

    IEnumerator Start()
    {
        // yield return new WaitUntil(() => configReader.FinishedReader);   
        
        ros = ROSConnection.GetOrCreateInstance();
        VisualizationTopicsTab vistab = visTopicsTabGameObject.GetComponent<VisualizationTopicsTab>();

        // string PCTopic = "/surfacepoints";
        Debug.Log("Setting up subscriber for PointCloud2 topic: " + PointCloud2Topic);

        // Add new topic for /PCtoVisualize
        RosTopicState state = ros.GetOrCreateTopic(PointCloud2Topic, "sensor_msgs/PointCloud2", false);
        vistab.OnNewTopicPublic(state);

        VisualizationTopicsTabEntry vis;
        vis = vistab.getVisTab(PointCloud2Topic);

        if (vis == null)
        {
            Debug.LogError("VisualizationTopicsTabEntry not found for " + PointCloud2Topic);
            yield break;
        }

        Debug.Log(vis.GetType());
        // Debug.Log("1");
        Debug.Log(vis.GetVisualFactory());

        vis.GetVisualFactory().GetOrCreateVisual(PointCloud2Topic).SetDrawingEnabled(true);
        // PointCloud2DefaultVisualizer visFactory = vis.GetVisualFactory(); //(PointCloud2DefaultVisualizer)(vis.GetVisualFactory());
        // ((PointCloud2DefaultVisualizer)visFactory).GetOrCreateVisual(PCTopic).SetDrawingEnabled(true);
        // Debug.Log("VisualizationTopicsTab connected to" + ros.RosIPAddress + " " + ros.RosPort);       
    }
}
