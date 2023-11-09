using Core;
using Game_.CaseSystem;
using Game_.Weapons;
using Popups;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game_
{
    public class CaseController : Controller
    {
        public override bool isInitialized => this.model != null;
        public bool isRolling { get; private set; }

        private CaseModel model;

        private List<CaseSlot> slots;
        private List<WeaponInfo> weapons;
        private List<WeaponSlot> arsenal;

        private PlayerController player;
        private EnvironmentController environment;


        public override void Initialize<T>(T model)
        {
            this.model = model as CaseModel;
            this.player = GameManager.GetController<PlayerController>();
            this.environment = GameManager.GetController<EnvironmentController>();
            this.weapons = new List<WeaponInfo>();
            this.weapons.AddRange(Resources.LoadAll<WeaponInfo>("WeaponsInfo"));
            this.CreateSlots();
            GameManager.OnGameLoadedEvent += this.OnGameLoaded;
        }

        private void OnGameLoaded()
        {
            this.CreateArsenal();
        }


        public override void Update()
        {
            if (this.isRolling)
            {
                var cellSize = this.model.caseSlotPrefab.size.x;
                var rectTransform = this.model.rectTransform;
                var group = this.model.group;
                var toPosition = rectTransform.transform.position;
                var toX = ((rectTransform.childCount - this.model.offset) * -(cellSize + group.spacing))
+ (cellSize / 2f) + (Random.Range(-cellSize / 2.1f, cellSize / 2.1f));
                toPosition.x = toX;

                var allDistance = Vector3.Distance(toPosition, rectTransform.transform.position) -
    this.model.offset * (cellSize + group.spacing);

                var actualDistance = Vector3.Distance(toPosition, rectTransform.transform.position);
                var magnitude = actualDistance / allDistance;
                /*if (magnitude > 1f) */ magnitude = 1f;
                var speed = /*this.model.speedCurve.Evaluate(magnitude) * */ this.model.speed;
                rectTransform.Translate(Vector2.left * speed * Time.deltaTime);
            }
            base.Update();
        }

        public void Invoke()
        {
            if (this.isRolling) return;
            if (this.player.moneyAmount >= this.model.price)
            {
                this.player.Buy(this.model.price);
            }
            else 
            {
                PopupSystem.InvokeNotification<WatchAdvPopup>();
                return;
            } 

            foreach (var slot in this.slots)
                slot.Initialize(this.weapons[RandomizeID()]);
            var id = this.slots[this.slots.Count - this.model.offset - 1].id;
            var isOpen = this.player.IsWeaponOpen(id);
            if (!isOpen) this.player.OpenWeapon(id);

            GameManager.StartRoutine(this.RollingRoutine(id, isOpen));
        }

        private IEnumerator RollingRoutine(int id, bool isOpen)
        {
            this.isRolling = true;
            var cellSize = this.model.caseSlotPrefab.size.x;
            var rectTransform = this.model.rectTransform;
            var group = this.model.group;
            rectTransform.anchoredPosition = this.model.startPosition;
            var toX =  ((rectTransform.childCount - this.model.offset) * -(cellSize + group.spacing)) 
                + (cellSize/2f) + (Random.Range(-cellSize/2.1f, cellSize/2.1f));
            Debug.Log(toX);
            var toPosition = rectTransform.transform.position;
            toPosition.x = toX;
            var allDistance = Vector3.Distance(toPosition, rectTransform.transform.position) - 
                this.model.offset * (cellSize + group.spacing);
            this.model.caseIcon.SetActive(false);
            this.model.rolling.SetActive(true);
            var slotIndex = 0;
            while (rectTransform.anchoredPosition.x > toX)
            {
                if (this.slots[slotIndex].transform.position.x < this.model.startPosition.x)
                {
                    slotIndex++;
                    this.environment.InvokeSound(this.model.caseStepSound);
                }

/*                var actualDistance = Vector3.Distance(toPosition, rectTransform.transform.position);
                var magnitude = actualDistance / allDistance;
                if (magnitude > 1f) magnitude = 1f;
                var speed = this.model.speedCurve.Evaluate(magnitude) * this.model.speed;
                //rectTransform.Translate(Vector2.left * speed);*/
                yield return new WaitForFixedUpdate();
            }
            this.isRolling = false;
            yield return new WaitForSeconds(this.model.rewardDelay);
            var popup = PopupSystem.GetNotification<RewardPopup>();
            PopupSystem.InvokeNotification<RewardPopup>();
            this.environment.InvokeSound(this.model.rewardSound);
            popup.Initialize(this.weapons[id], this.model.price, isOpen);
            this.model.caseIcon.SetActive(true);
            this.model.rolling.SetActive(false);
            this.UpdateArsenal();
        }

        private void CreateSlots()
        {
            this.slots = new List<CaseSlot>();
            var cellSize = this.model.caseSlotPrefab.size.x + this.model.group.spacing;
            var requiredDistance = this.model.duration * this.model.speed;
            var slotsCount = (requiredDistance / cellSize) + this.model.offset;
            for (int i = 0; i < (int)slotsCount; i++)
            {
                this.slots.Add(GameManager.Instantiate(this.model.caseSlotPrefab, this.model.rectTransform.gameObject.transform));
                this.slots[i].Initialize(this.weapons[this.RandomizeID()]);
            }
        }

        private void CreateArsenal()
        {
            var prefab = this.model.weaponSlotPrefab;
            this.arsenal = new List<WeaponSlot>();
            for (int i = 0; i < this.weapons.Count; i++)
                this.arsenal.Add(GameManager.Instantiate(prefab, this.model.weaponsParent));
            this.UpdateArsenal();
        }

        public void UpdateArsenal()
        {
            for(int i = 0; i < this.weapons.Count; i++)
            {
                var isAvaliable = this.player.IsWeaponOpen(weapons[i].id);
                var isSelected = this.player.weaponInfo.id == weapons[i].id;
                this.arsenal[i].Initialize(this.weapons[i], isAvaliable, isSelected);
            }
        }


        public void Roll()
        {
            this.isRolling = true;
            var cellSize = this.model.caseSlotPrefab.size.x;
            var rectTransform = this.model.rectTransform;
            var group = this.model.group;
            rectTransform.anchoredPosition = this.model.startPosition;
            var toX = ((rectTransform.childCount - this.model.offset) * -(cellSize + group.spacing))
                + (cellSize / 2f) + (Random.Range(-cellSize / 2.1f, cellSize / 2.1f));
            Debug.Log(toX);
            var toPosition = rectTransform.transform.position;
            toPosition.x = toX;
            var allDistance = Vector3.Distance(toPosition, rectTransform.transform.position) -
                this.model.offset * (cellSize + group.spacing);
            this.model.caseIcon.SetActive(false);
            this.model.rolling.SetActive(true);
            var slotIndex = 0;
        }

        private int RandomizeID()
        {
            int result;
            var allPercent = 1f;
            var randomValue = Random.Range(0f, 1f);
            for(int i = this.model.chances.Count - 1; i >= 0; i--)
            {
                allPercent -= this.model.chances[i].chance;
                if (randomValue >= allPercent) return this.model.chances[i].id;
            }
            return 0;
        }

    }

    [System.Serializable]
    public struct Chance
    {
        public int id => this._id;
        public float chance => _chance;

        [SerializeField] private int _id;
        [SerializeField][Range(0f, 1f)] float _chance;
    }
}
