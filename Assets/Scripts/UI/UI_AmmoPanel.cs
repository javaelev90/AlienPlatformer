using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_AmmoPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text ammoInMagazine;
    [SerializeField] private TMP_Text ammoInTotal;
    [SerializeField] private TMP_Text reloadSign;

    public void SetAmmoInTotal(int ammo)
    {
        ammoInTotal.text = ammo.ToString();
    }

    public void SetAmmoInMagazine(int ammo)
    {
        ammoInMagazine.text = ammo.ToString();
    }

    public void ActivatePanel()
    {
        gameObject.SetActive(true);
    }

    public void InActivatePanel()
    {
        gameObject.SetActive(false);
    }

    public bool IsActive()
    {
        return gameObject.activeSelf;
    }

    public void ShowReloadSign()
    {
        reloadSign.enabled = true;
    }

    public void HideReloadSign()
    {
        reloadSign.enabled = false;
    }

}
