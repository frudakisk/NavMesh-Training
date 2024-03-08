using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    private EntityBehaviour entityBehaviour;

    // Start is called before the first frame update
    void Start()
    {
        entityBehaviour = GetComponent<EntityBehaviour>();
        healthSlider.maxValue = entityBehaviour.health;
        healthSlider.value = entityBehaviour.health;
        healthSlider.fillRect.gameObject.SetActive(true);
    }

}
