using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{


    private const int mapWhith = 8;
    private const int mapHight = 6;
    private Point[,] map = new Point[mapWhith, mapHight];



    // Use this for initialization
    void Start()
    {
        InitMap(); 
        Point start = map[2, 3];
        Point end = map[6, 3]; 
         FindPath(start, end); 
         ShowPath(start,end); 
        //List<Point> l = GetSurroundPoints(new Point(3, 3));
        //foreach (var item in l)
        //{
        //    Debug.Log(item.X + "-" + item.Y);
        //}
    } 
    private void InitMap()
    {
        for (int x = 0; x < mapWhith; x++)
        {
            for (int y = 0; y < mapHight; y++)
            {
                map[x, y] = new Point(x, y);
            }
        }  
        map[4, 2].IsWall = true;
        map[4, 3].IsWall = true;
        map[4, 4].IsWall = true;
      
    }



    /// <summary>
    /// 现实路径
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    public void ShowPath(Point start, Point end)
    {
        Point temp = end;
        while (true)
        {
            Debug.Log( temp.X + "--" + temp.Y);

            CreateCube(temp.X, temp.Y, Color.red);
            if (temp.Parent == null)
            {
                break;
            } 
            temp = temp.Parent;
        }

        for (int x = 0; x < mapWhith; x++)
        {
            for (int y = 0; y < mapHight; y++)
            {
                if (map[x, y].IsWall == true)
                {
                    CreateCube(x, y, Color.gray);
                }
            }
        }

    }

    public void CreateCube(float x,float y, Color color)
    {
        GameObject go   = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.position = new Vector3(x, y, 0);
        go.GetComponent<MeshRenderer>().material.color = color;
    }
    /// <summary>
    ///  路径的计算
    /// </summary>
    /// <param name="start">开始点</param>
    /// <param name="end">结束点</param>
    private void FindPath(Point start, Point end)
    {
        List<Point> openList = new List<Point>();//开始集合
        List<Point> closeList = new List<Point>();//关闭集合
        openList.Add(start);

        ///这个while有两个终止条件 1，openlist为空  2， 找到目标位置
        while (openList.Count > 0)
        {
            Point point = FindMinFPoint(openList);
            openList.Remove(point);
            closeList.Add(point); 
            List<Point> surroundPoints=   GetSurroundPoints(point); 
            PointsFilter(surroundPoints, closeList); 
            foreach (Point surroundPoint in surroundPoints)
            {
                if (openList.IndexOf(surroundPoint) > -1)
                {
                    float nowG = CalcG(surroundPoint, point);
                    if (nowG < surroundPoint.G)
                    {
                        surroundPoint.UpdateParent(point, nowG);
                    }
                }
                else
                {
                    surroundPoint.Parent = point;
                    CalcF(surroundPoint, end);
                    openList.Add(surroundPoint);
                }
            }
            // 判断结束条件  判断end 是否在结束列表中 
            if (openList.IndexOf(end) > -1)
            {
                break;
            }

        }
    }

    /// <summary>
    /// 检查集合是否在关闭列表中 （目的）如果 在关闭列表就不往开启列表添加数据了
    /// </summary>
    /// <param name="src"> 需要检查的集合</param>
    /// <param name="closePoint">  已经加入关闭列表的集合</param>
    /// <returns></returns>
    private void PointsFilter(List<Point> src, List<Point> closePoint)
    {
        foreach (Point p in closePoint)
        {
            if (src.IndexOf(p) > -1)
            {
                src.Remove(p);
            }
        }
    }

    /// <summary>
    /// 返回一个点附近的点的集合
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public List<Point> GetSurroundPoints(Point point)
    {
        Point up = null, down = null, left = null, right = null;
        Point lu = null, ru = null, ld = null, rd = null;
        if (point.Y < mapHight - 1)
        {
            up = map[point.X, point.Y + 1];
        }
        if (point.Y > 0)
        {
            down = map[point.X, point.Y - 1];
        }
        if (point.X > 0)
        {
            left = map[point.X - 1, point.Y];
        }
        if (point.X < mapWhith - 1)
        {
            right = map[point.X + 1, point.Y];
        }
        if (up != null && left != null)
        {
            lu = map[point.X - 1, point.Y + 1];
        }
        if (up != null && right != null)
        {
            ru = map[point.X + 1, point.Y + 1];
        }
        if (down != null && left != null)
        {
            ld = map[point.X - 1, point.Y - 1];
        }
        if (down != null && right != null)
        {
            rd = map[point.X + 1, point.Y - 1];
        }
        List<Point> list = new List<Point>();
        if (down != null && down.IsWall == false)
        {
            list.Add(down);
        }
        if (up != null && up.IsWall == false)
        {
            list.Add(up);
        }
        if (left != null && left.IsWall == false)
        {
            list.Add(left);
        }
        if (right != null && right.IsWall == false)
        {
            list.Add(right);
        }
        if (lu != null && lu.IsWall == false && left.IsWall == false && up.IsWall == false)
        {
            list.Add(lu);
        }
        if (ld != null && ld.IsWall == false && left.IsWall == false && down.IsWall == false)
        {
            list.Add(ld);
        }
        if (ru != null && ru.IsWall == false && right.IsWall == false && up.IsWall == false)
        {
            list.Add(ru);
        }
        if (rd != null && rd.IsWall == false && right.IsWall == false && down.IsWall == false)
        {
            list.Add(rd);
        }
        return list;

    }

    /// <summary>
    /// 找寻Point最小的F值
    /// </summary>
    /// <param name="openList"></param>
    /// <returns></returns>
    private Point FindMinFPoint(List<Point> openList)
    {
        float f = float.MaxValue;
        Point temp = null;
        foreach (Point p in openList)
        {
            if (p.F < f)
            {
                temp = p;
                f = p.F;
            }
        }
        return temp;
    }
    /// <summary>
    /// 计算G值
    /// </summary>
    /// <param name="now"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public float CalcG(Point now, Point parent)
    {
        return Vector2.Distance(new Vector2(now.X, now.Y), new Vector2(parent.X, parent.Y)) + parent.G;
    }

    /// <summary>
    /// 计算F值
    /// </summary>
    /// <param name="now"></param>
    /// <param name="end"></param>
    private void CalcF(Point now, Point end)
    {
        // F  =  G  +  H 
        // 求得F值
        float h = Mathf.Abs(end.X - now.X) + Mathf.Abs(end.Y - now.Y);
        //预计到达目标点的距离
        float g = 0;
        if (now.Parent == null)
        {
            g = 0;
        }
        else
        {
            g = Vector2.Distance(new Vector2(now.X, now.Y), new Vector2(now.Parent.X, now.Parent.Y)) + now.Parent.G;
        }

        float f = g + h;
        now.F = f;
        now.G = g;
        now.H = h;

    }
}
