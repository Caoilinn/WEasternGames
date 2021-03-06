using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float transitionDurationSeconds = 1f; // transitions will always take this amount of time no matter the difference

    private float originTimeScale = 1f;
    private float targetTimeScale = 1f;
    private Coroutine transitionCoroutine;

    public void ChangeTimescale (float targetTimeScale) {
        // reset the origin timescale to be the current timescale
        this.originTimeScale = Time.timeScale;

        //set new target time scale
        this.targetTimeScale = targetTimeScale;

        // stop the coroutine if its running currently running transitions
        if (transitionCoroutine != null) {
            StopCoroutine(transitionCoroutine);
        }

        // start a new coroutine
        transitionCoroutine = StartCoroutine(TransitionBetweenTimescales());
    }

    private IEnumerator TransitionBetweenTimescales() {
        int iterationCount = 0;
        float percentage = 0;
        while (this.targetTimeScale != Time.timeScale) {
            // get the percentage in between the origin and target timescale based on the time passed, uses unscaled delta time, before timescale is applied
            percentage = (Time.unscaledDeltaTime * iterationCount) / this.transitionDurationSeconds;

            // interpolate based on the percentage calculated
            Time.timeScale = LinearInterpolate(this.originTimeScale, this.targetTimeScale, percentage);

            iterationCount++;

            yield return new WaitForEndOfFrame();
        }

        // set the current timescale to be the target timeScale
        this.originTimeScale = this.targetTimeScale;
    } 

    private float LinearInterpolate(float from, float to, float percentage) {
        // limit percentage between 0 and 1
        percentage = Mathf.Clamp(percentage, 0, 1);

        // interpolation result
        float result = from + ((to - from) * percentage);

        return result;
    }
}
