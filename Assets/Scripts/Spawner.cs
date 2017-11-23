using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public Transform[] spawnPoints;
    public GameObject spawnObj;
    public float delayStartTime = 3f;
    public float intervalTime = 3f;

    private void OnEnable() {
        StartCoroutine("Excute");
    }

    private void OnDisable(){
        StopCoroutine("Excute");

    }

    private IEnumerator Excute(){
        yield return new WaitForSeconds(delayStartTime);
        while (enabled) {
            int pointIndex = Random.Range(0, spawnPoints.Length - 1);
            Instantiate(spawnObj, spawnPoints[pointIndex].position, spawnPoints[pointIndex].rotation);
            yield return new WaitForSeconds(intervalTime);
            
        }



    }

}
