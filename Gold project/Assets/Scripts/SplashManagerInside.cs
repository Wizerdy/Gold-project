using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashManagerInside : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] private float triggerChance;

    public Vector2Int number;
    public Vector2Int speed;
    public Vector2 size;
    public Color color;

    ParticleSystem ps;

    List<ParticleSystem.Particle> exit = new List<ParticleSystem.Particle>();
    List<ParticleSystem.Particle> inside = new List<ParticleSystem.Particle>();

    void OnEnable()
    {
        ps = GetComponent<ParticleSystem>();
    }

    public void SetParticleSystem(int numberMin, int numberMax, int speedMin, int speedMax, float sizeMin, float sizeMax, Color color)
    {
        ParticleSystem.MainModule main = ps.main;
        ps.emission.SetBurst(0, new ParticleSystem.Burst(0f, (short)numberMin, (short)numberMax));
        main.startSpeed = new ParticleSystem.MinMaxCurve(speedMin, speedMax);
        main.startSize = new ParticleSystem.MinMaxCurve(sizeMin, sizeMax);
        main.startColor = color;
    }

    void OnParticleTrigger()
    {
        if(ps == null)
            ps = GetComponent<ParticleSystem>();

        int numExit = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Exit, exit);
        int numInside = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside);

        for (int i = 0; i < numExit; i++)
            exit[i] = Plop(exit[i]);

        for (int i = 0; i < numInside; i++)
            if(Random.Range(0, 100) < triggerChance * 100)
                inside[i] = Plop(inside[i]);

        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Exit, exit);
        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside);
    }

    private ParticleSystem.Particle Plop(ParticleSystem.Particle p)
    {
        //p.startColor = new Color32(255, 0, 0, 255);
        //p.startColor = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 255);
        GameManager.instance.SpawnSplash(transform.TransformPoint(p.position), p.startColor);
        p.remainingLifetime = 0;
        return p;
    }
}
