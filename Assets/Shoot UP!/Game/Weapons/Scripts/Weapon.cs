using Game_.Weapons;
using Game_.PickableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

namespace Game_
{
    public class Weapon : MonoBehaviour
    {
        private static List<GameObject> bullets;
        private static EnvironmentController environment;

        public WeaponInfo info => this._info;

        public bool isControllable 
        { 
            get => this.rigidbody.simulated; 
            set 
            {
                if (value) this.gameObject.SetActive(value);
                this.rigidbody.simulated = value;
            }
        }
        public bool isDestroyed => !this.root.gameObject.activeSelf;


        public float reloadingProgress => this.currentReloadDelay / this.info.reloadTime;
        public float moneyDoubleProgress { get; private set; }
        public float shieldProgress { get; private set; }
        public float slowProgress { get; private set; }
        public Vector3 position => this.transform.position;

        public int earnedMoney { get; private set; }
        public int clipValue { get; private set; }



        [SerializeField] private WeaponInfo _info;
        [SerializeField] private GameObject root;
        [SerializeField] private SpriteRenderer outline;
        [SerializeField] private Transform bulletFromTransform;
        [SerializeField] private ParticleSystem explosionFX;
        [SerializeField] private ParticleSystem shootFX;

        [SerializeField] private AudioClip emptySound;
        [SerializeField] private AudioClip reloadingSound;
        [SerializeField] private AudioClip destroySound;
        [SerializeField] private AudioClip pickSound;


        private Rigidbody2D rigidbody;
        private int bulletIndex;

        private float currentReloadDelay;
        private float currentDoubleMoneyDuration;
        private float currentShieldDuration;
        private float currentSlowMotionDuration;


        private void Start()
        {
            this.rigidbody = this.GetComponent<Rigidbody2D>();
            if (bullets == null)
                this.CreateBulletsPool();
            this.Reset();
            this.isControllable = false;
            if (environment == null) environment = GameManager.GetController<EnvironmentController>();
        }

        private void Update()
        {
            this.ApplyBulletsMovement();

            if (this.transform.position.x < this.info.maxCenterOffset * -1f)
                this.transform.position = new Vector3(this.info.maxCenterOffset, this.transform.position.y); 
            else if (this.transform.position.x > this.info.maxCenterOffset)
                this.transform.position = new Vector3(this.info.maxCenterOffset * -1f, this.transform.position.y);

            //temp
        }

        private void CreateBulletsPool()
        {
            bullets = new List<GameObject>();
            var bulletsParent = new GameObject("Bullets").transform;
            for (int i = 0; i < this.info.bulletPoolCapacity; i++)
            {
                var newBullet = Instantiate(this.info.bulletPrefab, this.bulletFromTransform.position, Quaternion.identity, bulletsParent);
                newBullet.SetActive(false);
                bullets.Add(newBullet);
            }
            this.bulletIndex = 0;
            this.isControllable = false;
        }

        private void InvokeBullet()
        {
            var bullet = bullets[this.bulletIndex];
            bullet.SetActive(true);
            bullet.transform.position = this.bulletFromTransform.position;
            bullet.transform.rotation = this.transform.rotation;
            this.bulletIndex++;
            if (this.bulletIndex >= this.info.bulletPoolCapacity) this.bulletIndex = 0;
        }

        private void ApplyBulletsMovement()
        {
            foreach (var bullet in bullets)
                if (bullet.activeSelf)
                    bullet.transform.Translate(Vector3.right * this.info.bulletSpeed * Time.deltaTime);
        }

        private void Reload() => this.StartCoroutine(this.ReloadingRoutine());

        private void FastReload()
        {
            this.currentReloadDelay = 0f;
            this.clipValue = this.info.clipCapacity;
            this.InvokeSound(this.reloadingSound);
        }

        private IEnumerator ReloadingRoutine()
        {
            this.currentReloadDelay = this.info.reloadTime;
            this.InvokeSound(this.reloadingSound);
            while (this.currentReloadDelay > 0f)
            {
                this.currentReloadDelay -= Time.deltaTime;
                yield return null;
            }
            this.currentReloadDelay = 0f;
            this.clipValue = this.info.clipCapacity;
            this.StopSounds();
        }

        private IEnumerator DoubleMoneyRoutine(float duration)
        {
            this.currentDoubleMoneyDuration = duration;
            while(this.currentDoubleMoneyDuration > 0f)
            {
                this.currentDoubleMoneyDuration -= Time.deltaTime;
                this.moneyDoubleProgress = this.currentDoubleMoneyDuration / duration;
                yield return null;
            }
            this.moneyDoubleProgress = 0f;
        }
        private IEnumerator ShieldRoutine(float duration)
        {
            this.currentShieldDuration = duration;
            var defaultColor = this.outline.color;
            this.outline.color = this.info.shieldColor;
            while (this.currentShieldDuration > 0f)
            {
                this.currentShieldDuration -= Time.deltaTime;
                this.shieldProgress = this.currentShieldDuration / duration;
                yield return new WaitForEndOfFrame();
            }
            this.outline.color = defaultColor;
            this.shieldProgress = 0f;
        }

