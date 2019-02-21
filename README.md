# HitTestPolyAndLine
====

## 概要
平面と線分のあたり判定を行うプログラム。
+球と球のあたり判定を行うプログラム。
Unityにて作成している。

## 開発環境
Unity2018.2.11f1
Visual Studio 2017
Windows 10

## 詳細

### 平面と線分のあたり判定
PolyAndLineHitTestシーン内にて平面と線分のあたり判定を実装している。
平面はUnity3DプリミティブのQuadを使用し、
線分は2点をVector3で指定し、LineRendererで描画している。
当たり判定プログラムはPlaneHitTest.csに全て記述している。
PlaneHitTest.csをシーン内のTesterオブジェクトに貼り付け、動作している。

### 球と球のあたり判定
SphereAndSpereHitTestシーン内にて球と球のあたり判定を実装している。
球はUnity3DプリミティブのSphereを使用している。
当たり判定プログラムはSphereHitTest.csに記述している。
SphereHitTest.csをシーン内のTesterオブジェクトに貼り付け、動作している。