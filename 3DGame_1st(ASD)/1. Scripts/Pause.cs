using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public Sounds sounds;
    public SaveLoad saveLoad;
    public GameObject pauseView;
    public PlayerMovement pm;
    public PlayerCamera pc;
    public Slider xSensi;
    public Slider ySensi;
    public Text xSensiValue;
    public Text ySensiValue;
    public Text time;
    public Text wave;

    public bool isOpen;

    // Start is called before the first frame update
    void Start()
    {
        pauseView.gameObject.SetActive(false);
        isOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4) && isOpen == false)
        {
            isOpen = true;
            pauseView.gameObject.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            xSensi.value = pm.YrotateSpeed / 10;
            ySensi.value = pc.XrotateSpeed / 10;
            time.gameObject.SetActive(false);
            wave.gameObject.SetActive(false);
            Time.timeScale = 0;
            

        }

        if((Input.GetKeyDown(KeyCode.F1) && isOpen == true))
        {
            isOpen = false;
            pauseView.gameObject.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
            time.gameObject.SetActive(true);
            wave.gameObject.SetActive(true);

        }

        xSensiValue.text = (xSensi.value).ToString("00.0");
        ySensiValue.text = (ySensi.value).ToString("00.0");
    }


    public void RestartBtn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainBtn()
    {
        SceneManager.LoadScene("1.MainScene");

    }

    public void ApplyBtn()
    {
        GameManager.instance.xSensi = xSensi.value;
        GameManager.instance.ySensi = ySensi.value;

        pm.SendMessage("Apply");
        pc.SendMessage("Apply");
    }

}
