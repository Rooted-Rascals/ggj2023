using System;
using System.Collections;
using Script.Decorators.Plants;
using UnityEngine;
using UnityEngine.EventSystems;

public class PopupInfo : MonoBehaviour
{
    [SerializeField] private float TimeToPopup = 0.7f;

    public InfoPanel Panel { get; private set; }

    private void Awake()
    {
        GameObject panel = Instantiate(Resources.Load<GameObject>("Prefab/InfoPanel"), transform);
        panel.transform.SetParent(transform);
        Panel = panel.GetComponent<InfoPanel>();
        Panel.gameObject.SetActive(false);
    }

    private void OnMouseEnter()
    {
        StartCoroutine(nameof(OnHoldTimer));
    }

    private void OnMouseExit()
    {
        Panel.gameObject.SetActive(false);
        StopCoroutine(nameof(OnHoldTimer));
    }

    IEnumerator OnHoldTimer()
    {
        yield return new WaitForSeconds(TimeToPopup);
        Panel.gameObject.SetActive(true);
    }
}
