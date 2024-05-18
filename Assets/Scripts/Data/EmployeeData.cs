using Data.Common;
using System;

namespace Data
{
    [Serializable]
    public class EmployeeData : DataEntity
    {
        public string name;
        public string imageName;
        public EmployeeType empType;

        public override string ToString()
        {
            return $"{{id: {id}, name: {name}, imageName: {imageName}, empType: {empType}}}";
        }
    }
}
