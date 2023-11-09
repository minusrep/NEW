using Core;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Game_.CaseSystem
{
    [System.Serializable]
    public class CaseModel : Model
    {
        public List<Chance> chances => this._chances;
        public HorizontalLayoutGroup group => this._group;
        public RectTransform rectTransform => this._rectTransform;
        public Vector2 startPosition => this._startPosition;
        public float speed => this._speed;
        public float duration => this._duration;
        public CaseSlot caseSlotPrefab => this._caseSlotPrefab;
        public WeaponSlot weaponSlotPrefab => this._weaponSlotPrefab;
        public float slowdown => this._slowdown;
        public float slowdownSmoothness => this._slowdownSmoothness;
        public int offset => this._offset;
        public Transform weaponsParent => this._weaponsParent;
        public int price => this._price;
        public float rewardDelay => this._rewardDelay;
        public GameObject caseIcon => this._caseIcon;
        public GameObject rolling => this._rolling;
        public AnimationCurve speedCurve => this._speedCurve;
        public AudioClip caseStepSound => this._caseStepSound;
        public AudioClip rewardSound => this._rewardSound;

        [Header("Case Settings")]
        [SerializeField] private float _speed;
        [SerializeField] private float _duration;
        [SerializeField] private HorizontalLayoutGroup _group;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Vector2 _startPosition;

        [SerializeField] private CaseSlot _caseSlotPrefab;
        [SerializeField] private WeaponSlot _weaponSlotPrefab;
        
        [SerializeField] private List<Chance> _chances;
        [SerializeField] private float _slowdown;
        [SerializeField] private float _slowdownSmoothness;
        [SerializeField] private int _offset;
        [SerializeField] private Transform _weaponsParent;
        [SerializeField] private float _rewardDelay;
        [SerializeField] private int _price;
        [SerializeField] private GameObject _caseIcon;
        [SerializeField] private GameObject _rolling;

        [SerializeField] private AudioClip _caseStepSound;
        [SerializeField] private AudioClip _rewardSound;
        [SerializeField] private AnimationCurve _speedCurve;
    
    }


}