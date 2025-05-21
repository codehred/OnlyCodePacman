using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Ghost[] ghosts;
    [SerializeField] private Pacman pacman;
    [SerializeField] private Transform pellets;
     [SerializeField] private TMP_Text gameOver;
      [SerializeField] private TMP_Text pressKey;
        [SerializeField] private TMP_Text uWin;
      [SerializeField] private TMP_Text pressKeyWin;
      [SerializeField] private TMP_Text ScoreText;
      [SerializeField] private TMP_Text LivesText;
    [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip ghostMusicClip;  
[       SerializeField] private AudioClip frightenedMusicClip;
        [SerializeField] private AudioClip musicClip;
    
    public int score { get; private set; } = 0;
    public int lives { get; private set; } = 3;
 private bool isGameOver = false;
 private float speedMultiplier = 1.0f;
    private int ghostMultiplier = 1;
    private bool isFirstGame = true; 

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
private void PlayGhostMusic()
{
    audioSource.Stop();
    audioSource.clip = ghostMusicClip;
    audioSource.Play();
}

private void PlayFrightenedMusic()
{
    audioSource.Stop();
    audioSource.clip = frightenedMusicClip;
    audioSource.Play();
}
    private void Start()
    {
        
        gameOver.gameObject.SetActive(false);
        pressKey.gameObject.SetActive(false);
        uWin.gameObject.SetActive(false);
        pressKeyWin.gameObject.SetActive(false);
        PlayGhostMusic();
        NewGame();
        }

    private void Update()
    {
        //lose part
        if (lives <= 0 && Input.GetKey(KeyCode.Space))
        {
            gameOver.gameObject.SetActive(false);
            pressKey.gameObject.SetActive(false);
            uWin.gameObject.SetActive(false);
            pressKeyWin.gameObject.SetActive(false);
            SceneManager.LoadScene("IntroScene");
        }
        if (lives <= 0 && Input.GetKey(KeyCode.Escape))
        {
            gameOver.gameObject.SetActive(false);
            pressKey.gameObject.SetActive(false);
            uWin.gameObject.SetActive(false);
            pressKeyWin.gameObject.SetActive(false);
            SceneManager.LoadScene("Menu");
        }

        //winning part

        if (lives > 0 && !HasRemainingPellets() && Input.GetKey(KeyCode.Return)) // enter
    {
        gameOver.gameObject.SetActive(false);
        pressKey.gameObject.SetActive(false);
        uWin.gameObject.SetActive(false);
        pressKeyWin.gameObject.SetActive(false);
        NewRound();
    }
    if (lives > 0 && !HasRemainingPellets() && Input.GetKey(KeyCode.Space))
        {
            gameOver.gameObject.SetActive(false);
            pressKey.gameObject.SetActive(false);
            uWin.gameObject.SetActive(false);
            pressKeyWin.gameObject.SetActive(false);
            SceneManager.LoadScene("Menu");
        }
    }
   
    public void NewGame()
{
    gameOver.gameObject.SetActive(false);
    pressKey.gameObject.SetActive(false);
    uWin.gameObject.SetActive(false);
    pressKeyWin.gameObject.SetActive(false);

    if (isFirstGame)
    {
       
        SetScore(0);
        SetLives(3);
        isFirstGame = false;
    }
    else if (isGameOver)
    {
        
        speedMultiplier = 1.0f;
        SetScore(0);
        SetLives(3);
    }
    
    
    isGameOver = false;
    NewRound();
}


    private void NewRound()
    {
        gameOver.gameObject.SetActive(false);
        pressKey.gameObject.SetActive(false);
        uWin.gameObject.SetActive(false);
        pressKeyWin.gameObject.SetActive(false);
         if (!isGameOver) // Solo incrementa la velocidad si no es GameOver
        {
            speedMultiplier += 0.1f; // Incrementa ligeramente la velocidad de los fantasmas
            UpdateGhostSpeeds(); // Actualiza la velocidad de los fantasmas
        }
        foreach (Transform pellet in pellets)
        {
            if (pellet != null)
            {
                pellet.gameObject.SetActive(true);
            }
        }

        ResetState();
    }
    private void UpdateGhostSpeeds()
{
    foreach (Ghost ghost in ghosts)
    {
        ghost.UpdateSpeed(speedMultiplier); // Incrementa la velocidad de cada fantasma
    }
}

    private void ResetState()
    {
        foreach (Ghost ghost in ghosts)
        {
            ghost.ResetState();
        }

        pacman.ResetState();
    }

    private void GameOver()
    {
         for (int i = 0; i < ghosts.Length; i++) {
            ghosts[i].gameObject.SetActive(false);
        }

        gameOver.gameObject.SetActive(true);
        pressKey.gameObject.SetActive(true);
        uWin.gameObject.SetActive(false);
        pressKeyWin.gameObject.SetActive(false);
        pacman.gameObject.SetActive(false);
        
    }



    private void SetLives(int lives)
    {
        this.lives = lives;
        LivesText.text = "x" + lives.ToString();
    }

    private void SetScore(int score)
    {
        this.score = score;
        ScoreText.text = score.ToString().PadLeft(2, '0');
    }

    public void PacmanEaten()
    {
        if (pacman != null)
        {
            pacman.DeathSequence();
        }

        SetLives(lives - 1);

        if (lives > 0)
        {
            Invoke(nameof(ResetState), 3f);
        }
        else
        {
            GameOver();
        }
    }

    public void GhostEaten(Ghost ghost)
    {
        if (ghost != null)
        {
            int points = ghost.points * ghostMultiplier;
            SetScore(score + points);
            ghostMultiplier++;
        }
    }

    public void PelletEaten(Pellet pellet)
    {
        if (pellet != null)
        {
            pellet.gameObject.SetActive(false);
            SetScore(score + pellet.points);

            pacman?.PlayEatSound();

            if (!HasRemainingPellets())
            {
                uWin.gameObject.SetActive(true);
                pressKeyWin.gameObject.SetActive(true);
                pacman.gameObject.SetActive(false);
                  foreach (Ghost ghost in ghosts)
                    {
                        ghost.gameObject.SetActive(false);
                    }
            }
        }
    }

    public void PowerPelletEaten(PowerPellet pellet)
    {
       


        foreach (Ghost ghost in ghosts)
        {
            ghost?.frightened.Enable(pellet.duration);
        }
         PlayFrightenedMusic();

        pacman?.PlayEatSound();
        PelletEaten(pellet);

        CancelInvoke(nameof(ResetGhostMultiplier));
        Invoke(nameof(ResetGhostMultiplier), pellet.duration);

         Invoke(nameof(ResetGhostMultiplier), pellet.duration);
        Invoke(nameof(PlayGhostMusic), pellet.duration);
    }

    private bool HasRemainingPellets()
    {
        foreach (Transform pellet in pellets)
        {
            if (pellet.gameObject.activeSelf)
            {
                return true;
            }
        }

        return false;
    }

    private void ResetGhostMultiplier()
    {
        ghostMultiplier = 1;
    }
}