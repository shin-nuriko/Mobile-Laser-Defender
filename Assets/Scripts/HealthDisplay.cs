using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] Player player;
    Slider healthDisplay;

    // Start is called before the first frame update
    void Start()
    {
        player  = FindObjectOfType<Player>();
        healthDisplay = GetComponent<Slider>();
        healthDisplay.maxValue = player.GetMaxHealth();
    }

    // Update is called once per frame
    void Update()
    {
        healthDisplay.value = player.GetHealth();
    }
}
