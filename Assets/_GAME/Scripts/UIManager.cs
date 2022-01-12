using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public RectTransform gunBar;
    public Transform gunParent;
    public RectTransform gunUIPrefab;

    private Transform[] gunUITransforms;
    private TextMeshProUGUI[] gunBulletCountTexts;
    private int currentGunIndex;

    public Image healthBar;

    void Awake()
    {
        instance = this;
    }

    public void SetGunUI(int gunIndex)
    {
        gunUITransforms[currentGunIndex].localScale = Vector3.one;
        gunUITransforms[gunIndex].localScale = Vector3.one * 1.25f;
        currentGunIndex = gunIndex;
    }

    public void SetGunBulletCount(Gun gun)
    {
        gunBulletCountTexts[currentGunIndex].text = gun.currentBulletCount.ToString();
    }

    public void SetGunBulletCount(Gun gun, int gunIndex)
    {
        gunBulletCountTexts[gunIndex].text = gun.currentBulletCount.ToString();
    }

    public void SetHealthBarFill(float fill)
    {
        healthBar.fillAmount = fill;
    }

    public void InitGunUI(Gun[] guns)
    {
        foreach (Transform child in gunParent)
        {
            Destroy(child.gameObject);
        }

        gunBar.sizeDelta *= Vector2.up;

        gunUITransforms = new Transform[guns.Length];
        gunBulletCountTexts = new TextMeshProUGUI[guns.Length];

        for (int i = 0; i < guns.Length; i++)
        {
            RectTransform gun = Instantiate(gunUIPrefab);
            gun.SetParent(gunParent);
            gun.localScale = Vector3.one;
            gunUITransforms[i] = gun;
            gun.SetSiblingIndex(i);

            float width = gunBar.sizeDelta.x;
            width += gun.sizeDelta.x * 1.25f;
            Vector2 sizeDelta = gunBar.sizeDelta;
            sizeDelta.x = width;
            gunBar.sizeDelta = sizeDelta;

            gun.Find("sprite").GetComponent<Image>().sprite = guns[i].gunSprite;
            gun.Find("name").GetComponent<TextMeshProUGUI>().text = guns[i].gunName;
            gun.Find("number").GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();
            gun.Find("bulletcount").GetComponent<TextMeshProUGUI>().text = guns[i].baseBulletCount.ToString();
            gunBulletCountTexts[i] = gun.Find("bulletcount").GetComponent<TextMeshProUGUI>();
        }
    }
}
