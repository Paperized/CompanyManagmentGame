using Data.Common;
using System;

namespace Data
{
    [Serializable]
    public class ProjectData : DataEntity
    {
        public string name;
        public string imageName;
        public string description;
        public long moneyGained;
        public long moneySpent;


        public override string ToString()
        {
            return $"{{id: {id}, name: {name}, imageName: {imageName}, description: {description}, moneyGained: {moneyGained}, moneySpent: {moneySpent}}}";
        }
    }
}
