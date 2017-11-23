using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class AttackableBehavior : MonoBehaviour {
    public int maxHealth = 100;
    public int currentHealth;
    public float smooth = 5f;
    public bool autoDestroy;
    public float delayDestroyTime = 1.5f;
    public Slider healthSlider;
    public UnityEvent onDead;
    public UnityEvent onHurt;

    private void OnEnable()
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = healthSlider.maxValue;
        currentHealth = maxHealth;
    }

    public void Hurt(int damage)
    {
        if (currentHealth <= 0)
            return;
        currentHealth -= damage;
        onHurt.Invoke();
        if(currentHealth <= 0)
        {
            onDead.Invoke();
            if (autoDestroy)
                Destroy(gameObject, delayDestroyTime);
        }
    }

    private void Update()
    {
        healthSlider.value = Mathf.Lerp(healthSlider.value, currentHealth, Time.deltaTime * smooth);
    }
}
