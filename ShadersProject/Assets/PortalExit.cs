using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalExit : MonoBehaviour
{
    public static PortalExit instance;

    [SerializeField] private ScreenFade fade;
    public bool canExit = false;

    private void Awake()
    {
        instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canExit)
        {
            fade.FadeOut();
            canExit = false;
        }
    }
}
