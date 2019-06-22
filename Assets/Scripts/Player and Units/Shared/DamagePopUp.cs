using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopUp : MonoBehaviour
{
    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
        textColor = textMesh.color;
        disappearTimer = 1f;
    }

    public void Setup(float damangeAmount)
    {
        textMesh.SetText(damangeAmount.ToString());
    }

    public void Update()
    {

        transform.LookAt(-Camera.main.transform.position);
        float moveYspeed = 2.5f;
        transform.position += new Vector3(0, moveYspeed) * Time.deltaTime;
        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            //starts disappearing then destroyed
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
