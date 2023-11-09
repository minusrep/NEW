using Game_;
using System.Collections;
using TMPro;
using UnityEngine;

namespace UI.Windows
{
    public class UIRestartWindow : UIWindow
    {
        [SerializeField] private TextMeshProUGUI heightInfo;
        [SerializeField] private TextMeshProUGUI earnedMoneyInfo;

        private PlayerController player;
        private GameplayController gameplay;
        protected override void OnControllersCreated()
        {
            this.player = GameManager.GetController<PlayerController>();
            this.gameplay = GameManager.GetController<GameplayController>();
            base.OnControllersCreated();
        }
        protected override void OnActive()
        {
            this.heightInfo.text = $"{Mathf.Round(this.gameplay.maxHeight)}";
            this.earnedMoneyInfo.text = $"{this.player.earnedMoney}";
            base.OnActive();
        }
    }
}