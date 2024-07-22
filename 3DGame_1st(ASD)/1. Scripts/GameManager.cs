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
        // 인스턴스가 존재하지 않는다면 나를 할당
        if (instance == null)
        {
            instance = this;
        }
        // 인스턴스가 이미 존재하고 내가 그 인스턴스가 아니라면
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
