using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class ThirdPersonShooterController : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private AimConstraint aimConstraint;
    private float cameraRotationY;
    
    [Header("Weapon")]
    [SerializeField] private LayerMask aimColliderLayerMask;
    [SerializeField] private Weapon weapon;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform target;
    
    [Header("UI")]
    public TMP_Text bulletsText;
    public Canvas weaponCanvas;
    public Image reloadingSlide;
    
    [Header("Audio")]
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioSource reloadSound;
    [SerializeField] [Range(0, 1)] private float weaponAudioVolume;
    
    private StarterAssetsInputs _starterAssetsInputs;
    
    private Vector3 mouseWorldPosition;
    private Transform hitTransform;
    private GameObject _bullet;
    private bool isReloading;

    private void Awake()
    {
        _starterAssetsInputs = GetComponent<StarterAssetsInputs>();
    }

    private void Update()
    {
        if(_starterAssetsInputs.aim)
        {
            aimVirtualCamera.gameObject.SetActive(true);
            aimConstraint.weight = 1f;

            cameraRotationY = Camera.main.transform.rotation.eulerAngles.y;
            
            transform.rotation = Quaternion.Euler(0f, cameraRotationY, 0f);
            
            if (_starterAssetsInputs.shoot)
            {
               ShootingWeapon();
            }
            else
            {
                _starterAssetsInputs.shoot = false;
            }
        }
        else
        {
            aimVirtualCamera.gameObject.SetActive(false);
            aimConstraint.weight = 0f;
        }
        
        if(_starterAssetsInputs.reload)
        {
            _starterAssetsInputs.reload = false;
            ReloadingStartForShootingWeapon();
        }

        WeaponCanvas();
    }
    private void FixedUpdate()
    {
        ScreenCenterAiming();
        RateOfFireForShootingWeapon();
    }

    private void WeaponCanvas()
    {
        weaponCanvas.enabled = _starterAssetsInputs.aim;
        bulletsText.enabled = !isReloading;
        reloadingSlide.enabled = isReloading;
    }
    
    private void ScreenCenterAiming()
    {
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        RaycastHit hit;
        
        hitTransform = null;
        if(Physics.Raycast(ray, out hit, 100f, aimColliderLayerMask))
        {
            mouseWorldPosition = hit.point;
            hitTransform = hit.transform;
        }
    }
    
    void ShootingWeapon()
    {
        if (!isReloading)
        {
            if (weapon.bullets > 0)
            {
                if (weapon.shootingSpeed >= weapon.maxShootingSpeed)
                {
                    weapon.bullets--;
                    weapon.shootingSpeed = 0;
                    AudioSource.PlayClipAtPoint(shootSound, weapon.shootingPoint.position, weaponAudioVolume);

                    if (hitTransform != null)
                    {
                        if (hitTransform.TryGetComponent<EnemyHealthSystem>(out EnemyHealthSystem enemyHealth))
                        {
                            enemyHealth.TakeDamage(weapon.damage);
                        }
                    } 
                        
                    _bullet = Instantiate(bulletPrefab, weapon.shootingPoint.position, Quaternion.LookRotation(target.position, Vector3.up), null);
                    _bullet.GetComponent<BulletProjectile>().target = mouseWorldPosition;

                    if (weapon.bullets == 0)
                        ReloadingStartForShootingWeapon();
                }
            }
            else
            {
                ReloadingStartForShootingWeapon();
            }
        }
    }
    
    void RateOfFireForShootingWeapon()
    {
        if (weapon.shootingSpeed < weapon.maxShootingSpeed)
        {
            weapon.shootingSpeed += Time.deltaTime * 10f;
        }

        if (weapon.reloading < weapon.maxReloading)
        {
            weapon.reloading++;
        }
        else
        {
            if (isReloading)
                ReloadingFinishForShootingWeapon();
        }
    }
    
    void ReloadingStartForShootingWeapon()
    {
        weapon.reloading = 0;
        reloadSound.Play();
        isReloading = true;
    }
    
    void ReloadingFinishForShootingWeapon()
    {
        if (weapon.bullets >= 0 && weapon.bullets < weapon.bulletsMax)
        {
            float remainingBullets = weapon.bulletsMax - weapon.bullets;
        
            if (weapon.bulletsAll >= remainingBullets)
            {
                weapon.bulletsAll -= remainingBullets;
                weapon.bullets = weapon.bulletsMax;
            }
            else
            {
                weapon.bullets += weapon.bulletsAll;
                weapon.bulletsAll = 0;
            }
        }
        else if (weapon.bulletsAll <= weapon.bulletsMax)
        {
            weapon.bullets = weapon.bulletsAll;
            weapon.bulletsAll = 0;
        }

        isReloading = false;
    }
}
