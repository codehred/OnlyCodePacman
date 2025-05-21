using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class ReadySceneManager : MonoBehaviour
{
    [SerializeField] private TMP_Text readyText;  // Referencia al texto READY!
    [SerializeField] private AudioSource audioSource;  // Para la música de intro
    [SerializeField] private AudioClip introMusic;  // El clip de audio de intro

    private void Start()
    {
        // Asegurarnos que el texto esté visible
        if (readyText != null)
        {
            readyText.text = "READY!";
            readyText.gameObject.SetActive(true);
        }

        // Iniciar la música de intro
        if (audioSource != null && introMusic != null)
        {
            audioSource.clip = introMusic;
            audioSource.Play();
        }

        // Iniciar la secuencia de espera
        StartCoroutine(ReadySequence());
    }

    private IEnumerator ReadySequence()
    {
        // Esperar 5 segundos
        yield return new WaitForSeconds(5f);

        // Detener la música si aún está sonando
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        // Cargar la escena principal del juego
        SceneManager.LoadScene("Pacman");
    }
}