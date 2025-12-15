using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public int score = 0;
    string scenename = "游戏";//MainUI
    public Text textComponent;

    public GameObject scoreTextGo;


    Transform startpanel, introducepanel, endpanel;
    Button b1, b2, b3, BX, bEnd;


    //start每次游戏开始只执行一次
    private void Start()
    {


        scoreTextGo = GameObject.Find("Canvas").transform.Find("ScoreText").gameObject;

        textComponent = scoreTextGo.GetComponent<Text>();
        textComponent.text = "Point：" + score.ToString() + "/10";

        startpanel = GameObject.Find("Canvas").transform.Find("startpanel").Find("bg");
        introducepanel = GameObject.Find("Canvas").transform.Find("introducepanel").Find("bg");
        endpanel = GameObject.Find("Canvas").transform.Find("endpanel").Find("bg");
        b1 = startpanel.Find("btn1").GetComponent<Button>();
        b2 = startpanel.Find("btn2").GetComponent<Button>();
        b3 = startpanel.Find("btn3").GetComponent<Button>();
        BX = introducepanel.Find("X").GetComponent<Button>();
        bEnd = endpanel.Find("X").GetComponent<Button>();
        b1.onClick.RemoveAllListeners();
        b1.onClick.AddListener(() => {

            SceneManager.LoadScene(scenename);
        });
        b2.onClick.RemoveAllListeners();
        b2.onClick.AddListener(() => {
            startpanel.gameObject.SetActive(false);
            introducepanel.gameObject.SetActive(true);
        });
        b3.onClick.RemoveAllListeners();
        b3.onClick.AddListener(() => {
            Debug.LogError("quit");
            Application.Quit();
        });
        BX.onClick.RemoveAllListeners();
        BX.onClick.AddListener(() => {
            startpanel.gameObject.SetActive(true);
            introducepanel.gameObject.SetActive(false);
        });
        bEnd.onClick.RemoveAllListeners();
        bEnd.onClick.AddListener(() => {
            Debug.LogError("quit");
            Application.Quit();
        });
    }
}