        private IEnumerator SlowMotionRoutine(float duration, float slowdown)
        {
            duration *= 1f - slowdown;
            Time.timeScale = 1f - slowdown;
            this.currentSlowMotionDuration = duration;
            this.SetCameraFocusState(true);
            while (this.currentSlowMotionDuration > 0f)
            {
                this.currentSlowMotionDuration -= Time.deltaTime;
                this.slowProgress = this.currentSlowMotionDuration / duration;
                yield return null;
            }
            this.SetCameraFocusState(false);
            this.slowProgress = 0f;
            Time.timeScale = 1f;
        }

        private void InvokeSound(AudioClip clip)
        {
            if (environment != null) environment.InvokeSound(clip);
        }
        private void StopSounds()
        {
            if (environment != null) environment.StopSounds();
        }

        private void SetCameraFocusState(bool value)
        {
            if (environment != null) environment.isCameraFocus = value;
        }

        public void Shoot()
        {
            if (!this.isControllable) return;
            else if (this.reloadingProgress > 0f) InvokeSound(this.emptySound);

            if (this.reloadingProgress > 0f || !(this.isControllable)) return;
            this.rigidbody.velocity = Vector3.zero;
            this.rigidbody.angularVelocity = 0f;
            this.rigidbody.AddForce(this.transform.right * -1f * this.info.shotForce, ForceMode2D.Impulse);
            this.rigidbody.AddTorque(this.info.shotTorque);

/*            if (this.rigidbody.angularVelocity > this.info.maxAngularVelocity)
                this.rigidbody.angularVelocity = this.info.maxAngularVelocity;*/

            this.clipValue--;
            if (this.clipValue == 0) this.Reload();
            this.InvokeBullet();
            this.InvokeSound(this.info.shootSound);
            this.shootFX.Play();
        }

        public void InvokeStartingToss(Vector3 startPosition, float force = 10f, float torque = 15f)
        {
            this.rigidbody.velocity = Vector3.zero;
            this.transform.position = startPosition;
            this.rigidbody.AddForce(Vector3.up * force, ForceMode2D.Impulse);
            this.rigidbody.AddTorque(torque);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent<PickableObject>(out var pickableObject))
            {
                this.InvokeSound(this.pickSound);
                switch (pickableObject)
                {
                    case AmmoBox:
                        this.FastReload();
                        break;
                    case Money:
                        var money = pickableObject as Money;
                        if (this.moneyDoubleProgress > 0f) this.earnedMoney += money.amount * 2;
                        else this.earnedMoney += money.amount;
                        break;
                    case DoubleMoney:
                        var doubleMoney = pickableObject as DoubleMoney;
                        if (this.moneyDoubleProgress > 0f) this.currentDoubleMoneyDuration = doubleMoney.duration;
                        else this.StartCoroutine(this.DoubleMoneyRoutine(doubleMoney.duration));
                        break;
                    case Shield:
                        var shield = pickableObject as Shield;
                        if (this.shieldProgress > 0f) this.currentShieldDuration = shield.duration;
                        else this.StartCoroutine(this.ShieldRoutine(shield.duration));
                        break;
                    case SlowMotion:
                        var slowMotion = pickableObject as SlowMotion;
                        if (this.slowProgress > 0f) this.currentSlowMotionDuration = slowMotion.duration;
                        else this.StartCoroutine(this.SlowMotionRoutine(slowMotion.duration, slowMotion.slowdown));
                        break;
                }
                pickableObject.Pick();
            }
            else if (collision.gameObject.layer == 7 && this.shieldProgress == 0f) // Obstacles
                this.Destroy();

        }

        public void Invoke(Vector3 position)
        {
            this.isControllable = true;
            this.root.SetActive(true);
            this.InvokeStartingToss(position);
        }

        public void Destroy()
        {
            this.isControllable = false;
            this.root.SetActive(false);
            this.explosionFX.Play();
            this.InvokeSound(this.destroySound);
        }

        public void Reset()
        {
            this.earnedMoney = 0;
            this.currentDoubleMoneyDuration = 0f;
            this.currentReloadDelay = 0f;
            this.currentDoubleMoneyDuration = 0f;
            this.currentShieldDuration = 0f;
            this.currentSlowMotionDuration = 0f;
            this.earnedMoney = 0;
            this.clipValue = this.info.clipCapacity;
        }
    }

}
