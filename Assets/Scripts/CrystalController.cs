using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrystalController : MonoBehaviour
{

    public float crystalChargeTime = 30f;
    private float curChargeTime;
    public Slider chargeSlider;
    public Text progressLabel;
    public ParticleSystem chargeEffect;
    public ParticleSystem chargeCompleteEffect;
    private Collider mCollider;

    private void Awake(){
        mCollider = GetComponent<Collider>();
        chargeSlider.maxValue = crystalChargeTime;
        StartCoroutine(Execute());
    }
    IEnumerator Execute()
    {
        chargeEffect.Play();
        while (enabled)
        {
            chargeSlider.value += Time.deltaTime;
            progressLabel.text = Mathf.FloorToInt(chargeSlider.normalizedValue * 100) + "%";
            if (chargeSlider.value == chargeSlider.maxValue)
            {
                chargeEffect.Stop();
                chargeCompleteEffect.Play();
                foreach (RhinoController rhino in GameObject.FindObjectsOfType<RhinoController>())
                {
                    rhino.GetComponent<AttackableBehavior>().Hurt(int.MaxValue);

                }
                enabled = false;
            }
            yield return null;
        }
    }
    public void OnDead() {
        chargeEffect.Stop();
        enabled = false;
        mCollider.enabled = false;


    }
}
