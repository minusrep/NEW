using Game_;
using System.Collections;
using UnityEngine;

namespace Popups
{
    public class ContinuePopup : Popup
    {
        public void ContinueButton()
        {
            GameManager.yandexSDK.ShowRewardToContinueAdv();
            this.Close();
            
        }

        public void OpenRestartWindowButton()
        {
            GameManager.GetController<InputController>().OpenRestartWindow();
            this.Close();
        }
    }
}