using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    public GridLevel gridLevel = default;

    [SerializeField] Sprite comptoirIcon = null;

    [SerializeField] Color normalBackground = default;
    [SerializeField] Color comptoirBackground = default;

    [SerializeField] TextMeshProUGUI[] lettersTexts = default;
    [SerializeField] Image[] backgroundsImages = default;

    public Dictionary<Vector2, TextMeshProUGUI> letters = new Dictionary<Vector2, TextMeshProUGUI>();
    public Dictionary<Vector2, Image> backgrounds = new Dictionary<Vector2, Image>();

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

        for (int j = gridLevel.gridDimensions - 1; j >= 0; j--)
        {
            for (int i = 0; i < gridLevel.gridDimensions; i++)
            {
                Vector2 gridCoords = new Vector2(i, j);

                letters.Add(gridCoords, lettersTexts[cnt]);
                backgrounds.Add(gridCoords, backgroundsImages[cnt]);

                cnt++;
            }
        }

        OnChunkChanged();
    }

    public void OnChunkChanged()
    {
        foreach (KeyValuePair<Vector2, RoomChunk> chunk in gridLevel.chunks)
        {

            if (gridLevel.comptoirCoordinates == chunk.Key)
            {
                letters[chunk.Key].SetText("<sprite index=0>");
                backgrounds[chunk.Key].color = comptoirBackground;
            }
            else
            {
                letters[chunk.Key].SetText(alphabet[chunk.Value.letterID]);
                backgrounds[chunk.Key].color = normalBackground;
            }

        }
    }
}