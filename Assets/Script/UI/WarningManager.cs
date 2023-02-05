using System.Collections;
using Script;
using UnityEngine;
using UnityEngine.UI;

public class WarningManager : MonoBehaviour
{
    private bool WaterWarningVisible = false;
    private bool TreeWarningVisible = false;
    
    [SerializeField]
    private Image WaterWarning;
    
    [SerializeField]
    private Image TreeWarning;
    
    [SerializeField]
    private Animation WaterWarningAnimation;
    
    [SerializeField]
    private Animation TreeWarningAnimation;

    private bool Initialized = false;
    
    private void Start()
    {
        WaterWarning.enabled = false;
        TreeWarning.enabled = false;
        
    }

    IEnumerator ActivateHealthWarning()
    {
        TreeWarning.enabled = true;
        yield return new WaitForSeconds(5);
        TreeWarning.enabled = false;
    }

    void Update()
    {
        if (!Initialized)
        {
            MotherTreeOrchestrator mainTree = GameManager.Instance.GetMotherTree();
            if (mainTree == null)
                return;
            
            mainTree.GetComponent<HealthManager>().onDamage.AddListener(
                () =>
                {
                    StopCoroutine(nameof(ActivateHealthWarning));
                    StartCoroutine(nameof(ActivateHealthWarning));
                });

            Initialized = true;
        }

        UpdateWaterWarning();
        UpdateTreeWarning();
    }

    private void UpdateTreeWarning()
    {
        if (TreeWarning.enabled && !TreeWarningVisible)
        {
            TreeWarningAnimation.Play("ShowWarningTree");
            TreeWarningVisible = true;
        }
        else if (!TreeWarning.enabled && TreeWarningVisible)
        {
            TreeWarningAnimation.Play("HideWarningTree");
            TreeWarningVisible = false;
        }    
    }

    private void UpdateWaterWarning()
    {
        WaterWarning.enabled = ResourcesManager.Instance.GetWaterCount() <= 0;

        if (WaterWarning.enabled && !WaterWarningVisible)
        {
            WaterWarningAnimation.Play("ShowWarningWater");
            WaterWarningVisible = true;
        }
        else if (!WaterWarning.enabled && WaterWarningVisible)
        {
            WaterWarningAnimation.Play("HideWarningWater");
            WaterWarningVisible = false;
        }
    }
}
