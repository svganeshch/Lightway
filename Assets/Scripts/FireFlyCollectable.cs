using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlyCollectable : MonoBehaviour, ICollectable
{
    ParticleSystem particle;
    WindZone wind;

    private void Start()
    {
        particle = GetComponentInChildren<ParticleSystem>();
        wind = GetComponentInChildren<WindZone>();
    }

    public void OnCollect(Transform player)
    {
        //wind.mode = WindZoneMode.Directional;
        //wind.transform.LookAt(player.position);

        particle.gameObject.transform.parent = player.transform;
        particle.gameObject.transform.SetLocalPositionAndRotation(new Vector3(0, 1.5f, 0), Quaternion.identity);

        Debug.Log("collected");
    }
}
