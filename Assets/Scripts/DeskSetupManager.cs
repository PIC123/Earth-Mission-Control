using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Desk
{
    public float length;
    public float width;
    public Vector3 pivotPosition;
    public float yaw;
}

public class DeskSetupManager : MonoBehaviour
{
    public GameObject visual;
    public Transform pivot;
    public float heightOffset;
    public float defaultWidth = 0.03f;
    public float defaultHeight = 0.01f;

    public enum CreationState { start, length, height, finish }
    private CreationState creationState = CreationState.start;

    private Vector3 startLengthPosition;
    public Transform creationHand;

    public GameObject[] objectsToSpawn;

    private Vector3 startPosition;
    private bool isUpdatingShape;

    // Start is called before the first frame update
    void Start()
    {
        visual.SetActive(false);
        foreach (var item in objectsToSpawn)
        {
            item.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (creationState)
        {
            case CreationState.start:
                visual.SetActive(false);
                if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
                {
                    startLengthPosition = creationHand.position;
                    creationState = CreationState.length;
                }
                break;
            case CreationState.length:
                visual.SetActive(true);
                UpdateShape();

                if (!OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
                {
                    creationState = CreationState.height;
                }
                break;

            case CreationState.height:
                float maxY = Mathf.Min(pivot.position.y, creationHand.transform.position.y + heightOffset);

                pivot.position = new Vector3(pivot.position.x, maxY, pivot.position.z);

                if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
                {
                    creationState = CreationState.finish;
                    foreach (var item in objectsToSpawn)
                    {
                        item.SetActive(true);
                    }
                }

                break;
            case CreationState.finish:
                //visual.SetActive(false);
                if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
                {
                    //startLengthPosition = creationHand.position;
                    visual.SetActive(false);
                    foreach (var item in objectsToSpawn)
                    {
                        item.SetActive(false);
                    }
                    creationState = CreationState.start;
                }
                break;
            default:
                break;
        }
    }

    public void UpdateShape()
    {
        //Scaling
        float distance = Vector3.ProjectOnPlane(creationHand.position - startLengthPosition, Vector3.up).magnitude;
        visual.transform.localScale = new Vector3(distance, defaultHeight, defaultWidth);

        //Position
        pivot.position = startLengthPosition + pivot.rotation * new Vector3(visual.transform.localScale.x / 2, heightOffset, visual.transform.localScale.z / 2);
     
        //Rotation
        pivot.right = Vector3.ProjectOnPlane(creationHand.position - startLengthPosition, Vector3.up);
    }
}
