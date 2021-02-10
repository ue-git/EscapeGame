using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyCameraController : MonoBehaviour
{
    //プレイヤーのオブジェクト
    private GameObject player;

    //ゲーム終了メッセージのテキスト
    private GameObject GameEndText;
    //ゲーム終了メッセージの値
    private string GameEndValue;

    // Use this for initialization
    void Start()
    {
        //プレイヤーのオブジェクトを取得
        player = GameObject.Find("unitychan");

        //カメラのrotation設定
        this.transform.Rotate(new Vector3(0f, 0f, 0f),90);

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
            //プレイヤーの位置に合わせてカメラの位置を移動
            this.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 1f, player.transform.position.z);

            this.transform.rotation = Quaternion.Euler(0f, this.transform.localEulerAngles.y, 0f);

            //右矢印キーを押したときの移動
            if (Input.GetKey(KeyCode.RightArrow))
            {
                this.transform.Rotate(new Vector3(0f, 1f, 0f));
            }

            //右矢印キーを押したときの移動
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                this.transform.Rotate(new Vector3(0f, -1f, 0f));
            }
        }
    }
}
