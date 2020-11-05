using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HologramBeam : MonoBehaviour
{
    private LineRenderer lr;
    [SerializeField] private Transform LinePoint;
    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, LinePoint.position);
    }
}
