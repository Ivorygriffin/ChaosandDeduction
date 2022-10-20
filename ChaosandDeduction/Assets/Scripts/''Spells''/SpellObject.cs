using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell")]
public class SpellObject : ScriptableObject
{
    public float fireRate;
    float fireTimer;
    public float spread;
    public int maxAmmo;
    int ammo;
    public float reloadTime;
    float reloadTimer;

    public bool automatic;

    public GameObject prefab;

    public void Start()
    {
        ammo = maxAmmo;
        reloadTimer = 0;
        fireTimer = 0;
    }

    public GameObject Fire(Vector3 position, Quaternion rotation)
    {
        if (fireTimer > 0 || reloadTimer > 0)
            return null;

        fireTimer = (1 / fireRate);
        ammo--;
        if (ammo <= 0)
            reloadTimer = reloadTime;

        Vector3 aimSpread = new Vector3(Random.Range(-spread, spread), Random.Range(-spread, spread), Random.Range(-spread, spread));
        aimSpread.Normalize();

        //pass out the GameObject to be spawned across the network
        return Instantiate(prefab, position,
             rotation * Quaternion.Euler(aimSpread));
    }

    public void Update()
    {
        if (fireTimer > 0)
            fireTimer -= Time.deltaTime;

        if (reloadTimer > 0)
        {
            reloadTimer -= Time.deltaTime;
            if (reloadTimer <= 0)
                ammo = maxAmmo;
        }
    }

    public float GetAmmoCount()
    {
        return (float)ammo / maxAmmo;
    }
}
