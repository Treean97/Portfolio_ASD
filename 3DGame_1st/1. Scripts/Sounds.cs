using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    public AudioClip[] audioClips;

    AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        audio = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //gameScene
    public void Game_ShootSound()
    {
        audio.clip = audioClips[0];
        audio.Play();
    }

    public void Game_ReloadSound()
    {
        audio.clip = audioClips[1];
        audio.Play();
    }
    
    public void Game_BuildSound()
    {
        audio.clip = audioClips[2];
        audio.Play();
    }

    public void Game_ButtonSound()
    {
        audio.clip = audioClips[3];
        audio.Play();
    }

    public void Game_BuySound()
    {
        audio.clip = audioClips[4];
        audio.Play();
    }

    // mainScene
    public void Main_OnMouseSound()
    {
        audio.clip = audioClips[0];
        audio.Play();
    }

    public void Main_ClickSound()
    {
        audio.clip = audioClips[1];
        audio.Play();
    }

    // gameinfoScene
    public void Info_LRClickSound()
    {
        audio.clip = audioClips[0];
        audio.Play();
    }

    public void Info_ReturnClickSound()
    {
        audio.clip = audioClips[1];
        audio.Play();
    }


    // settingScene
    public void Setting_ClickSound()
    {
        audio.clip = audioClips[0];
        audio.Play();
    }


}
