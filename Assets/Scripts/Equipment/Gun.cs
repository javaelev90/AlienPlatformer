using Assets.Scripts.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections;

public class Gun : Shooter, IReloadable, IStateful
{
    public static GameObject Instance {get; private set;}
    private GunState gunState;
    [SerializeField] private AudioClip gunShotSound;
    [SerializeField] private AudioSource audioSource;
    private string nameOfGun;
    public int ammoInMagazine;
    [SerializeField] private UI_AmmoPanel UI_AmmoPanel;
    private bool isShooting = false;
    [SerializeField] private float shotInterval = 0.3f;
    private float shotCooldown = 0.3f;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        UI_AmmoPanel.HideReloadSign();
        UI_AmmoPanel.InActivatePanel();
        SaveState();
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if(Instance == null){
            Instance = gameObject;
        }
    }

    private void Update()
    {
        if (UI_AmmoPanel.IsActive())
        {
            UI_AmmoPanel.SetAmmoInMagazine(ammoInMagazine);
            UI_AmmoPanel.SetAmmoInTotal(ammo);
        }
         // Show/hide reload sign depending on magazine amount
        if (ammoInMagazine > 0)
        {
            UI_AmmoPanel.HideReloadSign();
        } else
        {
            UI_AmmoPanel.ShowReloadSign();
        }

    }

    public Gun()
    {
        nameOfGun = "MachineGun";
        ammo = 20;
        damage = 1;
        magazine = new GunMagazine();
        ammoInMagazine = magazine.ClipSize();
    }

    public override string GetItemName()
    {
        return nameOfGun;
    }

    public override void Activate()
    {
        if (!UI_AmmoPanel.IsActive())
        {
            UI_AmmoPanel.ActivatePanel();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            isShooting = true;
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            isShooting = false;
        }
        
        shotCooldown += Time.deltaTime;
        if(isShooting && shotCooldown >= shotInterval){
            Shoot();
            shotCooldown = 0;
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    public override void InActivate()
    {
        if (UI_AmmoPanel.IsActive())
        {
            UI_AmmoPanel.InActivatePanel();
        }
    }

    private void Shoot()
    {
        if (ammoInMagazine > 0)
        {
            Instantiate(bullet, firePoint.transform.position, firePoint.transform.rotation);
            ammoInMagazine--;
            audioSource.PlayOneShot(gunShotSound);
        } 
    }

    public void Reload()
    {
        // get total amount of ammo
        // figure out how much ammo that can be loaded into magazine
        // remove loaded amount from total amount of ammo

        increaseAmmo(ammoInMagazine);
        if(ammo >= magazine.ClipSize())
        {
            ammoInMagazine = magazine.ClipSize();
        } else
        {
            ammoInMagazine = ammo;
        }
        decreaseAmmo(ammoInMagazine);
    }

    public float ReloadTime()
    {
        return 1;
    }


    public void SaveState()
    {
        gunState = new GunState()
        {
            ammo = this.ammo,
            ammoInMagazine = this.ammoInMagazine,
        };
    }
    public void LoadSavedState()
    {
        ammo = gunState.ammo;
        ammoInMagazine = gunState.ammoInMagazine;
        if ((ammo + ammoInMagazine) < 10)
        {
            ammo = 5;
            ammoInMagazine = 5;
        }
    }
    public void ChangeAudioSource(AudioSource audioSource){
        this.audioSource = audioSource;
    }


    private class GunState
    {
        public int ammo;
        public int ammoInMagazine;
    }

}


