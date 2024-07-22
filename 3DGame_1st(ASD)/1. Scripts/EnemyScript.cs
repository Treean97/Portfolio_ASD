using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject player; 
    Animator anim;
    Rigidbody rig;
    GameObject gs;
    public TowerScript ts;
    public EnemySpawn eSpawn;
    public ItemEffect ie;
    public Shop shop;
    public IngameScript ig;
    public int enemyHp;
    public int enemyDamage;
    public float damagedDelay;
    public float maxDamagedDelay;
    bool canAttack = true;
    bool isDead = false;
    bool isItem2Slow = false;
    bool isTower2Slow = false;
    float baseSpeed;
    // Start is called before the first frame update
    void Start()
    {
        eSpawn = GameObject.FindGameObjectWithTag("EnemySpawners").GetComponent<EnemySpawn>();
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();
        eSpawn.AddEnemy(gameObject);

        player = GameObject.FindWithTag("Player");
        gs = GameObject.FindWithTag("GameScene");
        ts = gs.GetComponent<TowerScript>();
        ig = gs.GetComponent<IngameScript>();
        ie = gs.GetComponent<ItemEffect>();
        shop = gs.GetComponent<Shop>();
        agent = GetComponent<NavMeshAgent>();
        damagedDelay = maxDamagedDelay;

        baseSpeed = agent.speed;
       
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("speed", rig.velocity.magnitude);

        if (damagedDelay <= maxDamagedDelay)
        {
            damagedDelay += Time.deltaTime;
        }

        // �÷��̾� ����
        agent.destination = player.transform.position;

        if (enemyHp <= 0 && isDead == false)
        {
            isDead = true;
            Dead();
        }

        // ����
        switch (gameObject.name)
        {
            case "Enemy1(Clone)":
                if ((player.transform.position - transform.position).magnitude <= 2.5f && canAttack)
                {
                    Attack();
                }
                break;

            case "Enemy2(Clone)":
                if ((player.transform.position - transform.position).magnitude <= 3 && canAttack)
                {
                    Attack();
                }
                break;

            case "Enemy3(Clone)":
                if ((player.transform.position - transform.position).magnitude <= 2 && canAttack)
                {
                    Attack();
                }
                break;
        }





        if(isItem2Slow || isTower2Slow)
        {
            Slow();
        }

    }

    public void EnemyPlayerDamaged(int damaged)
    {
        enemyHp -= damaged;
    }

    // Ÿ�� 1 �ǰ�
    public void EnemyTower1Hit(int damaged)
    {
        enemyHp -= damaged;
    }

    // Ÿ�� 2 �ǰ�
    public void EnemyTower2Hit()
    {
        isTower2Slow = true;
    }

    public void EnemyTower2Exit()
    {
        isTower2Slow = false;
    }

    // ��ȭȿ��
    public void Slow()
    {
        print("slow");

        int item2Slow = ie.item2Slow;
        int tower2Slow = ts.tower2Slow;

        if (!isItem2Slow)
        {
            item2Slow = 100;
        }
        else if (!isTower2Slow)
        {
            tower2Slow = 100;
        }

        agent.speed = baseSpeed * ((item2Slow / 100f) * (tower2Slow / 100f));
    }


    // ������ 1 �ǰ�
    public void EnemyItem1Hit(int damaged)
    {
        
        if (damagedDelay >= maxDamagedDelay)
        {
            enemyHp -= damaged;
            damagedDelay = 0;
        }

    }

    // ������ 2 �ǰ�
    public void EnemyItem2Hit()
    {
        isItem2Slow = true;
    }

    public void EnemyItem2Exit()
    {
        isItem2Slow = false;
    }

    public void Dead()
    {
        // ����
        agent.isStopped = true;

        // ųī��Ʈ ����
        ig.KillCount();
        eSpawn.killedUnit++;

        // ���� �Ұ�
        gameObject.GetComponent<BoxCollider>().enabled = false;

        // �ִϸ��̼�
        anim.SetTrigger("dead");

        // �÷��̾� ��ȭ ȹ��
        if (gameObject.name == "Enemy1(Clone)")
        {
            // 50%Ȯ��
            if (Random.Range(0, 2) % 2 == 0)
            {
                shop.AddGold(5);
            }
        }
        else if (gameObject.name == "Enemy2(Clone)")
        {
            // 50%Ȯ��
            if (Random.Range(0, 2) % 2 == 0)
            {
                shop.AddGold(10);
            }
        }
        else if (gameObject.name == "Enemy3(Clone)")
        {
            // 50%Ȯ��
            if (Random.Range(0, 2) % 2 == 0)
            {
                shop.AddGold(10);
            }
        }
        else if (gameObject.name == "FlyingEnemy(Clone)")
        {
            // 50%Ȯ��
            if (Random.Range(0, 2) % 2 == 0)
            {
                shop.AddGold(20);
            }
        }


        eSpawn.DelEnemy(gameObject);
        
        Destroy(gameObject,2);
    }

    public void Attack()
    {
        canAttack = false;
        anim.SetTrigger("attack");
        agent.isStopped = true;
        
    }

    // �ִϸ��̼� �̺�Ʈ
    public void EndAttack()
    {
        canAttack = true;
        agent.isStopped = false;
    }

    // �ִϸ��̼� �̺�Ʈ
    public void RealHit()
    {
        PlayerBehavior pb = player.GetComponent<PlayerBehavior>();
        pb.DamagedByEnemy(enemyDamage);
    }

}
