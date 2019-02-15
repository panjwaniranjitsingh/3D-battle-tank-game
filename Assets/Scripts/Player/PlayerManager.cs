﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using Manager;
using System;
using Reward;
using StateMachine;
using UI;

namespace Player
{
    [System.Serializable]
    public class PositionData
    {
        public int enemyCount;
        public Vector3 position;
    }

    public class PlayerManager : Singleton<PlayerManager>
    {
        public InputComponentScriptableList inputComponentScriptableList;

        public List<PlayerController> playerControllerList { get; private set; }

        [SerializeField]
        private int totalPlayers = 1;

        public int TotalPlayer { get { return totalPlayers; } }

        private GameObject playerPrefab;

        public event Action<int> playerSpawned;
        public event Action<int> playerDestroyed;

        private List<Vector3> playerSpawnPosList;

        [SerializeField]
        private int maxIteration = 10;

        private int currentIteration = 0;
        private List<PositionData> allPositionData = new List<PositionData>();

        public float safeRadius = 3f;
        public Vector3 safePos { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            playerControllerList = new List<PlayerController>();
        }

        private void Start()
        {
            RewardManager.Instance.RewardButtonClicked += SetPlayerPrefab;
        }

        public void SpawnPlayer()
        {
            playerControllerList = new List<PlayerController>();
            if (inputComponentScriptableList==null)
            {
                Debug.Log("[PlayerManager] Missing InputComponentScriptableList");
            }

            if(GameManager.Instance.currentState.gameStateType == GameStateType.Game)
            {
                playerSpawnPosList = new List<Vector3>();
            }

            for (int i = 0; i < totalPlayers; i++)
            {
                Debug.Log("[PlayerManager] PlayerSpawned");
                if (GameManager.Instance.currentState.gameStateType == StateMachine.GameStateType.Game)
                {
                    GetSafePosition();
                }
                else if (GameManager.Instance.currentState.gameStateType == StateMachine.GameStateType.Replay)
                    safePos = playerSpawnPosList[i];

                //GetSafePosition();

                PlayerController playerController = new PlayerController(inputComponentScriptableList.inputComponentScriptables[i],
                                                                         safePos, playerPrefab, i);
                playerControllerList.Add(playerController);

                playerSpawned?.Invoke(i);
            }

        }

        public void DestroyPlayer(PlayerController _playerController)
        {
            Inputs.InputManager.Instance.RemoveInputComponent(_playerController.playerInput);
            playerDestroyed?.Invoke(_playerController.playerID);
            _playerController.DestroyPlayer();
            RemovePlayerController(_playerController);
            _playerController = null;

            if(playerControllerList.Count <= 0)
            {
                GameManager.Instance.UpdateGameState(new GameOverState());
            }

        }

        public void RemovePlayerController(PlayerController playerController)
        {
            for (int i = 0; i < playerControllerList.Count; i++)
            {
                if (playerControllerList[i] == playerController)
                {
                    playerControllerList.RemoveAt(i);
                    Debug.Log("[InputManager] Remove InputComponent at index " + i);
                }
            }
        }

        public void GetSafePosition()
        {
            currentIteration++;
            Vector3 pos = RandomPos();
            foreach (Enemy.EnemyController enemy in Enemy.EnemyManager.Instance.EnemyList)
            {
                float distance = Vector3.Distance(pos, enemy.enemyView.transform.position);
                //Debug.Log("[PlayerManager] Distance " + distance);
                if(distance < safeRadius)
                {
                    //if (currentIteration < maxIteration)
                    //{
                    //    GetSafePosition();
                    //}
                    //else
                    //{

                    //}

                    GetSafePosition();
                    return;
                }
            }
            //Debug.Log("[PlayerManager] Player Spawnpos " + pos);
            playerSpawnPosList.Add(pos);
            safePos = pos;
            currentIteration = 0;
        }

        public Vector3 RandomPos()
        {
            float x = UnityEngine.Random.Range(-GameManager.Instance.MapSize, GameManager.Instance.MapSize);
            float y = 0;
            float z = UnityEngine.Random.Range(-GameManager.Instance.MapSize, GameManager.Instance.MapSize);

            return new Vector3(x, y, z);
        }

        void SetPlayerPrefab(GameObject PlayerPrefab)
        {
            playerPrefab = PlayerPrefab;
        }

    }
}