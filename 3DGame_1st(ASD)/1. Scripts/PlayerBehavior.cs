using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehavior : MonoBehaviour
{
    public PlayerItem pi;
    public Sounds sounds;
    public EnemySpawn es;
    public ItemEffect ie;
    public IngameScript ingameS;
    public Transform buildingSamples;
    public Transform selected;
    public Text currentBulletText;
    public Text carryBulletText;
    public Text noItem;
    public Text noBullet;
    public Slider hpBar;
    public Transform itemUI;
    public Transform BuildUI;
    public Transform GoldUI;
    public Shop shop;
    public Pause pause;
    public Object item1;
    public Object item2;

    public int playerHp;
    public int playerDamage;
    public int itemHeal;
    public int buildingNum;
    public int itemNum;
    public Vector3 buildingRot;
    public int currentBulletCount; // ���� źâ ���� �Ѿ�
    public int maxReloadBulletCount; // źâ �ִ� ���� ���� �Ѿ�
    public int carryBulletCount; // źâ �ܺο� ������ �Ѿ�
    public float fireDelay;
    public float maxDelay;
    public float damagedDelay;
    public float maxDamagedDelay = 1;
    bool canFire;
    bool canNoItemText = true;
    bool canNoBulletText = true;
    bool reloadEnd = true;
    Animator anim;
    Vector3 buildingPos;
    Vector3[] set;

    public enum PlayerMode
    {
        gun,
        build
    }

    PlayerMode playerMode;

    // Start is called before the first frame update
    void Start()
    {
        playerMode = PlayerMode.gun;

        GunModeOn();

        canFire = true;
        carryBulletText.text = carryBulletCount.ToString("00");
        currentBulletText.text = currentBulletCount.ToString("00 /");

        anim = GetComponent<Animator>();

        // Ŀ�� �Ⱥ���
        Cursor.visible = false;

        // Ŀ����ġ ���
        Cursor.lockState = CursorLockMode.Locked;

        // ������Ʈ ���� ������
        set = new Vector3[buildingSamples.childCount];

        // y�� ������
        set[0] = Vector3.up * 0;
        set[1] = Vector3.up * 0;
        set[2] = Vector3.up * 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        // �÷��̾� ü�� 0 ġƮ
        if (Input.GetKeyDown(KeyCode.L))
        {
            Damaged(playerHp);
        }

        // ������ ������
        if (damagedDelay < maxDamagedDelay)
        {
            damagedDelay += Time.deltaTime;
        }

        // ��� ����
        if (Input.GetKeyDown(KeyCode.Alpha1) && playerMode == PlayerMode.build)
        {
            GunModeOn();
            
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && playerMode == PlayerMode.gun)
        {
            BuildModeOn();
        }

        if (playerMode == PlayerMode.gun && shop.isOpen == false && pause.isOpen == false && ingameS.isOpen == false)
        {
            // �� ��忡�� ��Ŭ�� �Է� �� 
            if (Input.GetMouseButton(0))
            {
                // �Ѿ� ������ �߻�
                if (currentBulletCount > 0 && canFire == true)
                {
                    StartCoroutine(BulletCool());
                    Fire();
                    
                }

                // ������ ������
                else if (currentBulletCount == 0 && canNoBulletText)
                {
                    StartCoroutine(NoBullet("������ "));
                }

            }

            // �� ��忡�� RŰ �Է½�
            if (Input.GetKeyDown(KeyCode.R))
            {
                Reload();
            }

            // �� �Է�
            float scroll = Input.GetAxis("Mouse ScrollWheel");

            // �� ��
            if (scroll < 0)
            {
                int lastnum = itemNum;

                if (itemNum < 2)
                {
                    itemNum++;
                }
                else
                {
                    itemNum = 0;
                }

                ChangeItem(itemNum, lastnum);
            }

            // �� �ٿ�
            if (scroll > 0)
            {
                int lastnum = itemNum;

                if (itemNum > 0)
                {
                    itemNum--;
                }
                else
                {
                    itemNum = 2;
                }

                ChangeItem(itemNum, lastnum);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {

                // ������ ������ 0�� �ƴϸ� ��� ����
                if (int.Parse(itemUI.GetChild(itemNum).
                    GetComponentInChildren<Text>().text) != 0)
                {
                    UseItem(itemNum);

                }
                else if (canNoItemText)
                {
                    StartCoroutine(NoItem());

                }

            }

        }


        if (playerMode == PlayerMode.build && shop.isOpen == false && pause.isOpen == false && ingameS.isOpen == false)
        {

            RaycastHit hit;

            Ray buildRay = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));

            if (Physics.Raycast(buildRay, out hit))
            {
                if (hit.collider.tag == "Ground")
                {
                    buildingPos = hit.point + set[buildingNum];
                    buildingSamples.GetChild(buildingNum).transform.position = buildingPos;
                }

            }

            // ������Ʈ �� ȸ��
            if (Input.GetKey(KeyCode.Q))
            {
                buildingRot.y += 0.5f;
                buildingSamples.rotation = Quaternion.Euler(buildingRot);
            }
            else if (Input.GetKey(KeyCode.E))
            {
                buildingRot.y -= 0.5f;
                buildingSamples.rotation = Quaternion.Euler(buildingRot);
            }

            // �� �Է�
            float scroll = Input.GetAxis("Mouse ScrollWheel");

            // �� ��
            if (scroll < 0)
            {
                int lastnum = buildingNum;

                if (buildingNum < 2)
                {
                    buildingNum++;
                }
                else
                {
                    buildingNum = 0;
                }

                ChangeBuilding(buildingNum, lastnum);
            }

            // �� �ٿ�
            if (scroll > 0)
            {
                int lastnum = buildingNum;

                if (buildingNum > 0)
                {
                    buildingNum--;
                }
                else
                {
                    buildingNum = 2;
                }

                ChangeBuilding(buildingNum, lastnum);
            }


            // ���� ��ġ
            if (Input.GetMouseButtonDown(0))
            {
                UseBuild(buildingNum, buildingPos);
            }

        }

    }

    void Fire()
    {
        RaycastHit hit;
        Ray gunRay = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));

        if (Physics.Raycast(gunRay, out hit) && reloadEnd == true)
        {
            // �ִϸ��̼�
            anim.SetTrigger("shoot");
            // �Ҹ�
            sounds.Game_ShootSound();

            currentBulletCount--;
            // �ؽ�Ʈ�� ����
            currentBulletText.text = currentBulletCount.ToString() + " /";

            if(hit.collider.gameObject.tag == "Enemy")
            {
                EnemyScript es = hit.collider.gameObject.GetComponent<EnemyScript>();
                if (es != null)
                {
                    es.EnemyPlayerDamaged(playerDamage);
                }
                else
                {
                    FlyingEnemyScript fes = hit.collider.gameObject.GetComponent<FlyingEnemyScript>();
                    fes.EnemyPlayerDamaged(playerDamage);
                }

            }
        }
    }

    void Reload()
    {
        if (carryBulletCount != 0)
        {
            reloadEnd = false;

            // �ִϸ��̼�
            anim.SetTrigger("reload");
            //����
            sounds.Game_ReloadSound();

            // ���� �Ѿ��� źâ�� �� �� �ִ� �Ѿ˺��� ������
            if (carryBulletCount + currentBulletCount >= maxReloadBulletCount)
            {
                carryBulletCount -= (maxReloadBulletCount - currentBulletCount);
                currentBulletCount += (maxReloadBulletCount - currentBulletCount);
                // �ؽ�Ʈ�� ����
                currentBulletText.text = currentBulletCount.ToString() + " /";
                // �ؽ�Ʈ�� ����
                carryBulletText.text = carryBulletCount.ToString();
            }
            // ������
            else
            {
                currentBulletCount += carryBulletCount;
                carryBulletCount = 0;
                // �ؽ�Ʈ�� ����
                currentBulletText.text = currentBulletCount.ToString() + " /";
                // �ؽ�Ʈ�� ����
                carryBulletText.text = carryBulletCount.ToString();
            }
        }
        else
        {
            StartCoroutine(NoBullet(null));
        }

    }

    public void EndReload()
    {
        reloadEnd = true;
    }

    IEnumerator BulletCool()
    {
        canFire = false;
        float time = 0;

        while(time < maxDelay)
        {
            time += Time.deltaTime;

            yield return null;
        }

        canFire = true;
    }

    // ������ ��ȯ
    void BuildModeOn()
    {
        // ��� ��ȯ
        playerMode = PlayerMode.build;

        // ���õ� ��ġ ������Ʈ �� ������ �ʱ�ȭ
        buildingNum = 0;

        // UI ��ȯ
        itemUI.gameObject.SetActive(false);
        BuildUI.gameObject.SetActive(true);

        // ��ġ ������Ʈ On
        buildingSamples.GetChild(0).gameObject.SetActive(true);

        // ���� ���� �� �� ���� Off
        for (int i = 0; i < selected.childCount; i++)
        {
            selected.GetChild(i).gameObject.SetActive(false);
        }
        selected.GetChild(0).gameObject.SetActive(true);


    }

    // �ѱ��� ��ȯ
    void GunModeOn()
    {
        // ��� ��ȯ
        playerMode = PlayerMode.gun;

        // UI ��ȯ
        BuildUI.gameObject.SetActive(false);
        itemUI.gameObject.SetActive(true);

        // ���õ� ������ �� ������ �ʱ�ȭ
        itemNum = 0;

        // ��ġ ������Ʈ Off
        for (int i = 0; i < buildingSamples.childCount; i++)
        {
            buildingSamples.GetChild(i).gameObject.SetActive(false);
        }

        // ���� ���� �� �� ���� Off
        for (int i = 0; i < selected.childCount; i++)
        {
            selected.GetChild(i).gameObject.SetActive(false);
        }
        selected.GetChild(0).gameObject.SetActive(true);
    }

    // ��ġ ������Ʈ ���� (���� ������Ʈ on / off)
    // ���� ���� ���� (���� ������Ʈ on / off)
    void ChangeItem(int num, int lastnum)
    {
        // ������ ����
        // ���� ���� ����
        selected.GetChild(lastnum).gameObject.SetActive(false);
        selected.GetChild(num).gameObject.SetActive(true);

    }

    // ��ġ ������Ʈ ���� (���� ������Ʈ on / off)
    // ���� ���� ���� (���� ������Ʈ on / off)
    void ChangeBuilding(int num, int lastnum)
    {
        // ��ġ ������Ʈ ����
        buildingSamples.GetChild(lastnum).gameObject.SetActive(false);
        buildingSamples.GetChild(num).gameObject.SetActive(true);

        // ���� ���� ����
        selected.GetChild(lastnum).gameObject.SetActive(false);
        selected.GetChild(num).gameObject.SetActive(true);

    }

    // ������ ���
    void UseItem(int num)
    {
        sounds.Game_BuildSound();
       
        switch (num)
        {
            case 0:
                Instantiate(item1, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
                break;

            case 1:
                Instantiate(item2, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
                break;

            case 2:
                HealingItem();
                break;
        }

        itemUI.GetChild(num).GetComponentInChildren<Text>().text = 
            ((int.Parse(itemUI.GetChild(num).GetComponentInChildren<Text>().text)) - 1).ToString();

        pi.item[num]--;
    }

    public void HealingItem()
    {
        playerHp += itemHeal;
        if(playerHp > 100)
        {
            playerHp = 100;
        }
        
        hpBar.value = playerHp;
    }


    // ��ġ ������Ʈ ��ġ
    void UseBuild(int num, Vector3 pos)
    {
        // ������ ������ ������ ��ġ ����
        if (int.Parse(BuildUI.GetChild(num).GetComponentInChildren<Text>().text) != 0)
        {
            // ����
            sounds.Game_BuildSound();

            Object copyTower = Instantiate(Resources.Load(("BuildingSample_") + (num + 1)), pos, buildingSamples.rotation);
            GameObject tower = (GameObject)copyTower;

            BuildUI.GetChild(num).GetComponentInChildren<Text>().text = 
                (int.Parse(BuildUI.GetChild(num).GetComponentInChildren<Text>().text) - 1).ToString();

            pi.build[num]--;

            if (buildingNum == 0) 
            {
                es.AddTower1(tower);
            }
        }
        else if(canNoItemText)
        {
            
            StartCoroutine(NoItem());
        }
    }

    // �ǰ� ������
    public void Damaged(int damage)
    {
        if (damagedDelay >= maxDamagedDelay)
        {
            playerHp -= damage;
            hpBar.value = playerHp;
            damagedDelay = 0;
            if (playerHp <= 0)
            {
                // ���� ����
                ingameS.Ending("����");

            }
        }
    }


    public void DamagedByEnemy(int damage)
    {
        playerHp -= damage;
        hpBar.value = playerHp;
        if (playerHp <= 0)
        {
            // ���� ����
            ingameS.Ending("����");

        }
    }


    // ������ ���� �˾�
    IEnumerator NoItem()
    {
        canNoItemText = false;
        noItem.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        noItem.gameObject.SetActive(false);
        canNoItemText = true;
    }

    // �Ѿ� ���� �˾�
    IEnumerator NoBullet(string text)
    {
        canNoBulletText = false;
        // ���� ����
        noBullet.text = text + noBullet.text;
        noBullet.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        noBullet.gameObject.SetActive(false);
        // ���� �ʱ�ȭ
        noBullet.text = "�Ѿ� ����!";
        canNoBulletText = true;
    }


    // �ǰ� ����
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Item1Effect")
        {
            Damaged(ie.item1Damage);
        }
        
    }



}
