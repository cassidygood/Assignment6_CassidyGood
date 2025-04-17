using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Light sun;
    public Transform moon;
    public GameObject stars;

    public float dayDuration = 120f;
    private float currentTime = 0f;

    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= dayDuration)
            currentTime = 0f;

        float sunAngle = (currentTime / dayDuration) * 360f;

        sun.transform.rotation = Quaternion.Euler(sunAngle - 90f, 170f, 0f);
        moon.transform.rotation = Quaternion.Euler(sunAngle + 90f, 170f, 0f);

        bool isNight = Vector3.Dot(sun.transform.forward, Vector3.down) > 0;

        moon.gameObject.SetActive(isNight);
        stars.SetActive(isNight);
    }
}


