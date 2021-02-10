using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //スペースが押されたらジャンプする
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //クリア・ゲームオーバーになったらリスタート
            SceneManager.LoadScene("GameScene");
        }
    }
}
