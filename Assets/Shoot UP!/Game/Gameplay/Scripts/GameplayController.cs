using Core;
using Game_.Gameplay;
using Popups;
using System.Collections.Generic;
using UnityEngine;

namespace Game_
{
    public class GameplayController : Controller
    {

        public override bool isInitialized => this.model != null;
        public bool isPlaying { get; private set; }

        public float maxHeight { get; private set; }

        private GameplayModel model;
        private PlayerController player;
        private EnvironmentController environment;
        private float currentBoosterCooldown;

        private List<PickableObject> boostersPool;
        
        private List<GameObject> hardObstacles;
        private List<GameObject> mediumObstacles;
        private List<GameObject> easyObstacles;
        private List<Animator> marks;
        private float lastObstacleSpawnHeight;

        public override void Initialize<T>(T model)
        {
            this.model = model as GameplayModel;
            this.player = GameManager.GetController<PlayerController>();
            this.environment = GameManager.GetController<EnvironmentController>();
            this.CreateBoostersPool();
            this.easyObstacles = this.CreateObstaclesPool("Easy Obstacles");
            this.mediumObstacles = this.CreateObstaclesPool("Medium Obstacles");
            this.hardObstacles = this.CreateObstaclesPool("Hard Obstacles");
            this.CreateMarksPool();

        }

        public void OnStart()
        {
            this.maxHeight = this.model.startPosition.y;
            this.lastObstacleSpawnHeight = this.maxHeight;
            this.player.InvokeWeapon(this.model.startPosition);
            this.isPlaying = true;
            this.environment.ResetCameraPosition();
            this.DisableObstacles();
        }

        public void OnRestart()
        {
            this.OnStart();
        }

        public override void Update()
        {
            if (!this.isPlaying) return;
            else if (this.player.isDestroyed) this.Lose();

            this.ControlHeight();
            this.SpawnBooster();
            this.ApplyBoostersMovement();
            this.SpawnObstacles();
        }

        private void ControlHeight()
        {
            var minHeight = this.maxHeight + this.model.minHeightOffset;
            var currentHeight = this.player.position.y;
            if (currentHeight > this.maxHeight) this.maxHeight = currentHeight;
            else if (currentHeight < minHeight)
                this.Lose();
            foreach (var mark in this.marks)
                if (mark.transform.position.y <= currentHeight) mark.SetTrigger("Invoke");
        }


        private void CreateBoostersPool()
        {
            this.boostersPool = new List<PickableObject>();
            var parent = new GameObject("BoostersPool").transform;
            parent.transform.parent = this.model.gameObject.transform;
            foreach (var booster in this.model.boosters)
            {
                var newBooster = GameManager.Instantiate(booster, parent);
                newBooster.gameObject.SetActive(false);
                this.boostersPool.Add(newBooster);
            }
            this.currentBoosterCooldown = this.model.boosterCooldown;
        }
        private void CreateMarksPool()
        {
            this.marks = new List<Animator>();
            var markPrefab = Resources.Load<Animator>("Mark");
            var parent = new GameObject("Marks").transform;
            parent.transform.parent = this.model.gameObject.transform;
            for (int i = 0; i < 3; i++)
                this.marks.Add(GameManager.Instantiate(markPrefab, parent));
            marks[0].transform.position = new Vector3(0f, this.model.mediumDifficultyHeight);
            marks[1].transform.position = new Vector3(0f, this.model.hardDifficultyHeight);
            marks[2].transform.position = new Vector3(0f, this.player.maxHeight);
        }
        
        private void SpawnBooster()
        {
            if (this.currentBoosterCooldown <= 0f)
            {
                this.currentBoosterCooldown = this.model.boosterCooldown;
                var randomX = Random.Range(-this.player.weaponInfo.maxCenterOffset + this.model.boosterSpawnOffsetX,
                    this.player.weaponInfo.maxCenterOffset - this.model.boosterSpawnOffsetX);
                var Y = this.maxHeight + this.model.boosterSpawnOffsetY;
                var position = new Vector3(randomX, Y);

                var booster = this.boostersPool[Random.Range(0, this.boostersPool.Count)];
                while (booster.gameObject.activeSelf)
                    booster = this.boostersPool[Random.Range(0, this.boostersPool.Count)];
                booster.transform.position = position;
                booster.gameObject.SetActive(true);

            }
            else this.currentBoosterCooldown -= Time.deltaTime;
        }
        private void ApplyBoostersMovement()
        {
            foreach(var booster in this.boostersPool)
            {
                if (booster.gameObject.transform.position.y <= this.maxHeight + this.model.minHeightOffset)
                    booster.gameObject.SetActive(false);
                else booster.gameObject.transform.Translate(Vector3.down * booster.moveSpeed * Time.deltaTime);
            }
        }


        private List<GameObject> CreateObstaclesPool(string type)
        {
            var obstaclesPool = new List<GameObject>();
            var parent = new GameObject(type).transform;
            parent.transform.parent = this.model.gameObject.transform;
            var obstacles = Resources.LoadAll<GameObject>(type);
            foreach (var obstacle in obstacles)
            {
                var newObstacle = GameManager.Instantiate(obstacle, parent);
                newObstacle.SetActive(false);
                obstaclesPool.Add(newObstacle);
            }
            return obstaclesPool;
        }


        private void SpawnObstacles()
        {
            var delta = Mathf.Abs(this.maxHeight - this.lastObstacleSpawnHeight);
            if (this.maxHeight >= this.lastObstacleSpawnHeight)
            {
                List<GameObject> actualPool = null;
                if (this.maxHeight >= this.model.hardDifficultyHeight) actualPool = this.hardObstacles;
                else if (this.maxHeight >= this.model.mediumDifficultyHeight) actualPool = this.mediumObstacles;
                else actualPool = this.easyObstacles;
                this.CleanObstacles(actualPool);


                this.lastObstacleSpawnHeight = this.maxHeight + this.model.obstacleSpawnOffsetY;
                var position = Vector3.zero;
                position.y = this.lastObstacleSpawnHeight;


                var obstacle = actualPool[Random.Range(0, actualPool.Count)];
                while (obstacle.gameObject.activeSelf)
                    obstacle = actualPool[Random.Range(0, actualPool.Count)];
                obstacle.transform.position = position;
                obstacle.gameObject.SetActive(true);
            }
        }

        private void CleanObstacles(List<GameObject> pool)
        {
            foreach (var obstacle in pool)
                if (obstacle.activeSelf && obstacle.transform.position.y <= this.maxHeight + this.model.minHeightOffset)
                    obstacle.SetActive(false);
        }

        private void DisableObstacles()
        {
            foreach (var obstacle in this.easyObstacles)
                obstacle.gameObject.SetActive(false);
            foreach (var obstacle in this.mediumObstacles)
                obstacle.gameObject.SetActive(false);
            foreach (var obstacle in this.hardObstacles)
                obstacle.gameObject.SetActive(false);
        }

        private void DisableAllObjects()
        {
            this.DisableObstacles();
        }


        private void Lose()
        {
            this.isPlaying = false;
            this.player.OnLose(this.maxHeight);
            PopupSystem.InvokeNotification<ContinuePopup>();
            
        }

        public void Continue()
        {
            var continuePosition = new Vector3(0f, this.maxHeight + this.model.continueOffsetY, 0f);
            this.player.InvokeWeapon(continuePosition);
            this.DisableObstacles();
            this.isPlaying = true;
        }
    }
}