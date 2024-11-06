using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour
{
    private int score = 0;
    private bool _gameOver = false;
    private float timer = 1f;


    private FlipCardScript first_opened;
    private FlipCardScript second_opened;

    

    [SerializeField] private Text scoreText;
    [SerializeField] private Text timerText;
    [SerializeField] private float timerLimit = 5;

    public bool isGameOver
    {
        get { return _gameOver; } 
    }
    public bool canOpen
    {
        get { return second_opened == null; }
    }

    //private Virus virusSelected;
    public void imageOpened(FlipCardScript cardOpened)
    {
        if(first_opened == null)
        {
            first_opened = cardOpened;
        } else
        {
            second_opened = cardOpened;
            StartCoroutine(CheckGuessed());
        }
    }

    private IEnumerator CheckGuessed()
    {
        if (first_opened.spriteId == second_opened.spriteId)
        {
            score++;
            scoreText.text = score.ToString();
        }
        else
        {
            yield return new WaitForSeconds(1f);
            first_opened.CloseCard();
            second_opened.CloseCard();
        }

        first_opened = null;
        second_opened = null;
    }

    private void Update()
    {
        if (timer < timerLimit)
        {
            timer += Time.deltaTime;
            timerText.text = timer.ToString();
            
        } else
        {
            _gameOver = true;
            Restart();
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene("MainScene");
    }


}
