using Game_;
using Game_.Weapons;
using UnityEngine;
using UnityEngine.UI;

namespace Popups
{
    public class RewardPopup : Popup
    {
        [SerializeField] private Image rarityFlag;
        [SerializeField] private Image icon;
        [SerializeField] private GameObject applyButton;
        [SerializeField] private GameObject sellButton;
        private int id;
        private int price;

        public void Initialize(WeaponInfo info, int price, bool isOpen)
        {

            this.rarityFlag.color = info.outlineColor;
            this.icon.sprite = info.sprite;
            this.id = info.id;
            if (isOpen)
            {
                this.sellButton.SetActive(true);
                this.applyButton.SetActive(false);
            }
            else 
            { 
                this.sellButton.SetActive(false);
                this.applyButton.SetActive(true);
            }
            this.price = price;
            base.Invoke();
        }

        public void ApplyWeaponButton()
        {
            GameManager.GetController<PlayerController>().SelectWeapon(this.id);
            GameManager.GetController<CaseController>().UpdateArsenal();
            this.Close();
        }

        public void SellWeaponButton()
        {
            GameManager.GetController<PlayerController>().AddMoney(this.price/2);
            this.Close();
        }
    }
}

