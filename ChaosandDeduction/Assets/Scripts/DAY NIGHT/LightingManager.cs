using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[ExecuteAlways]
public class LightingManager : NetworkBehaviour
{

    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset Preset;

    [SyncVar]
    [SerializeField, Range(0, 180)] private float TimeOfDay;
    private float StartTime;

    private void Update()
    {


        if (!isServer)
            return;
        if (Application.isPlaying)
        {
            if (TimeOfDay < 180)
                TimeOfDay += Time.deltaTime;
            //Debug.Log(TimeOfDay);
            //TimeOfDay %= 180;
            RpcUpdateLighting(TimeOfDay / 180f);

        }
        else
        {
            RpcUpdateLighting(TimeOfDay / 180f);

        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        CmdStartLighting();
    }
    [Command(requiresAuthority = false)]
    public void CmdStartLighting()
    {
        RpcUpdateLighting(TimeOfDay);
    }

    [ClientRpc]
    private void RpcUpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);

        if (DirectionalLight != null)
        {
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);
            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 100f) + 90f, 200, 0));
        }
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
