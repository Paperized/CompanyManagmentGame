using UnityEngine;

namespace Common
{
    public class DependsOnGameState : MonoBehaviour
    {
        protected GameManager gameManager;
        protected bool canTick;
        
        protected virtual void Start()
        {
            gameManager.OnGameplayStarted += OnGameStart;
            gameManager.OnGameplayResume += OnGameResume;
            gameManager.OnGameplayPaused += OnGameStopOrPaused;
        }

        protected virtual void Update()
        {
            if (!canTick) return;
        }

        protected virtual void FixedUpdate()
        {
            if (!canTick) return;
        }

        protected virtual void OnDestroy()
        {
            gameManager.OnGameplayStarted -= OnGameStart;
            gameManager.OnGameplayResume -= OnGameResume;
            gameManager.OnGameplayPaused -= OnGameStopOrPaused;
        }

        protected virtual void OnGameStart() => canTick = true;
        protected virtual void OnGameResume() => canTick = true;
        protected virtual void OnGameStopOrPaused() => canTick = false;
    }
}