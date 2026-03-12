using SimpleMedievalPlatformer.Systems;
using UnityEngine;
using UnityEngine.UI;

namespace SimpleMedievalPlatformer.UI
{
    [DisallowMultipleComponent]
    public sealed class MobileControlPanelBuilder : MonoBehaviour
    {
        private void Awake()
        {
            RectTransform root = GetComponent<RectTransform>();
            root.anchorMin = Vector2.zero;
            root.anchorMax = Vector2.one;
            root.offsetMin = Vector2.zero;
            root.offsetMax = Vector2.zero;

            Build();
        }

        private void Build()
        {
            Image leftPad = RuntimeFactory.CreatePanel("LeftPad", transform, new Color(0f, 0f, 0f, 0f));
            RectTransform leftPadRect = leftPad.rectTransform;
            leftPadRect.anchorMin = new Vector2(0f, 0f);
            leftPadRect.anchorMax = new Vector2(0f, 0f);
            leftPadRect.anchoredPosition = new Vector2(230f, 150f);
            leftPadRect.sizeDelta = new Vector2(360f, 180f);

            CreateMoveButton(leftPad.transform, "Left", "<", new Vector2(-80f, 0f), -1f);
            CreateMoveButton(leftPad.transform, "Right", ">", new Vector2(80f, 0f), 1f);

            Image rightPad = RuntimeFactory.CreatePanel("RightPad", transform, new Color(0f, 0f, 0f, 0f));
            RectTransform rightPadRect = rightPad.rectTransform;
            rightPadRect.anchorMin = new Vector2(1f, 0f);
            rightPadRect.anchorMax = new Vector2(1f, 0f);
            rightPadRect.anchoredPosition = new Vector2(-250f, 160f);
            rightPadRect.sizeDelta = new Vector2(520f, 240f);

            CreateActionButton(rightPad.transform, "Jump", "Jump", new Vector2(-120f, 50f), new Color(0.2f, 0.45f, 0.8f), () => GameManager.Instance.Player?.PressJump());
            CreateActionButton(rightPad.transform, "Attack", "Atk", new Vector2(40f, 50f), new Color(0.85f, 0.28f, 0.22f), () => GameManager.Instance.Player?.PressAttack());
            CreateActionButton(rightPad.transform, "Switch", "Swap", new Vector2(200f, 50f), new Color(0.4f, 0.3f, 0.7f), () => GameManager.Instance.Player?.SwitchWeapon());
            CreateActionButton(rightPad.transform, "Pause", "II", new Vector2(200f, -60f), new Color(0.35f, 0.35f, 0.35f), () => GameManager.Instance.TogglePause());
        }

        private void CreateMoveButton(Transform parent, string name, string label, Vector2 anchoredPosition, float direction)
        {
            Button button = RuntimeFactory.CreateButton(name, parent, label, new Vector2(140f, 140f), new Color(0f, 0f, 0f, 0.42f), null);
            RectTransform rect = (RectTransform)button.transform;
            rect.anchoredPosition = anchoredPosition;

            MobileButtonHold hold = button.gameObject.AddComponent<MobileButtonHold>();
            hold.Direction = direction;
        }

        private void CreateActionButton(Transform parent, string name, string label, Vector2 anchoredPosition, Color color, UnityEngine.Events.UnityAction action)
        {
            Button button = RuntimeFactory.CreateButton(name, parent, label, new Vector2(130f, 96f), color, action);
            RectTransform rect = (RectTransform)button.transform;
            rect.anchoredPosition = anchoredPosition;
        }
    }
}
