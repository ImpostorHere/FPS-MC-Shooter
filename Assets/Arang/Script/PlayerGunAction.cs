using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerGunAction : MonoBehaviour
{
    [Range(0, 1)]
    public float recoilProportion;
    public float currentRecoverySpeed;
    public int currentAmmo;
    public int reserveAmmo;
    public bool isReloading;

    [Header("References")]
    public Animator weaponAnimator;
    public GunEventBroadcaster weaponEventBroadcaster;

    [Header("Weapon")]
    public List<WeaponConfiguration> weaponConfigs;
    public WeaponType usedWeaponType;
    public WeaponConfiguration UsedWeaponConfig => weaponConfigs.Find(x => x.weaponType == usedWeaponType);

    [Header("Setting")]
    public LayerMask raycastLayer;

    [Header("FX")]
    public GameObject hitFxPrefab;
    
    PlayerMovement _movementCont;
    float _currentAutoFireTimer;
    float _currentRecoveryTime;

    void Start()
    {
        _movementCont = GetComponent<PlayerMovement>();
        InitWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReloading && IsEligibleToReload())
        {
            weaponAnimator.SetTrigger("Reload");
            isReloading = true;
        }

        if (UsedWeaponConfig.fireType == FireType.Single)
        {
            if (Input.GetMouseButtonDown(0) && !isReloading)
            {
                DoFire();
            }
        }
        else if (UsedWeaponConfig.fireType == FireType.Burst)
        {
            if (Input.GetMouseButtonDown(0) && !isReloading)
            {
                DoBurstFire();
            }
        }
        else if (UsedWeaponConfig.fireType == FireType.Auto)
        {
            if (Input.GetMouseButtonDown(0) && !isReloading)
            {
                DoFire();
                _currentAutoFireTimer = UsedWeaponConfig.fireDelay;
            }

            if (Input.GetMouseButton(0) && !isReloading)
            {
                if (_currentAutoFireTimer > 0)
                {
                    _currentAutoFireTimer -= Time.deltaTime;
                }
                else
                {
                    DoFire();
                    _currentAutoFireTimer = UsedWeaponConfig.fireDelay;
                }
            }
        }

        recoilProportion -= currentRecoverySpeed * Time.deltaTime;
        recoilProportion = Mathf.Max(0, recoilProportion);

        _currentRecoveryTime += UsedWeaponConfig.recoilRecoveryTimeFactor * Time.deltaTime;
        currentRecoverySpeed *= 1 + _currentRecoveryTime;
    }

    void InitWeapon()
    {
        currentAmmo = UsedWeaponConfig.maxAmmo;
        reserveAmmo = UsedWeaponConfig.maxAmmo * 3;
        currentRecoverySpeed = UsedWeaponConfig.recoilRecoverySpeed;

        weaponEventBroadcaster.OnReloadMagazineAttached -= DoReload;
        weaponEventBroadcaster.OnReloadMagazineAttached += DoReload;

        weaponEventBroadcaster.OnReloadDone -= ReloadDoneHandler;
        weaponEventBroadcaster.OnReloadDone += ReloadDoneHandler;
    }

    bool IsEligibleToReload()
    {
        if (currentAmmo < UsedWeaponConfig.maxAmmo && reserveAmmo > 0)
            return true;
        else
            return false;
    }

    void ReloadDoneHandler()
    {
        isReloading = false;
    }

    void DoReload()
    {
        // Not deleted for reference purpose
        // int ammoNeededToFullCap = UsedWeaponConfig.maxAmmo - currentAmmo;
        // if (reserveAmmo < ammoNeededToFullCap)
        // {
        //     currentAmmo += reserveAmmo;
        //     reserveAmmo = 0;
        // }
        // else
        // {
        //     reserveAmmo = reserveAmmo - ammoNeededToFullCap;
        //     currentAmmo = UsedWeaponConfig.maxAmmo;
        // }

        int ammoNeeded = UsedWeaponConfig.maxAmmo - currentAmmo;
        int ammoToLoad = Mathf.Min(reserveAmmo, ammoNeeded);

        currentAmmo += ammoToLoad;
        reserveAmmo -= ammoToLoad;
    }

    void DoBurstFire()
    {
        if (burstCoroutine != null)
        {
            return;
        }

        burstCoroutine = StartCoroutine(BurstFireCo(UsedWeaponConfig.burstFireCount, UsedWeaponConfig.fireDelay));
    }

    Coroutine burstCoroutine;
    IEnumerator BurstFireCo(int fireCount, float fireDelay)
    {
        int fireCountLeft = fireCount;
        while (fireCountLeft > 0)
        {
            DoFire();
            fireCountLeft--;

            if (fireCountLeft == 0)
                break;
            else
                yield return new WaitForSeconds(fireDelay);
        }

        burstCoroutine = null;
    }

    void DoFire()
    {
        if (currentAmmo <= 0)
        {
            // Do no ammo SFX
            return;
        }

        Transform camTransform = _movementCont.PlayerCamera.transform;
        Vector3 camPos = camTransform.position;
        Vector3 camDir = camTransform.forward;

        float recoilX = Random.Range(-UsedWeaponConfig.recoil, UsedWeaponConfig.recoil) * recoilProportion;
        float recoilY = Random.Range(-UsedWeaponConfig.recoil, UsedWeaponConfig.recoil) * recoilProportion;

        camDir += camTransform.right * recoilX;
        camDir += camTransform.up * recoilY;

        Ray ray = new Ray();
        ray.origin = camPos;
        ray.direction = camDir;

        if (Physics.Raycast(ray, out RaycastHit hitInfo, float.PositiveInfinity, raycastLayer))
        {
            Vector3 hitPos = hitInfo.point;
            GameObject spawnedFx = Instantiate(hitFxPrefab, hitPos, Quaternion.identity);
            Destroy(spawnedFx, 2);
        }

        weaponAnimator.SetTrigger("Shoot");
        currentAmmo--;

        recoilProportion += UsedWeaponConfig.recoilProportionIncrement;
        recoilProportion = Mathf.Min(recoilProportion, 1);

        _currentRecoveryTime = 0;
        currentRecoverySpeed = UsedWeaponConfig.recoilRecoverySpeed;
    }
}

[Serializable]
public class WeaponConfiguration
{
    public WeaponType weaponType;
    public FireType fireType;
    public GameObject weaponPrefab;
    public int maxAmmo;
    public float fireDelay;
    public float recoil;

    // Menambahkan nilai Recoil Proportion di tiap tembakan
    [Range(0, 1)]
    public float recoilProportionIncrement = 0.1f;

    // Mengurangin nilai Recoil Proportion per waktu
    [Range(0, 1)]
    public float recoilRecoverySpeed = 0.25f;

    [Range(0, 1)]
    public float recoilRecoveryTimeFactor = 0.25f;

    public int burstFireCount = 3;
}

public enum WeaponType
{
    AK47, Glock
}

public enum FireType
{
    Single, Burst, Auto
}