using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotInitializer : MonoBehaviour
{
    [SerializeField] Transform[] elements = null;

    public void Initialize()
    {
        //for (int i = 0; i < elements.Length; i++)
        //{
        //    elements[i].transform.parent = transform.parent;
        //}

        gameObject.SetActive(true);
    }
}
