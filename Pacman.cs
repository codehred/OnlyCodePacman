using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Movement))]
public class Pacman : MonoBehaviour
{
    [SerializeField]
    private AudioClip normalSound;  //EATTTT
    [SerializeField]
    private AudioClip deathSound;   // Sonido de muerte

    private AudioSource audioSource;  // Referencia al AudioSource
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D circleCollider;
    private Movement movement;
    private AnimatedSprite deathSequence;
     private bool canMove = true;

    private void Awake()
    {
        // Obtener componentes
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
        movement = GetComponent<Movement>();
        audioSource = GetComponent<AudioSource>();  // Obtener el componente AudioSource
        deathSequence = GetComponent<AnimatedSprite>();  // Obtener AnimatedSprite para manejar la animación
    }
public void PlayEatSound()
    {
        if (normalSound != null && audioSource != null)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(normalSound);
            }
        }
    }
    private void Update()
    {
        if (!canMove)
            {
                return;  // Si el movimiento está deshabilitado, no proceses la entrada del jugador
            }


        // Movimiento de Pacman con teclas
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
            movement.SetDirection(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
            movement.SetDirection(Vector2.down);
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
            movement.SetDirection(Vector2.left);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
            movement.SetDirection(Vector2.right);
        }

       
        if (movement.direction != Vector2.zero) {
            float angle = Mathf.Atan2(movement.direction.y, movement.direction.x);
            transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
        }
    }

    public void ResetState()
    {
     enabled = true;
    gameObject.SetActive(true); // Asegúrate de que el objeto esté activo

    // Detén la animación de muerte y reinicia la animación normal
    deathSequence.StopAnimation();
    deathSequence.enabled=true;
    ResetToNormalAnimation();

    // Asegura que todos los componentes necesarios estén habilitados
    spriteRenderer.enabled = true;
    circleCollider.enabled = true;
    movement.enabled = true;
    // Asegura que el script de movimiento se resetee y Pacman esté listo para moverse
    movement.ResetState();
    }

    public void DeathSequence()
{
    if (!gameObject.activeInHierarchy) // Verifica si Pacman está activo en la escena
    {
        Debug.LogWarning("Pacman ya está desactivado.");
        return;
    }

    enabled = false;  // Desactiva el script principal de Pacman para evitar interferencia
    spriteRenderer.enabled = true;  
    circleCollider.enabled = false;  // Desactiva la colisión
    movement.enabled = false; 

    // Cambiar al sonido de muerte cuando Pacman muere
    if (audioSource != null && deathSound != null)
    {
        audioSource.clip = deathSound;
        audioSource.Play();
    }

    // Detener la animación de muerte y reproducirla
    if (deathSequence != null)
    {
         deathSequence.PlayDeathAnimation();
    }
    else
    {
        Debug.LogError("Death Sequence no está asignado en el Inspector.");
    }
}
public void SetActionsEnabled(bool enabled)
{
    // Detén o reanuda el movimiento
    GetComponent<Movement>().enabled = enabled;

    // Otras acciones específicas
}
private IEnumerator WaitForDeathAnimation()
{
    // Espera a que termine la animación de muerte
    while (deathSequence.isPlayingDeathAnimation)
    {
        yield return null; // Espera un frame
    }

    // Una vez que la animación de muerte ha terminado, reinicia el estado
    ResetState();
}

    public void EnableMovement()
    {
         canMove = true; // Permite que Pacman se mueva
        movement.enabled = true; // Asegúrate de habilitar el componente Movement
    }

    public void DisableMovement()
    {
        canMove = false; // Detén el movimiento
        movement.SetDirection(Vector2.zero); // Asegúrate de detener el movimiento actual
        movement.enabled = false; 
    }
public void ResetToNormalAnimation()
{
    // Asegúrate de que el componente AnimatedSprite esté habilitado
    if (deathSequence != null)
    {
        deathSequence.enabled = true; // Asegúrate de que el script de animación esté habilitado
        deathSequence.Restart(); // Reinicia la animación normal
    }
}
}