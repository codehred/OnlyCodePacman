using UnityEngine;

public class Pellet : MonoBehaviour
{
   public int points=10;
   protected virtual void Eat() //permite acceder a ella desde subclases y modificarla (override) desde la misma.
   {
        // Sustituimos FindObjectsOfType por FindObjectsByType
         GameManager[] gameManagers = Object.FindObjectsByType<GameManager>(FindObjectsSortMode.None);

       foreach (GameManager manager in gameManagers)
        {
            manager.PelletEaten(this);
        }
       
   }

   private void OnTriggerEnter2D(Collider2D other)
   {
    if(other.gameObject.layer==LayerMask.NameToLayer("Pacman"))
    {
        Eat();
    }
   }
}
