using System;
using System.Collections.Generic;
using System.Threading;
using AimingTrainingRoom;
using EventSystem;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace AimingRoom
{
    public class AimingRoom : MonoBehaviour
    {
         
        [SerializeField] private GameObject dartboardPrefab;
        [SerializeField] private int numberDartboardsOnScene;
        [SerializeField] private List<Dartboard> dartboards;

        public static bool IsPlayerIn { get; private set; } = false;
        
        private int _currentLevel;
        private int _startLevel = 1;
        private int _hitDartboardsCount;
        
        public void Start()
        {
            dartboards = new();
            numberDartboardsOnScene = 3;
            Messenger<int>.AddListener(UIEvent.AimRoomLevelInSettingsChanged, OnLevelChangeInSettings);
        }
        
        public void Update()
        {
            if (dartboards.Count == 0 && _currentLevel >= 1)
            {
                LevelUp();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("ВХОД в тренировочную прицела.");
                IsPlayerIn = true;
                StartGame();
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                IsPlayerIn = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                IsPlayerIn = false;
                Debug.Log("ВЫХОД в тренировочную прицела.");
                FinishGame();
            }
        }

        private void OnLevelChangeInSettings(int level)
        {
            _startLevel = level;
            if(IsPlayerIn)
                StartGame();
        }

        private void FinishGame()
        {
            Debug.Log($"Игра закончилась на уровне {_currentLevel}.");
            _currentLevel = 0;
            ClearDartBoard();
        }

        private void StartGame()
        {
            Debug.Log("Игра началась");
            _currentLevel = _startLevel;
            SetDartboardsGroup();
            _hitDartboardsCount = 0;
        }

        private void LevelUp()
        {
            _currentLevel++;
            Debug.Log($"Поднятие уровня на {_currentLevel}");
            SetDartboardsGroup();
        }

        public void RemoveDartboard(Dartboard dartboard)
        {
            dartboards.Remove(dartboard);
        }
        
        private void ClearDartBoard()
        {
            foreach (var dartboard in dartboards)
            {
                dartboard.StartCoroutine(dartboard.Die());
            }
        }
        
        private void SetDartboardsGroup()
        {
            Messenger<int>.Broadcast(UIEvent.AimRoomLevelChanged, _currentLevel);
            ClearDartBoard();
            
            Thread.Sleep(2);
            
            for (int i = 0; i < numberDartboardsOnScene; i++)
            {
                // Генерируем случайные координаты для новой мишени
                float randomX = Random.Range(-10f, 10f);
                float randomY = 1f;
                float randomZ = Random.Range(-25f, -50f);
            
                // Создаем новую мишень в случайной позиции и добавляем ее в список
                Vector3 randomPosition = new Vector3(randomX, randomY, randomZ);
                var newDartboard = Instantiate(dartboardPrefab, randomPosition, Quaternion.Euler(0, 180, 0));
                var dartboard = newDartboard.GetComponent<Dartboard>();
                dartboard.StartCoroutine(dartboard.StartMoving(_currentLevel));
                dartboards.Add(dartboard);
            }
        }

        public void MarkHitTarget(Dartboard target)
        {
            if (dartboards.Contains(target))
                _hitDartboardsCount++;
        }
    }

    class AimingRoomImpl : AimingRoom
    {
    }
}