using UnityEngine;

public class GhostChase : GhostBehaviour
{

    private void OnDisable() 
    {

        this.ghost.scatter.Enable();
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {

        Node node=other.GetComponent<Node>();
        if (node!=null && this.enabled && !this.ghost.frightened.enabled)
        {
            Vector2 direction = Vector2.zero;
            float minDistance=float.MaxValue;

            foreach(Vector2 availableDirection in node.availableDirections)
            {
                Vector3 newPosition=this.transform.position + new Vector3(availableDirection.x,availableDirection.y,0.0f);
                float distance=(this.ghost.target.position - newPosition).sqrMagnitude; 
                //no usar magnitud normal por eficiencia, al no usar sqr, hará el
                //programa más lento por la cantidad de procesos q llevará la magnitud normal.
                if(distance<minDistance)
                {
                    direction=availableDirection;
                    minDistance=distance;
                }
            }
            this.ghost.movement.SetDirection(direction);
        }
    }
}
