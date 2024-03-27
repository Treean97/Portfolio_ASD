using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItem : MonoBehaviour
{
    public Transform itemUI;
    public Transform buildUI;
    public Transform goldUI;

    public int[] item = { 0, 0, 0 };
    public int[] build = { 0, 0, 0 };

    void Start()
    {
        ChangeItemCount();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItem(int num)
    {
        switch (num)
        {
            case 0:
                item[0]++;
                ChangeItemCount();
                break;
            case 1:
                item[1]++;
                ChangeItemCount();
                break;
            case 2:
                item[2]++;
                ChangeItemCount();
                break;

        }

    }

    public void AddBuild(int num)
    {
        switch (num)
        {
            case 0:
                build[0]++;
                ChangeItemCount();
                break;
            case 1:
                build[1]++;
                ChangeItemCount();
                break;
            case 2:
                build[2]++;
                ChangeItemCount();
                break;
        }
    }

    public void ChangeItemCount()
    {
        for (int i = 0; i < item.Length; i++)
        {
            itemUI.GetChild(i).GetComponentInChildren<Text>().text = item[i].ToString();
            
        }

        for(int j = 0; j< build.Length; j++)
        {
            buildUI.GetChild(j).GetComponentInChildren<Text>().text = build[j].ToString();
        }
    }


}
