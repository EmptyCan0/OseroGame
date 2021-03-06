using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Countloop
{
    public static int count00 = 0;
}

public class Board_con02 : MonoBehaviour
{
    public static int size = 6; //盤面の大きさ
    public int[,] field = new int[size, size];
    public int[,] field2 = new int[size, size]; //Aiの記憶用のfield

    public int[,,] putPos = new int[1000, size, size]; //AI用総合field

    int[] dx = { 1, 0, -1 };
    int[] dy = { 1, 0, -1 };
    bool done = false;
    bool isReversible = false;

    int p,q;
    int x, z;
    int y = 1;
    public int color;
    int i, j = 0;

    public GameObject okuyatuPrefab;
    public GameObject okerubasyoPrefab;

    public int CaluculateX(int x)
    {
        int a = -15 + (6 * x);
        return a;
    }//駒のｘ座標計算
    public int CaluculateZ(int z)
    {
        int a = 15 - (6 * z);
        return a;
    }//駒のz座標計算

    void Visible0()//駒の表示
    {
        int x, z;
        int y = 1;
        for (i = 1; i < size - 1; i++)
        {
            for (j = 1; j < size - 1; j++)
            {
                if (field[i, j] == 1)
                {
                    x = CaluculateX(j);
                    z = CaluculateZ(i);
                    GameObject go = Instantiate(okuyatuPrefab) as GameObject;
                    go.transform.position = new Vector3(x, y, z);
                    go.transform.localRotation = Quaternion.Euler(90, 0, 0);
                }
                else if (field[i, j] == -1)
                {
                    x = CaluculateX(j);
                    z = CaluculateZ(i);
                    GameObject go = Instantiate(okuyatuPrefab) as GameObject;
                    go.transform.position = new Vector3(x, y, z);
                    go.transform.localRotation = Quaternion.Euler(-90, 0, 0);
                }
                else
                {

                }
            }
        }
    }

    void Puton(int z, int x, int color)
    {
        int y = 1;
        if (color == 1)
        {
            x = CaluculateX(x);
            z = CaluculateZ(z);
            // Debug.Log("x = "+x);
            //  Debug.Log("z = " + z);
            GameObject go = Instantiate(okuyatuPrefab) as GameObject;
            go.transform.position = new Vector3(x, y, z);
            go.transform.localRotation = Quaternion.Euler(90, 0, 0);
        }
        else if (color == -1)
        {
            x = CaluculateX(x);
            z = CaluculateZ(z);
            //  Debug.Log("x = " + x);
            //  Debug.Log("z = " + z);
            GameObject go = Instantiate(okuyatuPrefab) as GameObject;
            go.transform.position = new Vector3(x, y, z);
            go.transform.localRotation = Quaternion.Euler(-90, 0, 0);
        }
    }
    int count(int n)
    {
        int res = 0;
        for (i = 0; i < size; i++)
        {
            for (j = 0; j < size; j++)
            {
                if (field[i, j] == n) res++;
            }
        }
        return res;
    }

    public int[] reversible(int x0, int y0, int color)
    {  //8方位でどれだけひっくり返せるか個数を配列で
        int x, y;
        int[] res = new int[8];
        int index = 0;
        // Debug.Log("x0=" + x0);
        // Debug.Log("y0=" + y0);

        foreach (int nx in dx)
        {
            foreach (int ny in dy)
            {  //調査方向の決定
                if (nx == 0 && ny == 0) continue;
                x = x0;
                y = y0;
                do
                {
                    x += nx;
                    y += ny;
                    // Debug.Log("XとYの値");
                    //  Debug.Log("X = " + x);
                    //   Debug.Log("Y = " + y);
                    if ((x >= size || x < 0 || y >= size || y < 0))
                    {
                        // Debug.Log("XとYの値");
                        // Debug.Log("X = " + x);
                        // Debug.Log("Y = " + y);
                        x = x0;
                        y = y0;
                        // Debug.Log("Break処理");
                        break;
                    }

                }
                while (field[x, y] == -color);
                if (field[x, y] == color)
                {
                    res[index] = Mathf.Max(Mathf.Abs(x - x0), Mathf.Abs(y - y0)) - 1;
                    res[index] = Mathf.Max(0, res[index]);
                    //ひっくり返せる個数
                }
                index++;
            }
        }
        // Debug.Log("reversibleの値を返します");
        return res;
    }

