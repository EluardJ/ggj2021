﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Sprite _icon;

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
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("=========>OnTriggerEnter<=========  (" + Time.frameCount + ")");
        ComptoirChunk comptoirChunk = other.GetComponent<ComptoirChunk>();
        if (comptoirChunk != null) {
            comptoirChunk.OnItemEnter(this);
        }
    }
}