using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using UI;
using UI.Windows;
using Game_;

public class UIController : Controller
{
    public override bool isInitialized => this.model != null;

    private UIModel model;
    private EnvironmentController environment;

    public override void Initialize<T>(T model)
    {
        Model tempModel = model;
        this.model = (UIModel) tempModel;
        this.environment = GameManager.GetController<EnvironmentController>();
    }


    public void OpenWindow<T>() where T : UIWindow
    {
        this.environment.InvokeMenuSound();
        foreach (var window in this.model.windows)
            if (window is T) window.isActive = true;
            else window.isActive = false;
    }
}
