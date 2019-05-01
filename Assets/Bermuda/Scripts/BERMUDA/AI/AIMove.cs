using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMove : MonoBehaviour {

    [SerializeField] private Vector2 changeDirectionDelayRange = new Vector2(1F, 6F);

    private Player player;
    Vector2 direction = Vector2.ClampMagnitude(offset, 0.15f);
    Vector2 offset = new Vector2(randomX, randomY);

    void Start() {
        this.player = this.transform.parent.gameObject.GetComponent<Player>();
        StartCoroutine(randomMove());
    }

    void FixedUpdate(){
        player.Move(direction);
    }

    IEnumerator randomMove(){
        while(true){
            Vector2 direction = Vector2.ClampMagnitude(offset, 0.15f);
            Vector2 offset = new Vector2(randomX, randomY);
            
            float changeDirectionDelay = UnityEngine.Random.Range(changeDirectionDelayRange.x, changeDirectionDelayRange.y);
            yield return new WaitForSeconds(changeDirectionDelay);
        }
    }

    string getRandomDirection(){
        List<string> directions = new List<string>(){
            "up", "down", "left", "right"
        };
        return directions[UnityEngine.Random.Range(0,4)];
    }

    void FixedUpdate() {
        base.FixedUpdate();
    }
}