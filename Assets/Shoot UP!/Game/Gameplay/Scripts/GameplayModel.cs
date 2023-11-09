using Core;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Game_.Gameplay
{
    [System.Serializable]
    public class GameplayModel : Model
    {
        public GameObject gameObject => this._gameObject;
        public float minHeightOffset => this._minHeightOffset;
        public List<PickableObject> boosters => this._boosters;
        public float boosterCooldown => this._boosterCooldown;
        public float boosterSpawnOffsetY => this._boosterSpawnOffsetY;
        public float boosterSpawnOffsetX => this._boosterSpawnOffsetX;

        public Vector3 startPosition => this._startPosition;
        public float continueOffsetY => this._continueOffsetY;
        public float obstacleSpawnOffsetY => this._obstacleSpawnOffsetY;

        public float mediumDifficultyHeight => this._mediumDifficultyHeight;
        public float hardDifficultyHeight => this._hardDifficultyHeight;


        [SerializeField] private GameObject _gameObject;
        [SerializeField] private float _minHeightOffset;
        [Header("Boosters Settings")]
        [SerializeField] private List<PickableObject> _boosters;
        [SerializeField] private float _boosterCooldown;
        [SerializeField] private float _boosterSpawnOffsetY;
        [SerializeField] private float _boosterSpawnOffsetX;
        [Header("Obstacles Settings")]
        [SerializeField] private float _obstacleSpawnOffsetY;
        [SerializeField] private float _mediumDifficultyHeight;
        [SerializeField] private float _hardDifficultyHeight;

        [Header("Positions")]
        [SerializeField] private Vector3 _startPosition;
        [SerializeField] private float _continueOffsetY;
    }
}