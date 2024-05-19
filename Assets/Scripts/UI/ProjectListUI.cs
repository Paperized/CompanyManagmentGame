using Common;
using Data;
using Repositories;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.Sub;
using UnityEngine;

namespace UI
{
    public class ProjectListUI : MonoBehaviour
    {
        [SerializeField]
        private ProjectElementSubUI prefabProjectSmallUI;

        [SerializeField]
        private TextMeshProUGUI titleDialogWithCounter;

        [SerializeField]
        private GameObject container;

        private void Start()
        {
            container.ClearChildren();

            ProjectsRepository projectsRepository = ProjectsRepository.RequireInstance;
            projectsRepository.OnPotentialProjectsChanged += Projects_OnPotentialProjectChanged;
        }

        private void Projects_OnPotentialProjectChanged(List<ProjectData> obj)
        {
            List<Transform> potentialProjects = container.GetChildren();

            foreach (ProjectData project in obj)
            {
                int transformIndex = potentialProjects.FindIndex(x => x.name.Equals(project.id));

                ProjectElementSubUI ui;
                if (transformIndex == -1)
                {
                    ui = Instantiate(prefabProjectSmallUI, container.transform);
                    ui.name = project.id;
                    ui.ProjectButton.onClick.AddListener(() => OpenPotentialProjectDialog(project));
                }
                else
                {
                    ui = potentialProjects[transformIndex].GetComponent<ProjectElementSubUI>();
                    potentialProjects.RemoveAt(transformIndex);
                    ui.ProjectButton.onClick.RemoveAllListeners();
                    ui.ProjectButton.onClick.AddListener(() => OpenPotentialProjectDialog(project));
                }

                ui.SetProjectData(project);
            }

            foreach (Transform transform in potentialProjects)
            {
                Destroy(transform.gameObject);
            }

            titleDialogWithCounter.text = $"Projects ({obj.Count})";
        }

        private void OpenPotentialProjectDialog(ProjectData projectData)
        {
            ProjectDetailUI.RequireInstance.SetupUI(projectData);
            ProjectDetailUI.RequireInstance.ShowDialog(true);
        }
    }
}