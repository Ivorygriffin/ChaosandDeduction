using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[ExecuteAlways]
public class LightingManager : NetworkBehaviour
{

    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset Preset;
    public Timer timer;

    //[SyncVar]
    //[SerializeField, Range(0, 180)] private float TimeOfDay;
    //private float StartTime;

    private void Update()
    {
        float TimeOfDay = timer.timeRemaining / Timer.cycleLength;

        float magnitude = TimeOfDay / 1;
        TimeOfDay %= 1;

        if (magnitude % 2 == 0)
            TimeOfDay = 1 - TimeOfDay;

        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(TimeOfDay);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(TimeOfDay);


        if (DirectionalLight != null)
        {
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(TimeOfDay);

            DirectionalLight.transform.localRotation = Quaternion.Euler(((TimeOfDay * 360) / 4) + 90, 200, 0);
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
