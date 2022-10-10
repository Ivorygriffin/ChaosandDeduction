using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[ExecuteAlways]
public class LightingManager : NetworkBehaviour
{

    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset Preset;
    public GameObject[] nightlights;
    public Timer timer;

    public bool lightsEnabled;

    //[SyncVar]
    //[SerializeField, Range(0, 180)] private float TimeOfDay;
    //private float StartTime;

    private void Update()
    {
        float TimeOfDay = timer.timeRemaining / Timer.cycleLength;

        TimeOfDay %= 1;
        TimeOfDay = 1 - TimeOfDay;

        UpdateLight(TimeOfDay);
    }

    private void UpdateLight(float time)
    {
        //time = debug1;
        float gradiant = 1 - time * 2;
        if (gradiant < 0) //reverse the time gradient
            gradiant = (time * 2) - 1;

        if (gradiant >= Preset.NightGradientPercent && !lightsEnabled)
            foreach (GameObject obj in nightlights)
            {
                obj.SetActive(true);
                lightsEnabled = true;
            }
        else if (gradiant < Preset.NightGradientPercent && lightsEnabled)
            foreach (GameObject obj in nightlights)
            {
                obj.SetActive(false);
                lightsEnabled = false;
            }


        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(gradiant);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(gradiant);


        if (DirectionalLight != null)
        {
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(gradiant);
            DirectionalLight.transform.localRotation = Quaternion.Euler(Preset.directionalLightAngle.Evaluate(time), 200, 0);//Quaternion.Euler(((time * 360)) - 90, 200, 0);
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        //CmdStartLighting();
    }


    private void OnValidate()
    {
        if (DirectionalLight != null)
            return;
        if (RenderSettings.sun != null)
        {
            DirectionalLight = RenderSettings.sun;
        }
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    DirectionalLight = light;
                    return;
                }
            }
        }

    }
}
