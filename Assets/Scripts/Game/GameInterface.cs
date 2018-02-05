using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class GameInterface : MonoBehaviour
    {
        [SerializeField] private Text _attemptsText;
        [SerializeField] private GameObject _loosMessage;
        [SerializeField] private GameObject _winMessage;

        public void OnAttemptsChanged(int attempts)
        {
            _attemptsText.text = attempts.ToString();
        }

        public void OnWin()
        {
            StartCoroutine(ShowWinMessage());
        }


        public void OnLose()
        {
            StartCoroutine(ShowLoseMessage());
        }

        private void Start()
        {
            _winMessage.gameObject.SetActive(false);
            _loosMessage.gameObject.SetActive(false);
        }

        private IEnumerator ShowWinMessage()
        {
            _winMessage.gameObject.SetActive(true);
            yield return new WaitForSeconds(5f);
            _winMessage.gameObject.SetActive(false);
        }

        private IEnumerator ShowLoseMessage()
        {
            _loosMessage.gameObject.SetActive(true);
            yield return new WaitForSeconds(5f);
            _loosMessage.gameObject.SetActive(false);
        }
    }
}