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

    public static IEnumerator SpawnScalingUpAndDecay(Transform transform, float duration, float spawnDuration)
    {
        Vector3 finalScale = transform.localScale;
        Vector3 initialScale = Vector3.zero;

        for (float time = 0; time < spawnDuration; time += Time.deltaTime)
        {
            float progress = Mathf.PingPong(time, spawnDuration) / spawnDuration;
            transform.localScale = Vector3.Lerp(initialScale, finalScale, progress);
            yield return null;
        }
        
        for (float time = 0; time < duration - spawnDuration; time += Time.deltaTime)
        {
            float progress = Mathf.PingPong(time, duration - spawnDuration) / (duration- spawnDuration);
            transform.localScale = Vector3.Lerp(finalScale, initialScale, progress);
            yield return null;
        }
    }
}
