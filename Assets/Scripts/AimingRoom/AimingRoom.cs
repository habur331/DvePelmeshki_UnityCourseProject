using System.Collections.Generic;
using System.Threading;
using AimingTrainingRoom;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace AimingRoom
{
    public class AimingRoom : MonoBehaviour
    {
        [SerializeField] private int level;
        [SerializeField] private GameObject dartboardPrefab;
        [SerializeField] private int numberDartboardsOnScene;
        [FormerlySerializedAs("hitedDartboards")] [SerializeField] private int hitedDartboardsCount;
        [SerializeField] private List<Dartboard> dartboards;

        public void Start()
        {
            dartboards = new();
            numberDartboardsOnScene = 3;
        }
        
        public void Update()
        {
            if (dartboards.Count == 0 && hitedDartboardsCount >= level * numberDartboardsOnScene && level >= 1)
            {
                LevelUp();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("ВХОД в тренировочную прицела.");
            if (other.CompareTag("Player"))
            {
                StartGame();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Debug.Log("ВЫХОД в тренировочную прицела.");
            FinishGame();
        }

        private void FinishGame()
        {
            Debug.Log($"Игра закончилась на уровне {level}.");
            level = 0;
            ClearDartBoard();
        }

        private void StartGame()
        {
            Debug.Log("Игра началась");
            level = 1;
            SetDartboardsGroup();
            hitedDartboardsCount = 0;
        }

        private void LevelUp()
        {
            level++;

            Debug.Log($"Поднятие уровня на {level}");
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
                dartboard.StartCoroutine(dartboard.StartMoving(level));
                dartboards.Add(dartboard);
            }
        }

        public void MarkHitTarget(Dartboard target)
        {
            if (dartboards.Contains(target))
                hitedDartboardsCount++;
        }
    }

    class AimingRoomImpl : AimingRoom
    {
    }
}