using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToPlayer : MonoBehaviour
{
    [SerializeField] private Transform player;

    void Update()
    {
        Vector3 LookAtPlayer = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(LookAtPlayer);
    }
}
