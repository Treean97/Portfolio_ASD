using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffect : MonoBehaviour
{
    public Object item1;
    public Object item2;
    public Object item1Effect;
    public Object item2Effect;
    public List<GameObject> enemyList = new List<GameObject> ();

    public float throwPower = 15;
    public int item1Damage = 10;
    public int item2Slow = 50;
    Rigidbody rig;

    Vector3 dir;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody>();

        if (gameObject.tag == "Item")
        {
            // 카메라 방향
            dir = Camera.main.transform.forward;
            // 투사체 방향 전환
            transform.forward = dir;

        }
        switch (gameObject.tag)
        {
            case "Item1Effect" :
                Destroy(gameObject, 5);
                break;

            case "Item2Effect":
                Destroy(gameObject, 10);
                break;
        }

        switch (gameObject.name)
        {
            case "Item1(Clone)":
                Item1();
                break;

            case "Item2(Clone)":
                Item2();
                break;
        }


    }

    // Update is called once per frame
    void Update()
    {
        if(enemyList.Count > 0)
        {
            switch (gameObject.tag)
            {
                case "Item1Effect":
                    foreach (GameObject item in enemyList)
                    {
                        if (item != null)
                        {
                            item.SendMessage("EnemyItem1Hit", item1Damage);
                        }
                    }

                    break;

                case "Item2Effect":
                    foreach (GameObject item in enemyList)
                    {
                        if (item != null) { 
                            item.SendMessage("EnemyItem2Hit");
                        }
                    }

                    break;

            }
            
        }
    }

    void Item1()
    {
        rig.velocity = dir.normalized * throwPower;
        Destroy(gameObject, 10);
    }

    void Item2()
    {
        rig.velocity = dir.normalized * throwPower;
        Destroy(gameObject, 10);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 효과 발동
        if (collision.gameObject.tag == "Enemy")
        {
            enemyList.Add(collision.gameObject);
        }

        // 아이템 발동
        if (collision.gameObject.tag == "Ground")
        {
            switch (gameObject.name)
            {
                case "Item1(Clone)":
                    // 파괴
                    Destroy(gameObject);
                    // 효과 발동
                    Instantiate(Resources.Load("item1Effect"),
                        new Vector3(transform.position.x, collision.transform.position.y, transform.position.z) + (Vector3.up * 0.01f)
                        , Quaternion.identity);
                    
                    break;

                case "Item2(Clone)":
                    // 파괴
                    Destroy(gameObject);
                    // 효과 발동
                    Instantiate(Resources.Load("item2Effect"),
                        new Vector3(transform.position.x, collision.transform.position.y, transform.position.z) + (Vector3.up * 0.02f)
                        , Quaternion.identity);

                    break;
            }

        }



    }

    public void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            enemyList.Remove(collision.gameObject);
        }

        if(gameObject.tag == "Item2Effect")
        {
            collision.gameObject.SendMessage("EnemyItem2Exit");
        }

    }

}
