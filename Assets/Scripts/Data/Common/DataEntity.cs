using System;

namespace Data.Common
{
    [Serializable]
    public abstract class DataEntity : IEquatable<DataEntity>
    {
        public string id;

        public bool Equals(DataEntity other)
        {
            if(other.GetType() != GetType()) return false;

            return string.Equals(other.id, id);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as DataEntity);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(id);
        }
    }
}
