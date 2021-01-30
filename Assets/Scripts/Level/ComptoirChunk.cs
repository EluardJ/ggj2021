using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComptoirChunk : RoomChunk
{
    public BoxCollider _comptoirTrigger;
    private List<IComptoirTriggerListener> _listeners = new List<IComptoirTriggerListener>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnItemEnter (Item item) {
        if (_listeners == null) {
            return;
        }
        if (item != null) {
            foreach (IComptoirTriggerListener listener in _listeners) {
                if (listener == null) {
                    continue;
                }
                listener.OnItemDropped(item);
            }
        }
    }
    public void RegisterListener (IComptoirTriggerListener listener) {
        Debug.Log("=========>RegisterListener<=========  (" + Time.frameCount + ")");
        if (_listeners == null || listener == null) {
            return;
        }
        _listeners.Add(listener);
    }

}
