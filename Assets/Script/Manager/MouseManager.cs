using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.Events;
using Plane = UnityEngine.Plane;
using Vector3 = UnityEngine.Vector3;

namespace Script.Manager
{
    public class MouseManager : MonoBehaviour
    {
        public static MouseManager Instance = null;
        
        private Camera _mainCamera;
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
        public GameObject CurrentSelectedObject { get; private set; }
        public GameObject CurrentHoverObject { get; private set; }

        private void Awake()
        {
            if (Instance != null)
                throw new Exception("Only one MouseManager can be in a scene.");
            
            Instance = this;
            
            _mainCamera = Camera.main;
            _speedRatio = _mainCamera.transform.localRotation.eulerAngles.x / 90 - _mainCamera.transform.localRotation.eulerAngles.x;
        }

        private void Start()
        {
            onSelection.AddListener(OnSelection);
            onHoverChanged.AddListener(OnHoverChanged);
        }

        // Update is called once per frame
        void Update()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out _currentHit);
            UpdateSelections(_currentHit.collider?.gameObject);
            
            Move();
            Rotate();
        }

        void Move()
        {
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
            
            Vector3 dir = (_mainCamera.transform.right * xSpeed)  + transform.forward * (zSpeed);
            transform.position += dir * (-1 * Time.deltaTime);    
        }

        void Rotate()
        {
            bool rotateLeft = Input.GetKey(KeyCode.Q);
            bool rotateRight = Input.GetKey(KeyCode.E);

            if (!rotateLeft && !rotateRight) 
                return;
            
            float angle = rotationSpeed;

            if (rotateRight)
                angle = angle * -1;
                
            transform.RotateAround(_currentHit.point, Vector3.up, angle);
        }

        private void UpdateSelections(GameObject obj)
        {
            if (Input.GetMouseButtonDown(0))
                onSelection.Invoke(obj);
                
            if (CurrentHoverObject != obj)
                onHoverChanged.Invoke(obj);    
        }

        private void OnSelection(GameObject obj)
        {
            CurrentSelectedObject = obj;
            print(CurrentSelectedObject != null ? $"Selected {CurrentSelectedObject.name}" : $"No object selected");
        }
        
        private void OnHoverChanged(GameObject obj)
        {
            CurrentHoverObject = obj;
            print(CurrentHoverObject != null ? $"Hovering {CurrentHoverObject.name}" : "Hovering nothing");
        }
    }
}
