using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public static class GameObjectExtensions
    {
        public static List<Transform> GetChildren(this GameObject gameObject)
        {
            List<Transform> children = new();
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                children.Add(gameObject.transform.GetChild(i));
            }

            return children;
        }

        public static List<Transform> GetChildren(this Transform transform)
        {
            return GetChildren(transform.gameObject);
        }

        public static void ClearChildren(this GameObject gameObject)
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                Object.Destroy(gameObject.transform.GetChild(i).gameObject);
            }
        }

        public static void ClearChildren(this Transform transform)
        {
            ClearChildren(transform.gameObject);
        }
    }
}
