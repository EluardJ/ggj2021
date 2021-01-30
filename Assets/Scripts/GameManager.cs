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
            _gridGenerator._useAutoGeneration = false;
            _gridGenerator.SetGridLevel(_gridGenerator.GetComponent<GridLevel>());
            _gridGenerator.Generate();
        }
        if (_requestManager != null) {
            _requestManager._gridLevel = _gridGenerator.GetComponent<GridLevel>();
            _requestManager.InitializeRequestItems();
            _requestManager.SetRequestCount(3);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
