using Game_.Weapons;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game_.CaseSystem
{
    public class WeaponSlot : MonoBehaviour
    {
        public int id { get; private set; }

        [SerializeField] private Image icon;
        [SerializeField] private Image rarityFlag;
        [SerializeField] private Image lockedFlag;
        [SerializeField] private Image selectedFlag;

        public void Initialize(WeaponInfo weapon,bool isAvaliable = false, bool isSelected = false)
        {
            this.icon.sprite = weapon.sprite;
            this.rarityFlag.color = weapon.outlineColor;
            this.id = weapon.id;
            this.lockedFlag.gameObject.SetActive(!isAvaliable);
            this.selectedFlag.gameObject.SetActive(isSelected);
        }

        public void SelectWeaponButton()
        {
            GameManager.GetController<PlayerController>().SelectWeapon(this.id);
            GameManager.GetController<EnvironmentController>().InvokeMenuSound();
            GameManager.GetController<CaseController>().UpdateArsenal();
        }
    }
}