using UnityEngine;
using UnityEngine.UI;

namespace Game.GUIElements
{
    public class TextItem : MonoBehaviour
    {
        private const string LETTER_PLACEHOLDER = "?";
        private string _originText = string.Empty;
        [SerializeField] private Text _text;

        private string Text
        {
            get { return _text.text; }
            set { _text.text = value; }
        }

        private static TextItem Instantiate(TextItem obj, Transform transform, string text, bool active = true)
        {
            var textItem = Instantiate(obj, transform);
            textItem.Text = text;
            textItem.gameObject.SetActive(active);
            return textItem;
        }

        public TextItem Duplicate(Transform parent, string text, bool active = true)
        {
            return Instantiate(
                this, parent, text, active
            );
        }

        private void Start()
        {
            _originText = Text;
            Hide();
        }

        public void Show()
        {
            Text = _originText;
        }

        public void Hide()
        {
            Text = LETTER_PLACEHOLDER;
        }
    }
}