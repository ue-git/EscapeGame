using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    //アニメーションするためのコンポーネントを入れる
    private Animator EnemyAnimator;
    //Unityちゃんを移動させるコンポーネントを入れる
    private Rigidbody EnemyRigidbody;

    //時間
    private float playTime;
    //動くときの時間
    private int workTime = 3;

    //ゲーム終了メッセージのテキスト
    private GameObject GameEndText;
    //ゲーム終了メッセージの値
    private string GameEndValue;

    // Start is called before the first frame update
    void Start()
    {
        //アニメータコンポーネントを取得
        EnemyAnimator = GetComponent<Animator>();

        //Rigidbodyコンポーネントを取得（追加）
        EnemyRigidbody = GetComponent<Rigidbody>();

        //ゲーム終了textのオブジェクトを取得
        GameEndText = GameObject.Find("GameEndText");
    }

    // Update is called once per frame
    void Update()
    {
        //ゲーム終了メッセージの値がなければ操作可能
        GameEndValue = GameEndText.GetComponent<Text>().text;
        if (GameEndValue == "")
        {
            //1秒に1ずつ減らしていく
            playTime += Time.deltaTime;
            //Debug.Log(playTime);
            if (((int)playTime) % workTime == 0)
            {
                //Debug.Log(playTime);
                //右回りに視界を移動
                EnemyRigidbody.transform.Rotate(new Vector3(0f, 1f, 0f));
            }
        }
    }
}
