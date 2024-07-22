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

    public void SaveData() // 데이터 저장
    {
        data.xSensi = GameManager.instance.xSensi;
        data.ySensi = GameManager.instance.ySensi;

        // 저장하려는 클래스를 제이슨을 통해 문자열로 변환
        string saveData = JsonUtility.ToJson(data, false);

        // 저장하려는 위치 경로  
        string path = Application.persistentDataPath + "/PlayerData.json";

        File.WriteAllText(path, saveData);

        print(saveData);
    }

    public void LoadData()
    {

        // 불러오려는 위치 경로  
        string path = Application.persistentDataPath + "/PlayerData.json";

        if (File.Exists(path))
        {
            // 파일에 저장되어있던 문자열 가져오기
            string loadData = File.ReadAllText(path);

            // 가져온 문자열을 제이슨을 통해 클래스로 변환
            data = JsonUtility.FromJson<PlayerData>(loadData);

        }

        GameManager.instance.xSensi = data.xSensi;
        GameManager.instance.ySensi = data.ySensi;
    }

}
