using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyScript : MonoBehaviour
{
    GameObject player;
    PlayerBehavior pb;
    Animator anim;
    Rigidbody rig;
    IngameScript ig;
    EnemySpawn eSpawn;
    Shop shop;

    public float moveSpeed;
    public int damage;
    public int enemyHp;
    public float attackDelay;
    public float maxAttackDelay;
    bool isAttack;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        pb = player.GetComponent<PlayerBehavior>();
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();
        ig = GameObject.FindGameObjectWithTag("GameScene").GetComponent<IngameScript>();
        eSpawn = GameObject.FindGameObjectWithTag("EnemySpawners").GetComponent<EnemySpawn>();
        shop = GameObject.FindGameObjectWithTag("GameScene").GetComponent<Shop>();
    }

    // Update is called once per frame
    void Update()
    {
        float dis = (transform.position - player.transform.position).magnitude;
        Vector3 dir = (player.transform.position - transform.position).normalized;
        transform.forward = dir;

        anim.SetFloat("speed", rig.velocity.magnitude);
        transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z);

        // 이동
        if (dis > 15)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);    
        }

        // 공격
        if (dis <= 15 && attackDelay >= maxAttackDelay && !isAttack && enemyHp > 0)
        {
            Attack();
        }

        if (attackDelay <= maxAttackDelay)
        {   
            attackDelay += Time.deltaTime;
        }

        
    }

    // 피격
    public void EnemyPlayerDamaged(int damaged)
    {
        enemyHp -= damaged;
        if(enemyHp <= 0)
        {
            Dead();
        }
    }

    void Attack()
    {
        isAttack = true;
        anim.SetTrigger("attack");
        pb.Damaged(damage);
    }

    public void Attack_End()
    {
        print("attackEnd");
        isAttack = false;
    }

    public void Dead()
    {
        // 정지
        rig.velocity = new Vector3(0, 0, 0);

        // 공격 불가
        gameObject.GetComponent<SphereCollider>().enabled = false;

        // 킬카운트 증가
        ig.KillCount();
        eSpawn.killedUnit++;

        // 애니메이션
        anim.SetTrigger("dead");

        // 플레이어 재화 획득
        if (gameObject.name == "Enemy1(Clone)")
        {
            // 50%확률
            if (Random.Range(0, 2) % 2 == 0)
            {
                shop.AddGold(10);
            }
        }

        eSpawn.DelEnemy(gameObject);

        Destroy(gameObject, 3);
    }

}
