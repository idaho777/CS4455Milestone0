﻿using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public int damagePerShot = 20;
    public float reloadTime = 0.8f;
    public float timeBetweenBullets = 0.15f;
    public float range = 100f;
    public int currAmmo;

    float timer;
    Ray shootRay;
    RaycastHit shootHit;
    int shootableMask;
    ParticleSystem gunParticles;
    LineRenderer gunLine;
    AudioSource[] gunAudio;
    Light gunLight;
    float effectsDisplayTime = 0.2f;
    int maxAmmo = 20;

    void Awake ()
    {
        shootableMask = LayerMask.GetMask ("Shootable");
        gunParticles = GetComponent<ParticleSystem> ();
        gunLine = GetComponent <LineRenderer> ();
        gunAudio = GetComponents<AudioSource> ();
        gunLight = GetComponent<Light> ();
        currAmmo = maxAmmo;
        AmmoManager.maxAmmo = maxAmmo;
    }


    void Update ()
    {
        timer += Time.deltaTime;
        
		if(Input.GetButton ("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0)
        {
            Shoot();
        }

        if ((Input.GetButton("Reload") || Input.GetButton("Fire2")) && Time.timeScale != 0 && currAmmo < maxAmmo)
        {
            DisableEffects();
            Reload();
        }

        if (timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects ();
        }
        AmmoManager.ammo = currAmmo;
    }

    void Reload ()
    {
        timer = -reloadTime;
        gunAudio[2].Play();

        currAmmo = maxAmmo;
    }

    public void DisableEffects ()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }
    
    void Shoot ()
    {
        timer = 0f;

        if (currAmmo == 0)
        {
            gunAudio[1].Play();
            return;
        }

        currAmmo--;

        gunAudio[0].Play ();

        gunLight.enabled = true;

        gunParticles.Stop ();
        gunParticles.Play ();

        gunLine.enabled = true;
        gunLine.SetPosition (0, transform.position);

        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        if(Physics.Raycast (shootRay, out shootHit, range, shootableMask))
        {
            EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth> ();
            if(enemyHealth != null)
            {
                enemyHealth.TakeDamage (damagePerShot, shootHit.point);
            }
            gunLine.SetPosition (1, shootHit.point);
        }
        else
        {
            gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
        }
    }
}
