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
        Debug.Log($"entity health: {entityBehaviour.health}");
        healthSlider.maxValue = entityBehaviour.health;
        healthSlider.value = entityBehaviour.health;
        healthSlider.fillRect.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
