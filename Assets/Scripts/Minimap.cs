using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Minimap : MonoBehaviour
{
    public GridLevel gridLevel = default;

    [SerializeField] TextMeshProUGUI[] lettersTexts = default;

    public Dictionary<Vector2, TextMeshProUGUI> letters = new Dictionary<Vector2, TextMeshProUGUI>();

    private string[] alphabet = new string[] {
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

    public void GenerateMinimap()
    {
        int cnt = 0;

        Debug.Log(gridLevel.gridDimensions);

        for (int j = gridLevel.gridDimensions - 1; j >= 0; j--)
        {
            for (int i = 0; i < gridLevel.gridDimensions; i++)
            {
                Vector2 gridCoords = new Vector2(i, j);

                letters.Add(gridCoords, lettersTexts[cnt]);

                cnt++;
            }
        }

        OnChunkChanged();
    }

    public void OnChunkChanged()
    {
        foreach (KeyValuePair<Vector2, RoomChunk> chunk in gridLevel.chunks)
        {
            Debug.Log(chunk.Key);
            letters[chunk.Key].SetText(alphabet[chunk.Value.letterID]);
        }
    }
}