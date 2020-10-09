using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sc_Gun : MonoBehaviour
{
    Sc_PlayerController player => FindObjectOfType<Sc_PlayerController>();
    Animator anim => GetComponent<Animator>();

    [SerializeField] AudioSource shootSound;
    [SerializeField] AnimationClip clip;
    
    [Header("Shooting")]
    [SerializeField] int damage = 5;
    [SerializeField] float shootRange = 50;
    [SerializeField] GameObject[] shootFX;
    [SerializeField] LayerMask shootLayer;
    [SerializeField] float shootDelay = 0.1f;
    float timer;
    Ray ray;

    [Header("Interactions")]
    public bool detectInteract;
    [SerializeField] bool Auto;
    [SerializeField] LayerMask interactLayer;
    [SerializeField] GameObject useText;
    Sc_Door door;

    [Header("Ammos")]
    [SerializeField] Slider ammoSlider;
    [SerializeField] float recuperationDelay = 0.25f;
    float currentAmmo;
    [SerializeField] float maxAmmo = 100;
    public float CurrentAmmo 
    { 
        get => currentAmmo;

        set
        {
            if (value < 0)
                value = 0;

            if (value > maxAmmo)
                value = maxAmmo;

            currentAmmo = value;
        }
    }

    private void Start()
    {
        CurrentAmmo = maxAmmo;
    }

    void Shooting()
    {
        timer += Time.deltaTime;
        if (Auto)
        {
            bool holdingFire = Input.GetButton("Fire1") && CurrentAmmo > 0 && !player.Health.isDead;
            anim.SetBool("HoldingFire", holdingFire);

            if (holdingFire && CurrentAmmo > 0)
            {
                if (!shootSound.isPlaying)
                    shootSound.Play();

                if (timer > shootDelay)
                {
                    Shoot();
                    timer = 0;
                }               
            }
            else
            {
                shootSound.Stop();
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
        detectInteract = Physics.Raycast(ray, out RaycastHit hit, 5, interactLayer);
        if (detectInteract)
        {
            Sc_Door obj = hit.collider.GetComponent<Sc_Door>();            
            if (obj)
            {
                if (Input.GetButtonDown("Interact") && obj.canActivate)
                    obj.Activate(player);

                door = obj;
                useText.SetActive(obj && obj.canActivate);
                door.outline.enabled = obj && obj.canActivate;
            }
            else
            {
                useText.SetActive(false);

                if (door != null)
                    door.outline.enabled = false;
            }
        }
        else
        {
            useText.SetActive(false);

            if (door != null)
                door.outline.enabled = false;
        }
    }

    void Update()
    {
        ammoSlider.value = CurrentAmmo;
        ammoSlider.maxValue = maxAmmo;
        if (!Input.GetButton("Fire1") && Time.timeScale > 0)
            CurrentAmmo += recuperationDelay;

        ray = player.viewCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector2 vel = new Vector2(player.rb.velocity.x, player.rb.velocity.z).normalized;
        anim.SetFloat("Velocity", vel.sqrMagnitude);

        if (player.Health.isDead)
            return;

        Shooting();
        Interact();
    }
}
