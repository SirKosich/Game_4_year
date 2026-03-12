using UnityEngine;

namespace SimpleMedievalPlatformer.Systems
{
    [DisallowMultipleComponent]
    public sealed class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Vector3 offset = new Vector3(0f, 1f, -10f);
        [SerializeField] private float smoothTime = 0.15f;
        [SerializeField] private Vector2 minPosition = new Vector2(-100f, -100f);
        [SerializeField] private Vector2 maxPosition = new Vector2(100f, 100f);

        private Transform target;
        private Vector3 velocity;

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }

        public void SetBounds(Vector2 min, Vector2 max)
        {
            minPosition = min;
            maxPosition = max;
        }

        private void LateUpdate()
        {
            if (target == null)
            {
                return;
            }

            Vector3 desired = target.position + offset;
            desired.x = Mathf.Clamp(desired.x, minPosition.x, maxPosition.x);
            desired.y = Mathf.Clamp(desired.y, minPosition.y, maxPosition.y);

            transform.position = Vector3.SmoothDamp(transform.position, desired, ref velocity, smoothTime);
        }
    }
}
