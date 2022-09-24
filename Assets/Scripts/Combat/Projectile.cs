using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Resources;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 1;
    [SerializeField] bool isHoming = false;
    [SerializeField] GameObject hitImpact = null;
    [SerializeField] float maxLifeTime = 10;
    [SerializeField] GameObject[] destroyOnHit = null;
    [SerializeField] float lifeAfterImpact = 1.5f;
    [SerializeField] UnityEvent OnLaunch;
    [SerializeField] UnityEvent OnHit;

    Health target = null;
    float damage = 0;
    GameObject instigator;
    void Start()
    {
        transform.LookAt(GetAimLocation());
    }
    void Update()
    {
        if (target == null) return;

        if(!target.IsDead() && isHoming)
            transform.LookAt(GetAimLocation());

        transform.Translate(Vector3.forward * speed * Time.deltaTime);      
    }

    public void SetTarget(Health target, GameObject instigator, float damage)
    {
        this.target = target;
        this.damage = damage;
        this.instigator = instigator;

        Destroy(gameObject, maxLifeTime);
    }

    private Vector3 GetAimLocation()
    {
        CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
        if (targetCapsule == null)
        {
            return target.transform.position;
        }
        return target.transform.position + Vector3.up * targetCapsule.height / 2;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.GetComponent<Health>() != target) return;  
        if(target.IsDead()) return;
        OnHit.Invoke();
        //Create impact effect
        if(hitImpact != null)
        {
            Instantiate(hitImpact, GetAimLocation(), transform.rotation);
        }

        //Destroy the projectile first
        foreach(GameObject obj in destroyOnHit)
        {
            Destroy(obj);
        }
        target.TakeDamage(instigator, damage); 

        //Destroy the rest of the Projectile object after a life time, 
        //so that the trail can continue to live up its path-life 
        Destroy(gameObject, lifeAfterImpact); 
    }

}
