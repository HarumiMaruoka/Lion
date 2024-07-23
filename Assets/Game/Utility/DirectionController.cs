using System;
using UnityEngine;

namespace Lion.Ally.Utility
{
    public class DirectionController : MonoBehaviour
    {
        private Vector2 _lastPos;

        private void Start()
        {
            _lastPos = transform.position;
        }

        private void Update()
        {
            Vector2 currentPos = transform.position;
            if (_lastPos != currentPos) UpdateDirection(currentPos - _lastPos);
            _lastPos = currentPos;
        }

        private void UpdateDirection(Vector2 direction)
        {
            if (direction.x > 0 && transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
            }
            else if (direction.x < 0 && transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
            }
        }
    }
}
