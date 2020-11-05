using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;
using TMPro;

public class DissolveEvent : MonoBehaviour
{
    [SerializeField] private float delayTime;
    [SerializeField] private float turnSpeed;
    [SerializeField] private string chestFullText;
    [SerializeField] private string holoFullText;
    [SerializeField] private TextMeshProUGUI UI;
    [SerializeField] private Transform LookAtCrystal;
    [SerializeField] private Transform LookAtChest;
    [SerializeField] private Transform LookAtHologram;

    [SerializeField] private CameraShake camShake;
    [SerializeField] private Dissolve dissolveChest;
    [SerializeField] private DissolveBeam dissolveBeam;
    [SerializeField] private ChangeObjectMaterial changeMats;

    private string currentText;
    private bool playedEvent = false;
    private bool ChestEventReady = false;
    private bool PlayerInTrigger = false;

    private FirstPersonController fpscontroller;

    private void Start()
    {
        fpscontroller = GameObject.Find("FPSController").GetComponent<FirstPersonController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!playedEvent)
            {
                playedEvent = true;
                PlayCrystalEvent();
            }

            PlayerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            PlayerInTrigger = false;
    }

    public void PlayCrystalEvent()
    {
        StartCoroutine(DisplayCrystalEvent());
        fpscontroller.enabled = false;
    }

    IEnumerator DisplayCrystalEvent()
    {
        Vector3 direction;
        Quaternion lookAt;
        Vector3 target;

        // look at the crystal
        target = LookAtCrystal.position;
        direction = (target - Camera.main.transform.position).normalized;
        lookAt = Quaternion.LookRotation(direction);

        while (Quaternion.Angle(Camera.main.transform.rotation, lookAt) > 0.01f)
        {
            Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, lookAt, turnSpeed * Time.deltaTime); //Quaternion.RotateTowards(Camera.main.transform.rotation, lookAt, Time.deltaTime * turnSpeed);
            yield return new WaitForEndOfFrame();
        }
        // turn on the crystal
        dissolveBeam.TurnOnCrystal();

        yield return new WaitForSeconds(1);

        // turn on beam
        dissolveBeam.TurnOnLineRenderer();

        // look at chest dissolving in
        target = LookAtChest.position;
        direction = (target - Camera.main.transform.position).normalized;
        lookAt = Quaternion.LookRotation(direction);

        while (Quaternion.Angle(Camera.main.transform.rotation, lookAt) > 0.01f)
        {
            Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, lookAt, turnSpeed * Time.deltaTime);  // Quaternion.RotateTowards(Camera.main.transform.rotation, lookAt, Time.deltaTime * turnSpeed);
            yield return new WaitForEndOfFrame();
        }

        // dissolve in chest
        dissolveChest.DissolveIn();

        yield return new WaitUntil(() => dissolveChest.dissolveFinished);

        dissolveBeam.TurnOffLineRenderer();

        // write text
        for (int i = 0; i < chestFullText.Length; i++)
        {
            currentText = chestFullText.Substring(0, i);
            UI.text = currentText;
            yield return new WaitForSeconds(delayTime);
        }

        // return access to player
        yield return new WaitForSeconds(1);

        UI.text = "";
        currentText = "";
        ChestEventReady = true;

        fpscontroller.gameObject.transform.LookAt(new Vector3(target.x, fpscontroller.gameObject.transform.position.y, target.z));
        Camera.main.transform.LookAt(target);
        fpscontroller.InitMouseLook();

        fpscontroller.enabled = true;
    }

    public void PlayerChestEvent()
    {
        StartCoroutine(DisplayChestEvent());
        fpscontroller.enabled = false;
    }

    IEnumerator DisplayChestEvent()
    {
        Vector3 target = LookAtChest.position;
        Vector3 direction = (target - Camera.main.transform.position).normalized;
        Quaternion lookAt = Quaternion.LookRotation(direction);

        // look at the chest
        while (Quaternion.Angle(Camera.main.transform.rotation, lookAt) > 0.01f)
        {
            Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, lookAt, turnSpeed * Time.deltaTime);  //Quaternion.RotateTowards(Camera.main.transform.rotation, lookAt, Time.deltaTime * turnSpeed);
            yield return new WaitForEndOfFrame();
        }

        // start dissolving chest out
        changeMats.SetHoloMaterials();
        changeMats.SetbeamMaterials();
        changeMats.SetCrystalMaterials();
        changeMats.SetDissolveMaterials();

        dissolveBeam.TurnOnLineRenderer();
        dissolveChest.DissolveOut();

        yield return new WaitUntil(() => dissolveChest.dissolveFinished);

        // turn off beam
        dissolveBeam.TurnOffLineRenderer();

        // look at the hologram
        target = LookAtHologram.position + Vector3.up;
        direction = (target - Camera.main.transform.position).normalized;
        lookAt = Quaternion.LookRotation(direction);

        while (Quaternion.Angle(Camera.main.transform.rotation, lookAt) > 0.01f)
        {
            Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, lookAt, turnSpeed * Time.deltaTime);//Quaternion.RotateTowards(Camera.main.transform.rotation, lookAt, Time.deltaTime * turnSpeed);
            yield return new WaitForEndOfFrame();
        }

        // hologram says text
        for (int i = 0; i < holoFullText.Length; i++)
        {
            currentText = holoFullText.Substring(0, i);
            UI.text = currentText;
            yield return new WaitForSeconds(delayTime);
        }

        yield return new WaitForSeconds(2);

        // return access to player
        UI.text = "";

        fpscontroller.gameObject.transform.LookAt(new Vector3(target.x, fpscontroller.gameObject.transform.position.y, target.z));
        Camera.main.transform.LookAt(target);
        fpscontroller.InitMouseLook();

        fpscontroller.enabled = true;

        PortalExit.instance.canExit = true;

        StartCoroutine(camShake.Shake(30f, .05f));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && ChestEventReady && PlayerInTrigger)
        {
            ChestEventReady = false;
            PlayerChestEvent();
        }
    }
}
