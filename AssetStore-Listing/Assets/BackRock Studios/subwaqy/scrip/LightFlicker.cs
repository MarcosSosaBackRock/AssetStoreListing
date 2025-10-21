using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    [Header("Light Settings")]
    public Light flickerLight;

    [Header("Flicker Controls")]
    [Tooltip("Minimum light intensity")]
    public float minIntensity = 0.8f;
    [Tooltip("Maximum light intensity")]
    public float maxIntensity = 1.5f;
    [Tooltip("How fast the light flickers. Lower = slower flicker.")]
    public float flickerSpeed = 5f;

    private float randomTimeOffset;

    void Start()
    {
        if (flickerLight == null)
            flickerLight = GetComponent<Light>();

        // Random offset so multiple lights don’t flicker in sync
        randomTimeOffset = Random.Range(0f, 100f);
    }

    void Update()
    {
        // Create a smooth flicker using Perlin noise (no harsh jumps)
        float noise = Mathf.PerlinNoise(Time.time * flickerSpeed + randomTimeOffset, 0.0f);
        flickerLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
    }
}
