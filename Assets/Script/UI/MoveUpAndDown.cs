using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpAndDown : MonoBehaviour
{
    //adjust this to change speed
    public float speed = 5f;
    //adjust this to change how high it goes
    public float height = 0.5f;

    public void FixedUpdate()
    {
        Vector3 pos = transform.position;

        float newY = Mathf.Sin(Time.time * speed);

        transform.localPosition = new Vector3(0, newY, 0) * height;
    }
}