    void reverse(int x0, int y0, int color)
    { //(x,y)は置けるマスであることが前提
        field[x0, y0] = color;
        int k;
        int index = 0;
        foreach (int nx in dx)
        {
            foreach (int ny in dy)
            {
                if (nx == 0 && ny == 0) continue;
                int x = x0;
                int y = y0;
               // Debug.Log("nx=" + nx + " ny=" + ny + "のとき" + "reversible(x, y, color)[" + index + "] = " + reversible(x, y, color)[index]);
                k = reversible(x, y, color)[index];

                for (i = 0; i < k; i++)
                {
                    x += nx;
                    y += ny;
                    field[x, y] = color;
                    //Debug.Log(i+1 + "回目を返したよ");

                }
                index++;
            }
        }
    }

    bool turn(int color)
    {
        int x, z;
        int y = 1;
        //置けるところがあるか判定
        bool isReversible = false;
        for (i = 0; i < size; i++)
        {
            for (j = 0; j < size; j++)
            {
                // Debug.Log("riversible実行前");

                //表示
                if (reversible(i, j, color).Sum() > 0)
                {
                    // Debug.Log("i=" + i + ",y=" + j + "のときreversible=" + reversible(i, j, color).Sum());
                }

                if (reversible(i, j, color).Sum() > 0 && field[i, j] == 0)
                { //1つでも0じゃないならtrue、全て非負
                    isReversible = true;
                    x = CaluculateX(j);
                    z = CaluculateZ(i);
                    GameObject go = Instantiate(okerubasyoPrefab) as GameObject;
                    go.transform.position = new Vector3(x, y, z);
                    // break;
                }
            }
        }
        // Debug.Log("riversible実行後");
        if (isReversible == false)
        { //ひっくり返せないなら終わり
            Debug.Log("You can't go anyhere");
            return false;
        }

        return true;
        //置けるところがある場合
        //適切な値がくるまで入力を受け取りたい
        /*     
                do
                {
                    Debug.Log("初期値x=2,y=1");
                   x =;
                   y =;
                }
                while (reversible(y, x, color).Sum() == 0);    
                reverse(y, x, color);
                Puton(y, x, color);
                */
    }

    void Copy()
    {
        int i, j;
        for (i = 0; i < size; i++)
        {
            for (j = 0; j < size; j++)
            {
                field2[i, j] = field[i, j];
            }
        }
    }


    void Remember()
    {
        int i, j;
        for (i = 0; i < size; i++)
        {
            for (j = 0; j < size; j++)
            {
                putPos[Countloop.count00,i, j] = field[i, j];
            }
        }
    }
    void Paste_from_field2()
    {
        int i, j;
        for (i = 0; i < size; i++)
        {
            for (j = 0; j < size; j++)
            {
                field[i, j] = field2[i, j];
            }
        }
    }

    void Paste_from_putPos(int countloop)
    {
        int i, j;
        for (i = 0; i < size; i++)
        {
            for (j = 0; j < size; j++)
            {
                field[i, j] = putPos[countloop, i, j];
            }
        }
    }




