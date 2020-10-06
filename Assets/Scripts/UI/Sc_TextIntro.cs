using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Sc_TextIntro : MonoBehaviour
{
    [SerializeField] float printSpeed;
    [SerializeField] float printDelay;

    [TextArea]
    [SerializeField] string[] lines;
    string currentLine;
    TextMeshPro txtMesh => GetComponent<TextMeshPro>();

    private void Start()
    {
        StartCoroutine(PrintLine());
    }

    IEnumerator PrintLine()
    {
        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[i].Length + 1; j++)
            {
                currentLine = lines[i].Substring(0, j);
                yield return new WaitForSeconds(printSpeed);
                txtMesh.text = currentLine;
            }

            yield return new WaitForSeconds(printDelay);
        }

        StartCoroutine(PrintLine());
    }
}
