using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GridGenerator _gridGenerator;
    public RequestManager _requestManager;
    // Start is called before the first frame update
    void Start()
    {
        if (_gridGenerator != null) {
            _gridGenerator.Generate();
        }
        if (_requestManager != null) {
            _requestManager.InitializeRequestItems();
            _requestManager.PushNextRequest();
            _requestManager.PushNextRequest();
            _requestManager.PushNextRequest();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
