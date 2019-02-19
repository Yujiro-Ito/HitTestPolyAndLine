using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class PlaneHitTest : MonoBehaviour {
    [SerializeField] GameObject _quadObject;
    GameObject _pointPrefab;

	// Use this for initialization
	void Start () {
        Addressables.LoadAsset<GameObject>("Point").Completed += op =>
        {
            Debug.Log(op.Result);
            _pointPrefab = op.Result;

            Main();
        };
	}

    void Main()
    {
        var mesh = _quadObject.GetComponent<MeshFilter>().sharedMesh;

        //始点終点の決定
        Vector3 start = new Vector3(-5, 2, 2);
        Vector3 end = new Vector3(3, -2, -5);

        //線分の可視化
        DrawLine(start, end);

        //平面上を線分が貫通しているか判定
        bool isThrough = IsPenetraitPointsOnPlane(mesh, start, end);
        if (isThrough == false)
        {
            Debug.Log("線分はポリゴンを貫通していません。(1)");
            return;
        }

        //貫通点を算出
        Vector3 penetraitPoint = CalcPenetraitPoint(mesh, start, end);
        Debug.Log(string.Format("penetraitPoint:{0}", penetraitPoint));

        //貫通点がポリゴン内に存在するか判定
        bool result = IsPenetraitInThePolygon(mesh, penetraitPoint);

        if (result)
        {
            Debug.Log("線分はポリゴンを貫通しています");
        }
        else
        {
            Debug.Log("線分はポリゴンを貫通していません(2)");
        }
    }

    /// <summary>
    /// 始点、終点からラインを描画する
    /// </summary>
    /// <param name="a">始点</param>
    /// <param name="b">終点</param>
    void DrawLine(Vector3 a, Vector3 b)
    {
        LineRenderer lineRenderer = new GameObject("aa").AddComponent<LineRenderer>();
        lineRenderer.SetPosition(0, a);
        lineRenderer.SetPosition(1, b);
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.01f;
        lineRenderer.startColor = Color.black;
        lineRenderer.endColor = Color.gray;
    }

    /// <summary>
    /// ポリゴンの法線ベクトルと線分の点から平面を貫通しているかどうかをチェック
    /// </summary>
    /// <param name="mesh">ポリゴンのメッシュデータ</param>
    /// <param name="start">線分の始点</param>
    /// <param name="end">線分の終点</param>
    /// <returns>ポリゴンの法線ベクトルと線分の点から平面を貫通しているかどうか</returns>
    bool IsPenetraitPointsOnPlane(Mesh mesh, Vector3 start, Vector3 end)
    {
        //変数の準備
        Vector3 normal = mesh.normals[0];
        Vector3 center = _quadObject.transform.position;

        //ポリゴンの中心点と線分へのベクトルを算出
        Vector3 v1 = start - center;
        Vector3 v2 = end - center;

        //線分（始点、終点）ベクトルと法線の内積の積をとる（鋭角*鈍角だった場合負の結果を算出する）
        float result = Vector3.Dot(v1, normal) * Vector3.Dot(v2, normal);

        //積の結果が負の場合、貫通している
        return result < 0;
    }

    /// <summary>
    /// ポリゴン平面までの距離から内分比を算出して貫通点の座標を確定
    /// </summary>
    /// <param name="mesh">ポリゴンのメッシュデータ</param>
    /// <param name="start">線分の始点</param>
    /// <param name="end">線分の終点</param>
    /// <returns>貫通点</returns>
    Vector3 CalcPenetraitPoint(Mesh mesh, Vector3 start, Vector3 end)
    {
        //変数の準備
        Vector3 n = mesh.normals[0];
        Vector3 p0 = _quadObject.transform.position + mesh.vertices[0];

        //ポリゴンの頂点から線分への距離ベクトル計算
        Vector3 v1 = start - p0;
        Vector3 v2 = end - p0;


        //線分の頂点と、ポリゴンとの距離を計算
        float d1 = Mathf.Abs(Vector3.Dot(n, v1)) / Vector3.Magnitude(n);
        float d2 = Mathf.Abs(Vector3.Dot(n, v2)) / Vector3.Magnitude(n);
        

        //内分比を計算
        float a = d1 / (d1 + d2);

        //内分点を計算
        Vector3 v3 = (1 - a) * v1 + a * v2;

        GameObject.Instantiate(_pointPrefab, p0 + v3, Quaternion.identity);
        return p0 + v3;
    }

    /// <summary>
    /// ポリゴン内部に貫通点が含まれるかをチェック
    /// </summary>
    /// <param name="mesh">ポリゴンのメッシュデータ</param>
    /// <param name="penetraitPoint">貫通点</param>
    /// <returns>貫通点がポリゴン内部を貫通していたか</returns>
    bool IsPenetraitInThePolygon(Mesh mesh, Vector3 penetraitPoint)
    {
        Vector3 min = Vector3.positiveInfinity, max = Vector3.negativeInfinity;
        //ポリゴンの座標最小値、最大値を取得
        for(int i = 0; i < mesh.vertices.Length; i++)
        {
            Vector3 current = mesh.vertices[i] + _quadObject.transform.position;
            if (current.x < min.x) min.x = current.x;
            if (current.x > max.x) max.x = current.x;
            if (current.y < min.y) min.y = current.y;
            if (current.y > max.y) max.y = current.y;
            if (current.z < min.z) min.z = current.z;
            if (current.z > max.z) max.z = current.z;
        }


        //ポリゴンの座標内に貫通点が存在するか判定
        if(min.x <= penetraitPoint.x && penetraitPoint.x <= max.x)
        {
            if(min.y <= penetraitPoint.y && penetraitPoint.y <= max.y)
            {
                if(min.z <= penetraitPoint.z && penetraitPoint.z <= max.z)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
