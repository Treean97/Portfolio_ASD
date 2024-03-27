using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public PlayerItem pi;
    public PlayerBehavior pb;
    public GameObject shopView;
    public Text currentgold;
    public Text noGold;
    public Text[] itemInfo;
    public Text[] buildInfo;
    public Text bulletInfo;
    public Text bulletReinInfo;
    public Text time;
    public Text wave;
    public bool isOpen;
    public bool canNoGoldText = true;
    // -------아이템 가격-----
    public int[] itemPrice = { 10, 20, 30 };
    public int[] buildPrice = { 100, 150, 200 };
    public int bulletPrice = 10;
    public int bulletReinPrice = 100;
    // ----------------------

    // Start is called before the first frame update
    void Start()
    {
        shopView.gameObject.SetActive(false);
        noGold.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // 치트 
        if (Input.GetKeyDown(KeyCode.P))
        {
            currentgold.text = 99999.ToString();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && isOpen == false)
        {
            isOpen = true;
            shopView.gameObject.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;

            time.gameObject.SetActive(false);
            wave.gameObject.SetActive(false);

            // 현재 보유중인 개수 표시
            for (int i = 0; i < itemInfo.Length; i++)
            {
                ShowItemCount(i);
            }
            for (int i = 0; i < itemInfo.Length; i++)
            {
                ShowBuildCount(i);
            }
            ShowBulletCount();
            // -------------------------
        }

        if (Input.GetKeyDown(KeyCode.F1) && isOpen == true)
        {
            isOpen = false;
            shopView.gameObject.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            time.gameObject.SetActive(true);
            wave.gameObject.SetActive(true);
            Time.timeScale = 1;

        }

    }

    public void BuyItemBtn(int itemNum)
    {
        if(int.Parse(currentgold.text) >= itemPrice[itemNum])
        {
            pi.AddItem(itemNum);
            currentgold.text = (int.Parse(currentgold.text) - itemPrice[itemNum]).ToString();
            // 보유 수량 최신화
            ShowItemCount(itemNum);
        }
        else if (canNoGoldText)
        {
            StartCoroutine(NoGold());
        }

    }

    public void BuyBuildBtn(int buildNum)
    {
        if (int.Parse(currentgold.text) >= buildPrice[buildNum])
        {
            pi.AddBuild(buildNum);
            currentgold.text = (int.Parse(currentgold.text) - buildPrice[buildNum]).ToString();
            // 보유 수량 최신화
            ShowBuildCount(buildNum);
        }
        else if (canNoGoldText)
        {
            StartCoroutine(NoGold());
        }

    }

    public void BuyBulletBtn()
    {
        if(int.Parse(currentgold.text) >= bulletPrice)
        {
            pb.carryBulletCount += 20;
            pb.carryBulletText.text = pb.carryBulletCount.ToString();
            currentgold.text = (int.Parse(currentgold.text) - bulletPrice).ToString();
            // 보유 수량 최신화
            ShowBulletCount();
        }
        else if (canNoGoldText)
        {
            StartCoroutine(NoGold());
        }

    }

    public void BuyBulletReinBtn()
    {
        if (int.Parse(currentgold.text) >= bulletReinPrice)
        {
            currentgold.text = (int.Parse(currentgold.text) - bulletReinPrice).ToString();
            pb.playerDamage += 5;
            bulletReinInfo.text = "총알 데미지 강화 \n현재 공격력 : " + pb.playerDamage.ToString("00");
        }
        else if (canNoGoldText)
        {
            StartCoroutine(NoGold());
        }

    }

    

    public void ShowItemCount(int num)
    {
        string[] str = itemInfo[num].text.Split(':');
        // 보유 수량으로 변경
        str[1] = pi.itemUI.GetChild(num).GetComponentInChildren<Text>().text;
        itemInfo[num].text = str[0] + ": " + str[1];

    }

    public void ShowBuildCount(int num)
    {
        string[] str = buildInfo[num].text.Split(':');
        // 보유 수량으로 변경
        str[1] = pi.buildUI.GetChild(num).GetComponentInChildren<Text>().text;
        buildInfo[num].text = str[0] + ": " + str[1];
    }

    public void ShowBulletCount()
    {
        string[] str = bulletInfo.text.Split(':');
        // 보유 수량으로 변경
        str[1] = (pb.currentBulletCount + pb.carryBulletCount).ToString();
        bulletInfo.text = str[0] + ": " + str[1];

    }

    public void AddGold(int gold)
    {
        currentgold.text = (int.Parse(currentgold.text) + gold).ToString();
    }

    IEnumerator NoGold()
    {
        canNoGoldText = false;
        noGold.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1);
        noGold.gameObject.SetActive(false);
        canNoGoldText = true;
    }

}

