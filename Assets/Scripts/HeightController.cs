using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightController : MonoBehaviour, IController
{
    public float speed = 1f;
    public float maxHeight = 3f;
    public Transform heightHandle;
    public Transform scaledHandle;
    float up;
    float down;
    float height;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void UpdateController()
    {
        up = Input.GetAxisRaw("Fire1");
        down = Input.GetAxisRaw("Fire2");
        height = height + (up - down) * speed * Time.deltaTime;
        height = Mathf.Clamp(height, 0, maxHeight);
        heightHandle.localPosition = new Vector3(0, height, 0);
        scaledHandle.localPosition = heightHandle.localPosition * 0.5f;
        scaledHandle.localScale = new Vector3(scaledHandle.localScale.x, heightHandle.localPosition.y, scaledHandle.localScale.z);
    }
    public void FixedUpdateController()
    {
    }
}
