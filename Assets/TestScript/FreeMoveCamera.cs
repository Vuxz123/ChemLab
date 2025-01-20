using System;
using UnityEngine;

namespace TestScript
{
    public class FreeMoveCamera : MonoBehaviour
    {
        private void Update()
        {
            var moveSpeed = 10f;
            var rotateSpeed = 100f;
            var dt = Time.deltaTime;
            var dx = Input.GetAxis("Horizontal") * moveSpeed * dt;
            var dy = Input.GetAxis("Vertical") * moveSpeed * dt;
            var dz = Input.GetAxis("Depth") * moveSpeed * dt;
            var drx = Input.GetAxis("Mouse Y") * rotateSpeed * dt;
            var dry = Input.GetAxis("Mouse X") * rotateSpeed * dt;
            var drz = Input.GetAxis("Roll") * rotateSpeed * dt;
            transform.Translate(dx, dy, dz);
            transform.Rotate(drx, dry, drz);
        }
    }
}