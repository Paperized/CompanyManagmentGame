using Common;
using Data;
using Repositories;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class EmployeeDetailUI : SingletonBehaviour<EmployeeDetailUI>
    {
        private string currentEmployeeId;

        [SerializeField]
        private Button quitButton;

        [SerializeField]
        private TextMeshProUGUI employeeNameText;

        [SerializeField]
        private Image employeeImage;

        [SerializeField]
        private Button hireAsDevButton;

        [SerializeField]
        private Button hireAsAnalystButton;

        [SerializeField]
        private Button declineProposalButton;

        private void Start()
        {
            ShowDialog(false);

            hireAsDevButton.onClick.AddListener(HireAsDev);
            hireAsAnalystButton.onClick.AddListener(HireAsAnalyst);
            declineProposalButton.onClick.AddListener(DeclineProposal);
            quitButton.onClick.AddListener(OnClosed);
        }

        private void OnDestroy()
        {
            hireAsDevButton.onClick.RemoveAllListeners();
            hireAsAnalystButton.onClick.RemoveAllListeners();
            declineProposalButton.onClick.RemoveAllListeners();
            quitButton.onClick.RemoveAllListeners();
        }

        public void SetupUI(EmployeeData employeeData)
        {
            currentEmployeeId = employeeData.id;

            employeeNameText.text = employeeData.name;
            //employeeImage.sprite = ??
        }

        public void HireAsDev()
        {
            EmployeesRepository.RequireInstance.AcceptPotentialDev(currentEmployeeId);
            OnClosed();
        }

        public void HireAsAnalyst()
        {
            EmployeesRepository.RequireInstance.AcceptPotentialAnalyst(currentEmployeeId);
            OnClosed();
        }

        public void DeclineProposal()
        {
            EmployeesRepository.RequireInstance.DeclinePotentialEmployee(currentEmployeeId);
            OnClosed();
        }

        public void ShowDialog(bool show)
        {
            gameObject.SetActive(show);
        }

        private void OnClosed()
        {
            ShowDialog(false);
            currentEmployeeId = null;
        }
    }
}
