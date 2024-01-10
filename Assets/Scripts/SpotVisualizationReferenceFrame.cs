using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.XR.MagicLeap;
using UnityEngine.InputSystem;
using static MagicLeapInputs;

public class SpotVisualizationReferenceFrame : MonoBehaviour
{
    // assign Controller
    private MagicLeapInputs mlInputs;
    private MagicLeapInputs.ControllerActions controllerActions;
    
    public GameObject ReferenceFrameObject; 

    // Start is called before the first frame update
    void Start()
    {
        //assign controller 
        mlInputs = new MagicLeapInputs();
        mlInputs.Enable();
        controllerActions = new MagicLeapInputs.ControllerActions(mlInputs);

        ReferenceFrameObject.transform.position = new Vector3(0,(float)-0.5,0);
        // ReferenceFrameObject.transform.scale = new Vector3((float)0.1, (float)0.1, (float)0.1);
        // ReferenceFrameObject.transform.Rotate(90, 0, 0);

        Quaternion Frame =  ReferenceFrameObject.transform.rotation ;
        ReferenceFrameObject.transform.rotation = new Quaternion(-Frame.x, -Frame.z, -Frame.y, Frame.w);
        Vector3 Frameposition = ReferenceFrameObject.transform.position;

        ReferenceFrameObject.transform.position = new Vector3(Frameposition.x,Frameposition.z, Frameposition.z);
        // ReferenceFrameObject.transform.localScale = new Vector3((float)0.5, (float) 0.5, (float) 0.5);   
    }

    // Update is called once per frame
    void Update()
    {
        bool BumperPressed = controllerActions.Bumper.IsPressed();
        //Debug.Log("framesize:" + ReferenceFrameObject.transform.localScale);

        if (BumperPressed == true)
        {
            //Debug.Log("controlPos");
            Vector2 controlPos = controllerActions.TouchpadPosition.ReadValue<Vector2>();
            //Debug.Log("controlPos");

            if (controlPos != Vector2.zero)
            {
                Vector3 newFrameSize = (controlPos.y + 1)/2 * new Vector3(1, 1, 1) ; // half of original size = maximal size 
                ReferenceFrameObject.transform.localScale = newFrameSize;
            }
        }
    }
}
