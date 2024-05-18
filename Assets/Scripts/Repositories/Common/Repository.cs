using Common;
using Repositories.Common;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Repositories.Common
{
    /// <summary>
    /// Base repository class, it includes all the basic implementations such as:
    /// 
    /// <list type="bullet">
    /// <item>Load and Save data</item>
    /// <item>Define path to the data</item>
    /// <item>Expose Data and Trigger data event when loaded from disk</item>
    /// <item>Basic backup feature in case of bad formatted file to avoid permanent data loss</item>
    /// </list>
    /// 
    /// <para>
    /// All classes that inhredit will have their Load and Save automatically called by the GameManager, if extending from IDataEmitter the BroadcastData will
    /// also be call after loading data to notify all objects listening to the OnDataLoaded event.
    /// </para>
    /// <para>
    /// The current implemention uses JSON files
    /// </para>
    /// 
    /// <strong>Note</strong>: The class that inheredit from this will be a singleton and so use .RequireInstance or .Instance to retrieve a reference to it
    /// </summary>
    /// <typeparam name="T">subclass repository class, used to make a singleton out of it since only one repository of one type can be made at a time</typeparam>
    /// <typeparam name="D">type of data loaded from disk, a class that group all the data inside the json file</typeparam>
    public abstract class Repository<T, D> : SingletonBehaviour<T>, IRepositoryLifecycle
        where T : MonoBehaviour
        where D : IDataRepository, new()
    {
        [SerializeField]
        protected D data;
        public event Action<D> OnDataLoaded = delegate { };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public D GetData() => data;

        protected void InvokeOnDataLoaded()
        {
            OnDataLoaded.Invoke(data);
        }

        public void LoadData()
        {
            string source = AbsoluteDataPath();
            D data = default;

            if (File.Exists(source))
            {
                byte[] fileBytes = File.ReadAllBytes(source);
                if (fileBytes.Length > 2)
                {
                    string fileText = System.Text.Encoding.Default.GetString(fileBytes);

                    try
                    {
                        data = JsonUtility.FromJson<D>(fileText);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogException(ex);
                        BackupData();
                        File.Delete(source);
                    }
                }
            }

            if (data == null)
            {
                data = new D();
                data.Reset();
            }


            this.data = data;
            InvokeOnDataLoaded();
        }

        private void BackupData()
        {
            string source = AbsoluteDataPath();
            string destination = $"{BaseDataPath()}/{FileName()}_{DateTime.Now:yyyy_MM_ddTHH_mm}.{FileExtension()}";
            byte[] fileBytes = File.ReadAllBytes(source);
            File.WriteAllBytes(destination, fileBytes);
        }

        public void SaveData()
        {
            string destination = AbsoluteDataPath();

            byte[] json = System.Text.Encoding.Default.GetBytes(JsonUtility.ToJson(data));
            File.WriteAllBytes(destination, json);
        }


        public string AbsoluteDataPath() => $"{BaseDataPath()}/{FileName()}.{FileExtension()}";
        public abstract string FileName();
        public virtual string FileExtension() => "json";
        public string BaseDataPath() => Application.persistentDataPath;
    }
}
