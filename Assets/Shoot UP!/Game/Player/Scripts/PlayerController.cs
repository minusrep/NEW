using Core;
using Game_.Player;
using Game_.Weapons;
using System.Collections.Generic;
using UnityEngine;

namespace Game_
{
    public class PlayerController : Controller
    {
        public override bool isInitialized => this.model != null;


        public bool isControllable { get => this.currentWeapon.isControllable; set => this.currentWeapon.isControllable = value; }
        public bool isDestroyed => this.currentWeapon.isDestroyed;
        public float reloadingProgress => this.currentWeapon.reloadingProgress;
        public float doubleMoneyProgress => this.currentWeapon.moneyDoubleProgress;
        public float shieldProgress => this.currentWeapon.shieldProgress;
        public float slowProgress => this.currentWeapon.slowProgress;
        public int earnedMoney => this.currentWeapon.earnedMoney;
        public int clipValue => this.currentWeapon.clipValue;
        public WeaponInfo weaponInfo => this.currentWeapon.info;
        public Vector3 position => this.currentWeapon.position;

        public int weaponsCount => this.weapons.Count;
        public int openWeaponsCount
        {
            get
            {
                var result = 0;
                foreach (var weapon in this.weapons)
                    if (IsWeaponOpen(weapon.info.id)) result++;
                return result;
            }
        }


        public int moneyAmount
        {
            get => this.data.moneyAmount;
            private set
            {
                this.data.moneyAmount = value;
                GameManager.SaveData(this.data);
            }
        }

        public float maxHeight
        {
            get  => Mathf.Round(this.data.maxHeight);
            private set
            {
                if (value > this.maxHeight)
                {
                    this.data.maxHeight = value;
                    GameManager.SaveData(this.data);
                    GameManager.yandexSDK.UpdateLeaderboard(Mathf.RoundToInt(value));
                }
            }
        }

        public bool muteState 
        { 
            get => this.data.muteState; 
            set 
            { 
                this.data.muteState = value; 
                GameManager.SaveData(this.data); 
            } 
        }

        public bool isRated => this.data.isRated;

        private Weapon currentWeapon { get; set; }

        private PlayerModel model;
        private Data data;
        private List<Weapon> weapons;


        public PlayerController(Data data)
        {
            this.data = data;
        }

        public override void Initialize<T>(T model) 
        {
            this.model = model as PlayerModel;

            this.weapons = new List<Weapon>();
            var weaponsPrefab = Resources.LoadAll<Weapon>("Weapons");
            var parent = new GameObject("Weapons").transform;
            foreach (var weapon in weaponsPrefab)
            {
                this.weapons.Add(GameManager.Instantiate(weapon, parent));
            }
            this.SelectWeapon(this.data.selectedWeaponID);
        }

        public void InvokeWeapon(Vector3 position) => currentWeapon.Invoke(position);

        public void SelectWeapon(int id = 0)
        {
            if (id < 0 || id > this.weapons.Count) id = 0;
            if (!this.IsWeaponOpen(id)) return;
            foreach (var weapon in this.weapons)
            {
                if (weapon.info.id == id) 
                {
                    this.currentWeapon = weapon;
                    this.currentWeapon.gameObject.SetActive(true);
                    this.currentWeapon.Reset();
                    this.data.selectedWeaponID = id;
                } 
                else weapon.gameObject.SetActive(false);
            }
        }

        public void OpenWeapon(int id)
        {
            if (id < 0 || id > this.weapons.Count) id = 0;
            if (!this.IsWeaponOpen(id)) this.data.weaponStates[id] = true;
            GameManager.SaveData(this.data);
        }


        public bool IsWeaponOpen(int id) => this.data.weaponStates[id];

        public void Buy(int price)
        {
            if (this.moneyAmount >= price) this.moneyAmount -= price;
        }
        
        public void AddMoney(int amount)
        {
            if (amount > 0) this.moneyAmount += amount;
        }

        public void OnLose(float height)
        {
            this.maxHeight = height;
            this.currentWeapon.Destroy();
        } 

        public void OnTryEnded()
        {
            this.moneyAmount += this.earnedMoney;
            this.currentWeapon.Reset();
            GameManager.yandexSDK.ShowFullscreenAdv();
        }
        public void OnInput()
        {
            this.currentWeapon.Shoot();
        }
        
        public void RateGame()
        {
            if (this.data.isRated) return;
            this.AddMoney(200);
            this.data.isRated = true;
            GameManager.SaveData(this.data);
        }
    }
}