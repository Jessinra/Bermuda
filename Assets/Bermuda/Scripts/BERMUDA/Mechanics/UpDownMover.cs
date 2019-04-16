using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDownMover : Mover
{
    [SerializeField] private float changeDirectionDelay = 1F;
    private string currentDirection = "up";

    // Start is called before the first frame update
    new void Start() {
        base.Start();
        StartCoroutine(upDownMove());
    }

    IEnumerator upDownMove(){
        while(true){

            currentDirection = "up";
            setDirection(currentDirection);
            yield return new WaitForSeconds(changeDirectionDelay);
            
            currentDirection = "down";
            setDirection(currentDirection);
            yield return new WaitForSeconds(changeDirectionDelay);
        }
    }

    new void FixedUpdate() {
        base.FixedUpdate();
    }
}
