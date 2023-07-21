using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SonarGun : Shooter
{

    [SerializeField] private GameObject UI_SonarPanel;
    [SerializeField] private AudioClip gunShotSound;
    [SerializeField] private AudioSource audioSource;
    private float shootCooldown = 1.1f;
    private float shootCooldownTimer = 1f;

    private string nameOfGun;

    private void Start()
    {
        nameOfGun = "SonarGun";
        if(UI_SonarPanel == null){
            GameObject UI = GameObject.FindWithTag("UI");
            Transform[] transforms= UI.GetComponentsInChildren<Transform>(true);
            foreach(Transform childTransform in transforms){
                if(childTransform.tag == "SonarPanel"){
                    UI_SonarPanel = childTransform.gameObject;
                    break;
                }
            }
        }
        if(playerEquipmentHolder == null){
            playerEquipmentHolder = GameObject.FindGameObjectWithTag("PlayerEquipmentHolder").transform;
        }
        UI_SonarPanel.SetActive(false);

    }

    private void Shoot()
    {
        Instantiate(bullet, firePoint.position, firePoint.rotation);
        audioSource.PlayOneShot(gunShotSound);
    }

    public override string GetItemName()
    {
        return nameOfGun;
    }

    public override void Activate()
    {
        if (!UI_SonarPanel.activeSelf)
        {
            UI_SonarPanel.SetActive(true);
        }
        if (Input.GetButtonDown("Fire1"))
        {
            if (shootCooldownTimer >= shootCooldown)
            {
                Shoot();
                shootCooldownTimer = 0f;
            } 
        }
        if (shootCooldownTimer < shootCooldown)
        {
            shootCooldownTimer += Time.deltaTime;
        }
    }

    public override void InActivate() {
        if (UI_SonarPanel.activeSelf)
        {
            UI_SonarPanel.SetActive(false);
        }
    }
}
