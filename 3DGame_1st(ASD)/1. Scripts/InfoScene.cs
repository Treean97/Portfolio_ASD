using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InfoScene : MonoBehaviour
{
    public Image panel;
    public Sounds sounds;
    public Button leftBtn;
    public Button rightBtn;
    public Transform infoView;
    public Transform howToWinView;


    // Start is called before the first frame update
    void Start()
    {
        panel.gameObject.SetActive(true);
        StartCoroutine(FadeIn(null));
        leftBtn.gameObject.SetActive(false);
        rightBtn.gameObject.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void LeftBtn()
    {
        sounds.Info_LRClickSound();

        rightBtn.gameObject.SetActive(true);
        leftBtn.gameObject.SetActive(false);

        infoView.SetSiblingIndex(1);
    }

    public void RightBtn()
    {
        sounds.Info_LRClickSound();

        rightBtn.gameObject.SetActive(false);
        leftBtn.gameObject.SetActive(true);

        howToWinView.SetSiblingIndex(1);
    }

    public void ReturnBtn()
    {
        sounds.Info_ReturnClickSound();

        StartCoroutine(FadeOut("1.MainScene"));
    }


}
