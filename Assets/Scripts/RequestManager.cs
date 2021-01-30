using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RequestManager : MonoBehaviour, IComptoirTriggerListener
{
    public GridLevel _gridLevel;
    public Item[] _requestedItems;
    public GameObject _requestUI_template;
    private List<Item> _currentlyRequestedItems = new List<Item>();
    private Dictionary<Item, GameObject> _requestUILookup = new Dictionary<Item, GameObject>();
    private int nextRequestedItemId = 0;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void InitializeRequestItems () {
        List<Item> items = new List<Item>();
        if (_gridLevel != null) {
            foreach (Vector2 coord in _gridLevel.chunks.Keys) {
                RoomChunk chunk = _gridLevel.chunks[coord];
                if (chunk == null) {
                    continue;
                }
                if (chunk as ComptoirChunk != null) {
                    (chunk as ComptoirChunk).RegisterListener(this);
                }
                Item[] chunkItems = chunk.GetItems(); 
                items.AddRange(chunkItems);
            }
        }
        _requestedItems = items.ToArray();
    }

    public void OnItemDropped (Item item) {   
        bool isSuccess = false;     
        foreach (Item currentlyRequestedItem in _currentlyRequestedItems) {
            if (item == currentlyRequestedItem) {
                Debug.Log("Success !");
                isSuccess = true;
            }
        }
        if (isSuccess) {
            if (_requestUILookup.ContainsKey(item)) {
                GameObject.Destroy(_requestUILookup[item]);
                _requestUILookup[item] = null;
            }
            _currentlyRequestedItems.Remove(item);
            PushNextRequest();
            GameObject.Destroy(item.gameObject);
        }
    }

    public void PushNextRequest () {
        if (nextRequestedItemId >= _requestedItems.Length) {
            return;
        }
        Item requestedItem = _requestedItems[nextRequestedItemId];
        nextRequestedItemId++;
        GameObject go = GameObject.Instantiate(_requestUI_template);
        go.transform.SetParent(_requestUI_template.transform.parent, false);
        go.SetActive(true);
        go.GetComponent<Image>().sprite = requestedItem.GetIcon(); 
        _currentlyRequestedItems.Add(requestedItem);
        _requestUILookup[requestedItem] = go;
    }
}
