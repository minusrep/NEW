using System.Collections.Generic;
using UnityEngine;
using Popups;
using Unity.VisualScripting;
using Game_;

public class PopupSystem : MonoBehaviour
{
    private const string NAME = "[POPUP SYSTEM]";


    private static PopupSystem instance
    {
        get
        {
            if (_instance == null)
                InitializePopupSystem();
            return _instance;

        }
    }

    private static PopupSystem _instance;
    private List<Popup> popups;

    private void Start()
    {
       InitializePopupSystem(this);
    }

    private static void InitializePopupSystem(PopupSystem popupSystem = null)
    {
        if (popupSystem == null)
        {
            popupSystem = Resources.Load<PopupSystem>("[POPUP SYSTEM]");
            _instance = Instantiate(popupSystem);
        }
        else _instance = popupSystem;
        var prefabs = Resources.LoadAll<Popup>("Popups");
        _instance.popups = new List<Popup>();
        foreach (var prefab in prefabs)
        {
            var newPopup = Instantiate(prefab, _instance.transform);
            newPopup.gameObject.SetActive(false);
            _instance.popups.Add(newPopup);
        }
    }

    public static T GetNotification<T>() where T : Popup
    {
        foreach (var popup in instance.popups)
        {
            if (popup is T) return (T)popup;
        }
        return null;
    }
    public static void InvokeNotification<T>() where T : Popup
    {
        GameManager.GetController<EnvironmentController>().InvokeMenuSound();
        foreach (var popup in instance.popups)
        {
            if (popup is T) 
            {
                popup.gameObject.SetActive(true);
            } 
            else popup.gameObject.SetActive(false);
        }
    }
}
