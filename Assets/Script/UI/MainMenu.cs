using Script.Decorators.Plants;
using Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Camera MainCamera;
    [SerializeField] private float angle;


    IEnumerator TurnCamera(Transform transform,float duration)
    {
        Vector3 initialScale = transform.localScale;

        for (float time = 0; time < duration; time += Time.deltaTime)
        {
            Ray ray = MainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            Physics.Raycast(ray, out RaycastHit middleHit, Mathf.Infinity, LayerMask.GetMask("Ground"));
            transform.RotateAround(middleHit.point, Vector3.up, angle); 
            yield return null;
        }
        for (float time = 0; time < duration; time += Time.deltaTime)
        {
            Ray ray2 = MainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            Physics.Raycast(ray2, out RaycastHit middleHit, Mathf.Infinity, LayerMask.GetMask("Ground"));
            transform.RotateAround(middleHit.point, Vector3.up, -angle);
            yield return null;
        }


        
    }

    void Start()
    {
        InvokeRepeating("RotateCamera", 1f, 10f);
    }

    private void RotateCamera()
    {
        print("test");
        StartCoroutine(TurnCamera(gameObject.transform, 5f));
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
