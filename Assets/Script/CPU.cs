using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class CPU : MonoBehaviour
{
    int xLength = 4;
    int yLength = 4;
    int zLength = 4;
    int[,,] square;
    int[,] vector;
    int cpuStone;
    float[,,] basicMap1; //各座標の基本点。座標(0,0,0)に自分の石があればbasicMap1[0,0,0]点入る
    float[,,] basicMap2;
    float[,,] basicMap3;
    float[,,,] sideMap; //各辺の石組み合わせに関する追加点。0:nostone,1:自分の石,2:相手の石。例えば辺上で0221という並びは高得点で実際sideMap[0,2,2,1]点
    float[,,,] surfaceMap; //各面上の直線上の石の組み合わせに関する追加点。0:nostone,1:自分の石,2:相手の石

    float[,,] basicMap1Random;
    float[,,] basicMap2Random;
    float[,,] basicMap3Random;
    float[,,,] sideMapRandom;
    float[,,,] surfaceMapRandom;
    public GameSystem gameSystem;
    public Texts texts;



    void Awake()
    {
      basicMap1 = new float[,,]
      {
          { {100f,7f,7f,100f}, {7f, -20f, -20f, 7f}, {7f, -20f, -20f, 7f}, {100f,7f,7f,100f} },
          { {7f, -20f, -20f, 7f}, {-20f, 15f, 15f, -20f}, {-20f, 15f, 15f, -20f}, {7f, -20f, -20f, 7f} },
          { {7f, -20f, -20f, 7f}, {-20f, 15f, 15f, -20f}, {-20f, 15f, 15f, -20f}, {7f, -20f, -20f, 7f} },
          { {100f,7f,7f,100f}, {7f, -20f, -20f, 7f}, {7f, -20f, -20f, 7f}, {100f,7f,7f,100f} }
      };
      basicMap2 = new float[,,]
      {
          { {75f,0f,0f,75f}, {0f, -3f, -3f, 0f}, {0f, -3f, -3f, 0f}, {75f,0f,0f,75f} },
          { {0f, -3f, -3f, 0f}, {-3f, 5f, 5f, -3f}, {-3f, 5f, 5f, -3f}, {0f, -3f, -3f, 0f} },
          { {0f, -3f, -3f, 0f}, {-3f, 5f, 5f, -3f}, {-3f, 5f, 5f, -3f}, {0f, -3f, -3f, 0f} },
          { {75f,0f,0f,75f}, {0f, -3f, -3f, 0f}, {0f, -3f, -3f, 0f}, {75f,0f,0f,75f} }
      };
      basicMap3 = new float[,,]
      {
          { {60f,7f,7f,60f}, {7f, 15f, 15f, 7f}, {7f, 15f, 15f, 7f}, {60f,7f,7f,60f} },
          { {7f, 15f, 15f, 7f}, {15f, -10f, -10f, 15f}, {15f, -10f, -10f, 15f}, {7f, 15f, 15f, 7f} },
          { {7f, 15f, 15f, 7f}, {15f, -10f, -10f, 15f}, {15f, -10f, -10f, 15f}, {7f, 15f, 15f, 7f} },
          { {60f,7f,7f,60f}, {7f, 15f, 15f, 7f}, {7f, 15f, 15f, 7f}, {60f,7f,7f,60f} }
      };
      sideMap = new float[,,,]
      {
          {
            { {0f, 0f, 0f}, {0f, 0f, -26f}, {0f, 26f, 0f} },
            { {0f, 5f, -24f}, {0f, -7f, -40f}, {-52f, -52f, -57f} },
            { {0f, 24f, -5f}, {-52f, 57f, 52f}, {0f, 40f, 7f} }
          },
          {
            { {0f, 0f, 0f}, {5f, 8f, -42f}, {24f, 47f, 2f} },
            { {0f, 8f, -2f}, {-7f, 52f, 45f}, {57f, -14f, 0f} },
            { {26f, 47f, 42f}, {-52f, -14f, 0f}, {40f, -10f, -45f} }
          },
          {
            { {0f, 0f, 0f}, {-24f, -2f, -47f}, {-5f, 42f, -8f} },
            { {-26f, -42f, -47f}, {-40f, 45f, 10f}, {52f, 0f, 14f} },
            { {0f, 2f, -8f}, {-57f, 0f, 14f}, {7f, -45f, -52f} }
          }
      };
      surfaceMap = new float[,,,]
      {
          {
            { {0f, 0f, 0f}, {0f, -18f, -36f}, {0f, 36f, 18f} },
            { {0f, 3f, -2f}, {0f, 7f, -50f}, {-56f, -37f, -64f} },
            { {0f, 2f, -3f}, {-56f, 64f, 37f}, {0f, 50f, -7f} }
          },
          {
            { {0f, 0f, 0f}, {3f, -10f, -47f}, {2f, 45f, -5f} },
            { {-18f, -10f, 5f}, {7f, 36f, 20f}, {64f, 7f, 0f} },
            { {36f, 45f, 47f}, {-37f, 7f, 0f}, {50f, 7f, 20f} }
          },
          {
            { {0f, 0f, 0f}, {-2f, 5f, -45f}, {-3f, 47f, 10f} },
            { {-36f, -47f, -45f}, {-50f, 20f, 7f}, {37f, 0f, -7f} },
            { {18f, -5f, 10f}, {-64f, 0f, -7f}, {-7f, -20f, -36f} }
          }
      };
    }

    void Start()
    {
      basicMap1Random = new float[xLength,yLength,zLength];
      basicMap2Random = new float[xLength,yLength,zLength];
      basicMap3Random = new float[xLength,yLength,zLength];
      sideMapRandom = new float[3,3,3,3];
      surfaceMapRandom = new float[3,3,3,3];

      MapCopy();
      RandomMap();
    }

    public void BestCPU()
    {
      int xCoordi = 0, yCoordi = 0, zCoordi = 0;
      float score = 0;
      float? bestScore = null;
      square = gameSystem.Square;
      cpuStone = gameSystem.Turn;
      int[,,] willSq = new int[xLength,yLength,zLength];
      for(int y=0; y<yLength; y++)
      {
        for(int z=0; z<zLength; z++)
        {
          for(int x=0; x<xLength; x++)
          {
            if(square[x,y,z] == 0)
            {
              willSq = TryFlip(square, cpuStone, x, y, z);
              if(willSq != null)
              {
                score = CulScoreBest(willSq);
                if(bestScore == null){ bestScore = score; xCoordi = x; yCoordi = y; zCoordi = z; }
                if(bestScore < score){ bestScore = score; xCoordi = x; yCoordi = y; zCoordi = z; }
                score = 0;
              }
            }
          }
        }
      }
      bool a = gameSystem.FlipStone(cpuStone,xCoordi, yCoordi, zCoordi);
    }

    public void RandomCPU()
    {
      int xCoordi = 0, yCoordi = 0, zCoordi = 0;
      float score = 0;
      float? bestScore = null;
      square = gameSystem.Square;
      cpuStone = gameSystem.Turn;
      int[,,] willSq = new int[xLength,yLength,zLength];
      for(int y=0; y<yLength; y++)
      {
        for(int z=0; z<zLength; z++)
        {
          for(int x=0; x<xLength; x++)
          {
            if(square[x,y,z] == 0)
            {
              willSq = TryFlip(square, cpuStone, x, y, z);
              if(willSq != null)
              {
                score = CulScoreRandom(willSq);
                if(bestScore == null){ bestScore = score; xCoordi = x; yCoordi = y; zCoordi = z; }
                if(bestScore < score){ bestScore = score; xCoordi = x; yCoordi = y; zCoordi = z; }
                score = 0;
              }
            }
          }
        }
      }
      //float rv = UnityEngine.Random.value;
      bool a = gameSystem.FlipStone(cpuStone,xCoordi, yCoordi, zCoordi);
    }




    public int TryFlipNum(int[,,] sq, int stone, int x, int y, int z, int vec) //stone{1,-1}を座標(x,y,z)に置いたらvec方向のコマを返せるはずの個数を返す
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
            if(sq[x,y,z] == yourStone)
            {
              flipNum++;
            }else if(sq[x,y,z] == myStone)
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

    public int[,,] TryFlip(int[,,] sq, int stone, int x, int y, int z) //座標(x,y,z)にstoneをおき裏返しturnを変更したらどうなるか見る。おいた後の配列を返す
    {
      int[,,] newsq = new int[xLength,yLength,zLength];
      Array.Copy(sq, newsq, sq.Length);
      int sumOfFlipNum = 0;
      int flipNum = 0;
      for(int vec=0; vec<vector.GetLength(0); vec++)
      {
        flipNum = TryFlipNum(sq, stone, x, y, z, vec);
        sumOfFlipNum += flipNum;
        for(int n=1; n<=flipNum; n++)
        {
          newsq[x+n*vector[vec,0], y+n*vector[vec,1], z+n*vector[vec,2]] = stone;
        }
        flipNum = 0;
      }
      if(sumOfFlipNum != 0)
      {
        newsq[x,y,z] = stone;
        return newsq;
      }
      else
      {
        return null;
      }
      /*for(int _y=0; _y<yLength; _y++) /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
      {
        for(int _z=0; _z<zLength; _z++)
        {

            Debug.Log("(" + _y + "," + _z + ") : " + square[0,_y,_z] + " " +square[1,_y,_z] + " " +square[2,_y,_z] + " " +square[3,_y,_z]);

        }
      }*/
    }


    private float CulScoreBest(int[,,] sq)
    {
      int n;
      if(gameSystem.TotalTurn < 20){ n = 1; } else if(gameSystem.TotalTurn < 40){ n = 2; } else{ n = 3; }
      return CulBasic(n,sq,basicMap1,basicMap2,basicMap3) + CulSide(sq,sideMap) + CulSurface(sq,surfaceMap);
    }

    private float CulScoreRandom(int[,,] sq)
    {
      int n;
      if(gameSystem.TotalTurn < 20){ n = 1; } else if(gameSystem.TotalTurn < 40){ n = 2; } else{ n = 3; }
      return CulBasic(n,sq,basicMap1Random,basicMap2Random,basicMap3Random) + CulSide(sq,sideMapRandom) + CulSurface(sq,surfaceMapRandom);
    }

    private float CulBasic(int n, int[,,] sq, float[,,] map1, float[,,] map2, float[,,] map3)
    {
      float[,,] basicMap = new float[xLength,yLength,zLength];
      if(n == 1){ basicMap = map1; }
      if(n == 2){ basicMap = map2; }
      if(n == 3){ basicMap = map3; }
      float score = 0;
      for(int y=0; y<yLength; y++)
      {
        for(int z=0; z<zLength; z++)
        {
          for(int x=0; x<xLength; x++)
          {
            if(sq[x,y,z] == cpuStone){ score += basicMap[x,y,z]; }
            if(sq[x,y,z] == -1*cpuStone){ score -= basicMap[x,y,z]; }
          }
        }
      }
      return score;
    }

    private float CulSide(int[,,] sq, float[,,,] map)
    {
      float score = 0;
      score += map[ TransSqIndex(cpuStone*sq[0,0,0]), TransSqIndex(cpuStone*sq[1,0,0]), TransSqIndex(cpuStone*sq[2,0,0]), TransSqIndex(cpuStone*sq[3,0,0])];
      score += map[ TransSqIndex(cpuStone*sq[0,0,3]), TransSqIndex(cpuStone*sq[1,0,3]), TransSqIndex(cpuStone*sq[2,0,3]), TransSqIndex(cpuStone*sq[3,0,3])];
      score += map[ TransSqIndex(cpuStone*sq[0,3,0]), TransSqIndex(cpuStone*sq[1,3,0]), TransSqIndex(cpuStone*sq[2,3,0]), TransSqIndex(cpuStone*sq[3,3,0])];
      score += map[ TransSqIndex(cpuStone*sq[0,3,3]), TransSqIndex(cpuStone*sq[1,3,3]), TransSqIndex(cpuStone*sq[2,3,3]), TransSqIndex(cpuStone*sq[3,3,3])];

      score += map[ TransSqIndex(cpuStone*sq[0,0,0]), TransSqIndex(cpuStone*sq[0,1,0]), TransSqIndex(cpuStone*sq[0,2,0]), TransSqIndex(cpuStone*sq[0,3,0])];
      score += map[ TransSqIndex(cpuStone*sq[3,0,0]), TransSqIndex(cpuStone*sq[3,1,0]), TransSqIndex(cpuStone*sq[3,2,0]), TransSqIndex(cpuStone*sq[3,3,0])];
      score += map[ TransSqIndex(cpuStone*sq[0,0,3]), TransSqIndex(cpuStone*sq[0,1,3]), TransSqIndex(cpuStone*sq[0,2,3]), TransSqIndex(cpuStone*sq[0,3,3])];
      score += map[ TransSqIndex(cpuStone*sq[3,0,3]), TransSqIndex(cpuStone*sq[3,1,3]), TransSqIndex(cpuStone*sq[3,2,3]), TransSqIndex(cpuStone*sq[3,3,3])];

      score += map[ TransSqIndex(cpuStone*sq[0,0,0]), TransSqIndex(cpuStone*sq[0,0,1]), TransSqIndex(cpuStone*sq[0,0,2]), TransSqIndex(cpuStone*sq[0,0,3])];
      score += map[ TransSqIndex(cpuStone*sq[0,3,0]), TransSqIndex(cpuStone*sq[0,3,1]), TransSqIndex(cpuStone*sq[0,3,2]), TransSqIndex(cpuStone*sq[0,3,3])];
      score += map[ TransSqIndex(cpuStone*sq[3,0,0]), TransSqIndex(cpuStone*sq[3,0,1]), TransSqIndex(cpuStone*sq[3,0,2]), TransSqIndex(cpuStone*sq[3,0,3])];
      score += map[ TransSqIndex(cpuStone*sq[3,3,0]), TransSqIndex(cpuStone*sq[3,3,1]), TransSqIndex(cpuStone*sq[3,3,2]), TransSqIndex(cpuStone*sq[3,3,3])];

      return score;
    }

    private float CulSurface(int[,,] sq, float[,,,] map)
    {
      float score = 0;
      score += map[ TransSqIndex(cpuStone*sq[0,0,1]), TransSqIndex(cpuStone*sq[1,0,1]), TransSqIndex(cpuStone*sq[2,0,1]), TransSqIndex(cpuStone*sq[3,0,1])];
      score += map[ TransSqIndex(cpuStone*sq[0,0,2]), TransSqIndex(cpuStone*sq[1,0,2]), TransSqIndex(cpuStone*sq[2,0,2]), TransSqIndex(cpuStone*sq[3,0,2])];
      score += map[ TransSqIndex(cpuStone*sq[0,1,3]), TransSqIndex(cpuStone*sq[1,1,3]), TransSqIndex(cpuStone*sq[2,1,3]), TransSqIndex(cpuStone*sq[3,1,3])];
      score += map[ TransSqIndex(cpuStone*sq[0,2,3]), TransSqIndex(cpuStone*sq[1,2,3]), TransSqIndex(cpuStone*sq[2,2,3]), TransSqIndex(cpuStone*sq[3,2,3])];
      score += map[ TransSqIndex(cpuStone*sq[0,3,2]), TransSqIndex(cpuStone*sq[1,3,2]), TransSqIndex(cpuStone*sq[2,3,2]), TransSqIndex(cpuStone*sq[3,3,2])];
      score += map[ TransSqIndex(cpuStone*sq[0,3,1]), TransSqIndex(cpuStone*sq[1,3,1]), TransSqIndex(cpuStone*sq[2,3,1]), TransSqIndex(cpuStone*sq[3,3,1])];
      score += map[ TransSqIndex(cpuStone*sq[0,2,0]), TransSqIndex(cpuStone*sq[1,2,0]), TransSqIndex(cpuStone*sq[2,2,0]), TransSqIndex(cpuStone*sq[3,2,0])];
      score += map[ TransSqIndex(cpuStone*sq[0,1,0]), TransSqIndex(cpuStone*sq[1,1,0]), TransSqIndex(cpuStone*sq[2,1,0]), TransSqIndex(cpuStone*sq[3,1,0])];

      score += map[ TransSqIndex(cpuStone*sq[1,0,0]), TransSqIndex(cpuStone*sq[1,1,0]), TransSqIndex(cpuStone*sq[1,2,0]), TransSqIndex(cpuStone*sq[1,3,0])];
      score += map[ TransSqIndex(cpuStone*sq[2,0,0]), TransSqIndex(cpuStone*sq[2,1,0]), TransSqIndex(cpuStone*sq[2,2,0]), TransSqIndex(cpuStone*sq[2,3,0])];
      score += map[ TransSqIndex(cpuStone*sq[3,0,1]), TransSqIndex(cpuStone*sq[3,1,1]), TransSqIndex(cpuStone*sq[3,2,1]), TransSqIndex(cpuStone*sq[3,3,1])];
      score += map[ TransSqIndex(cpuStone*sq[3,0,2]), TransSqIndex(cpuStone*sq[3,1,2]), TransSqIndex(cpuStone*sq[3,2,2]), TransSqIndex(cpuStone*sq[3,3,2])];
      score += map[ TransSqIndex(cpuStone*sq[2,0,3]), TransSqIndex(cpuStone*sq[2,1,3]), TransSqIndex(cpuStone*sq[2,2,3]), TransSqIndex(cpuStone*sq[2,3,3])];
      score += map[ TransSqIndex(cpuStone*sq[1,0,3]), TransSqIndex(cpuStone*sq[1,1,3]), TransSqIndex(cpuStone*sq[1,2,3]), TransSqIndex(cpuStone*sq[1,3,3])];
      score += map[ TransSqIndex(cpuStone*sq[0,0,2]), TransSqIndex(cpuStone*sq[0,1,2]), TransSqIndex(cpuStone*sq[0,2,2]), TransSqIndex(cpuStone*sq[0,3,2])];
      score += map[ TransSqIndex(cpuStone*sq[0,0,1]), TransSqIndex(cpuStone*sq[0,1,1]), TransSqIndex(cpuStone*sq[0,2,1]), TransSqIndex(cpuStone*sq[0,3,1])];

      score += map[ TransSqIndex(cpuStone*sq[0,1,0]), TransSqIndex(cpuStone*sq[0,1,1]), TransSqIndex(cpuStone*sq[0,1,2]), TransSqIndex(cpuStone*sq[0,1,3])];
      score += map[ TransSqIndex(cpuStone*sq[0,2,0]), TransSqIndex(cpuStone*sq[0,2,1]), TransSqIndex(cpuStone*sq[0,2,2]), TransSqIndex(cpuStone*sq[0,2,3])];
      score += map[ TransSqIndex(cpuStone*sq[1,3,0]), TransSqIndex(cpuStone*sq[1,3,1]), TransSqIndex(cpuStone*sq[1,3,2]), TransSqIndex(cpuStone*sq[1,3,3])];
      score += map[ TransSqIndex(cpuStone*sq[2,3,0]), TransSqIndex(cpuStone*sq[2,3,1]), TransSqIndex(cpuStone*sq[2,3,2]), TransSqIndex(cpuStone*sq[2,3,3])];
      score += map[ TransSqIndex(cpuStone*sq[3,2,0]), TransSqIndex(cpuStone*sq[3,2,1]), TransSqIndex(cpuStone*sq[3,2,2]), TransSqIndex(cpuStone*sq[3,2,3])];
      score += map[ TransSqIndex(cpuStone*sq[3,1,0]), TransSqIndex(cpuStone*sq[3,1,1]), TransSqIndex(cpuStone*sq[3,1,2]), TransSqIndex(cpuStone*sq[3,1,3])];
      score += map[ TransSqIndex(cpuStone*sq[2,0,0]), TransSqIndex(cpuStone*sq[2,0,1]), TransSqIndex(cpuStone*sq[2,0,2]), TransSqIndex(cpuStone*sq[2,0,3])];

      score += map[ TransSqIndex(cpuStone*sq[0,0,0]), TransSqIndex(cpuStone*sq[1,0,1]), TransSqIndex(cpuStone*sq[2,0,2]), TransSqIndex(cpuStone*sq[3,0,3])];
      score += map[ TransSqIndex(cpuStone*sq[0,0,3]), TransSqIndex(cpuStone*sq[1,0,2]), TransSqIndex(cpuStone*sq[2,0,1]), TransSqIndex(cpuStone*sq[3,0,0])];
      score += map[ TransSqIndex(cpuStone*sq[0,3,0]), TransSqIndex(cpuStone*sq[1,3,1]), TransSqIndex(cpuStone*sq[2,3,2]), TransSqIndex(cpuStone*sq[3,3,3])];
      score += map[ TransSqIndex(cpuStone*sq[0,3,3]), TransSqIndex(cpuStone*sq[1,3,2]), TransSqIndex(cpuStone*sq[2,3,1]), TransSqIndex(cpuStone*sq[3,3,0])];

      score += map[ TransSqIndex(cpuStone*sq[0,0,0]), TransSqIndex(cpuStone*sq[1,1,0]), TransSqIndex(cpuStone*sq[2,2,0]), TransSqIndex(cpuStone*sq[3,3,0])];
      score += map[ TransSqIndex(cpuStone*sq[3,0,0]), TransSqIndex(cpuStone*sq[2,1,0]), TransSqIndex(cpuStone*sq[1,2,0]), TransSqIndex(cpuStone*sq[0,3,0])];
      score += map[ TransSqIndex(cpuStone*sq[0,0,3]), TransSqIndex(cpuStone*sq[1,1,3]), TransSqIndex(cpuStone*sq[2,2,3]), TransSqIndex(cpuStone*sq[3,3,3])];
      score += map[ TransSqIndex(cpuStone*sq[3,0,3]), TransSqIndex(cpuStone*sq[2,1,3]), TransSqIndex(cpuStone*sq[1,2,3]), TransSqIndex(cpuStone*sq[0,3,3])];

      score += map[ TransSqIndex(cpuStone*sq[0,0,0]), TransSqIndex(cpuStone*sq[0,1,1]), TransSqIndex(cpuStone*sq[0,2,2]), TransSqIndex(cpuStone*sq[0,3,3])];
      score += map[ TransSqIndex(cpuStone*sq[0,3,0]), TransSqIndex(cpuStone*sq[0,2,1]), TransSqIndex(cpuStone*sq[0,1,2]), TransSqIndex(cpuStone*sq[0,0,3])];
      score += map[ TransSqIndex(cpuStone*sq[3,0,0]), TransSqIndex(cpuStone*sq[3,1,1]), TransSqIndex(cpuStone*sq[3,2,2]), TransSqIndex(cpuStone*sq[3,3,3])];
      score += map[ TransSqIndex(cpuStone*sq[3,3,0]), TransSqIndex(cpuStone*sq[3,2,1]), TransSqIndex(cpuStone*sq[3,1,2]), TransSqIndex(cpuStone*sq[3,0,3])];

      return score;
    }

    private int TransSqIndex(int n) //0->0,1->1,-1->2
    {
      return (3 * n * n - n) / 2;
    }

    public int XLength{ get{ return xLength; } set{xLength = value; } }
    public int YLength{ get{ return yLength; } set{yLength = value; } }
    public int ZLength{ get{ return zLength; } set{zLength = value; } }
    public int[,] Vector{ get{ return vector; } set{vector = value; } }

    public void MapCopy()
    {
      Array.Copy(basicMap1,basicMap1Random,basicMap1.Length);
      Array.Copy(basicMap2,basicMap2Random,basicMap2.Length);
      Array.Copy(basicMap3,basicMap3Random,basicMap3.Length);
      Array.Copy(sideMap,sideMapRandom,sideMap.Length);
      Array.Copy(surfaceMap,surfaceMapRandom,surfaceMap.Length);
    }

    public void RandomMap()//ランダムCPUを生成/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    {
      float epsilon; //epsilon-greedy法
      float rv; //盤面評価値の修正用乱数
/*
      epsilon = UnityEngine.Random.value;
      if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; basicMap1Random[0,0,1] = rv; basicMap1Random[0,0,2] = rv; basicMap1Random[0,1,0] = rv; basicMap1Random[0,1,3] = rv; basicMap1Random[0,2,0] = rv; basicMap1Random[0,2,3] = rv; basicMap1Random[0,3,1] = rv; basicMap1Random[0,3,2] = rv; basicMap1Random[1,0,0] = rv; basicMap1Random[1,0,3] = rv; basicMap1Random[1,3,0] = rv; basicMap1Random[1,3,3] = rv; basicMap1Random[2,0,0] = rv; basicMap1Random[2,0,3] = rv; basicMap1Random[2,3,0] = rv; basicMap1Random[2,3,3] = rv; basicMap1Random[3,0,1] = rv; basicMap1Random[3,0,2] = rv; basicMap1Random[3,1,0] = rv; basicMap1Random[3,1,3] = rv; basicMap1Random[3,2,0] = rv; basicMap1Random[3,2,3] = rv; basicMap1Random[3,3,1] = rv; basicMap1Random[3,3,2] = rv; }
      epsilon = UnityEngine.Random.value;
      if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; basicMap1Random[0,1,1] = rv; basicMap1Random[0,1,2] = rv; basicMap1Random[0,2,1] = rv; basicMap1Random[0,2,2] = rv; basicMap1Random[1,0,1] = rv; basicMap1Random[1,0,2] = rv; basicMap1Random[1,1,0] = rv; basicMap1Random[1,1,3] = rv; basicMap1Random[1,2,0] = rv; basicMap1Random[1,2,3] = rv; basicMap1Random[1,3,1] = rv; basicMap1Random[1,3,2] = rv; basicMap1Random[2,0,1] = rv; basicMap1Random[2,0,2] = rv; basicMap1Random[2,1,0] = rv; basicMap1Random[2,1,3] = rv; basicMap1Random[2,2,0] = rv; basicMap1Random[2,2,3] = rv; basicMap1Random[2,3,1] = rv; basicMap1Random[2,3,2] = rv; basicMap1Random[3,1,1] = rv; basicMap1Random[3,1,2] = rv; basicMap1Random[3,2,1] = rv; basicMap1Random[3,2,2] = rv; }
      epsilon = UnityEngine.Random.value;
      if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; basicMap1Random[1,1,1] = rv; basicMap1Random[1,1,2] = rv; basicMap1Random[1,2,1] = rv; basicMap1Random[1,2,2] = rv; basicMap1Random[2,1,1] = rv; basicMap1Random[2,1,2] = rv; basicMap1Random[2,2,1] = rv; basicMap1Random[2,2,2] = rv; }

      epsilon = UnityEngine.Random.value;
      if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value; basicMap2Random[0,0,0] = rv; basicMap2Random[0,0,3] = rv; basicMap2Random[0,3,0] = rv; basicMap2Random[0,3,3] = rv; basicMap2Random[3,0,0] = rv; basicMap2Random[3,0,3] = rv; basicMap2Random[3,3,0] = rv; basicMap2Random[3,3,3] = rv; }
      epsilon = UnityEngine.Random.value;
      if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; basicMap2Random[0,0,1] = rv; basicMap2Random[0,0,2] = rv; basicMap2Random[0,1,0] = rv; basicMap2Random[0,1,3] = rv; basicMap2Random[0,2,0] = rv; basicMap2Random[0,2,3] = rv; basicMap2Random[0,3,1] = rv; basicMap2Random[0,3,2] = rv; basicMap2Random[1,0,0] = rv; basicMap2Random[1,0,3] = rv; basicMap2Random[1,3,0] = rv; basicMap2Random[1,3,3] = rv; basicMap2Random[2,0,0] = rv; basicMap2Random[2,0,3] = rv; basicMap2Random[2,3,0] = rv; basicMap2Random[2,3,3] = rv; basicMap2Random[3,0,1] = rv; basicMap2Random[3,0,2] = rv; basicMap2Random[3,1,0] = rv; basicMap2Random[3,1,3] = rv; basicMap2Random[3,2,0] = rv; basicMap2Random[3,2,3] = rv; basicMap2Random[3,3,1] = rv; basicMap2Random[3,3,2] = rv; }
      epsilon = UnityEngine.Random.value;
      if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; basicMap2Random[0,1,1] = rv; basicMap2Random[0,1,2] = rv; basicMap2Random[0,2,1] = rv; basicMap2Random[0,2,2] = rv; basicMap2Random[1,0,1] = rv; basicMap2Random[1,0,2] = rv; basicMap2Random[1,1,0] = rv; basicMap2Random[1,1,3] = rv; basicMap2Random[1,2,0] = rv; basicMap2Random[1,2,3] = rv; basicMap2Random[1,3,1] = rv; basicMap2Random[1,3,2] = rv; basicMap2Random[2,0,1] = rv; basicMap2Random[2,0,2] = rv; basicMap2Random[2,1,0] = rv; basicMap2Random[2,1,3] = rv; basicMap2Random[2,2,0] = rv; basicMap2Random[2,2,3] = rv; basicMap2Random[2,3,1] = rv; basicMap2Random[2,3,2] = rv; basicMap2Random[3,1,1] = rv; basicMap2Random[3,1,2] = rv; basicMap2Random[3,2,1] = rv; basicMap2Random[3,2,2] = rv; }
      epsilon = UnityEngine.Random.value;
      if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; basicMap2Random[1,1,1] = rv; basicMap2Random[1,1,2] = rv; basicMap2Random[1,2,1] = rv; basicMap2Random[1,2,2] = rv; basicMap2Random[2,1,1] = rv; basicMap2Random[2,1,2] = rv; basicMap2Random[2,2,1] = rv; basicMap2Random[2,2,2] = rv; }

      epsilon = UnityEngine.Random.value;
      if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value; basicMap3Random[0,0,0] = rv; basicMap3Random[0,0,3] = rv; basicMap3Random[0,3,0] = rv; basicMap3Random[0,3,3] = rv; basicMap3Random[3,0,0] = rv; basicMap3Random[3,0,3] = rv; basicMap3Random[3,3,0] = rv; basicMap3Random[3,3,3] = rv; }
      epsilon = UnityEngine.Random.value;
      if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; basicMap3Random[0,0,1] = rv; basicMap3Random[0,0,2] = rv; basicMap3Random[0,1,0] = rv; basicMap3Random[0,1,3] = rv; basicMap3Random[0,2,0] = rv; basicMap3Random[0,2,3] = rv; basicMap3Random[0,3,1] = rv; basicMap3Random[0,3,2] = rv; basicMap3Random[1,0,0] = rv; basicMap3Random[1,0,3] = rv; basicMap3Random[1,3,0] = rv; basicMap3Random[1,3,3] = rv; basicMap3Random[2,0,0] = rv; basicMap3Random[2,0,3] = rv; basicMap3Random[2,3,0] = rv; basicMap3Random[2,3,3] = rv; basicMap3Random[3,0,1] = rv; basicMap3Random[3,0,2] = rv; basicMap3Random[3,1,0] = rv; basicMap3Random[3,1,3] = rv; basicMap3Random[3,2,0] = rv; basicMap3Random[3,2,3] = rv; basicMap3Random[3,3,1] = rv; basicMap3Random[3,3,2] = rv; }
      epsilon = UnityEngine.Random.value;
      if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; basicMap3Random[0,1,1] = rv; basicMap3Random[0,1,2] = rv; basicMap3Random[0,2,1] = rv; basicMap3Random[0,2,2] = rv; basicMap3Random[1,0,1] = rv; basicMap3Random[1,0,2] = rv; basicMap3Random[1,1,0] = rv; basicMap3Random[1,1,3] = rv; basicMap3Random[1,2,0] = rv; basicMap3Random[1,2,3] = rv; basicMap3Random[1,3,1] = rv; basicMap3Random[1,3,2] = rv; basicMap3Random[2,0,1] = rv; basicMap3Random[2,0,2] = rv; basicMap3Random[2,1,0] = rv; basicMap3Random[2,1,3] = rv; basicMap3Random[2,2,0] = rv; basicMap3Random[2,2,3] = rv; basicMap3Random[2,3,1] = rv; basicMap3Random[2,3,2] = rv; basicMap3Random[3,1,1] = rv; basicMap3Random[3,1,2] = rv; basicMap3Random[3,2,1] = rv; basicMap3Random[3,2,2] = rv; }
      epsilon = UnityEngine.Random.value;
      if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; basicMap3Random[1,1,1] = rv; basicMap3Random[1,1,2] = rv; basicMap3Random[1,2,1] = rv; basicMap3Random[1,2,2] = rv; basicMap3Random[2,1,1] = rv; basicMap3Random[2,1,2] = rv; basicMap3Random[2,2,1] = rv; basicMap3Random[2,2,2] = rv; }
*/
      epsilon = UnityEngine.Random.value;
      if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; sideMapRandom[0,0,1,1] = rv; sideMapRandom[0,0,2,2] = -rv; sideMapRandom[1,1,0,0] = rv; sideMapRandom[2,2,0,0] = -rv; }
      epsilon = UnityEngine.Random.value;
      if(epsilon < 0.3f){ rv = 50f * UnityEngine.Random.value; sideMapRandom[0,0,2,1] = rv; sideMapRandom[0,0,1,2] = -rv; sideMapRandom[1,2,0,0] = rv; sideMapRandom[2,1,0,0] = -rv; }
      epsilon = UnityEngine.Random.value;
      if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; sideMapRandom[0,2,0,2] = rv; sideMapRandom[0,1,0,1] = -rv; sideMapRandom[2,0,2,0] = rv; sideMapRandom[1,0,1,0] = -rv; }
      epsilon = UnityEngine.Random.value;
      if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; sideMapRandom[0,2,0,1] = rv; sideMapRandom[0,1,0,2] = -rv; sideMapRandom[1,0,2,0] = rv; sideMapRandom[2,0,1,0] = -rv; }
      epsilon = UnityEngine.Random.value;
      if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value; sideMapRandom[0,1,2,0] = -rv; sideMapRandom[0,2,1,0] = -rv; }
      epsilon = UnityEngine.Random.value;
      if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; sideMapRandom[0,1,1,1] = rv; sideMapRandom[0,2,2,2] = -rv; sideMapRandom[1,1,1,0] = rv; sideMapRandom[2,2,2,0] = -rv; }
      epsilon = UnityEngine.Random.value;
      if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value; sideMapRandom[0,2,2,1] = rv; sideMapRandom[0,1,1,2] = -rv; sideMapRandom[1,2,2,0] = rv; sideMapRandom[2,1,1,0] = -rv; }
      epsilon = UnityEngine.Random.value;
      if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value; sideMapRandom[0,2,1,2] = rv; sideMapRandom[0,1,2,1] = -rv; sideMapRandom[2,1,2,0] = rv; sideMapRandom[1,2,1,0] = -rv; }
      epsilon = UnityEngine.Random.value;
      if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value; sideMapRandom[0,2,1,1] = rv; sideMapRandom[0,1,2,2] = -rv; sideMapRandom[1,1,2,0] = rv; sideMapRandom[2,2,1,0] = -rv; }
      epsilon = UnityEngine.Random.value;
      if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; sideMapRandom[2,0,2,2] = rv; sideMapRandom[1,0,1,1] = -rv; sideMapRandom[2,2,0,2] = rv; sideMapRandom[1,1,0,1] = -rv; }
      epsilon = UnityEngine.Random.value;
      if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; sideMapRandom[2,0,1,1] = rv; sideMapRandom[1,0,2,2] = -rv; sideMapRandom[1,1,0,2] = rv; sideMapRandom[2,2,0,1] = -rv; }
      epsilon = UnityEngine.Random.value;
      if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value; sideMapRandom[1,0,2,1] = rv; sideMapRandom[2,0,1,2] = -rv; sideMapRandom[1,2,0,1] = rv; sideMapRandom[2,1,0,2] = -rv; }
      epsilon = UnityEngine.Random.value;
      if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value; sideMapRandom[2,0,2,1] = rv; sideMapRandom[1,0,1,2] = -rv; sideMapRandom[1,2,0,2] = rv; sideMapRandom[2,1,0,1] = -rv; }
      epsilon = UnityEngine.Random.value;
      if(epsilon < 0.3f){ rv = 150f * UnityEngine.Random.value - 50f; sideMapRandom[1,1,1,1] = rv; sideMapRandom[2,2,2,2] = -rv; }
      epsilon = UnityEngine.Random.value;
      if(epsilon < 0.3f){ rv = 150f * UnityEngine.Random.value - 50f; sideMapRandom[1,1,1,2] = rv; sideMapRandom[2,2,2,1] = -rv; sideMapRandom[2,1,1,1] = rv; sideMapRandom[1,2,2,2] = -rv; }
      epsilon = UnityEngine.Random.value;
      if(epsilon < 0.3f){ rv = 200f * UnityEngine.Random.value - 100f; sideMapRandom[2,2,1,2] = rv; sideMapRandom[1,1,2,1] = -rv; sideMapRandom[2,1,2,2] = rv; sideMapRandom[1,2,1,1] = -rv; }
      epsilon = UnityEngine.Random.value;
      if(epsilon < 0.3f){ rv = 150f * UnityEngine.Random.value - 50f; sideMapRandom[2,1,1,2] = rv; sideMapRandom[1,2,2,1] = -rv; }

      epsilon = UnityEngine.Random.value;
      if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; surfaceMapRandom[0,0,1,1] = rv; surfaceMapRandom[0,0,2,2] = -rv; surfaceMapRandom[1,1,0,0] = rv; surfaceMapRandom[2,2,0,0] = -rv; }
      epsilon = UnityEngine.Random.value;
      if(epsilon < 0.3f){ rv = 50f * UnityEngine.Random.value; surfaceMapRandom[0,0,2,1] = rv; surfaceMapRandom[0,0,1,2] = -rv; surfaceMapRandom[1,2,0,0] = rv; surfaceMapRandom[2,1,0,0] = -rv; }
      epsilon = UnityEngine.Random.value;
      if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; surfaceMapRandom[0,2,0,2] = rv; surfaceMapRandom[0,1,0,1] = -rv; surfaceMapRandom[2,0,2,0] = rv; surfaceMapRandom[1,0,1,0] = -rv; }
      epsilon = UnityEngine.Random.value;
      if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; surfaceMapRandom[0,2,0,1] = rv; surfaceMapRandom[0,1,0,2] = -rv; surfaceMapRandom[1,0,2,0] = rv; surfaceMapRandom[2,0,1,0] = -rv; }
      epsilon = UnityEngine.Random.value;
      if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value; surfaceMapRandom[0,1,2,0] = -rv; surfaceMapRandom[0,2,1,0] = -rv; }
      epsilon = UnityEngine.Random.value;
      if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; surfaceMapRandom[0,1,1,1] = rv; surfaceMapRandom[0,2,2,2] = -rv; surfaceMapRandom[1,1,1,0] = rv; surfaceMapRandom[2,2,2,0] = -rv; }
      epsilon = UnityEngine.Random.value;
      if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value; surfaceMapRandom[0,2,2,1] = rv; surfaceMapRandom[0,1,1,2] = -rv; surfaceMapRandom[1,2,2,0] = rv; surfaceMapRandom[2,1,1,0] = -rv; }
      epsilon = UnityEngine.Random.value;
      if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value; surfaceMapRandom[0,2,1,2] = rv; surfaceMapRandom[0,1,2,1] = -rv; surfaceMapRandom[2,1,2,0] = rv; surfaceMapRandom[1,2,1,0] = -rv; }
      epsilon = UnityEngine.Random.value;
      if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value; surfaceMapRandom[0,2,1,1] = rv; surfaceMapRandom[0,1,2,2] = -rv; surfaceMapRandom[1,1,2,0] = rv; surfaceMapRandom[2,2,1,0] = -rv; }
      epsilon = UnityEngine.Random.value;
      if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; surfaceMapRandom[2,0,2,2] = rv; surfaceMapRandom[1,0,1,1] = -rv; surfaceMapRandom[2,2,0,2] = rv; surfaceMapRandom[1,1,0,1] = -rv; }
      epsilon = UnityEngine.Random.value;
      if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; surfaceMapRandom[2,0,1,1] = rv; surfaceMapRandom[1,0,2,2] = -rv; surfaceMapRandom[1,1,0,2] = rv; surfaceMapRandom[2,2,0,1] = -rv; }
      epsilon = UnityEngine.Random.value;
      if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value; surfaceMapRandom[1,0,2,1] = rv; surfaceMapRandom[2,0,1,2] = -rv; surfaceMapRandom[1,2,0,1] = rv; surfaceMapRandom[2,1,0,2] = -rv; }
      epsilon = UnityEngine.Random.value;
      if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value; surfaceMapRandom[2,0,2,1] = rv; surfaceMapRandom[1,0,1,2] = -rv; surfaceMapRandom[1,2,0,2] = rv; surfaceMapRandom[2,1,0,1] = -rv; }
      epsilon = UnityEngine.Random.value;
      if(epsilon < 0.3f){ rv = 150f * UnityEngine.Random.value - 50f; surfaceMapRandom[1,1,1,1] = rv; surfaceMapRandom[2,2,2,2] = -rv; }
      epsilon = UnityEngine.Random.value;
      if(epsilon < 0.3f){ rv = 150f * UnityEngine.Random.value - 50f; surfaceMapRandom[1,1,1,2] = rv; surfaceMapRandom[2,2,2,1] = -rv; surfaceMapRandom[2,1,1,1] = rv; surfaceMapRandom[1,2,2,2] = -rv; }
      epsilon = UnityEngine.Random.value;
      if(epsilon < 0.3f){ rv = 200f * UnityEngine.Random.value - 100f; surfaceMapRandom[2,2,1,2] = rv; surfaceMapRandom[1,1,2,1] = -rv; surfaceMapRandom[2,1,2,2] = rv; surfaceMapRandom[1,2,1,1] = -rv; }
      epsilon = UnityEngine.Random.value;
      if(epsilon < 0.3f){ rv = 150f * UnityEngine.Random.value - 50f; surfaceMapRandom[2,1,1,2] = rv; surfaceMapRandom[1,2,2,1] = -rv; }

    }//ここまで//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    public void TextLog()
    {
      if(texts.deleteText("/Resources/TextFile1.txt")){}else{Debug.Log("Error;");}
      if(texts.deleteText("/Resources/TextFile2.txt")){}else{Debug.Log("Error;");}
      if(texts.deleteText("/Resources/TextFile3.txt")){}else{Debug.Log("Error;");}
      if(texts.deleteText("/Resources/TextFile4.txt")){}else{Debug.Log("Error;");}
      if(texts.deleteText("/Resources/TextFile5.txt")){}else{Debug.Log("Error;");}
      for(int y=0; y<yLength; y++)
      {
        for(int z=0; z<zLength; z++)
        {
          for(int x=0; x<xLength; x++)
          {
            basicMap1[x,y,z] = basicMap1Random[x,y,z];
            if(texts.saveText("/Resources/TextFile1.txt", basicMap1[x,y,z].ToString() + ",")){}else{Debug.Log("Error;");}
            basicMap2[x,y,z] = basicMap2Random[x,y,z];
            if(texts.saveText("/Resources/TextFile2.txt", basicMap2[x,y,z].ToString() + ",")){}else{Debug.Log("Error;");}
            basicMap3[x,y,z] = basicMap3Random[x,y,z];
            if(texts.saveText("/Resources/TextFile3.txt", basicMap3[x,y,z].ToString() + ",")){}else{Debug.Log("Error;");}
          }
        }
      }
      for(int a1=0; a1<3; a1++)
      {
        for(int a2=0; a2<3; a2++)
        {
          for(int a3=0; a3<3; a3++)
          {
            for(int a4=0; a4<3; a4++)
            {
              sideMap[a1,a2,a3,a4] = sideMapRandom[a1,a2,a3,a4];
              if(texts.saveText("/Resources/TextFile4.txt", sideMap[a1,a2,a3,a4].ToString() + ",")){}else{Debug.Log("Error;");}
              surfaceMap[a1,a2,a3,a4] = surfaceMapRandom[a1,a2,a3,a4];
              if(texts.saveText("/Resources/TextFile5.txt", surfaceMap[a1,a2,a3,a4].ToString() + ",")){}else{Debug.Log("Error;");}
            }
          }
        }
      }
    }

    public void TextDetalog()
    {
        for(int y=0; y<yLength; y++)
        {
          for(int z=0; z<zLength; z++)
          {
            for(int x=0; x<xLength; x++)
            {
              basicMap1[x,y,z] = basicMap1Random[x,y,z];
              if(texts.saveText("/Resources/TextFile1Deta.txt", basicMap1[x,y,z].ToString() + ",")){}else{Debug.Log("Error;");}
            }
          }
        }
        if(texts.saveText("/Resources/TextFile1Deta.txt", "\n")){}else{Debug.Log("Error;");}

        for(int y=0; y<yLength; y++)
        {
          for(int z=0; z<zLength; z++)
          {
            for(int x=0; x<xLength; x++)
            {
              basicMap2[x,y,z] = basicMap2Random[x,y,z];
              if(texts.saveText("/Resources/TextFile2Deta.txt", basicMap2[x,y,z].ToString() + ",")){}else{Debug.Log("Error;");}
            }
          }
        }
        if(texts.saveText("/Resources/TextFile2Deta.txt", "\n")){}else{Debug.Log("Error;");}

        for(int y=0; y<yLength; y++)
        {
          for(int z=0; z<zLength; z++)
          {
            for(int x=0; x<xLength; x++)
            {
              basicMap3[x,y,z] = basicMap3Random[x,y,z];
              if(texts.saveText("/Resources/TextFile3Deta.txt", basicMap3[x,y,z].ToString() + ",")){}else{Debug.Log("Error;");}
            }
          }
        }
        if(texts.saveText("/Resources/TextFile3Deta.txt", "\n")){}else{Debug.Log("Error;");}

        for(int a1=0; a1<3; a1++)
        {
          for(int a2=0; a2<3; a2++)
          {
            for(int a3=0; a3<3; a3++)
            {
              for(int a4=0; a4<3; a4++)
              {
                if(texts.saveText("/Resources/TextFile4Deta.txt", sideMap[a1,a2,a3,a4].ToString() + ",")){}else{Debug.Log("Error;");}
              }
            }
          }
        }
        if(texts.saveText("/Resources/TextFile4Deta.txt", "\n")){}else{Debug.Log("Error;");}

        for(int a1=0; a1<3; a1++)
        {
          for(int a2=0; a2<3; a2++)
          {
            for(int a3=0; a3<3; a3++)
            {
              for(int a4=0; a4<3; a4++)
              {
                if(texts.saveText("/Resources/TextFile5Deta.txt", surfaceMap[a1,a2,a3,a4].ToString() + ",")){}else{Debug.Log("Error;");}
              }
            }
          }
        }
        if(texts.saveText("/Resources/TextFile5Deta.txt", "\n")){}else{Debug.Log("Error;");};
    }

    public void LearningFinish()
    {
      Debug.Log("Learning successed.");
      EditorApplication.isPlaying = false;
      //SceneManager.LoadScene("Trainning");
    }
}
