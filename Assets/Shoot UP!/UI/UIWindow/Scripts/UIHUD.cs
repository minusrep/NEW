using Game_;
using System;
using System.Collections;
using TMPro;
using UI.Windows;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public class UIHUD : UIWindow
    {
        [SerializeField] private TextMeshProUGUI heightInfo;
        [SerializeField] private TextMeshProUGUI moneyInfo;
        [SerializeField] private TextMeshProUGUI clipInfo;

        [SerializeField] private Image reloadingFlag;
        [SerializeField] private Image doubleMoneyFlag;
        [SerializeField] private Image shieldFlag;
        [SerializeField] private Image slowMotionFlag;

        private PlayerController player;
        private GameplayController gameplay;

        protected override void OnControllersCreated()
        {
            this.player = GameManager.GetController<PlayerController>();
            this.gameplay = GameManager.GetController<GameplayController>();
            base.OnControllersCreated();
        }

        private void Update()
        {
            if (this.gameplay == null) return;
            if (this.gameplay.isPlaying)
            {
                this.UpdateHeightInfo();
                this.UpdateMoneyInfo();
                this.UpdateClipInfo();

                this.UpdateFlag(this.doubleMoneyFlag, this.player.doubleMoneyProgress);
                this.UpdateFlag(this.shieldFlag, this.player.shieldProgress);
                this.UpdateFlag(this.slowMotionFlag, this.player.slowProgress);
                this.UpdateFlag(this.reloadingFlag, this.player.reloadingProgress, true);
            }
        }

        private void UpdateFlag(Image flag, float progress, bool inverseFill = false)
        {
            if (progress > 0f)
            {
                if (!flag.gameObject.activeSelf)
                    flag.gameObject.SetActive(true);
                if (inverseFill)
                    flag.fillAmount = 1f - progress;
                else flag.fillAmount = progress;
            }
            else if (flag.gameObject.activeSelf) flag.gameObject.SetActive(false);

        }

        private void UpdateHeightInfo()
        {
            var height = Mathf.Round(this.gameplay.maxHeight);
            if (height < 0) height = 0;
            var numberCount = this.heightInfo.text.Length;
            var zeroCount = height.ToString().Length;
            var heightText = "";
            for (int i = 0; i < numberCount - zeroCount; i++)
                heightText += "0";
            heightText += height;
            this.heightInfo.text = heightText;
        }

        private void UpdateMoneyInfo()
        {
            var money = this.player.earnedMoney;
            this.moneyInfo.text = $"{money}";
        }

        private void UpdateClipInfo()
        {
            this.clipInfo.text = $"{this.player.clipValue}|{this.player.weaponInfo.clipCapacity}";
        }
    }
}