using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Sc_Digicode : Sc_Interactable
{
    [Header("Digicode")]
    [SerializeField] Sc_Door door;
    [SerializeField] TextMeshPro myText;
    [SerializeField] GameObject diode;
    [SerializeField] string correctCode = "0211";
    [SerializeField] char[] currentCode = new char[4] { '0', '0', '0', '0', };
    public bool maxSize;
    int _index;
    Material _lightMat;
    Sc_Digicode_Button_Slot[] textes;
    bool isChecking;
    Animator textAnim;

    private void Awake()
    {
        ResetCode();
        diode.SetActive(false);
        _lightMat = diode.GetComponent<MeshRenderer>().material;
        myText.text = currentCode.ArrayToString();
        textAnim = myText.GetComponent<Animator>();
        textes = GetComponentsInChildren<Sc_Digicode_Button_Slot>();

        for (int i = 0; i < textes.Length; i++)
        {
            string s = i.ToString();
            TextMeshPro slotText = textes[i].GetComponentInChildren<TextMeshPro>();
            slotText.text = s;
        }
    }

    public void AddCode(string s)
    {
        if (canActivate)
        {
            char[] oui = s.ToCharArray();
            currentCode[_index] = oui[0];
            _index++;
        }
    }

    public void ResetCode()
    {
        currentCode = new char[4] {'0', '0', '0', '0'};
        _index = 0;
        myText.color = Color.red;
        myText.material.SetColor("_EmissionColor", Color.red);
    }

    public void CheckCode()
    {
        bool good = currentCode.ArrayToString().Equals(correctCode);
        Color newColor;
        newColor = good ? Color.green : Color.red;
        myText.color = newColor;
        myText.material.SetColor("_EmissionColor", newColor);
        _lightMat.SetColor("_EmissionColor", newColor);
        Open(0.1f);
    }

    public override void Open(float delay)
    {
        textAnim.SetTrigger("TryCode");
        StartCoroutine(CheckAnim(delay));
        activated = true;
    }

    IEnumerator CheckAnim(float _delay)
    {
        isChecking = true;

        for (int i = 0; i < 4; i++)
        {
            yield return new WaitForSeconds(_delay);
            diode.SetActive(!diode.activeSelf);
        }

        ResetCode();
        isChecking = false;

        if (activated && door != null)
            door.Open(0);
    }

    private void Update()
    {
        maxSize = !isChecking && !textAnim.GetCurrentAnimatorStateInfo(0).IsName("DigicodeText_win") && !activated;
        canActivate = _index < correctCode.Length && maxSize;
        myText.text = currentCode.ArrayToString();
        if (_index > correctCode.Length)
        {
            ResetCode();
        }
    }
}
