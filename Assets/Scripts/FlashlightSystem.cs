using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class FlashlightSystem : MonoBehaviour
{
    public Light2D flashlight;
    public static float globalBatteryLevel = 100f; 
    public float currentBatteryDisplay;
    public float drainSpeed = 2.0f;
    public float maxIntensity = 2.0f;
    public float maxRadius = 6.0f;

    void Start()
    {
        if (flashlight == null)
            flashlight = GetComponent<Light2D>();

        currentBatteryDisplay = globalBatteryLevel;
    }

    void Update()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "House") 
        {
            flashlight.intensity = 0;
            return; 
        }

        if (globalBatteryLevel > 0)
        {
            globalBatteryLevel -= drainSpeed * Time.deltaTime;
        }
        else
        {
            globalBatteryLevel = 0;
        }

        currentBatteryDisplay = globalBatteryLevel;
        float batteryPercent = globalBatteryLevel / 100f;
        float nonLinearCurve = Mathf.Pow(batteryPercent, 0.4f);

        if (globalBatteryLevel > 20f)
        {
            flashlight.intensity = maxIntensity * nonLinearCurve;
            flashlight.pointLightOuterRadius = maxRadius * nonLinearCurve;
        }
        else if (globalBatteryLevel > 0)
        {
            float flicker = Mathf.PerlinNoise(Time.time * 20f, 0);
            flashlight.intensity = (maxIntensity * nonLinearCurve) * flicker;
            flashlight.pointLightOuterRadius = (maxRadius * nonLinearCurve) * flicker;
        }
        else
        {
            flashlight.intensity = 0;
        }
    }

    public void AddBattery(float amount)
    {
        globalBatteryLevel += amount;
        if (globalBatteryLevel > 100f) globalBatteryLevel = 100f;
    }
}