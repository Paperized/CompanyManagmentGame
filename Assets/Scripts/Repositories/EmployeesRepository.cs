using Data;
using Data.Common;
using Repositories.Common;
using System;
using System.Collections.Generic;

namespace Repositories
{
    [Serializable]
    public class EmployeesData : IDataRepository
    {
        public List<EmployeeData> hiredDevs;
        public List<EmployeeData> hiredAnalysts;

        public List<EmployeeData> employeesCurriculum;

        public void Reset()
        {
            hiredDevs = new List<EmployeeData> ();
            hiredAnalysts = new List<EmployeeData>();

            employeesCurriculum = new List<EmployeeData>();
        }
    }

    public class EmployeesRepository : Repository<EmployeesRepository, EmployeesData>, IDataEventEmitter
    {
        public event Action<List<EmployeeData>> OnPotentialEmployeeChanged;

        public event Action<List<EmployeeData>> OnDevsHiredChanged;
        public event Action<List<EmployeeData>> OnAnalystsHiredChanged;

        public event Action<EmployeeData, bool> OnDevHiredOrFired;
        public event Action<EmployeeData, bool> OnAnalystHiredOrFired;

        public event Action<EmployeeData> OnPotentialDevAccepted;
        public event Action<EmployeeData> OnPotentialAnalystAccepted;
        public event Action<EmployeeData> OnPotentialEmployeeDeclined;

        /// <summary>
        /// Add a potential emplyee, it does not have a specific role, the role is given when hired
        /// </summary>
        /// <param name="employeeData">Potential employee to be added</param>
        /// <returns>true if not already in potentials</returns>
        public bool AddPotentialEmployee(EmployeeData employeeData)
        {
            if (data.employeesCurriculum.Contains(employeeData)) return false;

            data.employeesCurriculum.Add(employeeData);
            OnPotentialEmployeeChanged?.Invoke(data.employeesCurriculum);
            return true;
        }

        /// <summary>
        /// Generic internal method to add or remove an employee based on their role
        /// </summary>
        /// <param name="employeeData">Employee to be hired or fired</param>
        /// <param name="type">Type of employee</param>
        /// <param name="isHired">Is hired or fired</param>
        /// <returns>true if the method had effect</returns>
        private bool HireOrFireEmployee(EmployeeData employeeData, EmployeeType type, bool isHired)
        {
            List<EmployeeData> targetList = EmployeeType.Dev.Equals(type) ? data.hiredDevs : data.hiredAnalysts;

            if (isHired && !targetList.Contains(employeeData))
            {
                employeeData.empType = type;
                targetList.Add(employeeData);
                return true;
            }
            else if (!isHired && targetList.Contains(employeeData))
            {
                targetList.Remove(employeeData);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Hire of fire a Dev
        /// </summary>
        /// <param name="employeeData">Employee</param>
        /// <param name="isHired">is hired or fired</param>
        /// <returns>true if the method had effect</returns>
        public bool HireOrFireDev(EmployeeData employeeData, bool isHired)
        {
            bool result = HireOrFireEmployee(employeeData, EmployeeType.Dev, isHired);
            if(result)
            {
                OnDevHiredOrFired?.Invoke(employeeData, isHired);
                OnDevsHiredChanged?.Invoke(data.hiredDevs);
            }

            return result;
        }

        /// <summary>
        /// Hire of fire a Analyst
        /// </summary>
        /// <param name="employeeData">Employee</param>
        /// <param name="isHired">is hired or fired</param>
        /// <returns>true if the method had effect</returns>
        public bool HireOrFireAnalyst(EmployeeData employeeData, bool isHired)
        {
            bool result = HireOrFireEmployee(employeeData, EmployeeType.Analyst, isHired);
            if (result)
            {
                OnAnalystHiredOrFired?.Invoke(employeeData, isHired);
                OnAnalystsHiredChanged?.Invoke(data.hiredAnalysts);
            }

            return result;
        }

        /// <summary>
        /// Accept an employee proposal as dev
        /// </summary>
        /// <param name="id">id employee</param>
        /// <returns>true if the operation had effect</returns>
        public bool AcceptPotentialDev(string id)
        {
            EmployeeData potentialDev = data.employeesCurriculum.Find(x => id.Equals(x.id));

            if (potentialDev is null) return false;

            bool result = HireOrFireDev(potentialDev, true);
            if(result)
            {
                data.employeesCurriculum.Remove(potentialDev);
                OnPotentialDevAccepted?.Invoke(potentialDev);
                OnPotentialEmployeeChanged?.Invoke(data.employeesCurriculum);
            }

            return false;
        }

        /// <summary>
        /// Accept an employee proposal as analyst
        /// </summary>
        /// <param name="id">id employee</param>
        /// <returns>true if the operation had effect</returns>
        public bool AcceptPotentialAnalyst(string id)
        {
            EmployeeData potentialAnalyst = data.employeesCurriculum.Find(x => id.Equals(x.id));

            if (potentialAnalyst is null) return false;

            bool result = HireOrFireAnalyst(potentialAnalyst, true);
            if (result)
            {
                data.employeesCurriculum.Remove(potentialAnalyst);
                OnPotentialAnalystAccepted?.Invoke(potentialAnalyst);
                OnPotentialEmployeeChanged?.Invoke(data.employeesCurriculum);
            }

            return false;
        }

        /// <summary>
        /// Decline an employee proposal
        /// </summary>
        /// <param name="id">id employee</param>
        /// <returns>true if the operation had effect</returns>
        public bool DeclinePotentialEmployee(string id)
        {
            EmployeeData potentialEmployee = data.employeesCurriculum.Find(x => id.Equals(x.id));

            if (potentialEmployee is null) return false;

            bool result = data.employeesCurriculum.Remove(potentialEmployee);
            if(result)
            {
                OnPotentialEmployeeDeclined?.Invoke(potentialEmployee);
                OnPotentialEmployeeChanged?.Invoke(data.employeesCurriculum);
            }

            return result;
        }

        /// <summary>
        /// Manually trigger data broadcast
        /// </summary>
        public void BroadcastAllData()
        {
            OnDevsHiredChanged?.Invoke(data.hiredDevs);
            OnAnalystsHiredChanged?.Invoke(data.hiredAnalysts);

            OnPotentialEmployeeChanged?.Invoke(data.employeesCurriculum);
        }

        public override string FileName() => "playerHires";
    }
}
