using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
    // Stage 선택 씬으로 이동
    public void ChangeStageSelectScene(){
        SceneManager.LoadScene("StageSelectScene");
    }

    // Setting 씬으로 이동
    public void ChangeStageSettingScene(){
        SceneManager.LoadScene("SettingScene");
    }

    public void ChangeStageOneScene(){
        SceneManager.LoadScene("SampleScene");
    }
}
