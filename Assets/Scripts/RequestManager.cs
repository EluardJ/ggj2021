using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RequestManager : MonoBehaviour, IComptoirTriggerListener
{
    public GridLevel _gridLevel;
    public GameObject[] _itemPrefabs;
    public Item[] _requestedItems;
    public GameObject _requestUI_template;
    private List<Item> _currentlyRequestedItems = new List<Item>();
    private Dictionary<Item, RequestUI> _requestUILookup = new Dictionary<Item, RequestUI>();
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

                foreach (Transform t in chunk.GetItemHandles()) {
                    GameObject itemGameObject = GameObject.Instantiate(_itemPrefabs[Random.Range(0, _itemPrefabs.Length)]);
                    itemGameObject.transform.position = t.position;
                    itemGameObject.transform.rotation = t.rotation;
                    Item newItem = itemGameObject.GetComponent<Item>();
                    items.Add(newItem);
                }

                items.AddRange(chunkItems);
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

    public void OnChunckChanged () {
        InitializeRequestItems();
        Item[] currentlyRequestedItems = _currentlyRequestedItems.ToArray();
        for (int i = 0; i < currentlyRequestedItems.Length; i++)
        {
            Item item  = currentlyRequestedItems[i];
            if (item == null) {
                _currentlyRequestedItems.Remove(item);
                PushNextRequest();
            }
        }
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
                GameObject.Destroy(_requestUILookup[item].gameObject);
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
        RequestUI requestUI = GameObject.Instantiate(_requestUI_template).GetComponent<RequestUI>();
        requestUI.transform.SetParent(_requestUI_template.transform.parent, false);
        requestUI.gameObject.SetActive(true);

        GameObject itemCopy = Instantiate(requestedItem.gameObject);
        if (itemCopy.transform.GetComponent<Rigidbody>() != null) {
            itemCopy.transform.GetComponent<Rigidbody>().isKinematic = true;
        }
        SetLayerRecursively(itemCopy, LayerMask.NameToLayer("UI"));
        itemCopy.transform.parent = requestUI.GetHandle();
        itemCopy.transform.localScale = Vector3.one;
        itemCopy.transform.localPosition = Vector3.zero;
        
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
