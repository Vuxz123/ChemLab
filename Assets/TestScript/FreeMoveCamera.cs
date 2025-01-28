using com.ethnicthv.chemlab.client.unity.renderer;
using UnityEngine;

namespace TestScript
{
    public class FreeMoveCamera : MonoBehaviour
    {
        public Camera mainCamera;
        [SerializeField] private float rotateSpeed = 100f;
        [SerializeField] private float zoomSpeed = 10f;
        [SerializeField] private float zoom = -25f;
        
        private void Update()
        {
            var transform1 = transform;

            if (RenderProgram.Instance.HasAnyRenderEntity())
            {
                var (lower, higer) = RenderProgram.Instance.GetBound(0);
            
                var center = (lower + higer) / 2;
            
                transform1.position = center;
            }

            if (Input.GetMouseButton(0))
            {
                var x = Input.GetAxis("Mouse X") * rotateSpeed * Mathf.Deg2Rad;
                var y = Input.GetAxis("Mouse Y") * rotateSpeed * Mathf.Deg2Rad;

                transform1.Rotate(Vector3.up, y);
                transform1.Rotate(Vector3.right, -x);
            }
            
            zoom += Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

            Transform transform2;
            (transform2 = mainCamera.transform).LookAt(transform1.position);
            transform2.localPosition = new Vector3(0, 0, zoom);
            
        }
    }
}