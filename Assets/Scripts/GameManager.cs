using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;

public class GameManager : MonoBehaviour
{
    public static GameManager Manager;

    public AudioClip match;
    public AudioClip wrong;
    public AudioSource audioSource;

    public RewardedAdsButton RestartButton;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI endText;
    public TextMeshProUGUI matchFailText;

    public GameObject card;
    public GameObject firstCard;
    public GameObject secondCard;
    public GameObject GameEndPanel;

    public float timer = 0f;

    float time = 0.0f;
    float remainTime = 40.0f;
    public int level;
    int matchFailCount = 0;
    int count = 16;
    bool isGameEnd = false;

    private void Awake()
    {       
        Manager = this;
    }
    void Start()
    {
        Time.timeScale = 1.0f;
        level = PlayerPrefs.GetInt("SelectedLevel", 1);
        count = level != 3 ? (2 * level) * (2 * level) : 16;

        int[] slimes = { };
        int row = (int)Mathf.Sqrt(count);
        for(int i = 0; i < count / 2; i++)
        {
            slimes = slimes.Append(i).ToArray();
            slimes = slimes.Append(i).ToArray();
        }
        //int[] rtans = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 };
        slimes = slimes.OrderBy(item => Random.Range(-1.0f,1.0f)).ToArray();

        for (int i = 0; i < count; i++)
        {
            GameObject newCard = Instantiate(card);
            newCard.transform.parent = GameObject.Find("Cards").transform;

            float x = (i / row) * 1.4f - row * 0.53f;
            float y = (i % row) * 1.4f - row * 0.8f;
            newCard.transform.position = new Vector3(x, y, 0.0f);
            
            string slimeName = "Slime" + slimes[i].ToString();
            newCard.transform.Find("Front").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Slime/{slimeName}");
        }

    }

    void Update()
    {
        time += Time.deltaTime;
        timeText.text = time.ToString("N2");
        if(time >= remainTime)
        {
            GameEnd();
        }
        
        if(remainTime - time < 10.0f)
        {
            timeText.color = Color.red;
        }

        if (Time.time - timer >= 3.0f && firstCard != null && secondCard == null)
        {
            firstCard.transform.Find("back").gameObject.SetActive(true); ;
            firstCard.transform.Find("Front").gameObject.SetActive(false);
            firstCard = null;
        }
    }

    public void IsMatched()
    {
        if(firstCard.transform.Find("Front").GetComponent<SpriteRenderer>().sprite.name == secondCard.transform.Find("Front").GetComponent<SpriteRenderer>().sprite.name)
        {
            audioSource.PlayOneShot(match);
            
            firstCard.GetComponent<Card>().DestroyCard();
            secondCard.GetComponent<Card>().DestroyCard();
            count -= 2;
            
            if (count == 0)
            {
                GameEnd();
            }
        }
        else
        {
            audioSource.PlayOneShot(wrong);
            matchFailCount += 1;
            time += 1.0f;
            firstCard.GetComponent<Card>().CloseCard();
            secondCard.GetComponent<Card>().CloseCard();
        }
        firstCard = null;
        secondCard = null;
    }
    void GameEnd()
    {
        if(isGameEnd == false)
        {
            GameEndPanel.gameObject.SetActive(true);
            matchFailText.text = $"{matchFailCount}¹øÀÇ miss";
            RestartButton.LoadAd();
            isGameEnd = true;
            Time.timeScale = 0.0f;
        }
    }
    public void retryGame()
    {
        SceneManager.LoadScene("LevelSelectScene");
    }
}
