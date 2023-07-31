using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using StarterAssets;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class ThirdPersonShooterController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    private StarterAssetsInputs _starterAssetsInputs;
    public LayerMask aimColliderLayerMask;
    [SerializeField] private GameObject bulletPrefab;
    public Transform target;
    private GameObject _bullet;
    private float cameraRotationY;
    private Vector3 mouseWorldPosition;
    public Weapon weapon;
    public AimConstraint aimConstraint;
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
            ReloadingStartForShootingWeapon();
            _starterAssetsInputs.reload = false;
        }
    }

    private void FixedUpdate()
    {
        ScreenCenterAiming();
        RateOfFireForShootingWeapon();
    }

    private void ScreenCenterAiming()
    {
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        RaycastHit hit;
        
        if(Physics.Raycast(ray, out hit, 100f, aimColliderLayerMask))
        {
            mouseWorldPosition = hit.point;
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
                    weapon.particle.Play();

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
