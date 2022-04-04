using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SensitivityChangerUI : MonoBehaviour
{
    Slider sens;
    GameObject player;
    public TMP_InputField input;

    string textValue;

    // Start is called before the first frame update
    void Start()
    {
        sens = GetComponent<Slider>();
        player = GameObject.FindWithTag("Player");
        input.onEndEdit.AddListener(delegate
        {
            OnEnterValue(input.text);
        });
        sens.onValueChanged.AddListener(delegate
        {
            player.BroadcastMessage("SetSens", new Vector2(sens.value, sens.value));
            UpdateSens();
        });
        input.text = "1";
    }

    // Update is called once per frame
    void Update()
    {
        // input.text = textValue;
    }

    private void LateUpdate()
    {
        //UpdateSens();
    }

    void UpdateSens()
    {
        textValue = sens.value.ToString();
        input.text = textValue;
    }

    void OnEnterValue(string value)
    {
        try
        {
            float temp = float.Parse(value);
            if(temp > 10)
            {
                temp = 10;
                value = temp.ToString();
            }
            else if (temp < 0.01)
            {
                temp = 0.01f;
                value = temp.ToString();
            }
            sens.value = temp;
            UpdateSens();
        }

        catch
        {
            UpdateSens();
        }
    }

}
