using Common;
using Repositories;
using Unity.Mathematics;
using UnityEngine;

namespace Systems
{
    public class MoneyCalculator : DependsOnGameState
    {
        private CompanyRepository playerDataRepository;
        private EmployeesRepository employeesRepository;

        [SerializeField]
        private long cashPerTime;

        [SerializeField]
        private float nextMoneyTime;
        private float nextMoneyTimeCurrent;

        // Use this for initialization
        protected override void Start()
        {
            base.Start();

            playerDataRepository = CompanyRepository.RequireInstance;
            employeesRepository = EmployeesRepository.RequireInstance;
        }

        protected override void Update()
        {
            base.Update();

            nextMoneyTimeCurrent += Time.deltaTime;
            if (nextMoneyTimeCurrent >= nextMoneyTime)
            {
                nextMoneyTimeCurrent -= nextMoneyTime;
                playerDataRepository.AddMoney(CalculateNextMoneyGain());
            }
        }

        private long CalculateNextMoneyGain()
        {
            EmployeesData employees = employeesRepository.GetData();
            float devExtra = employees.hiredDevs.Count * 2f;
            float analystExtra = employees.hiredAnalysts.Count * 2f;

            float deductedPercentage = math.min(employees.hiredDevs.Count, employees.hiredAnalysts.Count) / math.max(1f, math.max(employees.hiredDevs.Count, employees.hiredAnalysts.Count));

            float totalAdded = (devExtra + analystExtra) * deductedPercentage;

            Debug.LogFormat("Added {0} money extra because of employees", (long)totalAdded);

            return cashPerTime + (long)totalAdded;
        }
    }
}