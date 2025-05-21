using UnityEngine;

public class PowerPellet : Pellet
{
    public float duration=8.0f;
     protected override void Eat() //permite acceder a ella desde subclases y modificarla (override) desde la misma.
   {
        // Sustituimos FindObjectsOfType por FindObjectsByType
         GameManager[] gameManagers = Object.FindObjectsByType<GameManager>(FindObjectsSortMode.None);

       foreach (GameManager manager in gameManagers)
        {
            manager.PowerPelletEaten(this);
        }
       
   }
}
