using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    //制限時間
    private float gameTime = 100;
    //制限時間のテキスト
    private GameObject TimeText;
    //ゲーム終了のテキスト
    private GameObject GameEndText;
    //ゲーム終了メッセージの値
    private string GameEndValue;
    //ゲーム終了後の操作テキスト
    private GameObject GameEndSubText;

    //音関連
    //audio source取得用
    private AudioSource audioSource;
    //BGMの元ネタセット
    public AudioClip backSound;
    //発見音の元ネタセット
    public AudioClip discoverySound;
    //クリアの元ネタセット
    public AudioClip clearSound;
    //ゲーム終了音の再生確認
    private bool soundCheck = true;


    // Start is called before the first frame update
    void Start()
    {
        //ゲーム終了textのオブジェクトを取得
        GameEndText = GameObject.Find("GameEndText");

        //ゲーム終了textのオブジェクトを取得
        GameEndSubText = GameObject.Find("GameEndSubText");
        GameEndSubText.SetActive(false);

        //制限時間textのオブジェクトを取得
        TimeText = GameObject.Find("TimeText");

        //サウンドコンポネント取得
        audioSource = GetComponent<AudioSource>();
        //BGMの音量セット
        audioSource.volume = 0.1f;
        //BGMのサウンドセット
        audioSource.clip = backSound;
        //BGMのループ設定
        audioSource.loop = true;
        //再生
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        //ゲーム終了メッセージの値がなければ操作可能
        GameEndValue = GameEndText.GetComponent<Text>().text;
        if (GameEndValue == "")
        {
            //1秒に1ずつ減らしていく
            gameTime -= Time.deltaTime;
            //制限時間textに秒数表示
            TimeText.GetComponent<Text>().text = "残り時間　" + ((int)gameTime);

            if (((int)gameTime) == 0)
            {
                GameEndText.GetComponent<Text>().text = "GAME OVER";
            }
        }
        else if (GameEndValue == "CLEAR")
        {
            if (soundCheck)
            {
                //現在再生の音を止める
                audioSource.Stop();
                //BGMの音量セット
                audioSource.volume = 1f;
                //発見のサウンドセット
                audioSource.clip = clearSound;
                //ループ設定解除
                audioSource.loop = false;
                //再生
                audioSource.Play();
                //ゲーム終了音が再生確認
                soundCheck = false;
            }

            //スペースボタンでリスタート
            RestartScene();
        }
        else if (GameEndValue == "GAME OVER")
        {
            if (soundCheck)
            {
                //現在再生の音を止める
                audioSource.Stop();
                //BGMの音量セット
                audioSource.volume = 1f;
                //発見のサウンドセット
                audioSource.clip = discoverySound;
                //ループ設定解除
                audioSource.loop = false;
                //再生
                audioSource.Play();
                //ゲーム終了音が再生確認
                soundCheck = false;
            }

            //スペースボタンでリスタート
            RestartScene();
        }
    }

    //クリア・ゲームオーバーでリスタート
    private void RestartScene()
    {
        //リスタート操作メッセージ表示
        GameEndSubText.SetActive(true);
        
        //スペースが押されたらジャンプする
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //クリア・ゲームオーバーになったらリスタート
            SceneManager.LoadScene("GameScene");
        }
    }
}