using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;
using TMPro;

public class HollogramEvent : MonoBehaviour
{
    [SerializeField] private float delayTime;
    [SerializeField] private float turnSpeed;
    [SerializeField] private string fullText;
    [SerializeField] private TextMeshProUGUI UI;
    [SerializeField] private Transform TargetToLookat;

    private string currentText;
    private bool hasShown = false;
    private FirstPersonController fpscontroller;

    private void Start()
    {
        fpscontroller = GameObject.Find("FPSController").GetComponent<FirstPersonController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasShown)
        {
            hasShown = true;
            DisplayText();
        }
    }

    public void DisplayText()
    {
        StartCoroutine(DisplayHologramOnscreen());
        fpscontroller.enabled = false;
    }

    IEnumerator DisplayHologramOnscreen ()
    {
        Vector3 target = TargetToLookat.position + Vector3.up;
        Vector3 direction = (target - Camera.main.transform.position).normalized;
        Quaternion lookAt = Quaternion.LookRotation(direction);

        while (Quaternion.Angle(Camera.main.transform.rotation, lookAt) > 0.01f)
        {
            Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, lookAt, turnSpeed * Time.deltaTime); // Quaternion.RotateTowards(Camera.main.transform.rotation, lookAt, turnSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        for (int i = 0; i < fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i);
            UI.text = currentText;
            yield return new WaitForSeconds(delayTime);
        }

        yield return new WaitForSeconds(2);
        UI.text = "";


        fpscontroller.gameObject.transform.LookAt(new Vector3(target.x, fpscontroller.gameObject.transform.position.y, target.z));
        Camera.main.transform.LookAt(target);
        fpscontroller.InitMouseLook();

        fpscontroller.enabled = true;
    }
}
