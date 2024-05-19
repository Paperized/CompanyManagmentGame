using Common;
using Data;
using Repositories;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ProjectDetailUI : SingletonBehaviour<ProjectDetailUI>
    {
        private string currentProjectId;

        [SerializeField]
        private Button quitButton;

        [SerializeField]
        private TextMeshProUGUI projectNameText;

        [SerializeField]
        private Button acceptButton;

        [SerializeField]
        private Button declineButton;

        private void Start()
        {
            ShowDialog(false);

            acceptButton.onClick.AddListener(AcceptProject);
            declineButton.onClick.AddListener(DeclineProject);
            quitButton.onClick.AddListener(OnClosed);
        }

        private void OnDestroy()
        {
            acceptButton.onClick.RemoveAllListeners();
            declineButton.onClick.RemoveAllListeners();
            quitButton.onClick.RemoveAllListeners();
        }

        public void SetupUI(ProjectData projectData)
        {
            currentProjectId = projectData.id;

            projectNameText.text = projectData.name;
        }

        public void AcceptProject()
        {
            ProjectsRepository.RequireInstance.AcceptOrDeclinePotentialProject(currentProjectId, true);
            OnClosed();
        }

        public void DeclineProject()
        {
            ProjectsRepository.RequireInstance.AcceptOrDeclinePotentialProject(currentProjectId, false);
            OnClosed();
        }

        public void ShowDialog(bool show)
        {
            gameObject.SetActive(show);
        }

        private void OnClosed()
        {
            ShowDialog(false);
            currentProjectId = null;
        }
    }
}
