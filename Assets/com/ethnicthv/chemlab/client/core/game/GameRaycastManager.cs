using System;
using System.Collections;
using System.Collections.Generic;
using com.ethnicthv.assets.input;
using com.ethnicthv.chemlab.client.api.core.game;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace com.ethnicthv.chemlab.client.core.game
{
    [RequireComponent(typeof(ClientManager))]
    public class GameRaycastManager : MonoBehaviour
    {
        public static GameRaycastManager Instance { get; private set; }

        [SerializeField] private int skipFixedFrames = 5;
        [SerializeField] private float dragVelocity = 10;
        [SerializeField] private LayerMask draggableLayer;
        [SerializeField] private LayerMask tableLayer;

        public event Action<RaycastHit> OnRaycastHit = delegate { };

        private GameInteract _gameInteract;

#if UNITY_EDITOR
        private readonly Queue<Vector3> _debugRaycastHits = new();
#endif

        private int _skipFrames;

        private void Awake()
        {
            Instance = this;

            _gameInteract = new GameInteract();

            _gameInteract.GameEnvironment.Interact.performed += _ => OnLeftClick();
            _gameInteract.GameEnvironment.Options.performed += _ => OnRightClick();
            _gameInteract.GameEnvironment.Hold.performed += _ => OnDragStart();
            _gameInteract.GameEnvironment.Hold.canceled += _ => OnDragEnd();

            _gameInteract.Enable();
        }

        private void Start()
        {
            _skipFrames = skipFixedFrames;
        }

        private void FixedUpdate()
        {
            if (_skipFrames > 0)
            {
                _skipFrames--;
                return;
            }

            _skipFrames = skipFixedFrames;

            if (EventSystem.current.IsPointerOverGameObject()) return;
            var ray = ClientManager.Instance.mainCamera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit)) return;

            OnRaycastHit.Invoke(hit);
        }

        private void OnLeftClick()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            var ray = ClientManager.Instance.mainCamera.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out var hit)) return;

            if (InteractableManager.TryGetInteractable(hit.collider.gameObject, out var interactable))
            {
                interactable.OnInteract();
            }

#if UNITY_EDITOR
            _debugRaycastHits.Enqueue(hit.point);
            if (_debugRaycastHits.Count > 5)
            {
                _debugRaycastHits.Dequeue();
            }
#endif
        }

        private void OnRightClick()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            var ray = ClientManager.Instance.mainCamera.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out var hit)) return;

            if (InteractableManager.TryGetInteractable(hit.collider.gameObject, out var interactable))
            {
                OpenOptionsPanel(interactable);
            }

#if UNITY_EDITOR
            _debugRaycastHits.Enqueue(hit.point);
            if (_debugRaycastHits.Count > 5)
            {
                _debugRaycastHits.Dequeue();
            }
#endif
        }

        private void OpenOptionsPanel(IInteractable interactable)
        {
            var options = interactable.GetOptions();
            //TODO: Open options panel
        }

        private Coroutine _dragCoroutine;
        private Vector3 _velocity = Vector3.zero;
        private IInteractable _dragInteractable;

        private void OnDragStart()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            var ray = ClientManager.Instance.mainCamera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit, maxDistance: Mathf.Infinity, layerMask: draggableLayer)) return;

            if (!InteractableManager.TryGetInteractable(hit.collider.gameObject, out var interactable)) return;

            _dragInteractable = interactable;
            _dragCoroutine = StartCoroutine(DragCoroutine(interactable));
        }

        private IEnumerator DragCoroutine(IInteractable interactable)
        {
            while (true)
            {
                var ray = ClientManager.Instance.mainCamera.ScreenPointToRay(Input.mousePosition);
                var interactableTransform = interactable.GetMainTransform();
                var newPosition = interactable.GetMainTransform().position;
                if (Physics.Raycast(ray, out var hit, maxDistance: Mathf.Infinity, layerMask: tableLayer))
                {
                    newPosition = hit.point;
                }

                interactableTransform.position = Vector3.SmoothDamp(interactableTransform.position, newPosition,
                    ref _velocity, 1 / dragVelocity);
                yield return null;
            }
        }

        private void OnDragEnd()
        {
            if (_dragCoroutine != null)
            {
                StopCoroutine(_dragCoroutine);
                var ray = ClientManager.Instance.mainCamera.ScreenPointToRay(Input.mousePosition);
                GameObject other = null;
                if (Physics.Raycast(ray, out var hit, maxDistance: Mathf.Infinity, layerMask: draggableLayer))
                {
                    other = hit.collider.gameObject;
                }
                _dragInteractable.OnDrop(other);
            }
            _dragCoroutine = null;
            _dragInteractable = null;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;
            if (_debugRaycastHits.Count < 2) return;
            foreach (var hit in _debugRaycastHits)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(hit, 0.1f);
            }
        }
#endif
    }
}