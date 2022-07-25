using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SinSlider : MonoBehaviour
{
    public Slider Slider;

    [Range(1, 10)]
    public float Senstivity;

    public float Acceleration;

    private void Awake()
    {
        Senstivity = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        Acceleration += Time.deltaTime;
        Acceleration %= (180f / Senstivity);

        Slider.value = Mathf.Sin(Acceleration * Senstivity * Mathf.Deg2Rad);
    }
}
