using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

    [SerializeField] private float rotateCycleDuration = 0.1F;
    [SerializeField] private float degreeChangePerCycle = 0.005F;

    protected Rigidbody2D rigidBody = null;
    private float currentDegrees = 0;

    protected void Start() {
        rigidBody = GetComponent<Rigidbody2D>();
        StartCoroutine(constantRotation());
    }

    protected void FixedUpdate() {
        this.transform.Rotate(0, 0, currentDegrees, Space.Self);
    }

    IEnumerator constantRotation() {
        while (true) {
            currentDegrees += degreeChangePerCycle;
            if (currentDegrees > 180) {
                currentDegrees -= 360;
            }
            yield return new WaitForSeconds(rotateCycleDuration);
        }
    }
}