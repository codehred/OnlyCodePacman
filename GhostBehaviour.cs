using UnityEngine;
[RequireComponent(typeof(Ghost))]
public abstract class GhostBehaviour : MonoBehaviour //abstract para que no pueda estar llamado por su cuenta, si no que
// tiene que haber una clase que la herede
{
    public Ghost ghost {get; private set;}
    public float duration;
    private void Awake() 
    {
        this.ghost=GetComponent<Ghost>();
    }
    public void Enable()
    {
        Enable(this.duration);
    }
    public virtual void Enable(float duration)
    {
        this.enabled=true;
        CancelInvoke();
        Invoke(nameof(Disable),duration);
    }
    public virtual void Disable()
    {
        this.enabled=false;
        CancelInvoke();
    }
}
