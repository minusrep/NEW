using Game_;
using System.Collections;
using TMPro;
using UnityEngine;

namespace UI.Windows
{
    public class UIStartWindow : UIWindow
    {
        [SerializeField] private TextMeshProUGUI maxHeightInfo;
        [SerializeField] private TextMeshProUGUI moneyAmountInfo;
        [SerializeField] private TextMeshProUGUI progressInfo;
        [SerializeField] private GameObject rateGameButton;
        [SerializeField] private GameObject muteIcon;
        [SerializeField] private GameObject unmuteIcon;

        private PlayerController player;
        protected override void OnControllersCreated()
        {
            this.player = GameManager.GetController<PlayerController>();
        }

        private void Update()
        {
            if (this.player == null) return;
            if (this.isActive) this.moneyAmountInfo.text = $"{this.player.moneyAmount}";
            if (this.player.muteState)
            {
                this.muteIcon.SetActive(false);
                this.unmuteIcon.SetActive(true);
            }
            else
            {
                this.muteIcon.SetActive(true);
                this.unmuteIcon.SetActive(false);
            }
        }

        protected override void OnActive()
        {
            this.rateGameButton.SetActive(!this.player.isRated);

            this.maxHeightInfo.text = $"{this.player.maxHeight}";

            var progress = ((float)this.player.openWeaponsCount / (float)this.player.weaponsCount) * 100f;
            progress = Mathf.Round(progress);
            this.progressInfo.text = $"{this.player.openWeaponsCount}/{this.player.weaponsCount} ({progress}%)";
            base.OnActive();
        }
    }
}