using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCodes : MonoBehaviour
{
    public Rigidbody rb;
    public Animator animator;
    private int health = 100;
    private float speed = 2f;
    private Transform target;
    private float damageTime = 1f, currentDamageTime;
    private int damage = 5;
    private bool die = false;

    void Start()
    {
        target = GameManager.instance.player.transform;
    }

    void Update()
    {
        if (!die)
        {
            FollowTarget();
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    void LateUpdate()
    {
        transform.position = GameManager.instance.GetInBorderPos(transform.position);
    }

    private void FollowTarget()
    {
        if (Vector3.Distance(target.position, transform.position) > 2f)
        {
            Vector3 rot = target.position - transform.position;
            rot.y = 0f;
            transform.rotation = Quaternion.LookRotation(rot);

            animator.SetBool("attack", false);
            rb.velocity = transform.forward * speed;
        }
        else
        {
            animator.SetBool("attack", true);
            rb.velocity = Vector3.zero;
            DamageControl();
        }
    }

    private void DamageControl()
    {
        if (currentDamageTime <= 0f)
        {
            GameManager.instance.player.TakeDamage(damage);
            currentDamageTime = damageTime;
        }
        else
        {
            currentDamageTime -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.bullet))
        {
            Destroy(other.gameObject);
            TakeDamage(int.Parse(other.name));
        }
    }

    private void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0 && !die)
        {
            //Destroy(gameObject);
            Destroy(GetComponent<Collider>());
            die = true;
            Invoke("DestroySelf", 5f);
            animator.SetTrigger("die");
        }
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
