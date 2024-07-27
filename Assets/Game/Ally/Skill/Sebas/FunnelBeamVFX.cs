using System;
using UnityEngine;

namespace Lion.Ally.Skill
{
    public class FunnelBeamVFX : MonoBehaviour
    {
        [SerializeField]
        private LineRenderer _lineRenderer;
        [SerializeField]
        private float _lifeTime = 0.2f;

        private Transform _from;
        private Vector3 _to;
        private Vector3[] _positions = new Vector3[2];

        public void Initialize(Transform from, Vector3 to)
        {
            _from = from;
            _to = to;

            _positions[0] = from.position;
            _positions[1] = to;
            _lineRenderer.SetPositions(_positions);
        }

        public void Update()
        {
            _positions[0] = _from.position;
            _positions[1] = _to;
            _lineRenderer.SetPositions(_positions);

            _lifeTime -= Time.deltaTime;
            if (_lifeTime < 0) Destroy(gameObject);
        }
    }
}