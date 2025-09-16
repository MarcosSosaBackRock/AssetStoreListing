using UnityEngine;

[ExecuteInEditMode]  // Runs in Scene view without pressing Play
public class RandomLightFlicker : MonoBehaviour
{
    public Light pointLight;
    public float minIntensity = 5f;
    public float maxIntensity = 200f;
    public float flickerSpeed = 10f;

    private float timer;
    private float lastTime;

    void Start()
    {
        if (pointLight == null)
            pointLight = GetComponent<Light>();

        lastTime = Time.realtimeSinceStartup;
    }

    void Update()
    {
        // use realtime so it works in Edit Mode
        float currentTime = Time.realtimeSinceStartup;
        float delta = currentTime - lastTime;
        lastTime = currentTime;

        timer -= delta;
        if (timer <= 0f && pointLight != null)
        {
            pointLight.intensity = Random.Range(minIntensity, maxIntensity);
            timer = flickerSpeed;
        }
    }
}
