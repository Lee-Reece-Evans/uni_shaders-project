using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeObjectMaterial : MonoBehaviour
{
    [SerializeField] private Material crystalMat;
    [SerializeField] private Material HoloMat;
    [SerializeField] private Material DissolveMat;

    [SerializeField] private Renderer[] crystals;
    [SerializeField] private LineRenderer[] lineRenderers;

    [SerializeField] private Renderer dissolve;
    [SerializeField] private Renderer water;
    [SerializeField] private Renderer[] holograms;

    public void SetCrystalMaterials()
    {
        foreach (Renderer ren in crystals)
        {
            ren.material = crystalMat;
        }
    }

    public void SetbeamMaterials()
    {
        foreach (Renderer ren in lineRenderers)
        {
            ren.material = HoloMat;
        }
    }

    public void SetHoloMaterials()
    {
        foreach (Renderer ren in holograms)
        {
            ren.material = HoloMat;
        }
    }

    public void SetDissolveMaterials()
    {
        dissolve.material = DissolveMat;
    }
}
