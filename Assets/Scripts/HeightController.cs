using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightController : MonoBehaviour, IController
{
    public bool useHeightSegment = false;
    public int heighSegment = 3;
    public float speed = 1f;
    public float maxHeight = 3f;
    public Transform heightHandle;
    public Transform scaledHandle;
    Vector3 heightHandleInitialLocalPosition;
    float up;
    float down;
    float height;
    int heightSegmentId = 0;
    // Start is called before the first frame update
    void Start()
    {
        heightHandleInitialLocalPosition = heightHandle.localPosition;
    }

    // Update is called once per frame
    public void UpdateController()
    {
        if (useHeightSegment) {
            if (Input.GetButtonDown("Fire4")) {
                heightSegmentId++;
            }
            if (Input.GetButtonDown("Fire5")) {
                heightSegmentId--;
            }
            heightSegmentId = Mathf.Clamp(heightSegmentId, 0, heighSegment);
            height = Mathf.MoveTowards(height, ((heightSegmentId+0f)/heighSegment) * maxHeight, speed * Time.deltaTime);
        }
        else {
            up = Input.GetButton("Fire4")?1f:0f;
            down = Input.GetButton("Fire5")?1f:0f;
            height = height + (up - down) * speed * Time.deltaTime;
            height = Mathf.Clamp(height, 0, maxHeight);
        }
        heightHandle.localPosition = heightHandleInitialLocalPosition + new Vector3(0, height / heightHandle.lossyScale.y, 0);
        if (scaledHandle != null) {
            scaledHandle.localPosition = heightHandleInitialLocalPosition + new Vector3(0, (height / heightHandle.lossyScale.y), 0);
            scaledHandle.localScale = new Vector3(scaledHandle.localScale.x, heightHandle.localPosition.y, scaledHandle.localScale.z);
        }
    }
    public void FixedUpdateController()
    {
    }
    public Transform GetHeightHandle () {
        return heightHandle;
    }
    public Vector3 GetHeightHandleInitialLocalPosition () {
        return heightHandleInitialLocalPosition;
    }
}
