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

    public GameObject prefab;

    public void Start()
    {
        ammo = maxAmmo;
        reloadTimer = 0;
        fireTimer = 0;
    }

    public void Fire(Vector3 position, Quaternion rotation)
    {
        if (fireTimer > 0 || reloadTimer > 0)
            return;

        Vector3 aimSpread = new Vector3(Random.Range(-spread, spread), Random.Range(-spread, spread), Random.Range(-spread, spread));
        aimSpread.Normalize();

        Instantiate(prefab, position,
            rotation * Quaternion.Euler(aimSpread));

        fireTimer = (1 / fireRate);

        ammo--;
        if (ammo <= 0)
            reloadTimer = reloadTime;
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
}
