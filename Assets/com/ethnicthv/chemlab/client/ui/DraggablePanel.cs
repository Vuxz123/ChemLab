using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace com.ethnicthv.chemlab.client.ui
{
    public class DraggablePanel : MonoBehaviour, IDragHandler, IBeginDragHandler
    {
        public RectTransform mainPanel;
        
        private Vector2 _pointerOffset;
        private RectTransform _canvasRectTransform;

        public void OnBeginDrag(PointerEventData eventData)
        {
            var position = mainPanel.position;
            var mousePosition = Input.mousePosition;
            _pointerOffset = mousePosition - position;
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            if (mainPanel == null) return;
            var mousePosition = Input.mousePosition;
            var pointerPosition = ClampToWindow(mousePosition);
            Vector3 position = pointerPosition - _pointerOffset;
            mainPanel.position = position;
        }
        
        private Vector2 ClampToWindow(Vector3 mousePosition)
        {
            var rawPointerPosition = mousePosition;
            var localPointerPosition = rawPointerPosition;
            
            var sizeDelta = mainPanel.sizeDelta;
            var halfWidth = sizeDelta.x * 0.5f;
            var halfHeight = sizeDelta.y * 0.5f;
            
            var position = mainPanel.position;
            // position.x = Mathf.Clamp(localPointerPosition.x, -halfWidth, halfWidth);
            // position.y = Mathf.Clamp(localPointerPosition.y, -halfHeight, halfHeight);

            position = localPointerPosition;
            
            return position;
        }
    }
}