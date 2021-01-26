# Othello3D_train
クソザコ強化学習

## 概要

CPU.csのMapをQ-Learningの要領で更新する。
BestCPUとRandomCPUをε-greedyをもちいて対戦し勝利した方をBestCPUに代入、
$回連続で勝利したCPUのMapをResource/TextFileData$$に保存する。

## 使い方

1. 以下の値をいじる
- GameSystem.csの xLength,yLength,zLength を(4,4,4)か(4,6,4)に設定する
- GameSystem.csの diagonal（falseを推奨）
- GameSystem.csのGameClear()の if(successTime % $ == 0 & successTime != 0){ cpu.TextDetalog($$); }
  - 連続成功回数（連続勝利回数のこと）$回で Resource/TextFileData$$ にデータを吐き出す
- GameSystem.csのGameClear()の Debug.Log("勝者 : " + result + "   回数 : " + learnTime + "　連続成功回数 : " + successTime);
- GameSystem.csのGameClear()の if(learnTime >= $$$){ Debug.Log("Be oversoon."); } if(learnTime >= $$$$){ cpu.LearningFinish(); }
  - 連続成功回数$$$$回でプログラムを終了
- CPU.csのRandomMap()でどの値を更新したいか設定

2. TrainingのSceneをプレイし学習

3. Resource/TextFileData$ $に$回以上連続で勝利したMapのn行m列データが吐き出されている
- 列はMapの位置（ba10000ならbasicMap1[0,0,0,0]、si01212ならsideMap[1,2,1,2]）、行は複数データの番号
- csvに変換するなどして解析してください

4. 1行m列のMapの完成版データを Resource/TextFileWrite にペーストしGetNewMapのSceneをプレイしデータの形式を整える
