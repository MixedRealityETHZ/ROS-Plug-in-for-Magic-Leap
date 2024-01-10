using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.XR.MagicLeap;
using static MagicLeapInputs;


public class ControlModeToggle : MonoBehaviour
{
    private MagicLeapInputs mlInputs;
    private MagicLeapInputs.ControllerActions controllerActions;

    // Timing variables for double press detection
    private float lastBumperPressTime = 0.0f;
    private float doublePressInterval = 0.5f; // Time interval to detect double press

    // Control scripts
    public SpotPositionControl positionControl;
    public SpotVelocityControl velocityControl;
    public SpotVisualizationReferenceFrame visualizationControl;
    public PointCloudRenderer pointCloudRenderer;
    // public PointCloud2VisualizerSettings pointCloud2VisualizerSettings; 

    // Start is called before the first frame update
    void Start()
    {
        mlInputs = new MagicLeapInputs();
        controllerActions = mlInputs.Controller;

        // Initialize control scripts (assuming they are attached to the same GameObject)
        positionControl = GetComponent<SpotPositionControl>();
        velocityControl = GetComponent<SpotVelocityControl>();
        visualizationControl = GetComponent<SpotVisualizationReferenceFrame>();
        pointCloudRenderer = GetComponent<PointCloudRenderer>();
        // pointCloud2VisualizerSettings = GetComponent<pointCloud2VisualizerSettings>();

        // Start with position mode enabled and visualization script active
        positionControl.enabled = true;
        velocityControl.enabled = false;
        visualizationControl.enabled = true;  
        pointCloudRenderer.enabled = true;
        // pointCloud2VisualizerSettings.enabled = true; 
    }

    // Update is called once per frame
    void Update()
    {
        if (controllerActions.Bumper.IsPressed())
        {
            Debug.Log("Bumper is pressed...");
            if (Time.time - lastBumperPressTime <= doublePressInterval)
            {
                // Double press detected, toggle control modes
                positionControl.enabled = !positionControl.enabled;
                velocityControl.enabled = !velocityControl.enabled;

                // Enable visualization only in position control mode
                visualizationControl.enabled = positionControl.enabled;

                Debug.Log("Changed mode"); 
            }
            lastBumperPressTime = Time.time;
        }
    }
}
