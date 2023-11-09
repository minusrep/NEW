using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Popups
{
    public class WatchAdvPopup : Popup
    {
        public void ApplyButton()
        {
            GameManager.yandexSDK.ShowRewardToMoneyAdv();
            this.Close();
        }
        public void BackButton()
        {
            this.Close();
        }
    }

}
