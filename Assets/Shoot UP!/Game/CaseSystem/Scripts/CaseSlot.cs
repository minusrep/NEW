using Game_.Weapons;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Game_.CaseSystem
{
    public class CaseSlot : MonoBehaviour
    {
        public int id { get; private set; }
        public Vector2 size => this.rectTransform.sizeDelta;

        [SerializeField] private Image icon;
        [SerializeField] private Image rarityFlag;
        [SerializeField] private RectTransform rectTransform;



        public void Initialize(WeaponInfo weapon)
        {
            this.icon.sprite = weapon.sprite;
            this.rarityFlag.color = weapon.outlineColor;
            this.id = weapon.id;
        }
        
    }

}
