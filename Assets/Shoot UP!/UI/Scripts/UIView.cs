using Core;
using Popups;
using UI.Windows;

namespace UI
{
    public class UIView : View<UIController, UIModel>
    {
        public void OpenStartWindowButton() => this.controller.OpenWindow<UIStartWindow>();
        public void OpenCaseWindowButton() => this.controller.OpenWindow<UICaseWindow>();
        public void OpenRateGamePopup() => PopupSystem.InvokeNotification<RatePopup>();
    }
}

