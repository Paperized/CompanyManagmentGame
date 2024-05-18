using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static readonly Dictionary<Type, object> instances = new();

        public static T Instance
        {
            get
            {
                return instances.TryGetValue(typeof(T), out object instance) ? instance as T : null;
            }
            private set
            {
                instances[typeof(T)] = value;
            }
        }

        public static T RequireInstance
        {
            get
            {
                T instance = Instance;
                if (instance == null)
                {
                    Debug.LogErrorFormat("Singleton of type {0} is not yet instanciated", typeof(T).Name);
                }

                return instance;
            }
        }

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
                Debug.LogFormat("Registered singleton of type {0}", typeof(T).Name);
            }
            else
            {
                Debug.LogErrorFormat("Singleton of type {0} violated by entity '{1}'", typeof(T).Name, name);
            }
        }
    }
}
