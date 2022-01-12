using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

public class PlayerCodes : MonoBehaviour
{
    //game system
    private int health = 100;
    private bool fail;

    //move
    public Rigidbody rb;
    private float speed = 10f;

    //look
    public LayerMask groundLayer;
    private Camera mainCamera;
    public Transform mouseTarget;

    //shoot
    public Transform gunParent;
    public Gun[] guns;
    private int activeGunIndex;
    private Transform bulletPoint;
    private BulletCodes bulletPrefab;
    private float bulletTime, currentBulletTime;

    //input
    private KeyCode[] keyCodes =
    {
         KeyCode.Alpha1,
         KeyCode.Alpha2,
         KeyCode.Alpha3,
         KeyCode.Alpha4,
         KeyCode.Alpha5,
         KeyCode.Alpha6,
         KeyCode.Alpha7,
         KeyCode.Alpha8,
         KeyCode.Alpha9,
    };

    //animations
    public Animator animator;

    void Start()
    {
        mouseTarget.parent = null;
        mainCamera = Camera.main;
        InitGuns();
    }

    void Update()
    {
        Move();
        SetMouseTarget();
        ShootControl();
        GunInputControl();
        SetAnimationBools();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10);
        }
    }

    void LateUpdate()
    {
        transform.position = GameManager.instance.GetInBorderPos(transform.position);
    }

    private void SetAnimationBools()
    {
        animator.SetBool("running", rb.velocity != Vector3.zero);
    }

    private void GunInputControl()
    {
        for (int i = 0; i < keyCodes.Length; i++)
        {
            if (Input.GetKeyDown(keyCodes[i]) && i != activeGunIndex && i < guns.Length)
            {
                SetGun(i);
            }
        }
    }

    private void InitGuns()
    {
        UIManager.instance.InitGunUI(guns);
        foreach (Transform child in gunParent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < guns.Length; i++)
        {
            guns[i].currentBulletCount = guns[i].baseBulletCount;
            GameObject gun = Instantiate(guns[i].gunPrefab);
            gun.transform.parent = gunParent;
            gun.transform.localPosition = Vector3.zero;
            gun.transform.localRotation = Quaternion.identity;
            //gun.transform.SetSiblingIndex(i);
            guns[i].bulletPoint = gun.transform.Find("BulletPoint");
            gun.gameObject.SetActive(false);
        }

        SetGun(0);
    }

    private void SetGun(int gunIndex)
    {
        UIManager.instance.SetGunUI(gunIndex);
        gunParent.GetChild(activeGunIndex).gameObject.SetActive(false);
        activeGunIndex = gunIndex;
        bulletPoint = guns[activeGunIndex].bulletPoint;
        bulletPrefab = guns[activeGunIndex].bulletPrefab;
        bulletTime = guns[activeGunIndex].bulletTime;
        currentBulletTime = guns[activeGunIndex].bulletTime;
        gunParent.GetChild(activeGunIndex).gameObject.SetActive(true);
    }

    private Vector3 GetMoveVector()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        return new Vector3(x, 0f, z).normalized;
    }

    private void Move()
    {
        Vector3 moveVector = GetMoveVector();
        rb.velocity = moveVector * speed;

        Vector3 rotVector = moveVector;
        rotVector.y = 0f;

        Quaternion rot = Quaternion.LookRotation(rotVector);
        if (!rb.velocity.Equals(Vector3.zero))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, 30f * Time.deltaTime);
        }
    }

    private void SetMouseTarget()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, float.MaxValue, groundLayer.value))
        {
            mouseTarget.position = hit.point;
        }
    }

    private void ShootControl()
    {
        if (Input.GetMouseButton(0) && currentBulletTime <= 0f && guns[activeGunIndex].currentBulletCount > 0)
        {
            currentBulletTime = bulletTime;
            CreateBullet();
            guns[activeGunIndex].currentBulletCount--;
            UIManager.instance.SetGunBulletCount(guns[activeGunIndex]);
        }
        else if (currentBulletTime >= 0f)
        {
            currentBulletTime -= Time.deltaTime;
        }
    }

    private void CreateBullet()
    {
        BulletCodes bullet = Instantiate(bulletPrefab);
        bullet.name = guns[activeGunIndex].bulletDamage.ToString();
        bullet.transform.position = bulletPoint.position;
        Vector3 moveVector = (mouseTarget.position - bulletPoint.position).normalized;
        moveVector.y = 0f;
        bullet.moveVector = moveVector;
        bullet.speed = 25f;
        bullet.endTime = 3f;
    }

    public void TakeDamage(int damage)
    {
        if (fail)
        {
            return;
        }

        health -= damage;
        if (health <= 0)
        {
            health = 0;
            SetFail();
        }
        UIManager.instance.SetHealthBarFill(health / 100f);
    }

    private void SetFail()
    {
        fail = true;
        Debug.Log("fail");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void AddBullet(int amount)
    {
        for (int i = 0; i < guns.Length; i++)
        {
            guns[i].currentBulletCount += amount;
            UIManager.instance.SetGunBulletCount(guns[i], i);
        }
    }

    public void AddHealth(int amount)
    {
        health += amount;
        if (health > 100)
        {
            health = 100;
        }
        UIManager.instance.SetHealthBarFill(health / 100f);
    }
}
