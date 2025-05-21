using UnityEngine;
using System.Collections;

public class GhostHome : GhostBehaviour
{
    public Transform inside;
    public Transform outside;
   private void OnEnable() 
   {
    StopAllCoroutines();
   }

    private void OnDisable() 
    {
        if (this.gameObject.activeSelf) // Verifica si el objeto está activo
        {
        StartCoroutine(ExitTransition()); 
        
        }
    }
    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (this.enabled && collision.gameObject.layer==LayerMask.NameToLayer("Obstacle"))
        {
            this.ghost.movement.SetDirection(-this.ghost.movement.direction);
        }
    }

    private IEnumerator ExitTransition()
    {
        this.ghost.movement.SetDirection(Vector2.up,true);
        this.ghost.movement.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        this.ghost.movement.enabled=false;

        Vector3 position = this.transform.position;
        float duration=0.5f;
        float elapsed=0.0f;

        while(elapsed<duration)
        {
            /* Lerp se utiliza para calcular un valor intermedio entre dos valores a lo largo de una línea recta, con base en un factor de 
            interpolación.
            Animación: Lerp es muy útil para hacer transiciones suaves entre dos valores, como el 
            movimiento de un objeto de una posición a otra, o la interpolación de colores.*/
            Vector3 newPosition= Vector3.Lerp(position,this.inside.position,elapsed/duration);
            newPosition.z=position.z;
            this.ghost.transform.position = newPosition;
            elapsed+=Time.deltaTime;
            yield return null;
        }

        elapsed=0.0f;

         while(elapsed<duration)
        {
            Vector3 newPosition= Vector3.Lerp(this.inside.position,this.outside.position,elapsed/duration);
            newPosition.z=position.z;
            this.ghost.transform.position = newPosition;
            elapsed+=Time.deltaTime;
            yield return null;
        }


        this.ghost.movement.SetDirection(new Vector2(Random.value<0.5f ? -1.0f : 1.0f,0.0f),true); //condición ? valor_si_verdadero : valor_si_falso; 
        //(el "?" es un condicional ternario)
        this.ghost.movement.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        this.ghost.movement.enabled=true;

    }
}