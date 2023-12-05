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

    public static bool IsPlayerIn { get; private set; }

    private List<Transform> _botSpawnPositions;
    private List<Transform> _bombSpawnPositions;

    [CanBeNull] private Bomb _bomb;
    [CanBeNull] private List<BotHealth> _bots;
    private Player _player;
    private int _botsNumber = 5;

    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _botSpawnPositions = GameObject.FindGameObjectsWithTag("BotSpawn")
            .Select(@object => @object.transform)
            .ToList();
        _bombSpawnPositions = GameObject.FindGameObjectsWithTag("BombSpawn")
            .Select(@object => @object.transform)
            .ToList();

        Messenger.AddListener(GameEvent.PlayerDied, OnPlayerDied);
        Messenger<int>.AddListener(UIEvent.BotsNumberInSettingsChanged, value => _botsNumber = value);
    }

    #region Trgger events

    private void OnTriggerEnter(Collider other)
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
    }

    #endregion

    private void StartRoom()
    {
        _ = SpawnBots();
        var bomb = SpawnBomb();

        Messenger<GameObject>.Broadcast(GameEvent.BombPlanted, bomb);
    }

    private void FinishRoom()
    {
        if (_bomb is not null)
            Destroy(_bomb.gameObject);

        if (_bots is not null)
            _bots.ForEach(bot => Destroy(bot.gameObject));

        _bomb = null;
        _bots = null;
    }

    private void RestartRoom()
    {
        FinishRoom();
        StartRoom();
    }

    private List<GameObject> SpawnBots()
    {
        var spawnPositions = _botSpawnPositions.GetRandomElements(_botsNumber);

        var botGameObjects = spawnPositions
            .Select(position => Instantiate(_botPrefab, position)).ToList();

        _bots = botGameObjects.Select(botGameObject =>
            {
                var bot = botGameObject.GetComponent<BotHealth>();
                bot.botDiedEvent.AddListener(OnBotDied);
                return bot;
            })
            .ToList();

        return botGameObjects;
    }

    private GameObject SpawnBomb()
    {
        var spawnPosition = _bombSpawnPositions.GetRandomElements(1).Single();
        var bombGameObject = Instantiate(_bombPrefab, spawnPosition);

        _bomb = bombGameObject.GetComponentInChildren<Bomb>();
        _bomb!.bombDiffusedEvent.AddListener(OnBombDiffused);
        _bomb!.bombBlowUpEvent.AddListener(OnBombBlowUp);

        return bombGameObject;
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
        _bots!.Remove(bot.GetComponent<BotHealth>());
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