using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using UI.Windows;

namespace UI
{
    [System.Serializable]
    public class UIModel : Model
    {
        public List<UIWindow> windows => this._windows;
        [SerializeField] private List<UIWindow> _windows;
    }
}
