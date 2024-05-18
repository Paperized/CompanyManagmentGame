using UnityEngine;
using TMPro;
using Repositories;
using Data;
using System.Collections.Generic;
using System;

namespace UI
{
    public class SummaryPanelUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI moneyText;
        [SerializeField]
        private TextMeshProUGUI devsCountText;
        [SerializeField]
        private TextMeshProUGUI analystsCountText;

        private void Awake()
        {
            EmployeesRepository employeesRepository = EmployeesRepository.RequireInstance;
            employeesRepository.OnDataLoaded += EmployeesRepository_OnDataLoaded;
            employeesRepository.OnAnalystsHiredChanged += EmployeesRepository_OnAnalystsHiredChanged;
            employeesRepository.OnDevsHiredChanged += EmployeesRepository_OnDevsHiredChanged;

            CompanyRepository playerDataRepository = CompanyRepository.RequireInstance;
            playerDataRepository.OnDataLoaded += PlayerDataRepository_OnDataLoaded;
            playerDataRepository.OnMoneyChanged += PlayerDataRepository_OnMoneyChanged;
        }

        private void OnDestroy()
        {
            EmployeesRepository employeesRepository = EmployeesRepository.RequireInstance;
            employeesRepository.OnDataLoaded -= EmployeesRepository_OnDataLoaded;
            employeesRepository.OnAnalystsHiredChanged -= EmployeesRepository_OnAnalystsHiredChanged;
            employeesRepository.OnDevsHiredChanged -= EmployeesRepository_OnDevsHiredChanged;

            CompanyRepository playerDataRepository = CompanyRepository.RequireInstance;
            playerDataRepository.OnDataLoaded -= PlayerDataRepository_OnDataLoaded;
            playerDataRepository.OnMoneyChanged -= PlayerDataRepository_OnMoneyChanged;
        }

        private void PlayerDataRepository_OnMoneyChanged(long money)
        {
            moneyText.text = "Money: " + GetMoneyAmountWithScale(money);
        }

        private void PlayerDataRepository_OnDataLoaded(CompanyData obj)
        {
            moneyText.text = "Money: " + GetMoneyAmountWithScale(obj.money);
        }

        private void EmployeesRepository_OnDataLoaded(EmployeesData obj)
        {
            devsCountText.text = "Devs: " + obj.hiredDevs.Count.ToString();
            analystsCountText.text = "Alyts: " + obj.hiredAnalysts.Count.ToString();
        }

        private void EmployeesRepository_OnDevsHiredChanged(List<EmployeeData> list)
        {
            devsCountText.text = "Devs: " + list.Count.ToString();
        }

        private void EmployeesRepository_OnAnalystsHiredChanged(List<EmployeeData> list)
        {
            analystsCountText.text = "Alyts: " + list.Count.ToString();
        }

        private string GetMoneyAmountWithScale(long money)
        {
            // TODO controllare per gli M e B
            if (money > 1000000000000)
            {
                int milions = (int)(money / 1000000000f);
                return milions + "B";
            }

            if (money > 1000000000)
            {
                int milions = (int)(money / 1000000f);
                return milions + "M";
            }

            if (money > 100000)
            {
                int milions = (int)(money / 1000f);
                return milions + "K";
            }

            return money.ToString();
        }
    }
}