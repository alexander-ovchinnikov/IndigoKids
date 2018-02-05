using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.GUIElements
{
    public class TextButton : UIBehaviour, IPointerClickHandler
    {
        [SerializeField] private Text _text;
        private Action OnClick { get; set; }

        private string Text
        {
            set { _text.text = value; }
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick();
        }

        public static TextButton Instantiate(TextButton obj, Transform transform, string text, Action action,
            bool active = true)
        {
            var textButton = Instantiate(obj, transform);
            textButton.Text = text;
            textButton.OnClick += action;
            textButton.gameObject.SetActive(active);
            return textButton;
        }

        public TextButton Duplicate(Transform parent, string text, Action action, bool active = true)
        {
            return Instantiate(
                this, parent, text, action, active
            );
        }


        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}