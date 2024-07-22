using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainScene : MonoBehaviour
{
    public Image panel;
    public Sounds sounds;
    public Image selectMapView;

    // Start is called before the first frame update
    void Start()
    {
        panel.gameObject.SetActive(true);
        selectMapView.gameObject.SetActive(false);

        StartCoroutine(FadeIn(null));

        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartBtn()
    {
        selectMapView.gameObject.SetActive(true);
        sounds.Main_ClickSound();
    }

    public void SelectMap1Btn()
    {
        StartCoroutine(FadeOut("2.GameScene"));
        sounds.Main_ClickSound();
    }

    public void SelectMap2Btn()
    {
        StartCoroutine(FadeOut("2.GameScene2"));
        sounds.Main_ClickSound();
    }
    public void SelectMap3Btn()
    {
        StartCoroutine(FadeOut("2.GameScene3"));
        sounds.Main_ClickSound();
    }
    public void SelectMap4Btn()
    {
        StartCoroutine(FadeOut("2.GameScene4"));
        sounds.Main_ClickSound();
    }
    public void SelectMap5Btn()
    {
        StartCoroutine(FadeOut("2.GameScene5"));
        sounds.Main_ClickSound();
    }
    public void SelectMap6Btn()
    {
        StartCoroutine(FadeOut("2.GameScene6"));
        sounds.Main_ClickSound();
    }

    public void InfoBtn()
    {
        StartCoroutine(FadeOut("3.GameInfoScene"));
        sounds.Main_ClickSound();
    }

    public void SettingBtn()
    {
        StartCoroutine(FadeOut("4.SettingScene"));
        sounds.Main_ClickSound();
    }



    public void ExitBtn()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif

    }

    IEnumerator FadeIn(string Scene)
    {
        panel.transform.SetAsLastSibling();

        float color = 1;

        while (color > 0)
        {
            color -= 0.03f;
            yield return new WaitForSeconds(0.01f);
            panel.color = new Color(0, 0, 0, color);
        }

        if (Scene != null)
        {
            SceneChange(Scene);
        }

        panel.transform.SetAsFirstSibling();
        
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

    private void OnMouseEnter()
    {
        sounds.Main_OnMouseSound();
    }
}
