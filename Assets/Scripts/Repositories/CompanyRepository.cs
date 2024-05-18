using Repositories.Common;
using System;

namespace Repositories
{
    [Serializable]
    public class CompanyData : IDataRepository
    {
        public string companyName;
        public long money;

        public void Reset()
        {
            money = 0;
        }
    }

    public class CompanyRepository : Repository<CompanyRepository, CompanyData>, IDataEventEmitter
    {
        public event Action<string> OnCompanyNameChanged;
        public event Action<long> OnMoneyChanged;

        public long SetMoney(long money)
        {
            data.money = money;
            OnMoneyChanged?.Invoke(money);
            return money;
        }

        public long AddMoney(long money)
        {
            data.money += money;
            OnMoneyChanged?.Invoke(data.money);
            return data.money;
        }

        public void BroadcastAllData()
        {
            OnCompanyNameChanged?.Invoke(data.companyName);
            OnMoneyChanged?.Invoke(data.money);
        }

        public override string FileName() => "playerData";
    }
}
