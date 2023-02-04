using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class LookAtCamera : MonoBehaviour
{
    private Camera _camera;
    
    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position + _camera.transform.rotation * Vector3.back, _camera.transform.rotation * Vector3.up);
        transform.Rotate(0,180,0);
    }
}
