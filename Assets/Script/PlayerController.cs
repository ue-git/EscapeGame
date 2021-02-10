using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerController : MonoBehaviour
{
    //アニメーションするためのコンポーネントを入れる
    private Animator myAnimator;
    //Unityちゃんを移動させるコンポーネントを入れる
    private Rigidbody myRigidbody;
    //前方向の速度（追加）
    private float velocityZ = 16f;
    //上下の矢印キーの押した値
    private float verticalInput = 0f;
    //キャラクターの移動速度
    private float speed = 3.0f;

    //ゲーム進行メッセージを表示するテキスト
    private GameObject MessageText;
    //ゲーム終了メッセージのテキスト
    private GameObject GameEndText;
    //ゲーム終了メッセージの値
    private string GameEndValue;

    //敵キャラクタのオブジェクト
    private GameObject enemy;

    //カード画像のオブジェクト
    private GameObject GetCardKeyImage;

    //出口のドアオブジェクト
    private GameObject doorLeft;
    private GameObject doorRight;

    //カードキー取得判定
    private bool getCardKey = false;
    //出口のカードキーあたり判定
    private bool exitDoorCardCheck = false;

    //クリア音の元ネタセット
    public AudioClip itemSound;
    //ドアOPEN音の元ネタセット
    public AudioClip doorOpenSound;
    //鍵なしブザー音の元ネタセット
    public AudioClip doorNoOpenSound;

    //ジャンプした時の床の位置
    private bool jumpCheck = false;

    // Use this for initialization
    void Start()
    {
        //アニメータコンポーネントを取得
        myAnimator = GetComponent<Animator>();
        
        //Rigidbodyコンポーネントを取得（追加）
        myRigidbody = GetComponent<Rigidbody>();

        //ゲーム進行メッセージのオブジェクトを取得
        MessageText = GameObject.Find("MessageText");

        //ゲーム終了textのオブジェクトを取得
        GameEndText = GameObject.Find("GameEndText");

        //出口のドアオブジェクトを取得
        doorLeft = GameObject.Find("doorLeft");
        doorRight = GameObject.Find("doorRight");

        //出口のドアオブジェクトを取得
        GetCardKeyImage = GameObject.Find("GetCardKeyImage");
        GetCardKeyImage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //ゲーム終了メッセージの値がなければ操作可能
        GameEndValue = GameEndText.GetComponent<Text>().text;
        if (GameEndValue == "")
        {
            //上下の矢印キーを押したときの値を取得（FixeUpdateでキャラ移動）
            verticalInput = Input.GetAxis("Vertical") * 1.4f;

            //キャラクターを左右の方向に向かせる
            //右矢印キーを押したときの移動
            if (Input.GetKey(KeyCode.RightArrow))
            {
                myRigidbody.transform.Rotate(new Vector3(0f, 1f, 0f));
            }
            //右矢印キーを押したときの移動
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                myRigidbody.transform.Rotate(new Vector3(0f, -1f, 0f));
            }

            //スペースが押されたらジャンプする
            if (Input.GetKeyDown(KeyCode.Space) && (!jumpCheck))
            {
                myRigidbody.velocity = new Vector3(0f, 10f, 0f);
                jumpCheck = true;
            }

            //出口のあたり操作
            if (exitDoorCardCheck)
            {
                //左のドアを左にスライドさせる
                Vector3 doorLeftPos = doorLeft.transform.position;
                doorLeftPos.x -= 0.01f;
                doorLeft.transform.position = doorLeftPos;

                //右のドアを右にスライドさせる
                Vector3 doorRightPos = doorRight.transform.position;
                doorRightPos.x += 0.01f;
                doorRight.transform.position = doorRightPos;
            }
        }
        else
        {
            //ゲーム終了の場合はプレイヤーへの力を止める
            verticalInput = 0f;
        }
    }


    //memo FixedUpdateは一定秒数ごとに処理させる
    void FixedUpdate()
    {
        //プレイヤーの現在の位置取得
        Vector3 myForward = myRigidbody.transform.forward;

        //力の加え方を計算
        Vector3 moveVector = speed * (myForward * verticalInput);
        moveVector.y = 0;

        //AddForceで瞬間的に力を加える
        myRigidbody.AddForce(moveVector, ForceMode.Impulse);

        //走るアニメーションを設定
        myAnimator.SetFloat("Speed", verticalInput);

        //走る効果音
        if(verticalInput > 0.3f)
        {
            GetComponent<AudioSource>().volume = 1;
        }
        else
        {
            GetComponent<AudioSource>().volume = 0;
        }
        //GetComponent<AudioSource>().volume = (isGround) ? 1 : 0;
    }


    //カードキーと接触した時の処理
    void OnTriggerEnter(Collider other)
    {
        //床に設置していたらジャンプできることにする
        if (other.gameObject.tag == "JumpPoint")
        {
            jumpCheck = false;
        }

        //カードキーと衝突
        if (other.gameObject.tag == "CardKey")
        {
            //カードキー取得
            getCardKey = true;
            //カードキー取得音再生
            GetComponent<AudioSource>().PlayOneShot(itemSound);
            //画面にカードキーimageを表示
            GetCardKeyImage.SetActive(true);
            //カードキー削除
            Destroy(other.gameObject);
            //進行メッセージ更新
            MessageText.GetComponent<Text>().text = "カードキー取得完了。出口目指せ。";
        }

        //出口と衝突
        if (other.gameObject.tag == "ExitDoor")
        {
            //カードキーの取得を確認
            if (getCardKey)
            {
                //ドアが開く音は1回のみとする
                if (!exitDoorCardCheck)
                {
                    //ドア開く音再生
                    GetComponent<AudioSource>().PlayOneShot(doorOpenSound);
                }
                //出口がカードキーと振れたことを保存 
                exitDoorCardCheck = true;
            }
            else
            {
                //カードキーなしのブザー音を再生
                GetComponent<AudioSource>().PlayOneShot(doorNoOpenSound);
            }
        }


        //ゴールに到着
        if (other.gameObject.tag == "Goal")
        {
            //進行メッセージ更新
            MessageText.GetComponent<Text>().text = "脱出成功！！";
            //ゲーム終了メッセージ更新
            GameEndText.GetComponent<Text>().text = "CLEAR";
        }

        //敵に発見される
        if (other.gameObject.tag == "Enemy")
        {
            //進行メッセージ更新
            MessageText.GetComponent<Text>().text = "監視者に発見されました。";
            //ゲーム終了メッセージ更新
            GameEndText.GetComponent<Text>().text = "GAME OVER";
        }
    }
}
