using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_Door : Sc_Interactable
{
    Sc_GameManager manager => FindObjectOfType<Sc_GameManager>();
    Animator anim => GetComponent<Animator>();
    Light myLight => GetComponentInChildren<Light>();

    public Sc_DoorSpawn destination;
    [SerializeField] MeshRenderer doorMesh;
    [SerializeField] Material lockMat, openMat;

    private void Awake()
    {
        outline.enabled = false;
        lockMat = doorMesh.material;
    }

    public override void Activate(Sc_Character chara)
    {
        base.Activate(chara);
        manager.IncreaseIndex();
        manager.SwitchRooms();
        chara.transform.position = destination.spawnPos.position;
        chara.transform.rotation = Quaternion.Euler(new Vector3(0, destination.spawnPos.rotation.eulerAngles.y, 0));

        foreach (Sc_LevelManager level in manager.levels)
        {
            level.LevelReset();
        }
    }

    public override void ResetMachine()
    {
        base.ResetMachine();
        doorMesh.material = lockMat;
        myLight.color = Color.red;
        myLight.GetComponent<Animator>().SetBool("Emergency", true);
    }

    public override void Open(float delay)
    {
        base.Open(delay);
        doorMesh.material = openMat;
        myLight.intensity = 4;
        myLight.GetComponent<Animator>().SetBool("Emergency", false);
        myLight.color = Color.green;
        StartCoroutine(LaunchOpen(delay));
    }

    public void SwitchDoor()
    {
        canActivate = true;
    }

    IEnumerator LaunchOpen(float delay)
    {
        yield return new WaitForSeconds(delay);
        anim.SetBool("CanActivate", canActivate);
        anim.SetTrigger("Open");
    }
}
