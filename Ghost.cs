using UnityEngine;
[DefaultExecutionOrder(-10)]
[RequireComponent(typeof(Movement))]

public class Ghost : MonoBehaviour
{
    
    public Movement movement {get; private set;}
    public GhostHome home {get; private set;}
    public GhostScatter scatter {get; private set;}
    public GhostChase chase {get; private set;}
    public GhostFrightened frightened {get; private set;}
    public GhostBehaviour initialBehaviour;
    public Transform target;
    
    public int points =200;
    private void Awake()
    {
        this.movement=GetComponent<Movement>();
        this.home=GetComponent<GhostHome>();
        this.scatter=GetComponent<GhostScatter>();
        this.chase=GetComponent<GhostChase>();
        this.frightened=GetComponent<GhostFrightened>();
    }
    private void Start() 
    {
        ResetState();
    }

    public void ResetState()
    {
        this.gameObject.SetActive(true);
        this.movement.ResetState();
        this.frightened.Disable();
        this.chase.Disable();
        this.scatter.Enable();
        if(this.home!=this.initialBehaviour)
        {
            this.home.Disable();
        }
        if(this.initialBehaviour!=null)
        {
            this.initialBehaviour.Enable();
        }
        
    }
     public void SetPosition(Vector3 position)
    {
        
        position.z = transform.position.z;
        transform.position = position;
    }
   private void OnCollisionEnter2D(Collision2D collision) 
{
    if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
    {
        var gameManager = FindFirstObjectByType<GameManager>();
        if (gameManager != null)
        {
            if (this.frightened.enabled)
            {
                gameManager.GhostEaten(this);
            }
            else
            {
                gameManager.PacmanEaten();
            }
        }
        else
        {
            Debug.LogError("GameManager not found.");
        }
    }
}
 public void UpdateSpeed(float multiplier)
    {
        this.movement.speed *= multiplier; // Incrementa la velocidad del fantasma
    }



}