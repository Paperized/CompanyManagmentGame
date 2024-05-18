using UnityEngine;

namespace Repositories.Common
{
    public interface IRepositoryLifecycle
    {
        void LoadData();
        void SaveData();
    }
}