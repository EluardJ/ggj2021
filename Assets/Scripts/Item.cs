using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Sprite _icon;
    string _chunkLabel;
    RequestManager _requestManager;

    public float _uiSize = 1f;
    public Vector3 _uiRotation;

    public void SetChunkLabel(string chunkLabel)
    {
        _chunkLabel = chunkLabel;
    }
    public string GetChunkLabel()
    {
        return _chunkLabel;
    }
    public Sprite GetIcon()
    {
        return _icon;
    }

    public void SetRequestManager(RequestManager requestManager)
    {
        _requestManager = requestManager;
    }

    private void OnDestroy()
    {
        Debug.Log("Item destroyed");
        if (_requestManager != null)
        {
            _requestManager.OnItemDestroyed(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Hook hook = null;
        if (other.attachedRigidbody != null)
        {
            hook = other.attachedRigidbody.GetComponent<Hook>();
        }
        if (hook != null)
        {
            Debug.Log("=========>HOOK<=========  (" + Time.frameCount + ")");
        }
        //ComptoirChunk comptoirChunk = other.GetComponent<ComptoirChunk>();
        //if (comptoirChunk != null) {
        //    comptoirChunk.OnItemEnter(this);
        //}
    }
}
