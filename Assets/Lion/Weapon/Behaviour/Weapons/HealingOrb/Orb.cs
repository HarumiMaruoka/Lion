using Lion.CameraUtility;
using System;
using System.Collections;
using UnityEngine;

namespace Lion.Weapon.Behaviour.HealingOrbModules
{
    public class Orb : MonoBehaviour
    {
        public IWeaponParameter Parameter { get; set; }

        [SerializeField]
        private HealingArea _healingSpotPrefab;

        [SerializeField]
        private float _fallingDuration = 1f;

        private void Start()
        {
            StartCoroutine(FallAsync());
        }

        private Vector3 GetTargetPosition()
        {
            var top = Camera.main.GetWorldTopRight().y;
            var bottom = Camera.main.GetWorldBottomLeft().y;

            var x = transform.position.x;
            var y = UnityEngine.Random.Range(bottom, top);

            return new Vector3(x, y, 0);
        }

        private IEnumerator FallAsync()
        {
            var targetPosition = GetTargetPosition();
            var startPosition = transform.position;
            var duration = _fallingDuration;

            for (float t = 0f; t < duration; t += Time.deltaTime)
            {
                var x = transform.position.x;
                var y = Mathf.Lerp(startPosition.y, targetPosition.y, t / duration);

                transform.position = new Vector3(x, y, 0);
                yield return null;
            }

            transform.position = targetPosition;

            var healingSpot = Instantiate(_healingSpotPrefab, transform.position, Quaternion.Euler(90, 0, 0));
            healingSpot.Parameter = Parameter;

            Destroy(gameObject);
        }
    }
}