using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glow : MonoBehaviour
{
    [SerializeField] private Vector2 relativeIntensityRange = new Vector2(0.8F, 1.3F);
    [SerializeField] private Vector2 glowCycleDuration = new Vector2(0.05F, 0.05F);
    [SerializeField] private float intensityChangePerCycle = 0.02F;

    private Light movingLight;
    private float defaultIntensity;
    private float lastIntensity;

    // Start is called before the first frame update
    void Start()
    {
        this.movingLight = GetComponent<Light>();

        this.defaultIntensity = this.movingLight.intensity;
        this.lastIntensity = 1;

        StartCoroutine(glow());
    }

    IEnumerator glow(){
        while(true){

            if(lastIntensity < relativeIntensityRange.x || lastIntensity > relativeIntensityRange.y){
                this.intensityChangePerCycle *= -1;
            }

            this.lastIntensity += this.intensityChangePerCycle;
            this.movingLight.intensity = this.lastIntensity * defaultIntensity; 

            float randomDelay = UnityEngine.Random.Range(glowCycleDuration.x, glowCycleDuration.y);
            yield return new WaitForSeconds(randomDelay);
        }
    }
}
