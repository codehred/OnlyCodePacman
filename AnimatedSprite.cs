using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedSprite : MonoBehaviour
{
    public SpriteRenderer spriteRenderer { get; private set; }
    public Sprite[] sprites; // Sprites normales
    public Sprite[] deathSprites; // Sprites para la animación de muerte
    public float animationTime = 0.25f; // Tiempo entre cuadros
    public bool loop = true;

    private float timer;
    private int animationFrame;
    public bool isPlayingDeathAnimation = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

  private void Update()
{
    if (isPlayingDeathAnimation || sprites == null || sprites.Length == 0)
    {
        return; // No proceses la animación normal si está en animación de muerte
    }

    timer += Time.deltaTime;

    if (timer >= animationTime)
    {
        timer -= animationTime;
        Advance();
    }
}
    private void Advance()
    {
        if (!spriteRenderer.enabled || sprites == null || sprites.Length == 0)
        {
            return;
        }

        animationFrame++;

        if (animationFrame >= sprites.Length)
        {
            if (loop)
            {
                animationFrame = 0; // Reinicia la animación si está en loop
            }
            else
            {
                animationFrame = sprites.Length - 1; // Mantén el último cuadro si no está en loop
            }
        }

        spriteRenderer.sprite = sprites[animationFrame]; // Asigna el sprite actual al SpriteRenderer
    }

    public void Restart()
    {
        isPlayingDeathAnimation = false;  // Resetea el estado de la animación de muerte
        animationFrame = -1;
        timer = 0;
        spriteRenderer.enabled = true;  // Asegura que el renderer esté activo
        Advance();
    }
    public void PlayDeathAnimation()
    {
        if (deathSprites != null && deathSprites.Length > 0)
        {
            isPlayingDeathAnimation = true; // Activa el estado de animación de muerte
            StopAnimation(); // Detén la animación normal
            StartCoroutine(PlayAnimationSequence(deathSprites)); // Comienza la animación de muerte
        }
        else
        {
            Debug.LogError("No hay sprites asignados para la animación de muerte.");
        }
    }

    private System.Collections.IEnumerator PlayAnimationSequence(Sprite[] animationSprites)
    {
        spriteRenderer.enabled = true;

        for (int i = 0; i < animationSprites.Length; i++)
        {
            spriteRenderer.sprite = animationSprites[i];
            yield return new WaitForSeconds(animationTime); // Espera antes de avanzar al siguiente sprite
        }

        isPlayingDeathAnimation = false;
        // Después de la animación, desactiva el objeto
         ResetToNormalAnimation();
    }

    public void StopAnimation()
    {
        // Reinicia el estado del sistema de animación
        timer = 0;
        animationFrame = 0;

        // Opcional: deja el último sprite visible o apaga el SpriteRenderer
        spriteRenderer.enabled = false;
    }

    // Método que restablece la animación normal
    public void ResetToNormalAnimation()
    {
        isPlayingDeathAnimation = false; // Asegúrate de que no esté en animación de muerte
        spriteRenderer.enabled = true; // Asegúrate de que el SpriteRenderer esté activo
        animationFrame = 0; // Reinicia el primer cuadro de la animación
        timer = 0;
        sprites = sprites ?? new Sprite[0];
        Advance(); // Comienza la animación normal
    }
}
