using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : MonoBehaviour
{
    //Initialize variables
    public float moveSpeed = 6;
    public int enemyHealth;
    public float meleeRange = 2.5f;
    public bool HasMeleeAttack = true;
    public string debugState;

    //Information variables
    public bool isDead;
    public bool isDeaf; 

    //Attack variables
    public int reactionTime;
    public int attackTimer;
    public GameObject projectile;
    public int targetThreshold;
    public int meleeDamage = 10;

    //Navigation variables
    public NavMeshAgent navMeshAgent;
    [SerializeField] public Transform destination;
    [SerializeField] public GameObject target;

    //Audio variables
    private AudioSource audioSource;
    public AudioClip[] audioClips;

    //State machine variables
    public readonly EnemyStateIdle STATE_IDLE = new EnemyStateIdle();
    public readonly EnemyStateChase STATE_CHASE = new EnemyStateChase();
    public readonly EnemyStateDead STATE_DEAD = new EnemyStateDead();
    public readonly EnemyStateRangeAttack STATE_RANGE_ATTACK = new EnemyStateRangeAttack();
    public readonly EnemyStateMeleeAttack STATE_MELEE_ATTACK = new EnemyStateMeleeAttack();

    private EnemyState currentState;

    void Start()
    {
        navMeshAgent = this.GetComponent<NavMeshAgent>();

        ChangeState(STATE_IDLE);
        debugState = "IDLE";

        enemyHealth = 60;

        isDead = false;
        isDeaf = false; 

        reactionTime = 11;
        targetThreshold = 100;

        audioSource = GetComponent<AudioSource>();
    }


    void FixedUpdate() {
        currentState.Update(this);
    }


    public void ChangeState(EnemyState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    public void PlaySound(AudioClip sound){
        audioSource.PlayOneShot(sound);
    }

    public void TakeDamage(int damageAmount, string damageType, GameObject damageOrigin){
        //Check if object hit itself
        if (damageOrigin != this.gameObject){
            //Subtract damage from health
            enemyHealth -= damageAmount;

            //If the damage has an origin attached, make it the target
            if (damageOrigin != null){
                target = damageOrigin;
            }
            
            //Come out of the idle state
            if (currentState == STATE_IDLE){
                audioSource.PlayOneShot(audioClips[Random.Range(0, 2)]);
                ChangeState(STATE_CHASE);
            } else {
                //If not idle, play hurt sound
                audioSource.PlayOneShot(audioClips[4]);
            }

            //Retaliate with ranged attack if possible
            if (!Physics.Raycast(transform.position, (target.transform.position - transform.position))){
                ChangeState(STATE_RANGE_ATTACK);
            }
        }
    }

    public void TakeDamage(int damageAmount, string damageType){
        enemyHealth -= damageAmount;

        if (currentState == STATE_IDLE){
            audioSource.PlayOneShot(audioClips[Random.Range(0, 2)]);
            ChangeState(STATE_CHASE);
        } else {
            audioSource.PlayOneShot(audioClips[4]);
        }
    }

    public void Kill(){
        Destroy(this.gameObject);
    }
}

