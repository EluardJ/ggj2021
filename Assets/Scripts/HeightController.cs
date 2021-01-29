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
    float up;
    float down;
    float height;
    int heightSegmentId = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void UpdateController()
    {
        if (useHeightSegment) {
            if (Input.GetButtonDown("Fire1")) {
                heightSegmentId++;
            }
            if (Input.GetButtonDown("Fire2")) {
                heightSegmentId--;
            }
            heightSegmentId = Mathf.Clamp(heightSegmentId, 0, heighSegment);
            height = Mathf.MoveTowards(height, ((heightSegmentId+0f)/heighSegment) * maxHeight, speed * Time.deltaTime);
        }
        else {
            up = Input.GetButton("Fire1")?1f:0f;
            down = Input.GetButton("Fire2")?1f:0f;
            height = height + (up - down) * speed * Time.deltaTime;
            height = Mathf.Clamp(height, 0, maxHeight);
        }
        heightHandle.localPosition = new Vector3(0, height, 0);
        scaledHandle.localPosition = heightHandle.localPosition * 0.5f;
        scaledHandle.localScale = new Vector3(scaledHandle.localScale.x, heightHandle.localPosition.y, scaledHandle.localScale.z);
    }
    public void FixedUpdateController()
    {
    }
}
