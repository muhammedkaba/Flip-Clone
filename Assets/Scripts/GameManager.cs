using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject fog;

    [Header("Plaftorms")]
    public GameObject currentPlatform;
    public GameObject nextPlatform;
    public GameObject platformPrefab;

    [Header("Score")]
    [SerializeField] GameObject scored;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] Animator perfect;
    int score;

    [Header("GameOver")]
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] Text gameOverScore;
    [SerializeField] Text gameOverHighScore;
    [SerializeField] GameObject newBest;
    public bool gameOver;

    [Header("Pool")]
    [SerializeField] ObjectPool objectPool;
    int platformCounter = 0;
    // Start is called before the first frame update
    void Awake()
    {
        score = 0;
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
        {
            scoreText.gameObject.SetActive(false);
        }
    }

    public void NewPlatform()
    {
        Vector3 a = new Vector3(0, 0, Random.Range(-(currentPlatform.transform.localScale.z + platformPrefab.transform.localScale.z) / 2, -90));
        GameObject newPlatform = objectPool.plaformPool[platformCounter % 4];
        newPlatform.transform.position = currentPlatform.transform.position + a;
        newPlatform.GetComponent<Animator>().enabled = true;
        newPlatform.GetComponent<Animator>().SetTrigger("Moved");
        nextPlatform = newPlatform;
        platformCounter++;
        //nextPlatform = Instantiate(platformPrefab, currentPlatform.transform.position + a
        //    , Quaternion.identity);
        fog.transform.position += a;
    }

    public void Scored(int a)
    {
        if (a == 2)
        {
            perfect.SetTrigger("perfect");
        }
        score += a;
        scored.GetComponent<TextMesh>().text = "+" + a.ToString();
        Instantiate(scored, player.transform.position + new Vector3(0, 10f, 0), Quaternion.Euler(14.197f, 158.853f, 8.006f));
        scoreText.text = score.ToString();
    }

    public void Started()
    {
        scoreText.gameObject.SetActive(true);
        NewPlatform();
        Destroy(GameObject.Find("TapToStart"));
        Time.timeScale = 1;
        player.GetComponent<PlayerMovement>().enabled = true;
    }

    public void Ended()
    {
        gameOverPanel.SetActive(true);
        gameOverScore.text = score.ToString();
        if (score > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", score);
            gameOverHighScore.text = score.ToString();
            newBest.SetActive(true);
        }
        else
        {
            gameOverHighScore.text = PlayerPrefs.GetInt("HighScore").ToString();
        }
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(0);
    }
}
