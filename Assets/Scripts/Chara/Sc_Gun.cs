using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_Gun : MonoBehaviour
{
    Sc_PlayerController player => FindObjectOfType<Sc_PlayerController>();
    Animator anim => GetComponent<Animator>();

    AudioSource sound => GetComponentInChildren<AudioSource>();
    [SerializeField] int damage = 5;
    [SerializeField] float shootRange = 50;
    [SerializeField] GameObject[] shootFX;
    [SerializeField] AnimationClip clip;
    [SerializeField] LayerMask shootLayer;
    [SerializeField] float shootDelay = 0.1f;
    float timer;
    Ray ray;

    public bool detectInteract;
    [SerializeField] bool Auto;
    [SerializeField] LayerMask interactLayer;
    [SerializeField] GameObject useText;

    private int currentAmmo;
    public int CurrentAmmo 
    { 
        get => currentAmmo;

        set
        {
            if (value < 0)
                value = 0;

            currentAmmo = value;
        }
    }

    void Shooting()
    {
        timer += Time.deltaTime;
        if (Auto)
        {
            bool holdingFire = Input.GetButton("Fire1");
            anim.SetBool("HoldingFire", holdingFire);

            if (holdingFire)
            {
                if (!sound.isPlaying)
                    sound.Play();

                if (timer > shootDelay)
                {
                    Shoot();
                    timer = 0;
                }               
            }
            else
            {
                sound.Stop();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1") && timer > shootDelay)
            {
                anim.SetTrigger("Shoot");
                timer = 0;
            }
        }
    }

    public void Shoot()
    {
        bool find = Physics.Raycast(ray, out RaycastHit hit, shootRange, shootLayer);
        CurrentAmmo--;
        if (find)
        {
            Vector3 impactPos = hit.point + hit.normal;
            Sc_Character chara = hit.collider.GetComponentInParent<Sc_Character>();
            if (chara)
            {
                Instantiate(shootFX[1], impactPos, Quaternion.LookRotation(hit.normal));

                if (!chara.Health.isDead)
                {
                    if (Auto)
                    {
                        chara.Hurt(damage);
                    }
                    else
                    {
                        chara.Hurt(damage * 3);
                    }
                }
            }
            else
            {
                Instantiate(shootFX[0], impactPos, Quaternion.LookRotation(hit.normal));
            }
        }
    }

    public void Interact()
    {
        detectInteract = Physics.Raycast(ray, out RaycastHit hit, 2, interactLayer);
        useText.SetActive(detectInteract);
        if (Input.GetButtonDown("Interact") && detectInteract)
        {
            Sc_Interactable obj = hit.collider.GetComponent<Sc_Interactable>();
            if (obj && obj.canActivate)
            {
                obj.Activate(player);
            }
        }
    }

    void Update()
    {
        ray = player.viewCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector2 vel = new Vector2(player.rb.velocity.x, player.rb.velocity.z).normalized;
        anim.SetFloat("Velocity", vel.sqrMagnitude);

        if (player.Health.isDead)
            return;

        Shooting();
        Interact();
    }
}
