using SimpleMedievalPlatformer.Systems;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SimpleMedievalPlatformer.UI
{
    public sealed class MobileButtonHold : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        public float Direction { get; set; }

        public void OnPointerDown(PointerEventData eventData)
        {
            GameManager.Instance.Player?.SetTouchMove(Direction);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            GameManager.Instance.Player?.ClearTouchMove();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            GameManager.Instance.Player?.ClearTouchMove();
        }

        private void OnDisable()
        {
            if (GameManager.Instance != null && GameManager.Instance.Player != null)
            {
                GameManager.Instance.Player.ClearTouchMove();
            }
        }
    }
}
