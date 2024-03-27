using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerScript : MonoBehaviour
{
    GameObject findEnemySpawner;
    GameObject tower1Target;
    EnemySpawn eSpawn;
    AudioSource audio;

    public float tower1Delay;
    public float maxTower1Delay;
    public int tower1Damage;
    public int tower2Slow;
    public float tower2SoundDelay;
    public List<GameObject> enemyList_InRange = new List<GameObject>();

    Vector3 dir;
    
    // Start is called before the first frame update
    void Start()
    {
        tower1Delay = maxTower1Delay;
        findEnemySpawner = GameObject.FindGameObjectWithTag("EnemySpawners");
        eSpawn = findEnemySpawner.GetComponent<EnemySpawn>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameObject.tag)
        {
            case "Tower1":
                Tower1(tower1Target);
                tower1Delay += Time.deltaTime;
                break;

            case "Tower2":
                Tower2();
                foreach(GameObject go in enemyList_InRange)
                {
                    go.GetComponent<EnemyScript>().EnemyTower2Hit();
                }
                tower2SoundDelay += Time.deltaTime;
                break;

            default:

                break;
        }


        
    }


    void Tower1(GameObject target)
    {

        if (eSpawn.enemyList.Count > 0)
        {

            if (tower1Delay >= maxTower1Delay)
            {
                EnemyScript target_es = target.GetComponent<EnemyScript>();
                target_es.EnemyItem1Hit(tower1Damage);
                audio.Play();
                tower1Delay = 0;
            }

            dir = (target.transform.position - gameObject.transform.position).normalized;
            dir.y = 0;

            transform.forward = dir;

        }

    }

    public void TakeTarget(GameObject target)
    {
        tower1Target = target;
    }

    public void Tower2()
    {
        if(eSpawn.enemyList.Count > 0)
        {
            foreach(GameObject go in eSpawn.enemyList)
            {
                float distance = (go.transform.position - gameObject.transform.position).magnitude;
                if (distance <= 10 && !enemyList_InRange.Contains(go))
                {
                    enemyList_InRange.Add(go);
                }
                else if(enemyList_InRange.Contains(go) && distance > 10)
                {
                    enemyList_InRange.Remove(go);
                }
                
            }

            if(tower2SoundDelay >= 3)
            {
                audio.Play();
                tower2SoundDelay = 0;
            }
        }
    }
}
