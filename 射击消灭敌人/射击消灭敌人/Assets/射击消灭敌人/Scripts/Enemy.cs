using System.Collections;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    public enum State { Idle, Chasing, Attack }//待机//追逐//攻击
    public GameObject effect;
    NavMeshAgent agent;
    Transform target;
    float nextAttackTime;//攻击时间间隔中间数
    State currentState;
    float myCollisonR;//敌人碰撞器半径
    float targetCollisonR;//玩家碰撞器半径
    Material material;
    float health = 1;
    bool dead = false;
    public static Enemy Instance;
    private void Awake()
    {
        Instance = this;
        material = GetComponent<Renderer>().material;
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        myCollisonR = GetComponent<CapsuleCollider>().radius;
        targetCollisonR = target.GetComponent<CapsuleCollider>().radius;
    }
    void Start()
    {
        currentState = State.Chasing;
        StartCoroutine(UpdatePath(0.25f, new Vector3()));
    }
    void Update()
    {
        if (Player.Instance.dead)
        {
            return;
        }
        if (Time.time > nextAttackTime)
        {
            float distance = (target.position - transform.position).sqrMagnitude;
            if (distance < Mathf.Pow(0.5f + myCollisonR + targetCollisonR, 2))
            {
                nextAttackTime = Time.time + 1;
                StartCoroutine(Attack());
            }
        }
    }
    public void TaskHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        if (damage >= health)
        {
            Game.Insatnce.Score();
            GameObject eff = Instantiate(effect, hitPoint, Quaternion.FromToRotation(Vector3.forward, hitDirection));
            eff.GetComponent<Renderer>().material.color = material.color;
            Destroy(eff, 2);
        }
        health -= damage;
        if (health <= 0 && !dead)
        {
            Die();
        }
    }
    public void Die()
    {
        dead = true;
        Spawn.Instance.OnEnemyDath();
        Destroy(gameObject);
    }
    IEnumerator Attack()
    {
        material.color = Color.red;
        currentState = State.Attack;
        agent.enabled = false;
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        Vector3 originalPosition = transform.position;
        Vector3 attackPosition = target.position - dirToTarget * (targetCollisonR);
        float percent = 0;
        bool hasAppliedDamage = false;//伤害判断避免重复收到伤害
        while (percent <= 1)
        {
            if (percent > 0.5 && !hasAppliedDamage)
            {
                Game.Insatnce.MP3(false);
                hasAppliedDamage = true;
                target.GetComponent<Rigidbody>().AddForce(dirToTarget * 15f, ForceMode.Impulse);
                target.GetComponent<Player>().TashDamage(10);
            }
            percent += Time.deltaTime * 3;
            float t = 4 * (-Mathf.Pow(percent, 2) + percent);//这个数他会当percent值在0~1之间的时候的时候它的值会从0~1再从1~0完美的符合了我们的需求 我们的需求是敌人攻击的玩家然后再回到原来的位置 和插值函数配和我们符合需求
            transform.position = Vector3.Lerp(originalPosition, attackPosition, t);
            yield return null;
        }
        material.color = Color.black;
        currentState = State.Chasing;
        agent.enabled = true;
    }
    IEnumerator UpdatePath(float refreshRate, Vector3 targePositon)
    {
        while (true)
        {
            if (currentState == State.Chasing)
            {
                if (Player.Instance.dead)
                {

                }
                else
                {
                    Vector3 dirToTarget = (target.position - transform.position).normalized;
                    targePositon = target.position - dirToTarget * (targetCollisonR + myCollisonR + 0.5f / 2);
                    if (!dead)
                    {
                        agent.SetDestination(targePositon);
                    }
                }
        }
 
        yield return new WaitForSeconds(refreshRate);
    }
    }
}
