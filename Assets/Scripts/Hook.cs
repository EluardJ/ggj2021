using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    #region Variables
    public bool isHoldingSomething = false;

    bool isActive = false;
    #endregion

    #region Unity Callbacks
    private void OnTriggerEnter(Collider other)
    {
        if (isActive && !isHoldingSomething)
            Grab(other.transform);
    }
    #endregion

    #region Functions
    public void Activate()
    {
        isActive = true;
    }

    private void Grab(Transform objectToGrab)
    {
        isHoldingSomething = true;

        objectToGrab.parent = transform;
    }
    #endregion
}
