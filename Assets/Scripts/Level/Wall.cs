using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Wall : MonoBehaviour
{
    public void GetUp()
    {
        transform.DOMove(transform.position + Vector3.up * 3, 0.5f);
    }

    public void GetDown()
    {
        transform.DOMove(transform.position + Vector3.down * 3, 0.5f);
    }
}