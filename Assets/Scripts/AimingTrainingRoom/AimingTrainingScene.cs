using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace AimingTrainingRoom
{
    public class AimingTrainingScene : MonoBehaviour
    {
        [SerializeField] private int level;
        [SerializeField] GameObject dartboardPrefab;
        private List<GameObject> dartboards;

        public void Start()
        {
            level = 1;

            CreateDartboard();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) // Предположим, что игрок имеет тег "Player"
            {
                StartGame();
            }
        }
        public void Update()
        {
            if (dartboards.Count == 0)
                LevelUp();
        }

        private void StartGame()
        {
            level = 0;
            
            CreateDartboard();
        }

        private void LevelUp()
        {
            level++;

            CreateDartboard();
        }

        private void CreateDartboard()
        {
            dartboards.Clear();

            int numberOfDartboardsToCreate = 3;

            for (int i = 0; i < numberOfDartboardsToCreate; i++)
            {
                // Генерируем случайные координаты для новой мишени
                float randomX = Random.Range(-10f, 10f); // Измените границы по X на ваши нужды
                float randomY = Random.Range(0.5f, 5f); // Измените границы по Y на ваши нужды
                float randomZ = Random.Range(-10f, 10f); // Измените границы по Z на ваши нужды

                // Создаем новую мишень в случайной позиции и добавляем ее в список
                Vector3 randomPosition = new Vector3(randomX, randomY, randomZ);
                GameObject newDartboard = Instantiate(dartboardPrefab, randomPosition, Quaternion.identity);
                dartboards.Add(newDartboard);
            }
        }
    }
}