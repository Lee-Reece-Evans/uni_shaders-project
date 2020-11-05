using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveBeam : MonoBehaviour
{
    private LineRenderer lr;

    [SerializeField] private Material redHoloMat;
    [SerializeField] private Transform LinePoint;
    [SerializeField] private Renderer crystal;
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private GameObject sphere;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        particles = GetComponent<ParticleSystem>();

        TurnOffLineRenderer();

        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, LinePoint.position);

        TurnOffCrystal();
    }

    public void TurnOnCrystal()
    {
        crystal.material.EnableKeyword("_EMISSION");
    }

    public void TurnOnLineRenderer()
    {
        lr.enabled = true;
        particles.Play();
        sphere.SetActive(true);
    }
    public void TurnOffCrystal()
    {
        crystal.material.DisableKeyword("_EMISSION");
    }

    public void TurnOffLineRenderer()
    {
        lr.enabled = false;
        particles.Stop();
        sphere.SetActive(false);
    }
}
