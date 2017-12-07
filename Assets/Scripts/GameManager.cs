using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public CanvasGroup menuPanel;
    public CanvasGroup gamePanel;
    public CanvasGroup missionSuccessPanel;
    public CanvasGroup missionDefeatPanel;
    public bool isGameStart;
    public CrystalController crystalController;
    public Spawner spawner;
    public float delayShowResultTime = 2f;


    private void Awake(){
        OnBattleFieldLOLost();

    }
    public void OnBattleFieldReady() {
        menuPanel.interactable = true;
    }
    public void OnBattleFieldLOLost(){
        Pause();
        menuPanel.interactable = false;
    }


    public void Pause() {
        Time.timeScale = 0;
        menuPanel.gameObject.SetActive(true);

    }


    public void Resume() {
        Time.timeScale = 1;
        menuPanel.gameObject.SetActive(false);
        if (!isGameStart) {
            StartCoroutine(DoGameFlow());

        }
        

    }
    private IEnumerator DoGameFlow() {
        isGameStart = true;
        gamePanel.gameObject.SetActive(true);
        yield return StartCoroutine(crystalController.Execute());
        spawner.enabled = false;
        yield return new WaitForSeconds(delayShowResultTime);
        if (crystalController.isDead)
        {
            missionDefeatPanel.gameObject.SetActive(true);


        }
        else {
            missionSuccessPanel.gameObject.SetActive(true);


        }
        

    }
    public void Reload() {
        SceneManager.LoadScene(0);

    }
    
}
