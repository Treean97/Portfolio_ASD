using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    public SaveLoad saveLoad;
    public Image panel;
    public Sounds sounds;
    public Slider xSensi;
    public Slider ySensi;
    public Text xSensiValue;
    public Text ySensiValue;

    // Start is called before the first frame update
    void Start()
    {
        xSensi.value = GameManager.instance.xSensi;
        ySensi.value = GameManager.instance.ySensi;

        panel.gameObject.SetActive(true);

        StartCoroutine(FadeIn(null));


    }

    // Update is called once per frame
    void Update()
    {
        xSensiValue.text = xSensi.value.ToString("00.0");
        ySensiValue.text = ySensi.value.ToString("00.0");
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

    public void ReturnBtn()
    {
        StartCoroutine(FadeOut("1.MainScene"));
        sounds.Setting_ClickSound();
    }

    public void ApplyBtn()
    {
        sounds.Setting_ClickSound();
        GameManager.instance.xSensi = xSensi.value;
        GameManager.instance.ySensi = ySensi.value;
        saveLoad.data.xSensi = xSensi.value;
        saveLoad.data.ySensi = ySensi.value;

    }

}
