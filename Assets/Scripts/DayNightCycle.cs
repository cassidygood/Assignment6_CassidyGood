using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Light sun;                   // The directional light acting as the sun
    public float dayDuration = 120f;    // Time (in seconds) for a full cycle
    private float currentTime = 0f;

    void Update()
    {
        // Update time and loop
        currentTime += Time.deltaTime;
        if (currentTime >= dayDuration)
            currentTime = 0f;

        // Calculate sun rotation (360 degrees over dayDuration)
        float sunAngle = (currentTime / dayDuration) * 360f;
        sun.transform.rotation = Quaternion.Euler(sunAngle - 90, 170, 0); // adjust Y/Z for your scene
    }
}

