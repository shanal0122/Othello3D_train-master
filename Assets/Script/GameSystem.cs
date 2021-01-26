using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSystem : MonoBehaviour //Training実行前にGameSystem.csのvoidAwakeのMokuji()とGameClearのsuccessTimeやLearningTimeをいじり、CPU.csのRandomMap()を調整する。そしてTextFileData?のテキストを消去する
{
    [SerializeField] private int xLength = 4;
    [SerializeField] private int yLength = 4;
    [SerializeField] private int zLength = 4;
    private int[,,] square; //最新の盤面が記録されている。noStone : 0, blackStone : 1, whiteStone : -1
    [SerializeField] private bool diagonal = false; //{1,1,1}系のベクトルを採用するか。採用するならtrue/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private int[,] vector;
    private int turn = 1;
    int totalTurn = 0;
    int learnTime = 0;
    int successTime = 0;
    private int result; //黒がかったら1、白がかったら-1,引き分けなら0
    bool passFlug = false;
    bool tempFlug = true;
    public CPU cpu;

    public GameObject blackStone;
    public GameObject whiteStone;


    void Awake()
    {
        cpu.Mokuji(1);
        cpu.Mokuji(2);
        cpu.Mokuji(3);

        cpu.XLength = xLength;
        cpu.YLength = yLength;
        cpu.ZLength = zLength;
        if(diagonal) //{1,1,1}系のベクトルを採用するか/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        {
          vector = new int[,]{{0,1,0},{1,1,0},{0,1,1},{-1,1,0},{0,1,-1},{1,0,0},{1,0,1},{0,0,1},{-1,0,1},{-1,0,0},{-1,0,-1},{0,0,-1},{1,0,-1},{1,-1,0},{0,-1,1},{-1,-1,0},{0,-1,-1},{0,-1,0},{1,1,1},{1,1,-1},{1,-1,1},{1,-1,-1},{-1,1,1},{-1,1,-1},{-1,-1,1},{-1,-1,-1}};
        }else
        {
          vector = new int[,]{{0,1,0},{1,1,0},{0,1,1},{-1,1,0},{0,1,-1},{1,0,0},{1,0,1},{0,0,1},{-1,0,1},{-1,0,0},{-1,0,-1},{0,0,-1},{1,0,-1},{1,-1,0},{0,-1,1},{-1,-1,0},{0,-1,-1},{0,-1,0}};
        }
        cpu.Vector = vector;


        square = new int[xLength,yLength,zLength];
        int a = xLength/2; int b = yLength/2; int c = zLength/2;
        square[a-1,b-1,c-1] = 1;
        square[a,b-1,c] = 1;
        square[a-1,b-1,c] = -1;
        square[a,b-1,c-1] = -1;
        square[a-1,b,c] = 1;
        square[a,b,c-1] = 1;
        square[a-1,b,c-1] = -1;
        square[a,b,c] = -1;

        turn = 1;

        /*for(int _y=0; _y<yLength; _y++) /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        {
          for(int _z=0; _z<zLength; _z++)
          {

              Debug.Log("(" + _y + "," + _z + ") : " + square[0,_y,_z] + " " +square[1,_y,_z] + " " +square[2,_y,_z] + " " +square[3,_y,_z]);

          }
        }*/
    }

    // Update is called once per frame
    void Update()
    {
      if(tempFlug)
      {
        tempFlug = false;
        Invoke("Train", 0f);
      }
    }

    void Train()
    {
      bool canPut = CanPut(turn);
      if(canPut)
      {
        passFlug = false;
        if(turn == 1){ cpu.BestCPU(); }
        if(turn == -1){ cpu.RandomCPU(); }
        //Check(); //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        turn *= -1;
        totalTurn++;
      }else
      {
        turn *= -1;
        if(passFlug){ GameClear(); }
        passFlug = true;
      }
      tempFlug = true;
    }



    public int FlipNum(int stone, int x, int y, int z, int vec) //stone{1,-1}を座標(x,y,z)に置いた時vec方向のコマを返せる個数を返す
      {
        int flipNum = 0;
        int myStone = stone;
        int yourStone = -1 * stone;
        while(true)
        {
          x += vector[vec,0];
          y += vector[vec,1];
          z += vector[vec,2];
          try
          {
            if(square[x,y,z] == yourStone)
            {
              flipNum++;
            }else if(square[x,y,z] == myStone)
            {
              break;
            }else
            {
              flipNum = 0;break;
            }
          }catch(IndexOutOfRangeException)
          {
            flipNum = 0;break;
          }
        }
        return flipNum;
      }

      private int VecFlipStone(int stone, int x, int y, int z, int vec) //stone{-1,1}を座標(x,y,z)に置いた時のvec方向のstoneを裏返す。戻り値は裏返す石の数
      {
        int flipNum = FlipNum(stone, x, y, z, vec);
        for(int n=1; n<=flipNum; n++)
        {
          square[x+n*vector[vec,0], y+n*vector[vec,1], z+n*vector[vec,2]] = stone;
        }
        return flipNum;
      }

      public bool FlipStone(int stone, int x, int y, int z) //座標(x,y,z)にstoneをおき裏返しturnを変更する。置けない時は何もしない。置けたらtrue、置けないならfalseを返す
      {
        if(square[x,y,z] == 0)
        {
          int sumOfFlipNum = 0;
          for(int n=0; n<vector.GetLength(0); n++)
          {
            sumOfFlipNum += VecFlipStone(stone, x, y, z, n);
          }
          if(sumOfFlipNum != 0)
          {
            square[x,y,z] = stone;
            /*for(int _y=0; _y<yLength; _y++) /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            {
              for(int _z=0; _z<zLength; _z++)
              {

                  Debug.Log("(" + _y + "," + _z + ") : " + square[0,_y,_z] + " " +square[1,_y,_z] + " " +square[2,_y,_z] + " " +square[3,_y,_z]);

              }
            }*/
            return true;
          }
        }
        return false;
      }

      public bool CanPut(int stone) //stoneを置ける場所が一つでもあればtrueを返す
      {
        if(stone != 1 && stone != -1)
        {
          Debug.Log("Error : Stone/CanPut");//////////////////////////////////////////////////////////////////////////////////////
        }
        for(int y=0; y<yLength; y++)
        {
          for(int z=0; z<zLength; z++)
          {
            for(int x=0; x<xLength; x++)
            {
              for(int n=0; n<vector.GetLength(0); n++)
              {
                if(square[x,y,z] == 0 && FlipNum(stone,x,y,z,n) != 0) {return true;}
              }
            }
          }
        }
        return false;
      }



      int CountStone(int stone) //盤上にあるstoneの数を数える
      {
        int stoneNum = 0;
        foreach(int sq in square) {if(sq == stone) {stoneNum++;}}
        if(stone != 1 && stone != -1)
        {
          Debug.Log("Error : Stone/CountStone");//////////////////////////////////////////////////////////////////////////////////////
        }
        return stoneNum;
      }

      void Judge()
      {
        int bl = CountStone(1);
        int wh = CountStone(-1);
        if(bl > wh) {result = 1;}
        if(bl == wh) {result = 0;}
        if(bl < wh) {result = -1; cpu.TextLog();}
      }

      void GameClear()
      {
        Judge();

        square = new int[xLength,yLength,zLength];
        int a = xLength/2; int b = yLength/2; int c = zLength/2;
        square[a-1,b-1,c-1] = 1;
        square[a,b-1,c] = 1;
        square[a-1,b-1,c] = -1;
        square[a,b-1,c-1] = -1;
        square[a-1,b,c] = 1;
        square[a,b,c-1] = 1;
        square[a-1,b,c-1] = -1;
        square[a,b,c] = -1;

        turn = 1;
        totalTurn = 0;
        learnTime++;
        if(result == -1){ successTime = 0; }else{ successTime++; }
        if(successTime % 1 == 0 & successTime != 0){ cpu.TextDetalog(1); }
        if(successTime % 35 == 0 & successTime != 0){ cpu.TextDetalog(2); }
        if(successTime % 50 == 0 & successTime != 0){ cpu.TextDetalog(3); }
        //Debug.Log("勝者 : " + result + "   回数 : " + learnTime + "　連続成功回数 : " + successTime); //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        cpu.MapCopy();
        cpu.RandomMap();

        if(learnTime >= 99900)
        {
          Debug.Log("Be oversoon.");
        }

        if(learnTime >= 100000){ cpu.LearningFinish(); }
      }

      public int[,,] Square{ get{ return square; } set{square = value; } }
      public int Turn{ get{ return turn; } set{turn = value; } }
      public int TotalTurn{ get{ return totalTurn; } set{totalTurn = value; } }
      public int LearnTime{ get{ return learnTime; } set{learnTime = value; } }














      private void Check()
      {
        GameObject[] stones = GameObject.FindGameObjectsWithTag("stone");
        foreach(GameObject stone in stones)
        {
          Destroy(stone);
        }
        for(int y=0; y<yLength; y++)
        {
          for(int z=0; z<zLength; z++)
          {
            for(int x=0; x<xLength; x++)
            {
                if(square[x,y,z] == 1){ Instantiate(blackStone, new Vector3(x,y,z), Quaternion.identity); }
                if(square[x,y,z] == -1){ Instantiate(whiteStone, new Vector3(x,y,z), Quaternion.identity); }

            }
          }
        }
      }

}
