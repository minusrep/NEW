using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using Yandex;

public class LocalizedText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI info;

    [SerializeField] private string ru;
    [SerializeField] private string en;
    [SerializeField] private string tr;

    private void Start()
    {
        switch (GameManager.language)
        {
            case "ru":
                this.info.text = this.ru;
                break;
            case "tr":
                this.info.text = this.tr;
                break;
            default:
                this.info.text = this.en;
                break;
        }


    }

    /*    private void OnEnable()
        {
            YandexSDK.OnLanguageChanged += this.OnLanguageChanged;
        }

        private void OnDisable()
        {
            YandexSDK.OnLanguageChanged -= this.OnLanguageChanged;
        }

        private void OnLanguageChanged(string value)
        {
            switch (value)
            {
                case "ru":
                    this.info.text = this.ru;
                    break;
                case "tr":
                    this.info.text = this.tr;
                    break;
                default:
                    this.info.text = this.en;
                    break;
            }
            Debug.Log($"{this.gameObject.name} changed text value");
        }*/
}
