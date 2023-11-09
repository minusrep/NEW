using Core;
using Game_;
using System.Collections;
using UnityEngine;

namespace Game_.CaseSystem
{
    public class CaseView : View<CaseController, CaseModel>
    {


        public void Invoke()
        {
            this.controller.Invoke();
        }
    }
}