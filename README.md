# HitTestPolyAndLine
====

## 概要
平面と線分のあたり判定を行うプログラム。
Unityにて作成している。

## 開発環境
Unity2018.2.11f1
Visual Studio 2017
Windows 10

## 詳細
SampleScene内に平面と線分のあたり判定を実装している。
平面はUnity3DプリミティブのQuadを使用し、
線分は2点をVector3で指定し、LineRendererで描画している。
当たり判定プログラムはPlaneHitTest.csに全て記述している。
PlaneHitTest.csをシーン内のTesterオブジェクトに貼り付け動作している。
