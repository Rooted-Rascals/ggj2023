using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;

public class HexCoordinates : MonoBehaviour
{
    //private static float xOffset = 2, yOffset = 1, zOffset = 1.73f;

    private static float size = 1;

    private static float sqrt = Mathf.Sqrt(3)/2;


    public static Vector3 ConvertOffsetToPosition(Vector3Int position)
    {
        float side = size / sqrt;
        float height = side;

        float x = 2 * size * position.x + size * position.z - size * position.y - 2 * position.x;
        float z = (height + side / 2) * (position.y + position.z);

        return new Vector3(x, 0, z);
    }
}
