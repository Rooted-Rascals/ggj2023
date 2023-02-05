using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coroutines : MonoBehaviour
{
    public static IEnumerator ScaleUpAndDown(Transform transform, Vector3 upScale, float duration)
    {
        Vector3 initialScale = transform.localScale;

        for (float time = 0; time < duration; time += Time.deltaTime)
        {
            float progress = Mathf.PingPong(time, duration) / duration;
            transform.localScale = Vector3.Lerp(initialScale, upScale, progress);
            yield return null;
        }
        for (float time = 0; time < duration; time += Time.deltaTime)
        {
            float progress = Mathf.PingPong(time, duration) / duration;
            transform.localScale = Vector3.Lerp(upScale, initialScale, progress);
            yield return null;
        }
    }
}
