using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhenomTools
{
    public class GracefulParticleDestroy : MonoBehaviour
    {
        public void Detach()
        {
            transform.SetParent(null);
            float longestLife = 0f;

            foreach(ParticleSystem fx in GetComponentsInChildren<ParticleSystem>())
            {
                if (fx.main.startLifetime.constantMax > longestLife)
                    longestLife = fx.main.startLifetime.constantMax;

                var em = fx.emission;
                em.enabled = false;
            }

            gameObject.DestroyDelayed(longestLife + 1f);
        }
    }
}
