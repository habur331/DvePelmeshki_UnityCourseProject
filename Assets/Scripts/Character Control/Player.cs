using System;
using DG.Tweening;
using UnityEngine;

namespace Character_Control
{
    [RequireComponent(typeof(FPSInput))]
    [RequireComponent(typeof(PlayerShoot))]
    [RequireComponent(typeof(PlayerHealth))]
    public class Player : MonoBehaviour
    {
        private MouseLook _mouseLook;
        private FPSInput _fpsInput;
        private PlayerShoot _playerShoot;
        private PlayerHealth _playerHealth;
        private Vector3 _spawnPosition;

        private void Start()
        {
            _spawnPosition = transform.position;
            _mouseLook = GetComponentInChildren<MouseLook>();
            _fpsInput = GetComponent<FPSInput>();
            _playerShoot = GetComponent<PlayerShoot>();
            _playerHealth = GetComponent<PlayerHealth>();

            Messenger<bool>.AddListener(GameEvent.PauseStateChanged, OnPauseToggle);
        }

        public void Die()
        {
            Debug.Log("You are dying");
            Messenger.Broadcast(GameEvent.PlayerDied);
        }

        public void TransferToSpawn()
        {
            transform.DOMove(_spawnPosition, 0.000001f);
        }

        private void OnPauseToggle(bool isPaused)
        {
            _mouseLook.enabled = !isPaused;
            _fpsInput.enabled = !isPaused;
            _playerShoot.enabled = !isPaused;
        }
    }
}