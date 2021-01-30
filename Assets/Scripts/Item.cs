using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Sprite _icon;
    RequestManager _requestManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Sprite GetIcon () {
        return _icon;
    }
    
    public void SetRequestManager (RequestManager requestManager) {
        _requestManager = requestManager;
    }

    private void OnDestroy () {
        Debug.Log("Item destroyed");
        if (_requestManager != null) {
            _requestManager.OnItemDestroyed(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Hook hook = null;
        if (other.attachedRigidbody != null) {
            hook = other.attachedRigidbody.GetComponent<Hook>();
        }
        if (hook != null){
            Debug.Log("=========>HOOK<=========  (" + Time.frameCount + ")");
        }
        ComptoirChunk comptoirChunk = other.GetComponent<ComptoirChunk>();
        if (comptoirChunk != null) {
            comptoirChunk.OnItemEnter(this);
        }
    }
}
