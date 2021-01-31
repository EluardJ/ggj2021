using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RequestUI : MonoBehaviour
{
    public Transform _3DObjectHandle;
    public Text _label;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetLabel (string label) {
        if (_label == null) {
            return;
        }
        _label.text = label;
    }
    public Transform GetHandle () {
        return _3DObjectHandle;
    }
}
