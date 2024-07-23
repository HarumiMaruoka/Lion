using System;
using UnityEngine;

namespace Lion.CameraUtility
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform _follow;

        private void Update()
        {
            var pos = _follow.position;
            transform.position = new Vector3(pos.x, pos.y, transform.position.z);
        }
    }
}