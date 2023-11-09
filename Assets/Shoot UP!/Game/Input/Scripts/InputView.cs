using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game_.Input_
{
    public class InputView : View<InputController, InputModel>
    {
        public void StartButton() => this.controller.Start();
        public void InputButton() => this.controller.Input();
        public void RollCaseButton() => this.controller.RollCase();
        public void ChangeMuteStateButton() => this.controller.ChangeMuteState();
    }

}
