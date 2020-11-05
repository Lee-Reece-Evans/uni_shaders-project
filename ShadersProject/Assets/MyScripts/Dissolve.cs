using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    [SerializeField] private Material redDissolveMat;

    private Renderer ren;
    private Material mat;

    public float dissolvetime;
    public float disolveInSpeed = 1.5f;
    public float disolveOutSpeed = 1.5f;

    public bool dissolveFinished = false;

    // Use this for initialization
    void Start()
    {
        ren = GetComponent<Renderer>();
        mat = ren.material;
    }

    public void DissolveOut()
    {
        dissolveFinished = false;
        mat = ren.material;
        StartCoroutine(PlayDissolveOut());
    }

    IEnumerator PlayDissolveOut()
    {
        while (mat.GetFloat("_DissolveAmount") < 0.8f)
        {
            dissolvetime += Time.deltaTime * disolveOutSpeed;
            mat.SetFloat("_DissolveAmount", dissolvetime);
            yield return new WaitForEndOfFrame();
        }

        if (mat.GetFloat("_DissolveAmount") > 0.8f)
            mat.SetFloat("_DissolveAmount", 0.8f);

        dissolvetime = 0.8f;
        dissolveFinished = true;
    }
    public void DissolveIn()
    {
        dissolveFinished = false;
        mat = ren.material;
        StartCoroutine(PlayDissolveIn());
    }

    IEnumerator PlayDissolveIn()
    {
        while (mat.GetFloat("_DissolveAmount") > 0f)
        {
            dissolvetime -= Time.deltaTime * disolveOutSpeed;
            mat.SetFloat("_DissolveAmount", dissolvetime);
            yield return new WaitForEndOfFrame();
        }

        if (mat.GetFloat("_DissolveAmount") < 0f)
            mat.SetFloat("_DissolveAmount", 0);

        dissolvetime = 0f;
        dissolveFinished = true;
    }
}
