using UnityEngine;

namespace BackRockStudios
{
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

        [Header("Spark Effect")]
        [Tooltip("Prefab of the spark effect to spawn.")]
        public GameObject sparkPrefab;
        public Transform spawnParticlePos;
        [Tooltip("Time range (in seconds) between spark spawns.")]
        public Vector2 sparkIntervalRange = new Vector2(1f, 3f);
        [Tooltip("How long the spark stays alive before being destroyed.")]
        public float sparkLifetime = 0.5f;

        private float randomTimeOffset;
        private float nextSparkTime;

        void Start()
        {
            if (flickerLight == null)
                flickerLight = GetComponent<Light>();

            randomTimeOffset = Random.Range(0f, 100f);
            SetNextSparkTime();
        }

        void Update()
        {
            // --- Flicker effect ---
            float noise = Mathf.PerlinNoise(Time.time * flickerSpeed + randomTimeOffset, 0.0f);
            flickerLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);

            // --- Spark spawning ---
            if (sparkPrefab != null && Time.time >= nextSparkTime)
            {
                SpawnSpark();
                SetNextSparkTime();
            }
        }

        void SpawnSpark()
        {
            GameObject spark = Instantiate(sparkPrefab, spawnParticlePos.position, Quaternion.identity);
            Destroy(spark, sparkLifetime);
        }

        void SetNextSparkTime()
        {
            nextSparkTime = Time.time + Random.Range(sparkIntervalRange.x, sparkIntervalRange.y);
        }
    }

}