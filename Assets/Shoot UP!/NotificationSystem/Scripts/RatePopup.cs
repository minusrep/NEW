using System.Collections;
using UnityEngine;

namespace Popups
{
    public class RatePopup : Popup
    {
        public void RateGameButton()
        {
            GameManager.yandexSDK.RateGame();
            this.Close();
        }
    }
}