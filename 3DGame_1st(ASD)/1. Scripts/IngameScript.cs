using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameScript : MonoBehaviour
{
    public EnemySpawn es;
    public GameObject endingView;
    public Text gold;
    public Text endingText;
    public Text endingInfoText;
    public Text timeText;
    public Text waveText;
    public Text killCountText;
    public bool isOpen;
    public bool isEnd;
    public int time;
    int killCount = 0;
    int wave = 1;
    bool isWave;
    bool isReady;
    float playTime;


    // Start is called before the first frame update
    void Start()
    {
        endingView.SetActive(false);
        timeText.gameObject.SetActive(true);
        waveText.gameObject.SetActive(true);
        playTime = 0;
        StartCoroutine(PlayTime());
        ReadyStart();
    }

    // Update is called once per frame
    void Update()
    {
        // 스킵
        if (Input.GetKeyDown(KeyCode.O))
        {
            time -= 20;
        }

    }

    IEnumerator PlayTime()
    {
        if(!isEnd)
        {
            yield return new WaitForSecondsRealtime(1);
            playTime++;
            StartCoroutine(PlayTime());
        }

    }

    public void Ending(string isWin)
    {
        endingView.SetActive(true);
        Time.timeScale = 0;
        endingText.text = "게임 " + isWin;
        timeText.gameObject.SetActive(false);
        waveText.gameObject.SetActive(false);
        // 커서
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        EndingInfo();
    }

    public void KillCount()
    {
        killCount++;
        killCountText.text = "Kill : " + killCount.ToString("000");
    }

    public void ChangeText(int wave)
    {
        if (isWave)
        {
            waveText.text = "Wave " + wave.ToString();
        }
        else if (isReady)
        {
            waveText.text = "Ready";
        }
    }

    void WaveStart()
    {
        isWave = true;
        isReady = false;
        ChangeText(wave);
        time = 179;
        StartCoroutine(WaveTime());
    }

    IEnumerator WaveTime()
    {
        while (time > 0)
        {
            timeText.text = (time / 60).ToString("00") + ":" + (time % 60).ToString("00");
            time -= 1;
            yield return new WaitForSeconds(1);
        }
        wave++;
        ReadyStart();
    }

    public void ReadyStart()
    {
        isWave = false;
        isReady = true;

        // 골드 지급
        gold.text = (int.Parse(gold.text) + (wave * 200)).ToString();

        ChangeText(wave);
        if (wave >= 5)
        {
            Ending("성공");
        }

        time = 30;
        StartCoroutine(ReadyTime());
    }

    IEnumerator ReadyTime()
    {
        while (time > 0)
        {
            timeText.text = (time / 60).ToString("00") + ":" + (time % 60).ToString("00");
            time -= 1;
            yield return new WaitForSeconds(1);
        }
        WaveStart();
    }

    void EndingInfo()
    {
        isEnd = true;
        endingInfoText.text = "처치한 적 : " + killCount + "\n\n막은 웨이브 : " + (wave - 1)
            + "\n\n플레이 타임 : " + (playTime / 60).ToString("00") + "분 " + (playTime % 60).ToString("00") + "초";
    }
}
