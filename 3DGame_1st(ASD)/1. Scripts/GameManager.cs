using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public SaveLoad saveLoad;
    public AudioSource audio;
    public float xSensi;
    public float ySensi;
    public AudioClip[] bgm;


    private void Awake()
    {
        // �ν��Ͻ��� �������� �ʴ´ٸ� ���� �Ҵ�
        if (instance == null)
        {
            instance = this;
        }
        // �ν��Ͻ��� �̹� �����ϰ� ���� �� �ν��Ͻ��� �ƴ϶��
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        saveLoad.LoadData();
        audio = GetComponent<AudioSource>();
        MenuBGM();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MenuBGM()
    {
        audio.clip = bgm[0];
        audio.Play();
    }

    public void InGameBGM()
    {
        audio.clip = bgm[1];
        audio.Play();
    }

}
