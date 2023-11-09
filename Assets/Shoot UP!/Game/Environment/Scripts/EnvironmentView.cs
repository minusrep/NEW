using Core;
using System.Collections;
using UnityEngine;

namespace Game_.Environment
{
    public class EnvironmentView : View<EnvironmentController, EnvironmentModel>
    {

        private void FixedUpdate()
        {
            if (this.controller != null) this.controller.FixedUpdate();
        }
    }
}