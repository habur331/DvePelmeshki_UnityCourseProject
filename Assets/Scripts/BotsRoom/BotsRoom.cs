using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Character_Control;
using EventSystem;
using Extensions;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class BotsRoom : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject _bombPrefab;

    [SerializeField] private GameObject _botPrefab;

    private List<Transform> _botSpawnPositions;
    private List<Transform> _bombSpawnPositions;
    private Door _enterDoor;
    private Door _exitDoor;

    [CanBeNull] private GameObject _bomb;
    [CanBeNull] private List<GameObject> _bots;
    private Player _player;
    private int _botsNumber = 5;

    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _enterDoor = GetComponentsInChildren<Door>().Single(door => door.Type == DoorType.Enter);
        _exitDoor = GetComponentsInChildren<Door>().Single(door => door.Type == DoorType.Exit);
        _botSpawnPositions = GameObject.FindGameObjectsWithTag("BotSpawn")
            .Select(@object => @object.transform)
            .ToList();
        _bombSpawnPositions = GameObject.FindGameObjectsWithTag("BombSpawn")
            .Select(@object => @object.transform)
            .ToList();

        Messenger.AddListener(GameEvent.PlayerDied, OnPlayerDied);
        Messenger<int>.AddListener(UIEvent.BotsNumberInSettingsChanged, value =>
        {
            _botsNumber = value;
        });
        
        _enterDoor.playerTransferred.AddListener(StartRoom);
        _exitDoor.playerTransferred.AddListener(FinishRoom);
    }

   

    #region Trgger events

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("enter");
            IsPlayerIn = true;
            StartRoom();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("stay");
            IsPlayerIn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("exit");
            IsPlayerIn = false;
            FinishRoom();
        }
    }*/

    #endregion

    private void StartRoom()
    {
        SpawnBots();
        SpawnBomb();

        Messenger<GameObject>.Broadcast(GameEvent.BombPlanted, _bomb);
    }

    private void FinishRoom()
    {
        if (_bomb != null)
            Destroy(_bomb);

        if (_bots is not null)
            _bots.ForEach(Destroy);

        _bomb = null;
        _bots = null;
    }

    private void RestartRoom()
    {
        FinishRoom();
        StartRoom();
    }

    private void SpawnBots()
    {
        var spawnPositions = _botSpawnPositions.GetRandomElements(_botsNumber);

        _bots = spawnPositions
            .Select(position => Instantiate(_botPrefab, position)).ToList();

        _bots.ForEach(botGameObject =>
        {
            var bot = botGameObject.GetComponent<BotHealth>();
            bot.botDiedEvent.AddListener(OnBotDied);
        });
    }

    private void SpawnBomb()
    {
        var spawnPosition = _bombSpawnPositions.GetRandomElements(1).Single();
        _bomb = Instantiate(_bombPrefab, spawnPosition);

        var bomb = _bomb!.GetComponentInChildren<Bomb>();
        bomb!.bombDiffusedEvent.AddListener(OnBombDiffused);
        bomb!.bombBlowUpEvent.AddListener(OnBombBlowUp);
    }

    private void OnBombBlowUp()
    {
        Messenger.Broadcast(GameEvent.BombBlewUp);
        TransferPlayerToSpawnIn(2);
    }

    private void OnBombDiffused()
    {
        Messenger.Broadcast(GameEvent.BombDiffused);
        FinishRoom();
        TransferPlayerToSpawnIn(2);
    }

    private void OnBotDied(GameObject bot)
    {
        //_bots!.Remove(bot);
    }

    private void OnPlayerDied()
    {
        FinishRoom();
        // player died and he will be transfered to spawn
    }

    private void TransferPlayerToSpawnIn(int seconds)
    {
        StartCoroutine(CoroutineHelper());
        return;

        IEnumerator CoroutineHelper()
        {
            yield return new WaitForSeconds(seconds);
            _player.TransferToSpawn();
        }
    }
}