using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public static Game Insatnce;
    public int score = 0;
    string scenename = "首页";//
    public Text textComponent;
    public Text texttimer;
    public GameObject scoreTextGo;
    public AudioClip audioClip;
    public AudioClip audioClipemeny;
    public AudioSource audioSource;

    Transform endpanel;
    Transform introducepanel;
    Button bEnd, brestart;
    Button btn1, btn2;
    Button btn3;
    bool isfinish;
    float timer;
    private void Awake()
    {
        Insatnce = this;
    }
    //start每次游戏开始只执行一次
    private void Start()
    {

        isfinish = false;
        scoreTextGo = GameObject.Find("Canvas").transform.Find("ScoreText").gameObject;
        texttimer = GameObject.Find("Canvas").transform.Find("timer").gameObject.GetComponent<Text>();
        textComponent = scoreTextGo.GetComponent<Text>();
        textComponent.text = "Enemies eliminated：" + score.ToString() + "/20";
        texttimer.text = "time left" + 100 + "秒";
        audioSource = GameObject.Find("mp3").GetComponent<AudioSource>();
        introducepanel = GameObject.Find("Canvas").transform.Find("introducepanel").Find("bg");
        endpanel = GameObject.Find("Canvas").transform.Find("endpanel").Find("bg");

        bEnd = endpanel.Find("X").GetComponent<Button>();
        brestart = endpanel.Find("restart").GetComponent<Button>();
        btn1 = GameObject.Find("Canvas").transform.Find("btn1").GetComponent<Button>();
        btn2 = GameObject.Find("Canvas").transform.Find("btn2").GetComponent<Button>();
        btn3 = introducepanel.Find("X").GetComponent<Button>();
        brestart.onClick.RemoveAllListeners();
        brestart.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(scenename);
        });
        bEnd.onClick.RemoveAllListeners();
        bEnd.onClick.AddListener(() =>
        {
            Debug.LogError("quit");
            Application.Quit();
        });
        btn1.onClick.RemoveAllListeners();
        btn1.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(scenename);
        });
        btn2.onClick.RemoveAllListeners();
        btn2.onClick.AddListener(() =>
        {
            introducepanel.gameObject.SetActive(true);
        });
        btn3.onClick.RemoveAllListeners();
        btn3.onClick.AddListener(() =>
        {
            introducepanel.gameObject.SetActive(false);
        });
    }
    public void MP3(bool isplayer)
    {
        if (isplayer)
        {
            audioSource.PlayOneShot(audioClip);
        }
        else
        {
            audioSource.PlayOneShot(audioClipemeny);
        }
    }
    public void End(bool isok)
    {
        if (isok)
        {
            endpanel.transform.Find("WinText").GetComponent<Text>().text = "Successful";
        }
        else { endpanel.transform.Find("WinText").GetComponent<Text>().text = "Failed."; }
       
        isfinish = true;
        endpanel.gameObject.SetActive(true);
    }
    public  void Score()
    {
        
        
            score++;
            textComponent.text = "Enemies eliminated：" + score.ToString() + "/20";
            audioSource.PlayOneShot(audioClip);
            if (score == 20)
            {
                isfinish = true;
                endpanel.gameObject.SetActive(true);
            }
        
    }
    private void Update()
    {
        if (isfinish)
        {
            return;
        }

      
        if (Input.GetMouseButtonDown(0))
        {
            if (Camera.main != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out var hit))
                {
                    if (hit.collider.name.Contains("Gem"))
                    {
                        Click(hit.collider.gameObject);
                    }
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Gem") || other.name.Contains("Crystal"))
        {
            score++;
            textComponent.text = "找到爱心数量：" + score.ToString() + "/20";
            Destroy(other.gameObject);
            audioSource.PlayOneShot(audioClip);
            if (score == 20)
            {
                isfinish = true;
                endpanel.gameObject.SetActive(true);
            }
        }
    }
    void Click(GameObject other)
    {
        score++;
        textComponent.text = "找到爱心数量：" + score.ToString() + "/20";
        Destroy(other.gameObject);
        audioSource.PlayOneShot(audioClip);
        if (score == 20)
        if (score == 20)
        {
            isfinish = true;
            endpanel.gameObject.SetActive(true);
        }
    }
}
