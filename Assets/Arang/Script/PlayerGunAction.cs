using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerGunAction : MonoBehaviour
{
    [Header("Weapon")]
    public List<WeaponConfiguration> weaponConfigs;
    public WeaponType usedWeaponType;
    public WeaponConfiguration UsedWeaponConfig => weaponConfigs.Find(x => x.weaponType == usedWeaponType);

    [Header("Setting")]
    public LayerMask raycastLayer;

    [Header("FX")]
    public GameObject hitFxPrefab;

    PlayerMovement _movementCont;

    void Start()
    {
        _movementCont = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DoFire();
        }
    }

    void DoFire()
    {
        Transform camTransform = _movementCont.PlayerCamera.transform;
        Vector3 camPos = camTransform.position;
        Vector3 camDir = camTransform.forward;

        Ray ray = new Ray();
        ray.origin = camPos;
        ray.direction = camDir;

        if (Physics.Raycast(ray, out RaycastHit hitInfo, float.PositiveInfinity, raycastLayer))
        {
            Vector3 hitPos = hitInfo.point;
            GameObject spawnedFx = Instantiate(hitFxPrefab, hitPos, Quaternion.identity);
            Destroy(spawnedFx, 2);
        }
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
}

public enum WeaponType
{
    AK47, Glock
}

public enum FireType
{
    Single, Burst, Auto
}