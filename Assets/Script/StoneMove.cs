using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneMove : MonoBehaviour
{
    int i, j;
    GameObject gamemaker;
    Board_con script;
    // Start is called before the first frame update
    void Start()
    {
        gamemaker = GameObject.Find("GameMaker");
        script = gamemaker.GetComponent<Board_con>();
    }

    // Update is called once per frame
    void Update()
    {
        //自分の座標からfield[i,j]を特定し続ける
        Transform myTransform = this.transform;
        Vector3 pos = myTransform.position;
        j = ((int)pos.x + 15) / 6;
        i = (15 - (int)pos.z) / 6;
       // Debug.Log("i=" + i + ",j=" + j);
        //field[i,j]の特定
        int state = script.field[i,j];
        if(state == 1)
        {
            myTransform.localRotation = Quaternion.Euler(90, 0, 0);
        }
        else if(state == -1)
        {
            myTransform.localRotation = Quaternion.Euler(-90, 0, 0);
        }
    }
}
