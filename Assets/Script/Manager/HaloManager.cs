using UnityEngine;
using UnityEngine.Serialization;

namespace Script.Manager
{
    public class HaloManager : MonoBehaviour
    {
        public static HaloManager Instance { get; private set; }

        [SerializeField] private float height = 0.5f;
        
        private Renderer _renderer;
        private void Awake()
        {
            Instance = this;
            GetComponentInChildren<Canvas>().worldCamera = Camera.main;
            _renderer = GetComponentInChildren<Renderer>();
            _renderer.enabled = false;
        }

        private void Start()
        {
            MouseManager.Instance.onSelection.AddListener(OnSelection);
        }

        private void OnSelection(GameObject gameObject)
        {
            if (gameObject is not null)
            {
                _renderer.enabled = true;
                transform.position =  gameObject.transform.position + new Vector3(0 , height, 0);
            }
            else
                _renderer.enabled = false;
        }
    }
}