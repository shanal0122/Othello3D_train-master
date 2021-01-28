using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class CPU : MonoBehaviour
{
    int xLength;
    int yLength;
    int zLength;
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

    float[,,] basicMap1_464; //各座標の基本点。座標(0,0,0)に自分の石があればbasicMap1[0,0,0]点入る
    float[,,] basicMap2_464;
    float[,,] basicMap3_464;
    float[,,,] sideMap464; //長さ4の辺の石組み合わせに関する追加点。0:nostone,1:自分の石,2:相手の石
    float[,,] sideLongMap464; //長さ6の辺を半分に割ったものの石組み合わせに関する追加点。0:nostone,1:自分の石,2:相手の石
    float[,,,] surfaceMap464; //各面上の長さ4の直線上の石の組み合わせに関する追加点。0:nostone,1:自分の石,2:相手の石
    float[,,] surfaceLongMap464; //各面上の長さ6の直線を半分に割ったものの石の組み合わせに関する追加点。0:nostone,1:自分の石,2:相手の石

    float[,,] basicMap1_464Random;
    float[,,] basicMap2_464Random;
    float[,,] basicMap3_464Random;
    float[,,,] sideMap464Random;
    float[,,] sideLongMap464Random;
    float[,,,] surfaceMap464Random;
    float[,,] surfaceLongMap464Random;

    public GameSystem gameSystem;
    public Texts texts;



    void Awake()
    {
      xLength = gameSystem.XLength;
      yLength = gameSystem.YLength;
      zLength = gameSystem.ZLength;
      basicMap1 = new float[,,]
      {
          { {100f,6f,6f,100f}, {6f, -12f, -12f, 6f}, {6f, -12f, -12f, 6f}, {100f,6f,6f,100f} },
          { {6f, -12f, -12f, 6f}, {-12f, 30f, 30f, -12f}, {-12f, 30f, 30f, -12f}, {6f, -12f, -12f, 6f} },
          { {6f, -12f, -12f, 6f}, {-12f, 30f, 30f, -12f}, {-12f, 30f, 30f, -12f}, {6f, -12f, -12f, 6f} },
          { {100f,6f,6f,100f}, {6f, -12f, -12f, 6f}, {6f, -12f, -12f, 6f}, {100f,6f,6f,100f} }
      };
      basicMap2 = new float[,,]
      {
          { {99f,-44f,-44f,99f}, {-44f, -36f, -36f, -44f}, {-44f, -36f, -36f, -44f}, {99f,-44f,-44f,99f} },
          { {-44f, -36f, -36f, -44f}, {-36f, 2f, 2f, -36f}, {-36f, 2f, 2f, -36f}, {-44f, -36f, -36f, -44f} },
          { {-44f, -36f, -36f, -44f}, {-36f, 2f, 2f, -36f}, {-36f, 2f, 2f, -36f}, {-44f, -36f, -36f, -44f} },
          { {99f,-44f,-44f,99f}, {-44f, -36f, -36f, -44f}, {-44f, -36f, -36f, -44f}, {99f,-44f,-44f,99f} }
      };
      basicMap3 = new float[,,]
      {
          { {95f,3f,3f,95f}, {3f, -49f, -49f, 3f}, {3f, -49f, -49f, 3f}, {95f,3f,3f,95f} },
          { {3f, -49f, -49f, 3f}, {-49f, -30f, -30f, -49f}, {-49f, -30f, -30f, -49f}, {3f, -49f, -49f, 3f} },
          { {3f, -49f, -49f, 3f}, {-49f, -30f, -30f, -49f}, {-49f, -30f, -30f, -49f}, {3f, -49f, -49f, 3f} },
          { {95f,3f,3f,95f}, {3f, -49f, -49f, 3f}, {3f, -49f, -49f, 3f}, {95f,3f,3f,95f} }
      };
      sideMap = new float[,,,]
      {
          {
            { {0f, 0f, 0f}, {0f, 0f, -20f}, {0f, 20f, 0f} },
            { {0f, 5f, -18f}, {0f, 18f, -20f}, {-85f, -30f, -57f} },
            { {0f, 18f, -5f}, {-85f, 57f, 30f}, {0f, 20f, -18f} }
          },
          {
            { {0f, 0f, 0f}, {5f, 8f, -20f}, {18f, 38f, -20f} },
            { {0f, 8f, 20f}, {18f, 65f, -5f}, {57f, -34f, 0f} },
            { {20f, 38f, 20f}, {-30f, -34f, 0f}, {20f, -60f, 5f} }
          },
          {
            { {0f, 0f, 0f}, {-18f, 20f, -38f}, {-5f, 20f, -8f} },
            { {-20f, -20f, -38f}, {-20f, -5f, 60f}, {30f, 0f, 34f} },
            { {0f, -20f, -8f}, {-57f, 0f, 34f}, {-18f, 5f, -65f} }
          }
      };
      surfaceMap = new float[,,,]
         {
             {
               { {0f, 0f, 0f}, {0f, -18f, -40f}, {0f, 40f, 18f} },
               { {0f, 3f, -6f}, {0f, 15f, -50f}, {-40f, -45f, -64f} },
               { {0f, 6f, -3f}, {-40f, 64f, 45f}, {0f, 50f, -15f} }
             },
             {
               { {0f, 0f, 0f}, {3f, -3f, -67f}, {6f, 25f, -20f} },
               { {-18f, -3f, 20f}, {15f, 66f, 70f}, {64f, 40f, 0f} },
               { {40f, 25f, 67f}, {-45f, 40f, 0f}, {50f, -40f, -70f} }
             },
             {
               { {0f, 0f, 0f}, {-6f, 20f, -25f}, {-3f, 67f, 3f} },
               { {-40f, -67f, -25f}, {-50f, 70f, 40f}, {45f, 0f, -40f} },
               { {18f, -20f, 3f}, {-64f, 0f, -40f}, {-15f, -70f, -66f} }
             }
        };

        basicMap1_464 = new float[,,]
        {
          { {100f,6f,6f,100f}, {6f, -12f, -12f, 6f}, {6f, -12f, -12f, 6f}, {6f, -12f, -12f, 6f}, {6f, -12f, -12f, 6f}, {100f,6f,6f,100f} },
          { {6f, -12f, -12f, 6f}, {-12f, 30f, 30f, -12f}, {-12f, 30f, 30f, -12f}, {-12f, 30f, 30f, -12f}, {-12f, 30f, 30f, -12f}, {6f, -12f, -12f, 6f} },
          { {6f, -12f, -12f, 6f}, {-12f, 30f, 30f, -12f}, {-12f, 30f, 30f, -12f}, {-12f, 30f, 30f, -12f}, {-12f, 30f, 30f, -12f}, {6f, -12f, -12f, 6f} },
          { {100f,6f,6f,100f}, {6f, -12f, -12f, 6f}, {6f, -12f, -12f, 6f}, {6f, -12f, -12f, 6f}, {6f, -12f, -12f, 6f}, {100f,6f,6f,100f} }
        };
        basicMap2_464 = new float[,,]
        {
          { {99f,-44f,-44f,99f}, {-44f, -36f, -36f, -44f}, {-44f, -36f, -36f, -44f}, {-44f, -36f, -36f, -44f}, {-44f, -36f, -36f, -44f}, {99f,-44f,-44f,99f} },
          { {-44f, -36f, -36f, -44f}, {-36f, 2f, 2f, -36f}, {-36f, 2f, 2f, -36f}, {-36f, 2f, 2f, -36f}, {-36f, 2f, 2f, -36f}, {-44f, -36f, -36f, -44f} },
          { {-44f, -36f, -36f, -44f}, {-36f, 2f, 2f, -36f}, {-36f, 2f, 2f, -36f}, {-36f, 2f, 2f, -36f}, {-36f, 2f, 2f, -36f}, {-44f, -36f, -36f, -44f} },
          { {99f,-44f,-44f,99f}, {-44f, -36f, -36f, -44f}, {-44f, -36f, -36f, -44f}, {-44f, -36f, -36f, -44f}, {-44f, -36f, -36f, -44f}, {99f,-44f,-44f,99f} }
        };
        basicMap3_464 = new float[,,]
        {
          { {95f,3f,3f,95f}, {3f, -49f, -49f, 3f}, {3f, -49f, -49f, 3f}, {3f, -49f, -49f, 3f}, {3f, -49f, -49f, 3f}, {95f,3f,3f,95f} },
          { {3f, -49f, -49f, 3f}, {-49f, -30f, -30f, -49f}, {-49f, -30f, -30f, -49f}, {-49f, -30f, -30f, -49f}, {-49f, -30f, -30f, -49f}, {3f, -49f, -49f, 3f} },
          { {3f, -49f, -49f, 3f}, {-49f, -30f, -30f, -49f}, {-49f, -30f, -30f, -49f}, {-49f, -30f, -30f, -49f}, {-49f, -30f, -30f, -49f}, {3f, -49f, -49f, 3f} },
          { {95f,3f,3f,95f}, {3f, -49f, -49f, 3f}, {3f, -49f, -49f, 3f}, {3f, -49f, -49f, 3f}, {3f, -49f, -49f, 3f}, {95f,3f,3f,95f} }
        };
        sideMap464 = new float[,,,]
        {
          {
            { {0f, 0f, 0f}, {0f, 0f, -20f}, {0f, 20f, 0f} },
            { {0f, 5f, -18f}, {0f, 18f, -20f}, {-85f, -30f, -57f} },
            { {0f, 18f, -5f}, {-85f, 57f, 30f}, {0f, 20f, -18f} }
          },
          {
            { {0f, 0f, 0f}, {5f, 8f, -20f}, {18f, 38f, -20f} },
            { {0f, 8f, 20f}, {18f, 65f, -5f}, {57f, -34f, 0f} },
            { {20f, 38f, 20f}, {-30f, -34f, 0f}, {20f, -60f, 5f} }
          },
          {
            { {0f, 0f, 0f}, {-18f, 20f, -38f}, {-5f, 20f, -8f} },
            { {-20f, -20f, -38f}, {-20f, -5f, 60f}, {30f, 0f, 34f} },
            { {0f, -20f, -8f}, {-57f, 0f, 34f}, {-18f, 5f, -65f} }
          }
        };
        sideLongMap464 = new float[,,]
        {
          { {0f, 0f, 0f}, {0f, 0f, 0f}, {0f, 0f, 0f} },
          { {0f, 0f, 0f}, {0f, 0f, 0f}, {0f, 0f, 0f} },
          { {0f, 0f, 0f}, {0f, 0f, 0f}, {0f, 0f, 0f} },
        };
        surfaceMap464 = new float[,,,]
        {
          {
               { {0f, 0f, 0f}, {0f, -18f, -40f}, {0f, 40f, 18f} },
               { {0f, 3f, -6f}, {0f, 15f, -50f}, {-40f, -45f, -64f} },
               { {0f, 6f, -3f}, {-40f, 64f, 45f}, {0f, 50f, -15f} }
          },
          {
               { {0f, 0f, 0f}, {3f, -3f, -67f}, {6f, 25f, -20f} },
               { {-18f, -3f, 20f}, {15f, 66f, 70f}, {64f, 40f, 0f} },
               { {40f, 25f, 67f}, {-45f, 40f, 0f}, {50f, -40f, -70f} }
          },
          {
               { {0f, 0f, 0f}, {-6f, 20f, -25f}, {-3f, 67f, 3f} },
               { {-40f, -67f, -25f}, {-50f, 70f, 40f}, {45f, 0f, -40f} },
               { {18f, -20f, 3f}, {-64f, 0f, -40f}, {-15f, -70f, -66f} }
          }
        };
        surfaceLongMap464 = new float[,,]
        {
          { {0f, 0f, 0f}, {0f, 0f, 0f}, {0f, 0f, 0f} },
          { {0f, 0f, 0f}, {0f, 0f, 0f}, {0f, 0f, 0f} },
          { {0f, 0f, 0f}, {0f, 0f, 0f}, {0f, 0f, 0f} },
        };
    }

    void Start()
    {
      if(yLength == 4)
      {
        basicMap1Random = new float[xLength,yLength,zLength];
        basicMap2Random = new float[xLength,yLength,zLength];
        basicMap3Random = new float[xLength,yLength,zLength];
        sideMapRandom = new float[3,3,3,3];
        surfaceMapRandom = new float[3,3,3,3];
      }
      if(yLength == 6)
      {
        basicMap1_464Random = new float[xLength,yLength,zLength];
        basicMap2_464Random = new float[xLength,yLength,zLength];
        basicMap3_464Random = new float[xLength,yLength,zLength];
        sideMap464Random = new float[3,3,3,3];
        sideLongMap464Random = new float[3,3,3];
        surfaceMap464Random = new float[3,3,3,3];
        surfaceLongMap464Random = new float[3,3,3];
      }

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
      if(yLength == 4){ return CulBasic(n,sq,basicMap1,basicMap2,basicMap3) + CulSide(sq,sideMap) + CulSurface(sq,surfaceMap); }
      else if(yLength == 6){ return CulBasic(n,sq,basicMap1_464,basicMap2_464,basicMap3_464) + CulSide464(sq,sideMap464) + CulSideLong464(sq,sideLongMap464) + CulSurface464(sq,surfaceMap464) + CulSurfaceLong464(sq,surfaceLongMap464); }
      else{ return 0f; }
    }

    private float CulScoreRandom(int[,,] sq)
    {
      int n;
      if(gameSystem.TotalTurn < 20){ n = 1; } else if(gameSystem.TotalTurn < 40){ n = 2; } else{ n = 3; }
      if(yLength == 4){ return CulBasic(n,sq,basicMap1Random,basicMap2Random,basicMap3Random) + CulSide(sq,sideMapRandom) + CulSurface(sq,surfaceMapRandom); }
      else if(yLength == 6){ return CulBasic(n,sq,basicMap1_464Random,basicMap2_464Random,basicMap3_464Random) + CulSide464(sq,sideMap464Random) + CulSideLong464(sq,sideLongMap464Random) + CulSurface464(sq,surfaceMap464Random) + CulSurfaceLong464(sq,surfaceLongMap464Random); }
      else{ return 0f; }
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
      score += map[ TransSqIndex(cpuStone*sq[1,0,0]), TransSqIndex(cpuStone*sq[1,0,1]), TransSqIndex(cpuStone*sq[1,0,2]), TransSqIndex(cpuStone*sq[1,0,3])];

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

    private float CulSide464(int[,,] sq, float[,,,] map)
      {
        float score = 0;
        score += map[ TransSqIndex(cpuStone*sq[0,0,0]), TransSqIndex(cpuStone*sq[1,0,0]), TransSqIndex(cpuStone*sq[2,0,0]), TransSqIndex(cpuStone*sq[3,0,0])];
        score += map[ TransSqIndex(cpuStone*sq[0,0,3]), TransSqIndex(cpuStone*sq[1,0,3]), TransSqIndex(cpuStone*sq[2,0,3]), TransSqIndex(cpuStone*sq[3,0,3])];
        score += map[ TransSqIndex(cpuStone*sq[0,5,0]), TransSqIndex(cpuStone*sq[1,5,0]), TransSqIndex(cpuStone*sq[2,5,0]), TransSqIndex(cpuStone*sq[3,5,0])];
        score += map[ TransSqIndex(cpuStone*sq[0,5,3]), TransSqIndex(cpuStone*sq[1,5,3]), TransSqIndex(cpuStone*sq[2,5,3]), TransSqIndex(cpuStone*sq[3,5,3])];

        score += map[ TransSqIndex(cpuStone*sq[0,0,0]), TransSqIndex(cpuStone*sq[0,0,1]), TransSqIndex(cpuStone*sq[0,0,2]), TransSqIndex(cpuStone*sq[0,0,3])];
        score += map[ TransSqIndex(cpuStone*sq[0,5,0]), TransSqIndex(cpuStone*sq[0,5,1]), TransSqIndex(cpuStone*sq[0,5,2]), TransSqIndex(cpuStone*sq[0,5,3])];
        score += map[ TransSqIndex(cpuStone*sq[3,0,0]), TransSqIndex(cpuStone*sq[3,0,1]), TransSqIndex(cpuStone*sq[3,0,2]), TransSqIndex(cpuStone*sq[3,0,3])];
        score += map[ TransSqIndex(cpuStone*sq[3,5,0]), TransSqIndex(cpuStone*sq[3,5,1]), TransSqIndex(cpuStone*sq[3,5,2]), TransSqIndex(cpuStone*sq[3,5,3])];

        return score;
      }

      private float CulSideLong464(int[,,] sq, float[,,] map)
      {
        float score = 0;
        score += map[ TransSqIndex(cpuStone*sq[0,0,0]), TransSqIndex(cpuStone*sq[0,1,0]), TransSqIndex(cpuStone*sq[0,2,0])];
        score += map[ TransSqIndex(cpuStone*sq[3,0,0]), TransSqIndex(cpuStone*sq[3,1,0]), TransSqIndex(cpuStone*sq[3,2,0])];
        score += map[ TransSqIndex(cpuStone*sq[0,0,3]), TransSqIndex(cpuStone*sq[0,1,3]), TransSqIndex(cpuStone*sq[0,2,3])];
        score += map[ TransSqIndex(cpuStone*sq[3,0,3]), TransSqIndex(cpuStone*sq[3,1,3]), TransSqIndex(cpuStone*sq[3,2,3])];

        score += map[ TransSqIndex(cpuStone*sq[0,5,0]), TransSqIndex(cpuStone*sq[0,4,0]), TransSqIndex(cpuStone*sq[0,3,0])];
        score += map[ TransSqIndex(cpuStone*sq[3,5,0]), TransSqIndex(cpuStone*sq[3,4,0]), TransSqIndex(cpuStone*sq[3,3,0])];
        score += map[ TransSqIndex(cpuStone*sq[0,5,3]), TransSqIndex(cpuStone*sq[0,4,3]), TransSqIndex(cpuStone*sq[0,3,3])];
        score += map[ TransSqIndex(cpuStone*sq[3,5,3]), TransSqIndex(cpuStone*sq[3,4,3]), TransSqIndex(cpuStone*sq[3,3,3])];

        return score;
      }

      private float CulSurface464(int[,,] sq, float[,,,] map)
      {
        float score = 0;
        score += map[ TransSqIndex(cpuStone*sq[0,0,1]), TransSqIndex(cpuStone*sq[1,0,1]), TransSqIndex(cpuStone*sq[2,0,1]), TransSqIndex(cpuStone*sq[3,0,1])];
        score += map[ TransSqIndex(cpuStone*sq[0,0,2]), TransSqIndex(cpuStone*sq[1,0,2]), TransSqIndex(cpuStone*sq[2,0,2]), TransSqIndex(cpuStone*sq[3,0,2])];
        score += map[ TransSqIndex(cpuStone*sq[0,1,3]), TransSqIndex(cpuStone*sq[1,1,3]), TransSqIndex(cpuStone*sq[2,1,3]), TransSqIndex(cpuStone*sq[3,1,3])];
        score += map[ TransSqIndex(cpuStone*sq[0,2,3]), TransSqIndex(cpuStone*sq[1,2,3]), TransSqIndex(cpuStone*sq[2,2,3]), TransSqIndex(cpuStone*sq[3,2,3])];
        score += map[ TransSqIndex(cpuStone*sq[0,3,3]), TransSqIndex(cpuStone*sq[1,3,3]), TransSqIndex(cpuStone*sq[2,3,3]), TransSqIndex(cpuStone*sq[3,3,3])];
        score += map[ TransSqIndex(cpuStone*sq[0,4,3]), TransSqIndex(cpuStone*sq[1,4,3]), TransSqIndex(cpuStone*sq[2,4,3]), TransSqIndex(cpuStone*sq[3,4,3])];
        score += map[ TransSqIndex(cpuStone*sq[0,5,2]), TransSqIndex(cpuStone*sq[1,5,2]), TransSqIndex(cpuStone*sq[2,5,2]), TransSqIndex(cpuStone*sq[3,5,2])];
        score += map[ TransSqIndex(cpuStone*sq[0,5,1]), TransSqIndex(cpuStone*sq[1,5,1]), TransSqIndex(cpuStone*sq[2,5,1]), TransSqIndex(cpuStone*sq[3,5,1])];
        score += map[ TransSqIndex(cpuStone*sq[0,4,0]), TransSqIndex(cpuStone*sq[1,4,0]), TransSqIndex(cpuStone*sq[2,4,0]), TransSqIndex(cpuStone*sq[3,4,0])];
        score += map[ TransSqIndex(cpuStone*sq[0,3,0]), TransSqIndex(cpuStone*sq[1,3,0]), TransSqIndex(cpuStone*sq[2,3,0]), TransSqIndex(cpuStone*sq[3,3,0])];
        score += map[ TransSqIndex(cpuStone*sq[0,2,0]), TransSqIndex(cpuStone*sq[1,2,0]), TransSqIndex(cpuStone*sq[2,2,0]), TransSqIndex(cpuStone*sq[3,2,0])];
        score += map[ TransSqIndex(cpuStone*sq[0,1,0]), TransSqIndex(cpuStone*sq[1,1,0]), TransSqIndex(cpuStone*sq[2,1,0]), TransSqIndex(cpuStone*sq[3,1,0])];

        score += map[ TransSqIndex(cpuStone*sq[0,1,0]), TransSqIndex(cpuStone*sq[0,1,1]), TransSqIndex(cpuStone*sq[0,1,2]), TransSqIndex(cpuStone*sq[0,1,3])];
        score += map[ TransSqIndex(cpuStone*sq[0,2,0]), TransSqIndex(cpuStone*sq[0,2,1]), TransSqIndex(cpuStone*sq[0,2,2]), TransSqIndex(cpuStone*sq[0,2,3])];
        score += map[ TransSqIndex(cpuStone*sq[0,3,0]), TransSqIndex(cpuStone*sq[0,3,1]), TransSqIndex(cpuStone*sq[0,3,2]), TransSqIndex(cpuStone*sq[0,3,3])];
        score += map[ TransSqIndex(cpuStone*sq[0,4,0]), TransSqIndex(cpuStone*sq[0,4,1]), TransSqIndex(cpuStone*sq[0,4,2]), TransSqIndex(cpuStone*sq[0,4,3])];
        score += map[ TransSqIndex(cpuStone*sq[1,5,0]), TransSqIndex(cpuStone*sq[1,5,1]), TransSqIndex(cpuStone*sq[1,5,2]), TransSqIndex(cpuStone*sq[1,5,3])];
        score += map[ TransSqIndex(cpuStone*sq[2,5,0]), TransSqIndex(cpuStone*sq[2,5,1]), TransSqIndex(cpuStone*sq[2,5,2]), TransSqIndex(cpuStone*sq[2,5,3])];
        score += map[ TransSqIndex(cpuStone*sq[3,4,0]), TransSqIndex(cpuStone*sq[3,4,1]), TransSqIndex(cpuStone*sq[3,4,2]), TransSqIndex(cpuStone*sq[3,4,3])];
        score += map[ TransSqIndex(cpuStone*sq[3,3,0]), TransSqIndex(cpuStone*sq[3,3,1]), TransSqIndex(cpuStone*sq[3,3,2]), TransSqIndex(cpuStone*sq[3,3,3])];
        score += map[ TransSqIndex(cpuStone*sq[3,2,0]), TransSqIndex(cpuStone*sq[3,2,1]), TransSqIndex(cpuStone*sq[3,2,2]), TransSqIndex(cpuStone*sq[3,2,3])];
        score += map[ TransSqIndex(cpuStone*sq[3,1,0]), TransSqIndex(cpuStone*sq[3,1,1]), TransSqIndex(cpuStone*sq[3,1,2]), TransSqIndex(cpuStone*sq[3,1,3])];
        score += map[ TransSqIndex(cpuStone*sq[2,0,0]), TransSqIndex(cpuStone*sq[2,0,1]), TransSqIndex(cpuStone*sq[2,0,2]), TransSqIndex(cpuStone*sq[2,0,3])];
        score += map[ TransSqIndex(cpuStone*sq[1,0,0]), TransSqIndex(cpuStone*sq[1,0,1]), TransSqIndex(cpuStone*sq[1,0,2]), TransSqIndex(cpuStone*sq[1,0,3])];

        score += map[ TransSqIndex(cpuStone*sq[0,0,0]), TransSqIndex(cpuStone*sq[1,0,1]), TransSqIndex(cpuStone*sq[2,0,2]), TransSqIndex(cpuStone*sq[3,0,3])];
        score += map[ TransSqIndex(cpuStone*sq[0,0,3]), TransSqIndex(cpuStone*sq[1,0,2]), TransSqIndex(cpuStone*sq[2,0,1]), TransSqIndex(cpuStone*sq[3,0,0])];
        score += map[ TransSqIndex(cpuStone*sq[0,5,0]), TransSqIndex(cpuStone*sq[1,5,1]), TransSqIndex(cpuStone*sq[2,5,2]), TransSqIndex(cpuStone*sq[3,5,3])];
        score += map[ TransSqIndex(cpuStone*sq[0,5,3]), TransSqIndex(cpuStone*sq[1,5,2]), TransSqIndex(cpuStone*sq[2,5,1]), TransSqIndex(cpuStone*sq[3,5,0])];

        score += map[ TransSqIndex(cpuStone*sq[0,0,0]), TransSqIndex(cpuStone*sq[1,1,0]), TransSqIndex(cpuStone*sq[2,2,0]), TransSqIndex(cpuStone*sq[3,3,0])];
        score += map[ TransSqIndex(cpuStone*sq[0,1,0]), TransSqIndex(cpuStone*sq[1,2,0]), TransSqIndex(cpuStone*sq[2,3,0]), TransSqIndex(cpuStone*sq[3,4,0])];
        score += map[ TransSqIndex(cpuStone*sq[0,2,0]), TransSqIndex(cpuStone*sq[1,3,0]), TransSqIndex(cpuStone*sq[2,4,0]), TransSqIndex(cpuStone*sq[3,5,0])];
        score += map[ TransSqIndex(cpuStone*sq[3,0,0]), TransSqIndex(cpuStone*sq[2,1,0]), TransSqIndex(cpuStone*sq[1,2,0]), TransSqIndex(cpuStone*sq[0,3,0])];
        score += map[ TransSqIndex(cpuStone*sq[3,1,0]), TransSqIndex(cpuStone*sq[2,2,0]), TransSqIndex(cpuStone*sq[1,3,0]), TransSqIndex(cpuStone*sq[0,4,0])];
        score += map[ TransSqIndex(cpuStone*sq[3,2,0]), TransSqIndex(cpuStone*sq[2,3,0]), TransSqIndex(cpuStone*sq[1,4,0]), TransSqIndex(cpuStone*sq[0,5,0])];
        score += map[ TransSqIndex(cpuStone*sq[0,0,3]), TransSqIndex(cpuStone*sq[1,1,3]), TransSqIndex(cpuStone*sq[2,2,3]), TransSqIndex(cpuStone*sq[3,3,3])];
        score += map[ TransSqIndex(cpuStone*sq[0,1,3]), TransSqIndex(cpuStone*sq[1,2,3]), TransSqIndex(cpuStone*sq[2,3,3]), TransSqIndex(cpuStone*sq[3,4,3])];
        score += map[ TransSqIndex(cpuStone*sq[0,2,3]), TransSqIndex(cpuStone*sq[1,3,3]), TransSqIndex(cpuStone*sq[2,4,3]), TransSqIndex(cpuStone*sq[3,5,3])];
        score += map[ TransSqIndex(cpuStone*sq[3,0,3]), TransSqIndex(cpuStone*sq[2,1,3]), TransSqIndex(cpuStone*sq[1,2,3]), TransSqIndex(cpuStone*sq[0,3,3])];
        score += map[ TransSqIndex(cpuStone*sq[3,1,3]), TransSqIndex(cpuStone*sq[2,2,3]), TransSqIndex(cpuStone*sq[1,3,3]), TransSqIndex(cpuStone*sq[0,4,3])];
        score += map[ TransSqIndex(cpuStone*sq[3,2,3]), TransSqIndex(cpuStone*sq[2,3,3]), TransSqIndex(cpuStone*sq[1,4,3]), TransSqIndex(cpuStone*sq[0,5,3])];

        score += map[ TransSqIndex(cpuStone*sq[0,0,0]), TransSqIndex(cpuStone*sq[0,1,1]), TransSqIndex(cpuStone*sq[0,2,2]), TransSqIndex(cpuStone*sq[0,3,3])];
        score += map[ TransSqIndex(cpuStone*sq[0,1,0]), TransSqIndex(cpuStone*sq[0,2,1]), TransSqIndex(cpuStone*sq[0,3,2]), TransSqIndex(cpuStone*sq[0,4,3])];
        score += map[ TransSqIndex(cpuStone*sq[0,2,0]), TransSqIndex(cpuStone*sq[0,3,1]), TransSqIndex(cpuStone*sq[0,4,2]), TransSqIndex(cpuStone*sq[0,5,3])];
        score += map[ TransSqIndex(cpuStone*sq[0,3,0]), TransSqIndex(cpuStone*sq[0,2,1]), TransSqIndex(cpuStone*sq[0,1,2]), TransSqIndex(cpuStone*sq[0,0,3])];
        score += map[ TransSqIndex(cpuStone*sq[0,4,0]), TransSqIndex(cpuStone*sq[0,3,1]), TransSqIndex(cpuStone*sq[0,2,2]), TransSqIndex(cpuStone*sq[0,1,3])];
        score += map[ TransSqIndex(cpuStone*sq[0,5,0]), TransSqIndex(cpuStone*sq[0,4,1]), TransSqIndex(cpuStone*sq[0,3,2]), TransSqIndex(cpuStone*sq[0,2,3])];
        score += map[ TransSqIndex(cpuStone*sq[3,0,0]), TransSqIndex(cpuStone*sq[3,1,1]), TransSqIndex(cpuStone*sq[3,2,2]), TransSqIndex(cpuStone*sq[3,3,3])];
        score += map[ TransSqIndex(cpuStone*sq[3,1,0]), TransSqIndex(cpuStone*sq[3,2,1]), TransSqIndex(cpuStone*sq[3,3,2]), TransSqIndex(cpuStone*sq[3,4,3])];
        score += map[ TransSqIndex(cpuStone*sq[3,2,0]), TransSqIndex(cpuStone*sq[3,3,1]), TransSqIndex(cpuStone*sq[3,4,2]), TransSqIndex(cpuStone*sq[3,5,3])];
        score += map[ TransSqIndex(cpuStone*sq[3,3,0]), TransSqIndex(cpuStone*sq[3,2,1]), TransSqIndex(cpuStone*sq[3,1,2]), TransSqIndex(cpuStone*sq[3,0,3])];
        score += map[ TransSqIndex(cpuStone*sq[3,4,0]), TransSqIndex(cpuStone*sq[3,3,1]), TransSqIndex(cpuStone*sq[3,2,2]), TransSqIndex(cpuStone*sq[3,1,3])];
        score += map[ TransSqIndex(cpuStone*sq[3,5,0]), TransSqIndex(cpuStone*sq[3,4,1]), TransSqIndex(cpuStone*sq[3,3,2]), TransSqIndex(cpuStone*sq[3,2,3])];

        return score;
      }

      private float CulSurfaceLong464(int[,,] sq, float[,,] map)
      {
        float score = 0;
        score += map[ TransSqIndex(cpuStone*sq[1,0,0]), TransSqIndex(cpuStone*sq[1,1,0]), TransSqIndex(cpuStone*sq[1,2,0])];
        score += map[ TransSqIndex(cpuStone*sq[2,0,0]), TransSqIndex(cpuStone*sq[2,1,0]), TransSqIndex(cpuStone*sq[2,2,0])];
        score += map[ TransSqIndex(cpuStone*sq[3,0,1]), TransSqIndex(cpuStone*sq[3,1,1]), TransSqIndex(cpuStone*sq[3,2,1])];
        score += map[ TransSqIndex(cpuStone*sq[3,0,2]), TransSqIndex(cpuStone*sq[3,1,2]), TransSqIndex(cpuStone*sq[3,2,2])];
        score += map[ TransSqIndex(cpuStone*sq[2,0,3]), TransSqIndex(cpuStone*sq[2,1,3]), TransSqIndex(cpuStone*sq[2,2,3])];
        score += map[ TransSqIndex(cpuStone*sq[1,0,3]), TransSqIndex(cpuStone*sq[1,1,3]), TransSqIndex(cpuStone*sq[1,2,3])];
        score += map[ TransSqIndex(cpuStone*sq[0,0,2]), TransSqIndex(cpuStone*sq[0,1,2]), TransSqIndex(cpuStone*sq[0,2,2])];
        score += map[ TransSqIndex(cpuStone*sq[0,0,1]), TransSqIndex(cpuStone*sq[0,1,1]), TransSqIndex(cpuStone*sq[0,2,1])];

        score += map[ TransSqIndex(cpuStone*sq[1,5,0]), TransSqIndex(cpuStone*sq[1,4,0]), TransSqIndex(cpuStone*sq[1,3,0])];
        score += map[ TransSqIndex(cpuStone*sq[2,5,0]), TransSqIndex(cpuStone*sq[2,4,0]), TransSqIndex(cpuStone*sq[2,3,0])];
        score += map[ TransSqIndex(cpuStone*sq[3,5,1]), TransSqIndex(cpuStone*sq[3,4,1]), TransSqIndex(cpuStone*sq[3,3,1])];
        score += map[ TransSqIndex(cpuStone*sq[3,5,2]), TransSqIndex(cpuStone*sq[3,4,2]), TransSqIndex(cpuStone*sq[3,3,2])];
        score += map[ TransSqIndex(cpuStone*sq[2,5,3]), TransSqIndex(cpuStone*sq[2,4,3]), TransSqIndex(cpuStone*sq[2,3,3])];
        score += map[ TransSqIndex(cpuStone*sq[1,5,3]), TransSqIndex(cpuStone*sq[1,4,3]), TransSqIndex(cpuStone*sq[1,3,3])];
        score += map[ TransSqIndex(cpuStone*sq[0,5,2]), TransSqIndex(cpuStone*sq[0,4,2]), TransSqIndex(cpuStone*sq[0,3,2])];
        score += map[ TransSqIndex(cpuStone*sq[0,5,1]), TransSqIndex(cpuStone*sq[0,4,1]), TransSqIndex(cpuStone*sq[0,3,1])];

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
      if(yLength == 4)
      {
        Array.Copy(basicMap1,basicMap1Random,basicMap1.Length);
        Array.Copy(basicMap2,basicMap2Random,basicMap2.Length);
        Array.Copy(basicMap3,basicMap3Random,basicMap3.Length);
        Array.Copy(sideMap,sideMapRandom,sideMap.Length);
        Array.Copy(surfaceMap,surfaceMapRandom,surfaceMap.Length);
      }
      if(yLength == 6)
      {
        Array.Copy(basicMap1_464,basicMap1_464Random,basicMap1_464.Length);
        Array.Copy(basicMap2_464,basicMap2_464Random,basicMap2_464.Length);
        Array.Copy(basicMap3_464,basicMap3_464Random,basicMap3_464.Length);
        Array.Copy(sideMap464,sideMap464Random,sideMap464.Length);
        Array.Copy(sideLongMap464,sideLongMap464Random,sideLongMap464.Length);
        Array.Copy(surfaceMap464,surfaceMap464Random,surfaceMap464.Length);
        Array.Copy(surfaceLongMap464,surfaceLongMap464Random,surfaceLongMap464.Length);
      }
    }

    public void RandomMap()//ランダムCPUを生成/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    {
      float epsilon; //epsilon-greedy法
      float rv; //盤面評価値の修正用乱数

      if(yLength == 4)
      {
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
              if(epsilon < 0.3f){ rv = 150f * UnityEngine.Random.value - 50f; surfaceMapRandom[2,1,1,2] = rv; surfaceMapRandom[1,2,2,1] = -rv; }*/
      }
      if(yLength == 6)
      {
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; basicMap1_464Random[0,0,1] = rv; basicMap1_464Random[0,0,2] = rv; basicMap1_464Random[0,5,1] = rv; basicMap1_464Random[0,5,2] = rv; basicMap1_464Random[1,0,0] = rv; basicMap1_464Random[1,0,3] = rv; basicMap1_464Random[1,5,0] = rv; basicMap1_464Random[1,5,3] = rv; basicMap1_464Random[2,0,0] = rv; basicMap1_464Random[2,0,3] = rv; basicMap1_464Random[2,5,0] = rv; basicMap1_464Random[2,5,3] = rv; basicMap1_464Random[3,0,1] = rv; basicMap1_464Random[3,0,2] = rv; basicMap1_464Random[3,5,1] = rv; basicMap1_464Random[3,5,2] = rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; basicMap1_464Random[0,1,0] = rv; basicMap1_464Random[0,1,3] = rv; basicMap1_464Random[0,4,0] = rv; basicMap1_464Random[0,4,3] = rv; basicMap1_464Random[3,1,0] = rv; basicMap1_464Random[3,1,3] = rv; basicMap1_464Random[3,4,0] = rv; basicMap1_464Random[3,4,3] = rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; basicMap1_464Random[0,2,0] = rv; basicMap1_464Random[0,2,3] = rv; basicMap1_464Random[0,3,0] = rv; basicMap1_464Random[0,3,3] = rv; basicMap1_464Random[3,2,0] = rv; basicMap1_464Random[3,2,3] = rv; basicMap1_464Random[3,3,0] = rv; basicMap1_464Random[3,3,3] = rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; basicMap1_464Random[0,1,1] = rv; basicMap1_464Random[0,1,2] = rv; basicMap1_464Random[0,4,1] = rv; basicMap1_464Random[0,4,2] = rv; basicMap1_464Random[1,1,0] = rv; basicMap1_464Random[1,1,3] = rv; basicMap1_464Random[1,4,0] = rv; basicMap1_464Random[1,4,3] = rv; basicMap1_464Random[2,1,0] = rv; basicMap1_464Random[2,1,3] = rv; basicMap1_464Random[2,4,0] = rv; basicMap1_464Random[2,4,3] = rv; basicMap1_464Random[3,1,1] = rv; basicMap1_464Random[3,1,2] = rv; basicMap1_464Random[3,4,1] = rv; basicMap1_464Random[3,4,2] = rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; basicMap1_464Random[0,2,1] = rv; basicMap1_464Random[0,2,2] = rv; basicMap1_464Random[0,3,1] = rv; basicMap1_464Random[0,3,2] = rv; basicMap1_464Random[1,2,0] = rv; basicMap1_464Random[1,2,3] = rv; basicMap1_464Random[1,3,0] = rv; basicMap1_464Random[1,3,3] = rv; basicMap1_464Random[2,2,0] = rv; basicMap1_464Random[2,2,3] = rv; basicMap1_464Random[2,3,0] = rv; basicMap1_464Random[2,3,3] = rv; basicMap1_464Random[3,2,1] = rv; basicMap1_464Random[3,2,2] = rv; basicMap1_464Random[3,3,1] = rv; basicMap1_464Random[3,3,2] = rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; basicMap1_464Random[1,0,1] = rv; basicMap1_464Random[1,0,2] = rv; basicMap1_464Random[1,5,1] = rv; basicMap1_464Random[1,5,2] = rv; basicMap1_464Random[2,0,1] = rv; basicMap1_464Random[2,0,2] = rv; basicMap1_464Random[2,5,1] = rv; basicMap1_464Random[2,5,2] = rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; basicMap1_464Random[1,1,1] = rv; basicMap1_464Random[1,1,2] = rv; basicMap1_464Random[1,4,1] = rv; basicMap1_464Random[1,4,2] = rv; basicMap1_464Random[2,1,1] = rv; basicMap1_464Random[2,1,2] = rv; basicMap1_464Random[2,4,1] = rv; basicMap1_464Random[2,4,2] = rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; basicMap1_464Random[1,2,1] = rv; basicMap1_464Random[1,2,2] = rv; basicMap1_464Random[1,3,1] = rv; basicMap1_464Random[1,3,2] = rv; basicMap1_464Random[2,2,1] = rv; basicMap1_464Random[2,2,2] = rv; basicMap1_464Random[2,3,1] = rv; basicMap1_464Random[2,3,2] = rv; }

              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value; basicMap2_464Random[0,0,0] = rv; basicMap2_464Random[0,0,3] = rv; basicMap2_464Random[0,5,0] = rv; basicMap2_464Random[0,5,3] = rv; basicMap2_464Random[3,0,0] = rv; basicMap2_464Random[3,0,3] = rv; basicMap2_464Random[3,5,0] = rv; basicMap2_464Random[3,5,3] = rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; basicMap2_464Random[0,0,1] = rv; basicMap2_464Random[0,0,2] = rv; basicMap2_464Random[0,5,1] = rv; basicMap2_464Random[0,5,2] = rv; basicMap2_464Random[1,0,0] = rv; basicMap2_464Random[1,0,3] = rv; basicMap2_464Random[1,5,0] = rv; basicMap2_464Random[1,5,3] = rv; basicMap2_464Random[2,0,0] = rv; basicMap2_464Random[2,0,3] = rv; basicMap2_464Random[2,5,0] = rv; basicMap2_464Random[2,5,3] = rv; basicMap2_464Random[3,0,1] = rv; basicMap2_464Random[3,0,2] = rv; basicMap2_464Random[3,5,1] = rv; basicMap2_464Random[3,5,2] = rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; basicMap2_464Random[0,1,0] = rv; basicMap2_464Random[0,1,3] = rv; basicMap2_464Random[0,4,0] = rv; basicMap2_464Random[0,4,3] = rv; basicMap2_464Random[3,1,0] = rv; basicMap2_464Random[3,1,3] = rv; basicMap2_464Random[3,4,0] = rv; basicMap2_464Random[3,4,3] = rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; basicMap2_464Random[0,2,0] = rv; basicMap2_464Random[0,2,3] = rv; basicMap2_464Random[0,3,0] = rv; basicMap2_464Random[0,3,3] = rv; basicMap2_464Random[3,2,0] = rv; basicMap2_464Random[3,2,3] = rv; basicMap2_464Random[3,3,0] = rv; basicMap2_464Random[3,3,3] = rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; basicMap2_464Random[0,1,1] = rv; basicMap2_464Random[0,1,2] = rv; basicMap2_464Random[0,4,1] = rv; basicMap2_464Random[0,4,2] = rv; basicMap2_464Random[1,1,0] = rv; basicMap2_464Random[1,1,3] = rv; basicMap2_464Random[1,4,0] = rv; basicMap2_464Random[1,4,3] = rv; basicMap2_464Random[2,1,0] = rv; basicMap2_464Random[2,1,3] = rv; basicMap2_464Random[2,4,0] = rv; basicMap2_464Random[2,4,3] = rv; basicMap2_464Random[3,1,1] = rv; basicMap2_464Random[3,1,2] = rv; basicMap2_464Random[3,4,1] = rv; basicMap2_464Random[3,4,2] = rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; basicMap2_464Random[0,2,1] = rv; basicMap2_464Random[0,2,2] = rv; basicMap2_464Random[0,3,1] = rv; basicMap2_464Random[0,3,2] = rv; basicMap2_464Random[1,2,0] = rv; basicMap2_464Random[1,2,3] = rv; basicMap2_464Random[1,3,0] = rv; basicMap2_464Random[1,3,3] = rv; basicMap2_464Random[2,2,0] = rv; basicMap2_464Random[2,2,3] = rv; basicMap2_464Random[2,3,0] = rv; basicMap2_464Random[2,3,3] = rv; basicMap2_464Random[3,2,1] = rv; basicMap2_464Random[3,2,2] = rv; basicMap2_464Random[3,3,1] = rv; basicMap2_464Random[3,3,2] = rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; basicMap2_464Random[1,0,1] = rv; basicMap2_464Random[1,0,2] = rv; basicMap2_464Random[1,5,1] = rv; basicMap2_464Random[1,5,2] = rv; basicMap2_464Random[2,0,1] = rv; basicMap2_464Random[2,0,2] = rv; basicMap2_464Random[2,5,1] = rv; basicMap2_464Random[2,5,2] = rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; basicMap2_464Random[1,1,1] = rv; basicMap2_464Random[1,1,2] = rv; basicMap2_464Random[1,4,1] = rv; basicMap2_464Random[1,4,2] = rv; basicMap2_464Random[2,1,1] = rv; basicMap2_464Random[2,1,2] = rv; basicMap2_464Random[2,4,1] = rv; basicMap2_464Random[2,4,2] = rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; basicMap2_464Random[1,2,1] = rv; basicMap2_464Random[1,2,2] = rv; basicMap2_464Random[1,3,1] = rv; basicMap2_464Random[1,3,2] = rv; basicMap2_464Random[2,2,1] = rv; basicMap2_464Random[2,2,2] = rv; basicMap2_464Random[2,3,1] = rv; basicMap2_464Random[2,3,2] = rv; }

              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value; basicMap3_464Random[0,0,0] = rv; basicMap3_464Random[0,0,3] = rv; basicMap3_464Random[0,5,0] = rv; basicMap3_464Random[0,5,3] = rv; basicMap3_464Random[3,0,0] = rv; basicMap3_464Random[3,0,3] = rv; basicMap3_464Random[3,5,0] = rv; basicMap3_464Random[3,5,3] = rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; basicMap3_464Random[0,0,1] = rv; basicMap3_464Random[0,0,2] = rv; basicMap3_464Random[0,5,1] = rv; basicMap3_464Random[0,5,2] = rv; basicMap3_464Random[1,0,0] = rv; basicMap3_464Random[1,0,3] = rv; basicMap3_464Random[1,5,0] = rv; basicMap3_464Random[1,5,3] = rv; basicMap3_464Random[2,0,0] = rv; basicMap3_464Random[2,0,3] = rv; basicMap3_464Random[2,5,0] = rv; basicMap3_464Random[2,5,3] = rv; basicMap3_464Random[3,0,1] = rv; basicMap3_464Random[3,0,2] = rv; basicMap3_464Random[3,5,1] = rv; basicMap3_464Random[3,5,2] = rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; basicMap3_464Random[0,1,0] = rv; basicMap3_464Random[0,1,3] = rv; basicMap3_464Random[0,4,0] = rv; basicMap3_464Random[0,4,3] = rv; basicMap3_464Random[3,1,0] = rv; basicMap3_464Random[3,1,3] = rv; basicMap3_464Random[3,4,0] = rv; basicMap3_464Random[3,4,3] = rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; basicMap3_464Random[0,2,0] = rv; basicMap3_464Random[0,2,3] = rv; basicMap3_464Random[0,3,0] = rv; basicMap3_464Random[0,3,3] = rv; basicMap3_464Random[3,2,0] = rv; basicMap3_464Random[3,2,3] = rv; basicMap3_464Random[3,3,0] = rv; basicMap3_464Random[3,3,3] = rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; basicMap3_464Random[0,1,1] = rv; basicMap3_464Random[0,1,2] = rv; basicMap3_464Random[0,4,1] = rv; basicMap3_464Random[0,4,2] = rv; basicMap3_464Random[1,1,0] = rv; basicMap3_464Random[1,1,3] = rv; basicMap3_464Random[1,4,0] = rv; basicMap3_464Random[1,4,3] = rv; basicMap3_464Random[2,1,0] = rv; basicMap3_464Random[2,1,3] = rv; basicMap3_464Random[2,4,0] = rv; basicMap3_464Random[2,4,3] = rv; basicMap3_464Random[3,1,1] = rv; basicMap3_464Random[3,1,2] = rv; basicMap3_464Random[3,4,1] = rv; basicMap3_464Random[3,4,2] = rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; basicMap3_464Random[0,2,1] = rv; basicMap3_464Random[0,2,2] = rv; basicMap3_464Random[0,3,1] = rv; basicMap3_464Random[0,3,2] = rv; basicMap3_464Random[1,2,0] = rv; basicMap3_464Random[1,2,3] = rv; basicMap3_464Random[1,3,0] = rv; basicMap3_464Random[1,3,3] = rv; basicMap3_464Random[2,2,0] = rv; basicMap3_464Random[2,2,3] = rv; basicMap3_464Random[2,3,0] = rv; basicMap3_464Random[2,3,3] = rv; basicMap3_464Random[3,2,1] = rv; basicMap3_464Random[3,2,2] = rv; basicMap3_464Random[3,3,1] = rv; basicMap3_464Random[3,3,2] = rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; basicMap3_464Random[1,0,1] = rv; basicMap3_464Random[1,0,2] = rv; basicMap3_464Random[1,5,1] = rv; basicMap3_464Random[1,5,2] = rv; basicMap3_464Random[2,0,1] = rv; basicMap3_464Random[2,0,2] = rv; basicMap3_464Random[2,5,1] = rv; basicMap3_464Random[2,5,2] = rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; basicMap3_464Random[1,1,1] = rv; basicMap3_464Random[1,1,2] = rv; basicMap3_464Random[1,4,1] = rv; basicMap3_464Random[1,4,2] = rv; basicMap3_464Random[2,1,1] = rv; basicMap3_464Random[2,1,2] = rv; basicMap3_464Random[2,4,1] = rv; basicMap3_464Random[2,4,2] = rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; basicMap3_464Random[1,2,1] = rv; basicMap3_464Random[1,2,2] = rv; basicMap3_464Random[1,3,1] = rv; basicMap3_464Random[1,3,2] = rv; basicMap3_464Random[2,2,1] = rv; basicMap3_464Random[2,2,2] = rv; basicMap3_464Random[2,3,1] = rv; basicMap3_464Random[2,3,2] = rv; }

              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; sideMap464Random[0,0,1,1] = rv; sideMap464Random[0,0,2,2] = -rv; sideMap464Random[1,1,0,0] = rv; sideMap464Random[2,2,0,0] = -rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 50f * UnityEngine.Random.value; sideMap464Random[0,0,2,1] = rv; sideMap464Random[0,0,1,2] = -rv; sideMap464Random[1,2,0,0] = rv; sideMap464Random[2,1,0,0] = -rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; sideMap464Random[0,2,0,2] = rv; sideMap464Random[0,1,0,1] = -rv; sideMap464Random[2,0,2,0] = rv; sideMap464Random[1,0,1,0] = -rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; sideMap464Random[0,2,0,1] = rv; sideMap464Random[0,1,0,2] = -rv; sideMap464Random[1,0,2,0] = rv; sideMap464Random[2,0,1,0] = -rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value; sideMap464Random[0,1,2,0] = -rv; sideMap464Random[0,2,1,0] = -rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; sideMap464Random[0,1,1,1] = rv; sideMap464Random[0,2,2,2] = -rv; sideMap464Random[1,1,1,0] = rv; sideMap464Random[2,2,2,0] = -rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value; sideMap464Random[0,2,2,1] = rv; sideMap464Random[0,1,1,2] = -rv; sideMap464Random[1,2,2,0] = rv; sideMap464Random[2,1,1,0] = -rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value; sideMap464Random[0,2,1,2] = rv; sideMap464Random[0,1,2,1] = -rv; sideMap464Random[2,1,2,0] = rv; sideMap464Random[1,2,1,0] = -rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value; sideMap464Random[0,2,1,1] = rv; sideMap464Random[0,1,2,2] = -rv; sideMap464Random[1,1,2,0] = rv; sideMap464Random[2,2,1,0] = -rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; sideMap464Random[2,0,2,2] = rv; sideMap464Random[1,0,1,1] = -rv; sideMap464Random[2,2,0,2] = rv; sideMap464Random[1,1,0,1] = -rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; sideMap464Random[2,0,1,1] = rv; sideMap464Random[1,0,2,2] = -rv; sideMap464Random[1,1,0,2] = rv; sideMap464Random[2,2,0,1] = -rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value; sideMap464Random[1,0,2,1] = rv; sideMap464Random[2,0,1,2] = -rv; sideMap464Random[1,2,0,1] = rv; sideMap464Random[2,1,0,2] = -rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value; sideMap464Random[2,0,2,1] = rv; sideMap464Random[1,0,1,2] = -rv; sideMap464Random[1,2,0,2] = rv; sideMap464Random[2,1,0,1] = -rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 150f * UnityEngine.Random.value - 50f; sideMap464Random[1,1,1,1] = rv; sideMap464Random[2,2,2,2] = -rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 150f * UnityEngine.Random.value - 50f; sideMap464Random[1,1,1,2] = rv; sideMap464Random[2,2,2,1] = -rv; sideMap464Random[2,1,1,1] = rv; sideMap464Random[1,2,2,2] = -rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 200f * UnityEngine.Random.value - 100f; sideMap464Random[2,2,1,2] = rv; sideMap464Random[1,1,2,1] = -rv; sideMap464Random[2,1,2,2] = rv; sideMap464Random[1,2,1,1] = -rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 150f * UnityEngine.Random.value - 50f; sideMap464Random[2,1,1,2] = rv; sideMap464Random[1,2,2,1] = -rv; }

              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; sideLongMap464Random[0,1,1] = rv; sideLongMap464Random[0,2,2] = -rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value; sideLongMap464Random[0,2,1] = rv; sideLongMap464Random[0,1,2] = -rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; sideLongMap464Random[2,0,2] = rv; sideLongMap464Random[1,0,1] = -rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; sideLongMap464Random[1,0,2] = rv; sideLongMap464Random[2,0,1] = -rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; sideLongMap464Random[1,1,0] = rv; sideLongMap464Random[2,2,0] = -rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value; sideLongMap464Random[1,2,0] = rv; sideLongMap464Random[2,1,0] = -rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 150f * UnityEngine.Random.value - 50f; sideLongMap464Random[1,1,1] = rv; sideLongMap464Random[2,2,2] = -rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value; sideLongMap464Random[1,1,2] = rv; sideLongMap464Random[2,2,1] = -rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 200f * UnityEngine.Random.value - 100f; sideLongMap464Random[1,2,1] = rv; sideLongMap464Random[2,1,2] = -rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value; sideLongMap464Random[1,2,2] = rv; sideLongMap464Random[2,1,1] = -rv; }

              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; surfaceMap464Random[0,0,1,1] = rv; surfaceMap464Random[0,0,2,2] = -rv; surfaceMap464Random[1,1,0,0] = rv; surfaceMap464Random[2,2,0,0] = -rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 50f * UnityEngine.Random.value; surfaceMap464Random[0,0,2,1] = rv; surfaceMap464Random[0,0,1,2] = -rv; surfaceMap464Random[1,2,0,0] = rv; surfaceMap464Random[2,1,0,0] = -rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; surfaceMap464Random[0,2,0,2] = rv; surfaceMap464Random[0,1,0,1] = -rv; surfaceMap464Random[2,0,2,0] = rv; surfaceMap464Random[1,0,1,0] = -rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; surfaceMap464Random[0,2,0,1] = rv; surfaceMap464Random[0,1,0,2] = -rv; surfaceMap464Random[1,0,2,0] = rv; surfaceMap464Random[2,0,1,0] = -rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value; surfaceMap464Random[0,1,2,0] = -rv; surfaceMap464Random[0,2,1,0] = -rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; surfaceMap464Random[0,1,1,1] = rv; surfaceMap464Random[0,2,2,2] = -rv; surfaceMap464Random[1,1,1,0] = rv; surfaceMap464Random[2,2,2,0] = -rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value; surfaceMap464Random[0,2,2,1] = rv; surfaceMap464Random[0,1,1,2] = -rv; surfaceMap464Random[1,2,2,0] = rv; surfaceMap464Random[2,1,1,0] = -rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value; surfaceMap464Random[0,2,1,2] = rv; surfaceMap464Random[0,1,2,1] = -rv; surfaceMap464Random[2,1,2,0] = rv; surfaceMap464Random[1,2,1,0] = -rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value; surfaceMap464Random[0,2,1,1] = rv; surfaceMap464Random[0,1,2,2] = -rv; surfaceMap464Random[1,1,2,0] = rv; surfaceMap464Random[2,2,1,0] = -rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; surfaceMap464Random[2,0,2,2] = rv; surfaceMap464Random[1,0,1,1] = -rv; surfaceMap464Random[2,2,0,2] = rv; surfaceMap464Random[1,1,0,1] = -rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; surfaceMap464Random[2,0,1,1] = rv; surfaceMap464Random[1,0,2,2] = -rv; surfaceMap464Random[1,1,0,2] = rv; surfaceMap464Random[2,2,0,1] = -rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value; surfaceMap464Random[1,0,2,1] = rv; surfaceMap464Random[2,0,1,2] = -rv; surfaceMap464Random[1,2,0,1] = rv; surfaceMap464Random[2,1,0,2] = -rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value; surfaceMap464Random[2,0,2,1] = rv; surfaceMap464Random[1,0,1,2] = -rv; surfaceMap464Random[1,2,0,2] = rv; surfaceMap464Random[2,1,0,1] = -rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 150f * UnityEngine.Random.value - 50f; surfaceMap464Random[1,1,1,1] = rv; surfaceMap464Random[2,2,2,2] = -rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 150f * UnityEngine.Random.value - 50f; surfaceMap464Random[1,1,1,2] = rv; surfaceMap464Random[2,2,2,1] = -rv; surfaceMap464Random[2,1,1,1] = rv; surfaceMap464Random[1,2,2,2] = -rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 200f * UnityEngine.Random.value - 100f; surfaceMap464Random[2,2,1,2] = rv; surfaceMap464Random[1,1,2,1] = -rv; surfaceMap464Random[2,1,2,2] = rv; surfaceMap464Random[1,2,1,1] = -rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 150f * UnityEngine.Random.value - 50f; surfaceMap464Random[2,1,1,2] = rv; surfaceMap464Random[1,2,2,1] = -rv; }

              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; surfaceLongMap464Random[0,1,1] = rv; surfaceLongMap464Random[0,2,2] = -rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value; surfaceLongMap464Random[0,2,1] = rv; surfaceLongMap464Random[0,1,2] = -rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; surfaceLongMap464Random[2,0,2] = rv; surfaceLongMap464Random[1,0,1] = -rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; surfaceLongMap464Random[1,0,2] = rv; surfaceLongMap464Random[2,0,1] = -rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value - 50f; surfaceLongMap464Random[1,1,0] = rv; surfaceLongMap464Random[2,2,0] = -rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value; surfaceLongMap464Random[1,2,0] = rv; surfaceLongMap464Random[2,1,0] = -rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 150f * UnityEngine.Random.value - 50f; surfaceLongMap464Random[1,1,1] = rv; surfaceLongMap464Random[2,2,2] = -rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value; surfaceLongMap464Random[1,1,2] = rv; surfaceLongMap464Random[2,2,1] = -rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 200f * UnityEngine.Random.value - 100f; surfaceLongMap464Random[1,2,1] = rv; surfaceLongMap464Random[2,1,2] = -rv; }
              epsilon = UnityEngine.Random.value;
              if(epsilon < 0.3f){ rv = 100f * UnityEngine.Random.value; surfaceLongMap464Random[1,2,2] = rv; surfaceLongMap464Random[2,1,1] = -rv; }
      }

    }//ここまで//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    public void TextLog()
    {
      if(yLength == 4)
      {
        if(texts.deleteText("/Resources/TextFile.txt")){}else{Debug.Log("Error;");}
        for(int x=0; x<xLength; x++)
        {
          for(int y=0; y<yLength; y++)
          {
            for(int z=0; z<zLength; z++)
            {
              basicMap1[x,y,z] = basicMap1Random[x,y,z];
              if(texts.saveText("/Resources/TextFile.txt", basicMap1[x,y,z].ToString() + ",")){}else{Debug.Log("Error;");}
            }
          }
        }
        for(int x=0; x<xLength; x++)
        {
          for(int y=0; y<yLength; y++)
          {
            for(int z=0; z<zLength; z++)
            {
              basicMap2[x,y,z] = basicMap2Random[x,y,z];
              if(texts.saveText("/Resources/TextFile.txt", basicMap2[x,y,z].ToString() + ",")){}else{Debug.Log("Error;");}
            }
          }
        }
        for(int x=0; x<xLength; x++)
        {
          for(int y=0; y<yLength; y++)
          {
            for(int z=0; z<zLength; z++)
            {
              basicMap3[x,y,z] = basicMap3Random[x,y,z];
              if(texts.saveText("/Resources/TextFile.txt", basicMap3[x,y,z].ToString() + ",")){}else{Debug.Log("Error;");}
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
                if(texts.saveText("/Resources/TextFile.txt", sideMap[a1,a2,a3,a4].ToString() + ",")){}else{Debug.Log("Error;");}
              }
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
                surfaceMap[a1,a2,a3,a4] = surfaceMapRandom[a1,a2,a3,a4];
                if(texts.saveText("/Resources/TextFile.txt", surfaceMap[a1,a2,a3,a4].ToString() + ",")){}else{Debug.Log("Error;");}
              }
            }
          }
        }
      }
      if(yLength == 6)
      {
        if(texts.deleteText("/Resources/TextFile.txt")){}else{Debug.Log("Error;");}
        for(int x=0; x<xLength; x++)
        {
          for(int y=0; y<yLength; y++)
          {
            for(int z=0; z<zLength; z++)
            {
              basicMap1_464[x,y,z] = basicMap1_464Random[x,y,z];
              if(texts.saveText("/Resources/TextFile.txt", basicMap1_464[x,y,z].ToString() + ",")){}else{Debug.Log("Error;");}
            }
          }
        }
        for(int x=0; x<xLength; x++)
        {
          for(int y=0; y<yLength; y++)
          {
            for(int z=0; z<zLength; z++)
            {
              basicMap2_464[x,y,z] = basicMap2_464Random[x,y,z];
              if(texts.saveText("/Resources/TextFile.txt", basicMap2_464[x,y,z].ToString() + ",")){}else{Debug.Log("Error;");}
            }
          }
        }
        for(int x=0; x<xLength; x++)
        {
          for(int y=0; y<yLength; y++)
          {
            for(int z=0; z<zLength; z++)
            {
              basicMap3_464[x,y,z] = basicMap3_464Random[x,y,z];
              if(texts.saveText("/Resources/TextFile.txt", basicMap3_464[x,y,z].ToString() + ",")){}else{Debug.Log("Error;");}
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
                sideMap464[a1,a2,a3,a4] = sideMap464Random[a1,a2,a3,a4];
                if(texts.saveText("/Resources/TextFile.txt", sideMap464[a1,a2,a3,a4].ToString() + ",")){}else{Debug.Log("Error;");}
              }
            }
          }
        }
        for(int a1=0; a1<3; a1++)
        {
          for(int a2=0; a2<3; a2++)
          {
            for(int a3=0; a3<3; a3++)
            {
              sideLongMap464[a1,a2,a3] = sideLongMap464Random[a1,a2,a3];
              if(texts.saveText("/Resources/TextFile.txt", sideLongMap464[a1,a2,a3].ToString() + ",")){}else{Debug.Log("Error;");}
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
                surfaceMap464[a1,a2,a3,a4] = surfaceMap464Random[a1,a2,a3,a4];
                if(texts.saveText("/Resources/TextFile.txt", surfaceMap464[a1,a2,a3,a4].ToString() + ",")){}else{Debug.Log("Error;");}
              }
            }
          }
        }
        for(int a1=0; a1<3; a1++)
        {
          for(int a2=0; a2<3; a2++)
          {
            for(int a3=0; a3<3; a3++)
            {
              surfaceLongMap464[a1,a2,a3] = surfaceLongMap464Random[a1,a2,a3];
              if(texts.saveText("/Resources/TextFile.txt", surfaceLongMap464[a1,a2,a3].ToString() + ",")){}else{Debug.Log("Error;");}
            }
          }
        }
      }
    }

    public void TextDetalog(int n)
    {
        if(yLength == 4)
        {
          for(int x=0; x<xLength; x++)
          {
            for(int y=0; y<yLength; y++)
            {
              for(int z=0; z<zLength; z++)
                {
                  basicMap1[x,y,z] = basicMap1Random[x,y,z];
                  if(texts.saveText("/Resources/TextFileDeta" + n + ".txt", basicMap1[x,y,z].ToString() + ",")){}else{Debug.Log("Error;");}
                }
              }
            }
            for(int x=0; x<xLength; x++)
            {
              for(int y=0; y<yLength; y++)
              {
                for(int z=0; z<zLength; z++)
                {
                  basicMap2[x,y,z] = basicMap2Random[x,y,z];
                  if(texts.saveText("/Resources/TextFileDeta" + n + ".txt", basicMap2[x,y,z].ToString() + ",")){}else{Debug.Log("Error;");}
                }
              }
            }
            for(int x=0; x<xLength; x++)
            {
              for(int y=0; y<yLength; y++)
              {
                for(int z=0; z<zLength; z++)
                {
                  basicMap3[x,y,z] = basicMap3Random[x,y,z];
                  if(texts.saveText("/Resources/TextFileDeta" + n + ".txt", basicMap3[x,y,z].ToString() + ",")){}else{Debug.Log("Error;");}
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
                    if(texts.saveText("/Resources/TextFileDeta" + n + ".txt", sideMap[a1,a2,a3,a4].ToString() + ",")){}else{Debug.Log("Error;");}
                  }
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
                    if(texts.saveText("/Resources/TextFileDeta" + n + ".txt", surfaceMap[a1,a2,a3,a4].ToString() + ",")){}else{Debug.Log("Error;");}
                  }
                }
              }
            }
        }
        if(yLength == 6)
        {
          for(int x=0; x<xLength; x++)
          {
            for(int y=0; y<yLength; y++)
            {
              for(int z=0; z<zLength; z++)
              {
                basicMap1_464[x,y,z] = basicMap1_464Random[x,y,z];
                if(texts.saveText("/Resources/TextFileDeta" + n + ".txt", basicMap1_464[x,y,z].ToString() + ",")){}else{Debug.Log("Error;");}
              }
            }
          }
          for(int x=0; x<xLength; x++)
          {
            for(int y=0; y<yLength; y++)
            {
              for(int z=0; z<zLength; z++)
              {
                basicMap2_464[x,y,z] = basicMap2_464Random[x,y,z];
                if(texts.saveText("/Resources/TextFileDeta" + n + ".txt", basicMap2_464[x,y,z].ToString() + ",")){}else{Debug.Log("Error;");}
              }
            }
          }
          for(int x=0; x<xLength; x++)
          {
            for(int y=0; y<yLength; y++)
            {
              for(int z=0; z<zLength; z++)
              {
                basicMap3_464[x,y,z] = basicMap3_464Random[x,y,z];
                if(texts.saveText("/Resources/TextFileDeta" + n + ".txt", basicMap3_464[x,y,z].ToString() + ",")){}else{Debug.Log("Error;");}
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
                  sideMap464[a1,a2,a3,a4] = sideMap464Random[a1,a2,a3,a4];
                  if(texts.saveText("/Resources/TextFileDeta" + n + ".txt", sideMap464[a1,a2,a3,a4].ToString() + ",")){}else{Debug.Log("Error;");}
                }
              }
            }
          }
          for(int a1=0; a1<3; a1++)
          {
            for(int a2=0; a2<3; a2++)
            {
              for(int a3=0; a3<3; a3++)
              {
                sideLongMap464[a1,a2,a3] = sideLongMap464Random[a1,a2,a3];
                if(texts.saveText("/Resources/TextFileDeta" + n + ".txt", sideLongMap464[a1,a2,a3].ToString() + ",")){}else{Debug.Log("Error;");}
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
                  surfaceMap464[a1,a2,a3,a4] = surfaceMap464Random[a1,a2,a3,a4];
                  if(texts.saveText("/Resources/TextFileDeta" + n + ".txt", surfaceMap464[a1,a2,a3,a4].ToString() + ",")){}else{Debug.Log("Error;");}
                }
              }
            }
          }
          for(int a1=0; a1<3; a1++)
          {
            for(int a2=0; a2<3; a2++)
            {
              for(int a3=0; a3<3; a3++)
              {
                surfaceLongMap464[a1,a2,a3] = surfaceLongMap464Random[a1,a2,a3];
                if(texts.saveText("/Resources/TextFileDeta" + n + ".txt", surfaceLongMap464[a1,a2,a3].ToString() + ",")){}else{Debug.Log("Error;");}
              }
            }
          }
        }
        if(texts.saveText("/Resources/TextFileDeta" + n + ".txt", "\n")){}else{Debug.Log("Error;");};
    }

    public void Mokuji(int n)
    {
      if(yLength == 4)
      {
        for(int x=0; x<xLength; x++)
        {
          for(int y=0; y<yLength; y++)
          {
            for(int z=0; z<zLength; z++)
            {
              if(texts.saveText("/Resources/TextFileDeta" + n + ".txt", "ba1" + x + y + z + ",")){}else{Debug.Log("Error;");}
            }
          }
        }
        for(int x=0; x<xLength; x++)
        {
          for(int y=0; y<yLength; y++)
          {
            for(int z=0; z<zLength; z++)
            {
              if(texts.saveText("/Resources/TextFileDeta" + n + ".txt", "ba2" + x + y + z + ",")){}else{Debug.Log("Error;");}
            }
          }
        }
        for(int x=0; x<xLength; x++)
        {
          for(int y=0; y<yLength; y++)
          {
            for(int z=0; z<zLength; z++)
            {
              if(texts.saveText("/Resources/TextFileDeta" + n + ".txt", "ba3" + x + y + z + ",")){}else{Debug.Log("Error;");}
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
                if(texts.saveText("/Resources/TextFileDeta" + n + ".txt", "si0" + a1 + a2 + a3 + a4 + ",")){}else{Debug.Log("Error;");}
              }
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
                if(texts.saveText("/Resources/TextFileDeta" + n + ".txt", "su0" + a1 + a2 + a3 + a4 + ",")){}else{Debug.Log("Error;");}
              }
            }
          }
        }
      }
      if(yLength == 6)
      {
        for(int x=0; x<xLength; x++)
        {
          for(int y=0; y<yLength; y++)
          {
            for(int z=0; z<zLength; z++)
            {
              if(texts.saveText("/Resources/TextFileDeta" + n + ".txt", "ba1" + x + y + z + ",")){}else{Debug.Log("Error;");}
            }
          }
        }
        for(int x=0; x<xLength; x++)
        {
          for(int y=0; y<yLength; y++)
          {
            for(int z=0; z<zLength; z++)
            {
              if(texts.saveText("/Resources/TextFileDeta" + n + ".txt", "ba2" + x + y + z + ",")){}else{Debug.Log("Error;");}
            }
          }
        }
        for(int x=0; x<xLength; x++)
        {
          for(int y=0; y<yLength; y++)
          {
            for(int z=0; z<zLength; z++)
            {
              if(texts.saveText("/Resources/TextFileDeta" + n + ".txt", "ba3" + x + y + z + ",")){}else{Debug.Log("Error;");}
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
                if(texts.saveText("/Resources/TextFileDeta" + n + ".txt", "si0" + a1 + a2 + a3 + a4 + ",")){}else{Debug.Log("Error;");}
              }
            }
          }
        }
        for(int a1=0; a1<3; a1++)
        {
          for(int a2=0; a2<3; a2++)
          {
            for(int a3=0; a3<3; a3++)
            {
              if(texts.saveText("/Resources/TextFileDeta" + n + ".txt", "si1" + a1 + a2 + a3 + ",")){}else{Debug.Log("Error;");}
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
                if(texts.saveText("/Resources/TextFileDeta" + n + ".txt", "su0" + a1 + a2 + a3 + a4 + ",")){}else{Debug.Log("Error;");}
              }
            }
          }
        }
        for(int a1=0; a1<3; a1++)
        {
          for(int a2=0; a2<3; a2++)
          {
            for(int a3=0; a3<3; a3++)
            {
              if(texts.saveText("/Resources/TextFileDeta" + n + ".txt", "su1" + a1 + a2 + a3 + ",")){}else{Debug.Log("Error;");}
            }
          }
        }
      }
      if(texts.saveText("/Resources/TextFileDeta" + n + ".txt", "\n")){}else{Debug.Log("Error;");}
    }

    public void LearningFinish()
    {
      Debug.Log("Learning successed.");
      EditorApplication.isPlaying = false;
    }
}