    int Geteval(int color,int pass)//このときのカラーはAIのカラー この関数を呼び出すときfield2に必ず記憶を行う passはプレイヤーがパスを行ったかどうかの数字
    {
        int a, b;
        int k;
        int n = count(1);//黒を数える
        n += count(-1);//白の数を足す
        if (n >= size * size)
        {
            return count(color) - count(color * (-1));//colorとcolor
        }
        int played = 0;
        int besteval = color == 1 ? -99 : 99; //color=1だったらbesteval=-99

        Debug.Log("先頭に戻って来たよ");
        //まず置ける場所を端っこを含めず特定
        for (p = 0; p < size; p++) 
        {
            for (q = 0; q < size; q++)
            {

                if(field[p,q] == 1 || field[p,q] == -1)//すでに埋められてるならパス
                {
                    continue;
                }

               
             
                k = reversible(p, q, color).Sum();

                if (k > 0)//置く場所があるか
                {
                    played++;
                    Debug.Log("i = " + p + "j = " + q + "で置く");
                    //置けるならば実際においてみる                           
                    reverse(p, q, color);
                    Countloop.count00++;
                    Remember();//一手先の盤面を記憶
                    //一手先の結果を表示
                    Debug.Log("countblack = " + count(1));
                    Debug.Log("countwhite = " + count(-1));
                    Debug.Log("Countloop = " + Countloop.count00 + "のとき");
                    for (a = 0; a < size; a++)
                    {
                        Debug.Log(field[a, 1] + " " + field[a, 2] + " " + field[a, 3] + " " + field[a, 4] + " " + field[a, 5]);
                    }
                    int eval = Geteval(color * (-1), 0);
                    Debug.Log("eval =" + eval);
                    if ((color == 1 && eval > besteval) || (color == -1 && eval < besteval))
                    //黒のときbestevalの最大値が最善手,白のときbestevalの最小値が最善手
                    {
                        besteval = eval;
                    }
                    //一手戻る

                    Countloop.count00 += -1;
                    Debug.Log("1手戻るcountloop = "+ Countloop.count00);
                    Debug.Log("このとき　i = "+ p+ " J = "+ q + "colorは " + color);
                    Paste_from_putPos(Countloop.count00);
                    for (a = 0; a < size; a++)
                    {
                        Debug.Log(field[a, 1] + " "+  field[a, 2] + " " + field[a, 3] + " " + field[a, 4] + " " + field[a, 5]);
                    }
                }

           
            }
        }


        Debug.Log("playedは" + played);
        Debug.Log("Forの外出たよ");

        if(played <= 0)//どこも置けなかった
        {
            if (pass == 1)
            {
                int diff = count(1) - count(-1);//黒の石-白の石
                int total = count(1) + count(-1);
                if(total < size * size) 
                {
                    if(count(1) > count(-1)) //黒＞白
                    {
                        diff += 36 - total;
                    }
                    else if(count(1) < count(-1))//黒＜白
                    {
                        diff += total - 36;
                    }
                }
                return diff;
            }
            else
            {
                Debug.Log("もしかして？");
                return Geteval(color * (-1), 1);
            }
        }
        return besteval;
    }
    
    void Start()
    {
        Countloop.count00 = 0;
        for (i = 0; i < size; i++)
        {
            for (j = 0; j < size; j++)
            {
                if ((i == size / 2 && j == size / 2) || (i == size / 2 - 1 && j == size / 2 - 1))
                {
                    field[i, j] = -1;
                }
                else if ((i == size / 2 && j == size / 2 - 1) || (i == size / 2 - 1 && j == size / 2))
                {
                    field[i, j] = 1;
                }
                else
                {
                    field[i, j] = 0;
                }
            }
        }

        for (i = 0; i < size; i++)
        {
            field[0, i] = 1;
            field[i, 0] = 1;
            field[size - 1, i] = -1;
            field[i, size - 1] = -1;
        }
        field[0, size - 1] = -1;

        Visible0();
        color = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //毎ターン一度だけ
        if (color == 1 && done == false)//ターン黒のとき
        {
            Debug.Log("BlackTurn");
            //isReversible判定と同時に置ける場所表示===================================================
            isReversible = turn(color);
            //isReversible判定-------------------------------------------------------------------------      

            if (isReversible == false)
            {
                // Debug.Log("おけないよ");
                color *= -1;
            }
            else
            {
                done = true;
                Copy();
                Remember();
                int eval2 = Geteval(color, 0);
                Debug.Log(eval2);
                Paste_from_field2();
            }
        }
        else if (color == -1 && done == false)//ターン白のとき
        {
            Debug.Log("WhiteTurn");
            //isReversible判定と同時に置ける場所表示===================================================
            isReversible = turn(color);
            //isReversible判定-------------------------------------------------------------------------        
            if (isReversible == false)
            {
                //Debug.Log("おけないよ");
                color *= -1;
            }
            else
            {
                done = true;
            }
        }

        if (Input.GetMouseButtonDown(0))//タップされた
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit_info = new RaycastHit();
            float max_distance = 100f;

            bool is_hit = Physics.Raycast(ray, out hit_info, max_distance);
            if (is_hit)
            {

                //             Debug.Log(hit_info.collider.gameObject.transform.position);
                z = (15 - (int)hit_info.collider.gameObject.transform.position.z) / 6;
                x = (15 + (int)hit_info.collider.gameObject.transform.position.x) / 6;
                //               Debug.Log("i=" + z);
                //              Debug.Log("j=" + x);
                reverse(z, x, color);  
                Puton(z, x, color);
                GameObject[] target = GameObject.FindGameObjectsWithTag("okerubasyo");
                foreach (GameObject clone in target)
                {
                    Destroy(clone);
                    //  Debug.Log("delete");
                }

                done = false;
                // Debug.Log("doen = false");
                color *= -1;
            }
        }

    }
}

