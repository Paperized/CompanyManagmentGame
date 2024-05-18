using Common;
using Data.Common;
using Repositories;
using Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : SingletonBehaviour<GameManager>
{
    [field: SerializeField]
    public bool GameplayStarted { get; private set; }

    [field: SerializeField]
    public bool GameplayPaused { get; private set; }

    [SerializeField]
    private float nextPotentialEmployee;
    private float nextPotentialEmployeeCurrent;

    private List<IDataEventEmitter> dataEmitters;
    private List<IRepositoryLifecycle> dataHolders;

    public event Action OnGameplayStarted = delegate { };
    public event Action OnGameplayPaused = delegate { };
    public event Action OnGameplayResume = delegate { };

    protected override void Awake()
    {
        base.Awake();

        var monos = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.InstanceID);
        dataEmitters = monos.OfType<IDataEventEmitter>().ToList();
        dataHolders = monos.OfType<IRepositoryLifecycle>().ToList();

        Debug.LogFormat("Loaded {0} data holders", dataHolders.Count);
        Debug.LogFormat("Loaded {0} data emitters", dataEmitters.Count);

        foreach (var item in dataHolders)
        {
            Debug.LogFormat("Loading data from {0}", (item as MonoBehaviour).name);
            item.LoadData();
        }
    }

    private void Start()
    {
        Debug.LogFormat("Broadcasting data emitter ({0})", dataEmitters.Count);
        foreach (var item in dataEmitters)
        {
            item.BroadcastAllData();
        }
    }

    private void OnDestroy()
    {
        Debug.LogFormat("Saving data before closing ({0})", dataHolders.Count);
        foreach (var item in dataHolders)
        {
            item.SaveData();
        }
    }

    private void Update()
    {
        if (!GameplayStarted || GameplayPaused) return;
        nextPotentialEmployeeCurrent += Time.deltaTime;

        if (nextPotentialEmployeeCurrent >= nextPotentialEmployee)
        {
            nextPotentialEmployeeCurrent -= nextPotentialEmployee;
            EmployeesRepository.RequireInstance.AddPotentialEmployee(new Data.EmployeeData()
            {
                id = Guid.NewGuid().ToString(),
                name = RandomName(),
                imageName = "test",
                empType = EmployeeType.Dev
            });
        }
    }

    public void StartGameplay()
    {
        if (GameplayStarted == true)
            return;

        GameplayStarted = true;

        OnGameplayStarted.Invoke();

        GameplayPaused = false;
    }

    public void PauseGameplay(bool isPaused)
    {
        if (!GameplayStarted || GameplayPaused == isPaused)
            return;

        GameplayPaused = isPaused;

        if(GameplayPaused)
            OnGameplayPaused.Invoke();
        else
            OnGameplayResume.Invoke();
    }

    private string RandomName()
    {
        List<string> list = new() { "Ivan", "Giorgio", "Pippo", "Gino", "Dino" };
        return list[UnityEngine.Random.Range(0, list.Count)];
    }
}
