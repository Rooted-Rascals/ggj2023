using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GrassTile : MonoBehaviour
{
    [SerializeField]
    private Vector3Int position;

    public void SetPosition(Vector3Int newPosition)
    {
        position = newPosition;
    }


}
