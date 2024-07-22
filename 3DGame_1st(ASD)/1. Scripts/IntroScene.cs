using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroScene : MonoBehaviour
{
    public SaveLoad saveLoad;
    public Image panel;
    public AudioSource audio;
    public Text introText;

    int cnt;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BlinkText());
        saveLoad.LoadData();
        cnt = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown)
        {
            
            if (cnt == 0)
            {
                StartCoroutine(FadeOut("1.MainScene"));
                audio.Play();
                cnt++;
            }
        }
    }

    IEnumerator FadeOut(string Scene)
    {
        panel.transform.SetAsLastSibling();

        float color = 0;
        while (color < 1)
        {
            color += 0.03f;
            yield return new WaitForSeconds(0.01f);
            panel.color = new Color(0, 0, 0, color);
        }

        if (Scene != null)
        {
            SceneChange(Scene);
        }


    }


    void SceneChange(string Scene)
    {
        SceneManager.LoadScene(Scene);
    }

    IEnumerator BlinkText()
    {
        float color = 1;

        for(int i = 0; i < 100; i++)
        {
            color -= 0.01f;
            yield return new WaitForSeconds(0.01f);
            introText.color = new Color(0, 0, 0, color);
        }

        for (int i = 0; i < 100; i++)
        {
            color += 0.01f;
            yield return new WaitForSeconds(0.01f);
            introText.color = new Color(0, 0, 0, color);
        }

        StartCoroutine(BlinkText());
    }

}
