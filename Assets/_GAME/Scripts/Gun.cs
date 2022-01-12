using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Gun", menuName = "Scriptables/Gun", order = 1)]
public class Gun : ScriptableObject
{
    public string gunName;
    public Sprite gunSprite;
    public GameObject gunPrefab;
    public BulletCodes bulletPrefab;
    [HideInInspector] public Transform bulletPoint;
    public float bulletTime;
    public int baseBulletCount;
    public int bulletDamage;
    [HideInInspector] public int currentBulletCount;
}
