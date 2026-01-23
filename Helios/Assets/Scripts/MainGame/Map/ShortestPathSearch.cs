using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// マップ上での最短経路探索クラス（BFS版）
/// </summary>
public class ShortestPathSearch : MonoBehaviour
{
    [SerializeField] Map map;
    private SpecifyingDeliveryRoutes specifying;

    [SerializeField] private string[] shortestPathStrings;

    private readonly Vector2Int[] directions =
    {
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(0, -1)
    };

    void Start()
    {
        map = GameObject.Find("Map").GetComponent<Map>();
        specifying = GameObject.Find("Drivers").GetComponent<SpecifyingDeliveryRoutes>();
    }

    /// <summary>
    /// スタートからゴールまでの最短経路を取得
    /// </summary>
    public List<Vector3> ShortestPath(int widthID, int heightID, int targetWidthID, int targetHeightID, ref List<int[]> routes)
    {
        Map.MapData[][] mapData = map.MapDatas;


        // shortestPath を初期化
        for (int y = 0; y < mapData.Length; y++)
            for (int x = 0; x < mapData[y].Length; x++)
                mapData[y][x].shortestPath = int.MaxValue;

        // BFSで最短距離を計算
        BFSShortestPath(mapData, targetWidthID, targetHeightID,widthID,heightID );

        // エディタ表示用に文字列化
        ConvertShortestPathToStrings(mapData);

        // 経路格納
        List<Vector3> positions = new List<Vector3>();
        positions.Add(mapData[heightID][widthID].obj.transform.localPosition); // スタート位置追加
        routes.Clear();
        SetRoute(widthID, heightID, targetWidthID, targetHeightID, ref mapData, ref positions,ref routes);
        int[] ints = new int[2];
        ints[0] = targetWidthID;
        ints[1] = targetHeightID;
        routes.Add(ints);
        // スタート→ゴール順にする
        //positions.Reverse();
        Debug.Log("数は"+positions.Count);
        return positions;
    }

    /// <summary>
    /// BFSで最短距離を計算
    /// </summary>
    private void BFSShortestPath(Map.MapData[][] mapData, int targetX, int targetY,int startX,int startY)
    {
      
        Queue<(int x, int y)> queue = new Queue<(int x, int y)>();
        mapData[targetY][targetX].shortestPath = 0;
        queue.Enqueue((targetX, targetY));
        
        while (queue.Count > 0)
        {
            Debug.Log("値を埋め込みます");
            var (x, y) = queue.Dequeue();
            int currentDist = mapData[y][x].shortestPath;
            Debug.Log(currentDist);
            foreach (var dir in directions)
            {
                int newX = x + dir.x;
                int newY = y + dir.y;

                // 範囲外チェック
                if (newX < 0 || newX >= mapData[0].Length || newY < 0 || newY >= mapData.Length)
                    continue;

                // 通行不可セルはスキップ
                if (!IsWalkable(mapData[newY][newX].objectID))
                    continue;

                // 既により短い距離が入っていればスキップ
                if (mapData[newY][newX].shortestPath <= currentDist + 1)
                    continue;

                // 最短距離を更新してキューに追加
                Debug.Log("Queueに追加しました");
                mapData[newY][newX].shortestPath = currentDist + 1;
                queue.Enqueue((newX, newY));
                if (mapData[startY][startX].objectID==(int)Map.MapObjectID.HOUSE_1)
                {

                    mapData[startY][startX].shortestPath = currentDist + 2;
                    queue.Enqueue((newX, newY));
                }
           

            }
        }
       
    }

    /// <summary>
    /// 通行可能かどうか
    /// </summary>
    private bool IsWalkable(int objectID)
    {
        return objectID >= (int)Map.MapObjectID.COMPANY && objectID <= (int)Map.MapObjectID.CROSS;
    }

    /// <summary>
    /// 最短経路を再帰で取得
    /// </summary>
    private void SetRoute(int startX, int startY, int targetX, int targetY, ref Map.MapData[][] mapData, ref List<Vector3> routesPos,ref List<int[]> routes)
    {
        if (startX == targetX && startY == targetY) 
            {
                Debug.Log("returnします");
                return;
            }

        int currentDist = mapData[startY][startX].shortestPath;
        Debug.Log(mapData[startY][startX].obj.name+currentDist);
        foreach (var dir in directions)
        {
            int newX = startX + dir.x;
            int newY = startY + dir.y;

            if (newX < 0 || newX >= mapData[0].Length || newY < 0 || newY >= mapData.Length)
                continue;
            Debug.Log("周囲"+ mapData[newY][newX].shortestPath);
            if (mapData[newY][newX].shortestPath == currentDist - 1)
            {
                Debug.Log("positionsに追加しました");
                routesPos.Add(mapData[newY][newX].obj.transform.localPosition);
                int[] ints = new int[2];
                ints[0] = mapData[newY][newX].heightPositionID;
                ints[1] = mapData[newY][newX].widthPositionID;
                routes.Add(ints);
                SetRoute(newX, newY, targetX, targetY, ref mapData, ref routesPos,ref routes);
                break; // 一度進めばOK
            }
        }
    }

    /// <summary>
    /// shortestPath を文字列に変換して配列に格納
    /// Unity エディタで確認用
    /// </summary>
    private void ConvertShortestPathToStrings(Map.MapData[][] mapData)
    {
        int rows = mapData.Length;
        int cols = mapData[0].Length;
        shortestPathStrings = new string[rows];

        for (int y = 0; y < rows; y++)
        {
            string row = "";
            for (int x = 0; x < cols; x++)
            {
                row += mapData[y][x].shortestPath == int.MaxValue ? "X" : mapData[y][x].shortestPath.ToString();
                if (x < cols - 1) row += ",";
            }
            shortestPathStrings[y] = row;
        }
    }
}