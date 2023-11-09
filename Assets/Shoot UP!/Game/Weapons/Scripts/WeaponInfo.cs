using UnityEngine;

namespace Game_.Weapons
{
    [CreateAssetMenu(fileName = "ShopItem", menuName = "Game/WeaponItem")]
    [System.Serializable]
    public class WeaponInfo : ScriptableObject
    {
        public int id => this._id;
        // Shoot Settings
        public float shotForce => this._shotForce;
        public float shotTorque => this._shotTorque;
        public float maxAngularVelocity => this._maxAngularVelocity;
        // Bullet Settings
        public GameObject bulletPrefab => this._bulletPrefab;
        public int bulletPoolCapacity => this._bulletPoolCapacity;
        public float bulletSpeed => this._bulletSpeed;
        // Movement Settings
        public float maxCenterOffset => this._maxCenterOffset;
        // Reloading Settings
        public int clipCapacity => this._clipCapacity;
        public float reloadTime => this._reloadTime;
        public Color shieldColor => this._shieldColor;
        public Color outlineColor => this._outlineColor;
        public Sprite sprite => this._sprite;
        public AudioClip shootSound => this._shootSound;

        [SerializeField] private int _id;
        [Header("Shoot Settings")]
        [SerializeField] private float _shotForce;
        [SerializeField] private float _shotTorque;
        [SerializeField] private float _maxAngularVelocity;
        [Header("Bullet Settings")]
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private int _bulletPoolCapacity;
        [SerializeField] private float _bulletSpeed;
        [Header("Movement Settings")]
        [SerializeField] private float _maxCenterOffset;
        [Header("Reloading Settings")]
        [SerializeField] private int _clipCapacity;
        [SerializeField] private float _reloadTime;
        [Header("Others")]
        [SerializeField] private Color _shieldColor;
        [SerializeField] private Color _outlineColor;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private AudioClip _shootSound;
    }
}