using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashManager : MonoBehaviour
{
    ParticleSystem ps;

    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
    List<ParticleSystem.Particle> exit = new List<ParticleSystem.Particle>();
    List<ParticleSystem.Particle> inside = new List<ParticleSystem.Particle>();

    void OnEnable()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void OnParticleTrigger()
    {
        if(ps == null)
            ps = GetComponent<ParticleSystem>();

        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);

        for (int i = 0; i < numEnter; i++)
            if (Random.Range(0, 2) == 0)
                enter[i] = Plop(enter[i]);

        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);

        int numExit = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Exit, exit);

        for (int i = 0; i < numExit; i++)
            exit[i] = Plop(exit[i]);

        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Exit, exit);

        int numInside = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside);

        for (int i = 0; i < numInside; i++)
            if(Random.Range(0, 2) == 0)
                inside[i] = Plop(inside[i]);

        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside);
    }

    private ParticleSystem.Particle Plop(ParticleSystem.Particle p)
    {
        p.startColor = new Color32(255, 0, 0, 255);
        GameManager.instance.SpawnSplash(transform.TransformPoint(p.position), p.startColor);
        p.remainingLifetime = 0;
        return p;
    }
}
