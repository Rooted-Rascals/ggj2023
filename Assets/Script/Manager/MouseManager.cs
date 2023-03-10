using System;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Script.Manager
{
    public class MouseManager : MonoBehaviour
    {
        public static MouseManager Instance = null;

        private Camera _mainCamera;
        private Camera _uiCamera;
        private GameObject _currentGameObject;

        private RaycastHit _currentHit;

        //Since we are at an angle with a orthographic view, we need to find a "speedratio" so that the speed feels the same.
        private float _speedRatio;
        public UnityEvent<GameObject> onHoverChanged;
        public UnityEvent<GameObject> onSelection;
        [SerializeField] private float rotationSpeed = 1f;
        [SerializeField] private float cameraSpeed = 1f;
        [SerializeField] private float mouseBorderDetection = 10f;
        [SerializeField] private bool mouseControlEnabled = true;
        [SerializeField] private float minZoom = 1f;
        [SerializeField] private float maxZoom = 2f;
        [SerializeField] private float zoomSpeed = 1f;
        [SerializeField] private float clampBuffer = -4f;

        private Vector3 startingPosition;
        private Quaternion startingRotation;


        public GameObject CurrentSelectedObject { get; private set; }
        public GameObject CurrentHoverObject { get; private set; }
        
        private void Awake()
        {
            if (Instance != null)
                throw new Exception("Only one MouseManager can be in a scene.");

            Instance = this;

            _mainCamera = Camera.main;
            _uiCamera = GetComponentsInChildren<Camera>().FirstOrDefault(b => !b.CompareTag("MainCamera"));
;            _speedRatio = _mainCamera.transform.localRotation.eulerAngles.x / 90 -
                          _mainCamera.transform.localRotation.eulerAngles.x;
        }

        private void Start()
        {
            startingPosition = transform.position;
            startingRotation = transform.rotation;
            
            onSelection.AddListener(OnSelection);
            onHoverChanged.AddListener(OnHoverChanged);
        }

        // Update is called once per frame
        void Update()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Physics.Raycast(ray, out _currentHit, Mathf.Infinity, LayerMask.GetMask("Tiles"));
                UpdateSelections(_currentHit.collider?.gameObject);    
            }
            
            Move();
            Rotate();
            Zoom();
        }

        void Move()
        {
            if (Input.GetKey(KeyCode.Space))
                Recenter();
            
            //Keyboard input
            float xInput = Input.GetAxis("Horizontal");
            float zInput = Input.GetAxis("Vertical");

            //Mouse input
            if (mouseControlEnabled && xInput == 0 && zInput == 0)
            {
                if (Input.mousePosition.x >= Screen.width - mouseBorderDetection)
                    xInput = 1; //Right edge
                else if (Input.mousePosition.x <= 0 + mouseBorderDetection)
                    xInput = -1; //Left edge
                if (Input.mousePosition.y >= Screen.height - mouseBorderDetection)
                    zInput = 1; //Top edge
                else if (Input.mousePosition.y <= 0 + mouseBorderDetection)
                    zInput = -1; //Bottom edge
            }

            float xSpeed = xInput * cameraSpeed * _speedRatio;
            float zSpeed = zInput * cameraSpeed * _speedRatio;

            Vector3 dir = (_mainCamera.transform.right * xSpeed) + transform.forward * (zSpeed);
            transform.position += dir * (-1 * Time.deltaTime);

            Vector3 clamped = Vector3.ClampMagnitude(transform.position,
                new Vector3(TilesManager.GridWidth - clampBuffer, 0, TilesManager.GridHeight - clampBuffer).magnitude);
            
            transform.position = new Vector3(clamped.x, transform.position.y, clamped.z);
        }

        void Rotate()
        {
            bool rotateLeft = Input.GetKey(KeyCode.Q);
            bool rotateRight = Input.GetKey(KeyCode.E);

            if (!rotateLeft && !rotateRight)
                return;

            float angle = rotationSpeed;

            if (rotateRight)
                angle *= -1;

            Ray ray = _mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            Physics.Raycast(ray, out RaycastHit middleHit, Mathf.Infinity, LayerMask.GetMask("Ground"));
            transform.RotateAround(middleHit.point, Vector3.up, angle);
        }

        private void Zoom()
        {
            //TODO : Make this smoother
            _mainCamera.orthographicSize += Input.GetAxis("Mouse ScrollWheel") * -1 * zoomSpeed;

            if (_mainCamera.orthographicSize > maxZoom)
                _mainCamera.orthographicSize = maxZoom;

            if (_mainCamera.orthographicSize < minZoom)
                _mainCamera.orthographicSize = minZoom;

            _uiCamera.orthographicSize = _mainCamera.orthographicSize;
        }

        private void UpdateSelections(GameObject obj)
        {
            if (Input.GetMouseButtonDown(0))
                onSelection.Invoke(obj);

            if (CurrentHoverObject != obj)
                onHoverChanged.Invoke(obj);
        }

        public void Recenter()
        {
            transform.position = startingPosition;
            transform.rotation = startingRotation;
        }

        public void Unselect()
        {
            onSelection.Invoke(null);
        }

        private void OnSelection(GameObject obj)
        {
            CurrentSelectedObject = obj;
        }

        private void OnHoverChanged(GameObject obj)
        {
            CurrentHoverObject = obj;
        }
    }
}