using Common;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class DialogPanelUI : SingletonBehaviour<DialogPanelUI>
    {
        [SerializeField]
        private Button clickableBackground;

        [SerializeField]
        private Button quitButton;

        [SerializeField]
        private TextMeshProUGUI titleText;

        [SerializeField]
        private TextMeshProUGUI descriptionText;

        [SerializeField]
        private Button okButton;

        [SerializeField]
        private Button koButton;

        [SerializeField]
        private TextMeshProUGUI okButtonText;

        [SerializeField]
        private TextMeshProUGUI koButtonText;

        private Action onClosedCallback;

        private void Start()
        {
            ShowDialog(false);

            clickableBackground.onClick.AddListener(OnClosed);
            quitButton.onClick.AddListener(OnClosed);
        }

        private void OnDestroy()
        {
            clickableBackground.onClick.RemoveAllListeners();
            quitButton.onClick.RemoveAllListeners();
        }

        public void SetupDialog(string title, string description, string okText, string koText, bool showQuitBtn)
        {
            titleText.text = title;
            descriptionText.text = description;
            okButtonText.text = okText;
            koButtonText.text = koText;

            quitButton.gameObject.SetActive(showQuitBtn);
        }

        public void ShowDialog(bool show)
        {
            gameObject.SetActive(show);
        }

        public void Subscribe(Action ok, Action ko)
        {
            Subscribe(ok, ko, null);
        }

        public void Subscribe(Action ok, Action ko, Action closed)
        {
            onClosedCallback = closed;

            okButton.onClick.RemoveAllListeners();
            koButton.onClick.RemoveAllListeners();

            okButton.onClick.AddListener(() => ok());
            koButton.onClick.AddListener(() => ko());
        }

        private void OnClosed()
        {
            ShowDialog(false);

            onClosedCallback?.Invoke();
        }
    }
}
