using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Sub
{
    public class ProjectElementSubUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI projectName;

        [field: SerializeField]
        public Button ProjectButton { get; private set; }

        public void SetProjectData(ProjectData projectData)
        {
            name = projectData.id;
            projectName.text = projectData.name;
        }
    }
}