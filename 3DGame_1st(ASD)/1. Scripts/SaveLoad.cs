using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class PlayerData
{
    public float xSensi;
    public float ySensi;

}

public class SaveLoad : MonoBehaviour
{
    public PlayerData data;

    // Start is called before the first frame update
    void Start()
    {
        LoadData();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SaveData() // ������ ����
    {
        data.xSensi = GameManager.instance.xSensi;
        data.ySensi = GameManager.instance.ySensi;

        // �����Ϸ��� Ŭ������ ���̽��� ���� ���ڿ��� ��ȯ
        string saveData = JsonUtility.ToJson(data, false);

        // �����Ϸ��� ��ġ ���  
        string path = Application.persistentDataPath + "/PlayerData.json";

        File.WriteAllText(path, saveData);

        print(saveData);
    }

    public void LoadData()
    {

        // �ҷ������� ��ġ ���  
        string path = Application.persistentDataPath + "/PlayerData.json";

        if (File.Exists(path))
        {
            // ���Ͽ� ����Ǿ��ִ� ���ڿ� ��������
            string loadData = File.ReadAllText(path);

            // ������ ���ڿ��� ���̽��� ���� Ŭ������ ��ȯ
            data = JsonUtility.FromJson<PlayerData>(loadData);

        }

        GameManager.instance.xSensi = data.xSensi;
        GameManager.instance.ySensi = data.ySensi;
    }

}
