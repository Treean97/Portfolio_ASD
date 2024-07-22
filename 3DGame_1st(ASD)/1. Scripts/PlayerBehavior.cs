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
    public int currentBulletCount; // 현재 탄창 내부 총알
    public int maxReloadBulletCount; // 탄창 최대 장전 가능 총알
    public int carryBulletCount; // 탄창 외부에 보유한 총알
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

        // 커서 안보임
        Cursor.visible = false;

        // 커서위치 잠금
        Cursor.lockState = CursorLockMode.Locked;

        // 오브젝트 높이 조정용
        set = new Vector3[buildingSamples.childCount];

        // y축 조정값
        set[0] = Vector3.up * 0;
        set[1] = Vector3.up * 0;
        set[2] = Vector3.up * 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        // 플레이어 체력 0 치트
        if (Input.GetKeyDown(KeyCode.L))
        {
            Damaged(playerHp);
        }

        // 데미지 딜레이
        if (damagedDelay < maxDamagedDelay)
        {
            damagedDelay += Time.deltaTime;
        }

        // 모드 변경
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
            // 총 모드에서 좌클릭 입력 시 
            if (Input.GetMouseButton(0))
            {
                // 총알 있으면 발사
                if (currentBulletCount > 0 && canFire == true)
                {
                    StartCoroutine(BulletCool());
                    Fire();
                    
                }

                // 없으면 재장전
                else if (currentBulletCount == 0 && canNoBulletText)
                {
                    StartCoroutine(NoBullet("장전된 "));
                }

            }

            // 총 모드에서 R키 입력시
            if (Input.GetKeyDown(KeyCode.R))
            {
                Reload();
            }

            // 휠 입력
            float scroll = Input.GetAxis("Mouse ScrollWheel");

            // 휠 업
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

            // 휠 다운
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

                // 아이템 개수가 0이 아니면 사용 가능
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

            // 오브젝트 각 회전
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

            // 휠 입력
            float scroll = Input.GetAxis("Mouse ScrollWheel");

            // 휠 업
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

            // 휠 다운
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


            // 물건 설치
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
            // 애니메이션
            anim.SetTrigger("shoot");
            // 소리
            sounds.Game_ShootSound();

            currentBulletCount--;
            // 텍스트에 갱신
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

            // 애니메이션
            anim.SetTrigger("reload");
            //사운드
            sounds.Game_ReloadSound();

            // 보유 총알이 탄창에 들어갈 수 있는 총알보다 많으면
            if (carryBulletCount + currentBulletCount >= maxReloadBulletCount)
            {
                carryBulletCount -= (maxReloadBulletCount - currentBulletCount);
                currentBulletCount += (maxReloadBulletCount - currentBulletCount);
                // 텍스트에 갱신
                currentBulletText.text = currentBulletCount.ToString() + " /";
                // 텍스트에 갱신
                carryBulletText.text = carryBulletCount.ToString();
            }
            // 적으면
            else
            {
                currentBulletCount += carryBulletCount;
                carryBulletCount = 0;
                // 텍스트에 갱신
                currentBulletText.text = currentBulletCount.ToString() + " /";
                // 텍스트에 갱신
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

    // 빌드모드 전환
    void BuildModeOn()
    {
        // 모드 전환
        playerMode = PlayerMode.build;

        // 선택된 설치 오브젝트 맨 앞으로 초기화
        buildingNum = 0;

        // UI 전환
        itemUI.gameObject.SetActive(false);
        BuildUI.gameObject.SetActive(true);

        // 설치 오브젝트 On
        buildingSamples.GetChild(0).gameObject.SetActive(true);

        // 선택 상자 맨 앞 제외 Off
        for (int i = 0; i < selected.childCount; i++)
        {
            selected.GetChild(i).gameObject.SetActive(false);
        }
        selected.GetChild(0).gameObject.SetActive(true);


    }

    // 총기모드 전환
    void GunModeOn()
    {
        // 모드 전환
        playerMode = PlayerMode.gun;

        // UI 전환
        BuildUI.gameObject.SetActive(false);
        itemUI.gameObject.SetActive(true);

        // 선택된 아이템 맨 앞으로 초기화
        itemNum = 0;

        // 설치 오브젝트 Off
        for (int i = 0; i < buildingSamples.childCount; i++)
        {
            buildingSamples.GetChild(i).gameObject.SetActive(false);
        }

        // 선택 상자 맨 앞 제외 Off
        for (int i = 0; i < selected.childCount; i++)
        {
            selected.GetChild(i).gameObject.SetActive(false);
        }
        selected.GetChild(0).gameObject.SetActive(true);
    }

    // 설치 오브젝트 변경 (게임 오브젝트 on / off)
    // 선택 상자 변경 (게임 오브젝트 on / off)
    void ChangeItem(int num, int lastnum)
    {
        // 아이템 변경
        // 선택 상자 변경
        selected.GetChild(lastnum).gameObject.SetActive(false);
        selected.GetChild(num).gameObject.SetActive(true);

    }

    // 설치 오브젝트 변경 (게임 오브젝트 on / off)
    // 선택 상자 변경 (게임 오브젝트 on / off)
    void ChangeBuilding(int num, int lastnum)
    {
        // 설치 오브젝트 변경
        buildingSamples.GetChild(lastnum).gameObject.SetActive(false);
        buildingSamples.GetChild(num).gameObject.SetActive(true);

        // 선택 상자 변경
        selected.GetChild(lastnum).gameObject.SetActive(false);
        selected.GetChild(num).gameObject.SetActive(true);

    }

    // 아이템 사용
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


    // 설치 오브젝트 설치
    void UseBuild(int num, Vector3 pos)
    {
        // 아이템 개수가 있으면 설치 가능
        if (int.Parse(BuildUI.GetChild(num).GetComponentInChildren<Text>().text) != 0)
        {
            // 사운드
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

    // 피격 데미지
    public void Damaged(int damage)
    {
        if (damagedDelay >= maxDamagedDelay)
        {
            playerHp -= damage;
            hpBar.value = playerHp;
            damagedDelay = 0;
            if (playerHp <= 0)
            {
                // 게임 종료
                ingameS.Ending("실패");

            }
        }
    }


    public void DamagedByEnemy(int damage)
    {
        playerHp -= damage;
        hpBar.value = playerHp;
        if (playerHp <= 0)
        {
            // 게임 종료
            ingameS.Ending("실패");

        }
    }


    // 아이템 없음 팝업
    IEnumerator NoItem()
    {
        canNoItemText = false;
        noItem.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        noItem.gameObject.SetActive(false);
        canNoItemText = true;
    }

    // 총알 없음 팝업
    IEnumerator NoBullet(string text)
    {
        canNoBulletText = false;
        // 문구 변경
        noBullet.text = text + noBullet.text;
        noBullet.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        noBullet.gameObject.SetActive(false);
        // 문구 초기화
        noBullet.text = "총알 없음!";
        canNoBulletText = true;
    }


    // 피격 판정
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Item1Effect")
        {
            Damaged(ie.item1Damage);
        }
        
    }



}
