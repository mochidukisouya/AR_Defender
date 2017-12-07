using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.Events;


public class BattleField : MonoBehaviour,ITrackableEventHandler {
    public UnityEvent onBattleFieldReady;
    public UnityEvent onBattleFieldLost;
    public float maxLostTime = 1f;
    private float lostTime;

    void Start() {
        TrackableBehaviour trackableBehaviour = GetComponent<TrackableBehaviour>();
        trackableBehaviour.RegisterTrackableEventHandler(this);


    }
    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus){
        if (newStatus == TrackableBehaviour.Status.DETECTED || newStatus == TrackableBehaviour.Status.TRACKED || newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED){
            Debug.Log("READY");
            onBattleFieldReady.Invoke();
            enabled = false;
        }else {
            enabled = true;
            lostTime = 0;
        }

    }
    private void Update(){
        lostTime += Time.unscaledDeltaTime;
        if (lostTime >= maxLostTime) {
            Debug.Log("LOST");
            onBattleFieldLost.Invoke();
            enabled = false;
        }

    }


}
