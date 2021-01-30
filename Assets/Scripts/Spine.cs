using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spine : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _spineParts;
    [SerializeField]
    private Transform _head;
    [Range(0, 3)]
    public float _stretch;
    public HeightController _heightController;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float stretch = Vector3.Distance(_heightController.GetHeightHandleInitialLocalPosition(), _heightController.GetHeightHandle().localPosition);
        float range = stretch / 3f;

        for (int i = 0; i < _spineParts.Length; i++)
        {
            GameObject go = _spineParts[i];
            go.transform.localPosition = new Vector3(0, (0.03f +  (0.025f * range)) * i, 0);
            go.SetActive(go.transform.localPosition.y <= stretch);
        }

    }

    [ContextMenu("PrepareSpine")]
    public void PrepareSpine () {
        System.Array.Sort<GameObject>(_spineParts, new Comparison<GameObject>( 
                  (i1, i2) => i1.transform.localPosition.y.CompareTo(i2.transform.localPosition.y)));
        
        for (int i = 0; i < _spineParts.Length; i++)
        {
            GameObject go = _spineParts[i];
            go.transform.SetAsLastSibling();
            go.transform.localPosition = new Vector3(0, 0.03f * i, 0);
            go.name = "Spine_Part" + (i%2==0?"A":"B") + "_" + i/2;
        }


    }

    
}
