using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RequestManager : MonoBehaviour, IComptoirTriggerListener
{
    public GridLevel _gridLevel;
    public GameObject[] _itemPrefabs;
    public Item[] _requestedItems;
    public GameObject _requestUI_template;
    public GameManager _gameManager;
    private List<Item> _currentlyRequestedItems = new List<Item>();
    private Dictionary<Item, RequestUI> _requestUILookup = new Dictionary<Item, RequestUI>();
    private Dictionary<Transform, Item> _transformToItemLookup = new Dictionary<Transform, Item>();
    // private Dictionary<Item, Chunk> _transformToItemLookup = new Dictionary<Transform, Item>();
    public int _itemPerChunk = 1;

    private int nextRequestedItemId = 0;
    private int _count;
    int prefabId = 0;
    private string[] lettres = new string[] {
        "A",
        "B",
        "C",
        "D",
        "E",
        "F",
        "G",
        "H",
        "I",
        "J",
        "K",
        "L",
        "M",
        "N",
        "O",
        "P",
        "Q",
        "R",
        "S",
        "T",
        "U",
        "V",
        "W",
        "X",
        "Y",
        "Z"        
    };
    public void Start () {
        // shuffle prefabs.              
        for (int i = 0; i < _itemPrefabs.Length; i++) {
            GameObject temp = _itemPrefabs[i];
            int randomIndex = Random.Range(i, _itemPrefabs.Length);
            _itemPrefabs[i] = _itemPrefabs[randomIndex];
            _itemPrefabs[randomIndex] = temp;
        }
    } 

    public void InitializeRequestItems () {
        List<Item> items = new List<Item>();
        if (_gridLevel != null) {
            foreach (Vector2 coord in _gridLevel.chunks.Keys) {
                RoomChunk chunk = _gridLevel.chunks[coord];
                if (chunk == null) {
                    continue;
                }
                if (chunk.HasItems()) {
                    
                }
                else {
                    // randomize handles.
                    Transform[] handles = chunk.GetItemHandles();                    
                    for (int i = 0; i < handles.Length; i++) {
                        Transform temp = handles[i];
                        int randomIndex = Random.Range(i, handles.Length);
                        handles[i] = handles[randomIndex];
                        handles[randomIndex] = temp;
                    }
                    // spawn and set items.
                    int chunkItemCount = Mathf.Min(handles.Length, _itemPerChunk);
                    Item[] newChunkItems = new Item[chunkItemCount];
                    for (int i = 0; i < chunkItemCount; i++) {
                        Transform t = handles[i];
                        GameObject itemGameObject = GameObject.Instantiate(_itemPrefabs[(prefabId++)%_itemPrefabs.Length]);
                        itemGameObject.transform.position = t.position;
                        itemGameObject.transform.rotation = t.rotation;
                        itemGameObject.transform.parent = t.parent;
                        Item newItem = itemGameObject.GetComponent<Item>();
                        int letterId = chunk.letterID;
                        string chunkLabel = "?";
                        if (letterId >= 0 && letterId < lettres.Length) {
                            chunkLabel = lettres[letterId];
                        }
                        newItem.SetChunkLabel(chunkLabel);
                        newItem.SetRequestManager(this);
                        newChunkItems[i] = newItem;
                        _transformToItemLookup[t] = newItem;
                    }
                    chunk.SetItems(newChunkItems);

                }




                if (chunk as ComptoirChunk != null) {
                    (chunk as ComptoirChunk).RegisterListener(this);
                }
                Item[] chunkItems = chunk.GetItems(); 
                if (chunkItems != null) {
                    items.AddRange(chunkItems);
                }
            }
        }
        for (int i = 0; i < items.Count; i++) {
            Item temp = items[i];
            int randomIndex = Random.Range(i, items.Count);
            items[i] = items[randomIndex];
            items[randomIndex] = temp;
        }

        _requestedItems = items.ToArray();
    }

    public void SetRequestCount (int count) {
        _count = count;
        for (int i = 0; i < count; i++)
        {   
            PushNextRequest();
        }
    }
 
    public void OnItemDestroyed (Item item) {
        if (_currentlyRequestedItems != null && _currentlyRequestedItems.Contains(item)) {
            GameObject.Destroy(_requestUILookup[item].gameObject);
            _requestUILookup[item] = null;
            PushNextRequest();
        }
    }

    public void OnChunckChanged () {
        InitializeRequestItems();
        Item[] currentlyRequestedItems = _currentlyRequestedItems.ToArray();
        for (int i = 0; i < currentlyRequestedItems.Length; i++)
        {
            Item item  = currentlyRequestedItems[i];
            if (item == null) {
                _currentlyRequestedItems.Remove(item);
            }
        }
        for (int i = 0; i < _count - currentlyRequestedItems.Length; i++) {
            PushNextRequest();
        }
    }

    public bool OnItemDropped (Item item) {   
        bool isSuccess = false;     
        foreach (Item currentlyRequestedItem in _currentlyRequestedItems) {
            if (item == currentlyRequestedItem) {
                Debug.Log("=========>SUCCESS<=========  (" + Time.frameCount + ")");
                // _gameManager.OnSuccess();
                isSuccess = true;
            }
        }
        if (isSuccess) {
            if (_requestUILookup.ContainsKey(item)) {
                GameObject.Destroy(_requestUILookup[item].gameObject);
                _requestUILookup[item] = null;
            }
            _currentlyRequestedItems.Remove(item);
            PushNextRequest();
            GameObject.Destroy(item.gameObject);
        }
        return isSuccess;
    }

    public void PushNextRequest () {
        if (nextRequestedItemId >= _requestedItems.Length) {
            return;
        }
        Item requestedItem = _requestedItems[nextRequestedItemId];
        nextRequestedItemId++;
        if (requestedItem == null || requestedItem.gameObject == null) {
            PushNextRequest();
        }
        RequestUI requestUI = GameObject.Instantiate(_requestUI_template).GetComponent<RequestUI>();
        requestUI.transform.SetParent(_requestUI_template.transform.parent, false);
        requestUI.gameObject.SetActive(true);
        requestUI.SetLabel(requestedItem.GetChunkLabel());

        GameObject itemCopy = Instantiate(requestedItem.gameObject);
        if (itemCopy.transform.GetComponent<Rigidbody>() != null) {
            itemCopy.transform.GetComponent<Rigidbody>().isKinematic = true;
        }
        SetLayerRecursively(itemCopy, LayerMask.NameToLayer("UI"));
        itemCopy.transform.parent = requestUI.GetHandle();
        itemCopy.transform.localScale = Vector3.one * requestedItem._uiSize;
        itemCopy.transform.localEulerAngles = requestedItem._uiRotation;
        itemCopy.transform.localPosition = Vector3.zero;
        Vector3 rot = new Vector3(0, 360, 0);
        // itemCopy.transform.DORotate(rot, 2f, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);



        
        _currentlyRequestedItems.Add(requestedItem);
        _requestUILookup[requestedItem] = requestUI;
    }
    private void SetLayerRecursively (GameObject gameObject, LayerMask layer) {
        gameObject.layer = layer;
        foreach(Transform t in gameObject.transform) {
            if (t == gameObject.transform) {
                continue;
            }
            SetLayerRecursively(t.gameObject, layer);
        }
    }
}
