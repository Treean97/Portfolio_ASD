using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawn : MonoBehaviour
{
    public List<GameObject> enemyList = new List<GameObject>();
    public List<GameObject> tower1List = new List<GameObject>();
    public IngameScript igs;
    public GameObject selectedEnemy;
    public Object enemy1;
    public Object enemy2;
    public Object enemy3;
    public Object flyingEnemy1;
    public Text waveText;
    public int unitCount;
    public int killedUnit;
    public int wave1MaxUnitCount;
    public float wave1UnitDelay;
    public int wave2MaxUnitCount;
    public float wave2UnitDelay;
    public int wave3MaxUnitCount;
    public float wave3UnitDelay;
    public int wave4MaxUnitCount;
    public float wave4UnitDelay;

    float shortDis;
    bool waveStart;
    bool isWaveOver = false;

    // Start is called before the first frame update
    void Start()
    {
        enemyList.Clear();
        unitCount = 0;
        waveStart = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (waveText.text == "Wave 1" && waveStart == false)
        {
            StartCoroutine(Wave1());
        }
        else if (waveText.text == "Wave 2" && waveStart == false)
        {
            StartCoroutine(Wave2());
        }
        else if (waveText.text == "Wave 3" && waveStart == false)
        {
            StartCoroutine(Wave3());
        }
        else if (waveText.text == "Wave 4" && waveStart == false)
        {
            StartCoroutine(Wave4());
        }

        // 다 죽이면 기다리지 않고 준비 시간으로
        switch (waveText.text) 
        {
            case "Wave 1":
                if (killedUnit >= wave1MaxUnitCount)
                {
                    isWaveOver = true;
                }
                break;

            case "Wave 2":
                if (killedUnit >= wave2MaxUnitCount)
                {
                    isWaveOver = true;
                }
                break;

            case "Wave 3":
                if (killedUnit >= wave3MaxUnitCount)
                {
                    isWaveOver = true;
                }
                break;

            case "Wave 4":
                if (killedUnit >= wave4MaxUnitCount)
                {
                    isWaveOver = true;
                }
                break;
        }

        if (isWaveOver)
        {
            if (waveText.text != "Ready")
            {
                // 남은 시간 스킵
                igs.time -= 300;
            }
            // 죽인 유닛 초기화
            killedUnit = 0;
            isWaveOver = false;
        }

        if(waveText.text == "Ready")
        {
            waveStart = false;
            unitCount = 0;
        }


        
        if (tower1List.Count > 0 && enemyList.Count > 0)
        {
            
            for (int i = 0; i < tower1List.Count; i++)
            {
                shortDis = Vector3.Distance(enemyList[0].transform.position, tower1List[i].transform.position);

                selectedEnemy = enemyList[0].gameObject;

                foreach (GameObject found in enemyList)
                {
                    float Distance = Vector3.Distance(tower1List[i].transform.position, found.transform.position);

                    if (Distance < shortDis)
                    {
                        selectedEnemy = found;
                        shortDis = Distance;
                    }
                }

                TowerScript ts = tower1List[i].gameObject.GetComponent<TowerScript>();
                ts.TakeTarget(selectedEnemy);
            }
            
        }

        
    }


    IEnumerator Wave1()
    {

        while (unitCount < wave1MaxUnitCount)
        {
            waveStart = true;
            Instantiate(enemy1, transform.GetChild(Random.Range(0, 7)).transform.position, Quaternion.identity);
            unitCount++;
            yield return new WaitForSeconds(wave1UnitDelay);
        }

        // 소환 다되면 
        waveStart = false;
    }

    IEnumerator Wave2()
    {
        while (unitCount < wave2MaxUnitCount)
        {
            int random = Random.Range(1, 11);
            waveStart = true;
            if (random <= 8)
            {
                Instantiate(enemy1, transform.GetChild(Random.Range(0, 7)).transform.position, Quaternion.identity);
            }
            else if(random <= 9)
            {
                Instantiate(enemy2, transform.GetChild(Random.Range(0, 7)).transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(enemy3, transform.GetChild(Random.Range(0, 7)).transform.position, Quaternion.identity);
            }
            unitCount++;
            yield return new WaitForSeconds(wave2UnitDelay);
        }

        // 소환 다되면 
        waveStart = false;
    }

    IEnumerator Wave3()
    {
        while (unitCount < wave3MaxUnitCount)
        {
            int random = Random.Range(1, 11);
            waveStart = true;
            if (random <= 5)
            {
                Instantiate(enemy1, transform.GetChild(Random.Range(0, 7)).transform.position, Quaternion.identity);
            }
            else if (random <= 7)
            {
                Instantiate(enemy2, transform.GetChild(Random.Range(0, 7)).transform.position, Quaternion.identity);
            }
            else if (random <= 9)
            {
                Instantiate(enemy3, transform.GetChild(Random.Range(0, 7)).transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(flyingEnemy1, transform.GetChild(Random.Range(0, 7)).transform.position, Quaternion.identity);
            }
            unitCount++;
            yield return new WaitForSeconds(wave3UnitDelay);
        }

        // 소환 다되면 
        waveStart = false;
    }

    IEnumerator Wave4()
    {
        while (unitCount < wave4MaxUnitCount)
        {
            int random = Random.Range(1, 11);
            waveStart = true;
            if (random <= 8)
            {
                Instantiate(enemy1, transform.GetChild(Random.Range(0, 7)).transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(enemy2, transform.GetChild(Random.Range(0, 7)).transform.position, Quaternion.identity);
            }
            unitCount++;
            yield return new WaitForSeconds(wave4UnitDelay);
        }

        // 소환 다되면 
        waveStart = false;
    }


    public void AddEnemy(GameObject go)
    {
        enemyList.Add(go);
    }

    public void DelEnemy(GameObject go)
    {
        enemyList.Remove(go);
    }

    public void AddTower1(GameObject tower1)
    {
        tower1List.Add(tower1);
    }

}
