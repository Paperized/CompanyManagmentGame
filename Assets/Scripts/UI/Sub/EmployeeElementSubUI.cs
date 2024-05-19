using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Data;
using System;

namespace UI.Sub
{
    public class EmployeeElementSubUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI employeeName;
        [SerializeField]
        private Image employeeImage;

        public Button employeeButton;

        public void SetEmployeeData(EmployeeData employeeData)
        {
            name = employeeData.id;
            employeeName.text = employeeData.name;
        }
    }
}
