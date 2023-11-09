using Game_;
using TMPro;
using UnityEngine;

namespace UI.Windows
{
    public class UICaseWindow : UIWindow
    {
        [SerializeField] private TextMeshProUGUI moneyAmountInfo;
        private PlayerController player;

        protected override void OnControllersCreated()
        {
            this.player = GameManager.GetController<PlayerController>();
            base.OnControllersCreated();
        }

        private void Update()
        {
            if (this.isActive)
            {
                this.moneyAmountInfo.text = $"{this.player.moneyAmount}";    
            }
        }

    }
}