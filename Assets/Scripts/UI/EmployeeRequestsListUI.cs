using Common;
using Data;
using Repositories;
using System.Collections.Generic;
using TMPro;
using UI.Sub;
using UnityEngine;

namespace UI
{
    public class EmployeeRequestsListUI : MonoBehaviour
    {
        [SerializeField]
        private EmployeeElementSubUI prefabEmployeeSmallUI;

        [SerializeField]
        private TextMeshProUGUI potentialTextCounter;

        [SerializeField]
        private GameObject container;

        private void Start()
        {
            container.ClearChildren();

            EmployeesRepository employees = EmployeesRepository.RequireInstance;
            employees.OnPotentialEmployeeChanged += Employees_OnPotentialEmployeeChanged;
        }

        private void Employees_OnPotentialEmployeeChanged(List<Data.EmployeeData> obj)
        {
            List<Transform> potentialEmployee = container.GetChildren();

            foreach (Data.EmployeeData employee in obj)
            {
                int transformIndex = potentialEmployee.FindIndex(x => x.name.Equals(employee.id));

                EmployeeElementSubUI ui;
                if (transformIndex == -1)
                {
                    ui = Instantiate(prefabEmployeeSmallUI, container.transform);
                    ui.name = employee.id;
                    ui.employeeButton.onClick.AddListener(() => OpenPotentialHireDialog(employee));
                }
                else
                {
                    ui = potentialEmployee[transformIndex].GetComponent<EmployeeElementSubUI>();
                    potentialEmployee.RemoveAt(transformIndex);
                    ui.employeeButton.onClick.RemoveAllListeners();
                    ui.employeeButton.onClick.AddListener(() => OpenPotentialHireDialog(employee));
                }

                ui.SetEmployeeData(employee);
            }

            foreach (Transform transform in potentialEmployee)
            {
                Destroy(transform.gameObject);
            }

            potentialTextCounter.text = "Potential Employees: " + obj.Count;
        }

        private void OpenPotentialHireDialog(EmployeeData employeeData)
        {
            EmployeeDetailUI.RequireInstance.SetupUI(employeeData);
            EmployeeDetailUI.RequireInstance.ShowDialog(true);
        }
    }
}
