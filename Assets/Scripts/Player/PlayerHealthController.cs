using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerHealthController : MonoBehaviour
{
    public float permDamage = 0;
    public float maxHealth = 100;
    public float health = 100;
    public float defense = 30;
    public float pulseSpeed = 0.1f;
    public float pulseStrength = 0.5f;
    private Volume volume;
    private ColorAdjustments colorGrading;
    private FilmGrain grain;
    private Vignette vignette;

    private PlayerController player;

    public UnityEvent deathEvent;

    bool dead = false;

    // Start is called before the first frame update
    void Start()
    {
        volume = GameObject.FindGameObjectWithTag("Post Processer").GetComponent<Volume>();
        volume.profile.TryGet(out colorGrading);
        volume.profile.TryGet(out grain);
        volume.profile.TryGet(out vignette);

        player = gameObject.GetComponent<PlayerController>();
    }

    private float timer = 0;
    // Update is called once per frame
    void Update()
    {
        float pulse = 1;
        if(health < maxHealth)
        {
            timer += Time.deltaTime;
            if (timer >= Mathf.PI*6) timer = Mathf.PI * 6;
            pulse = Mathf.Pow(Mathf.Sin(timer * pulseSpeed),2) * Mathf.Pow(Mathf.Sin(Mathf.PI * timer * pulseSpeed),2) * pulseStrength;
        }
        if(health <= maxHealth)
        {
            float healthPercantage = (health - 1) / maxHealth;
            player.SetBonusSpeed(healthPercantage);
            float rev = 1 - healthPercantage;
            
            float halfRev;
            float halfRev2;
            if (healthPercantage > 0.5)
            {
                halfRev = rev * 2;
                halfRev2 = 0;
            }
            else
            {
                halfRev = 1;
                halfRev2 = (rev - 0.5f) * 2;
                pulse -= pulse * halfRev2 * 0.5f;
            }

            pulse = 1 - pulse;

            Color redColor = new(1.0f, 1 - 0.5f * halfRev * pulse, 1 - 0.5f * halfRev * pulse);
            colorGrading.postExposure.value = -healthPercantage * pulse;
            colorGrading.colorFilter.value = redColor;
            colorGrading.saturation.value = (-10 * halfRev - 40 * halfRev2) * pulse;
            
            grain.intensity.value = (0.4f * halfRev + 0.6f * halfRev2) * pulse;
            grain.response.value = (1 - 0.6f * halfRev - 0.15f * halfRev2) * pulse;

            vignette.intensity.value = (0.4f * halfRev + 0.6f * halfRev2) * pulse;
            vignette.smoothness.value = (0.56f * halfRev + 0.44f * halfRev2) * pulse;
        }
    }

    public void Heal(float health)
    {
        this.health += health;
        if (this.health > maxHealth) this.health = maxHealth;
    }

    public void Damage(float attack)
    {
        if (dead) return;
        health -= CalcDamage(attack);
        if (health <= 0) {
            health = 0;
            dead = true;
            deathEvent.Invoke();
        }
    }

    float CalcDamage(float attack)
    {
        float f = Mathf.Pow(attack / defense, 2);
        float h = attack * ((f <= 1) ? f : 1);
        h -= defense / attack;
        return h;
    }
}
