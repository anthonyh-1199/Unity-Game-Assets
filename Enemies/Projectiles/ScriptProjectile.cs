using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptProjectile : MonoBehaviour
{
    //Initialize variables
    private int projectileDamage;
    private string damageType;
    private GameObject damageOrigin;
    private float projectileSpeed;

    // Start is called before the first frame update
    void Start()
    {
        projectileDamage = 15;
        damageType = "energy";
        projectileSpeed = 10.0f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * projectileSpeed;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, transform.localScale.x);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.tag == "Enemy"){
                if (hitCollider.transform.parent != null){
                    EnemyStateMachine enemy = hitCollider.transform.parent.gameObject.GetComponent<EnemyStateMachine>();
                    if (damageOrigin != null){
                        enemy.TakeDamage(projectileDamage, damageType, damageOrigin);
                    }
                    Destroy(this.gameObject);
                }
            }
            if (hitCollider.gameObject.layer == LayerMask.NameToLayer("Player")){
                PlayerStateMachine player = hitCollider.transform.parent.gameObject.GetComponent<PlayerStateMachine>();
                player.TakeDamage(projectileDamage, damageType);
                Destroy(this.gameObject);
            }

        }
    }

    public void SetOrigin(GameObject origin){
        this.damageOrigin = origin;
    }
}
