using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowAI : MonoBehaviour
{
    public Transform targetTranform;
    public float speed = 3;
    /// <summary>
    /// 开始速度
    /// </summary>
    private Vector3 startVector3;
    /// <summary>
    ///  分离的力 距离我们三米内的乌鸦 就会跟其他乌鸦进行分离
    /// </summary> 
    [Header("随机播放动画的时间")]
    public float animRandom = 2;

    /// <summary>
    /// 当前的速度
    /// </summary>
    public Vector3 velocity = Vector3.forward;

    /// <summary>
    /// 质量
    /// </summary>
    public float m = 1;

    /// <summary>
    /// 综合的力
    /// </summary>
    [Header("综合的力")]
    public Vector3 sumForce = Vector3.zero;

    /// <summary>
    /// 分离的力的全中
    /// </summary>
    [Header("分离的力的全中")]
    public float separationWeight = 1;
    [Header(" 分离的力 距离我们三米内的乌鸦  就会跟其他乌鸦进行分离")]
    public float separationDistance = 3;
    [Header(" 分离的集合 要添加的 3米附近的乌鸦集合 ")]
    public List<GameObject> seaprationNeighbors = new List<GameObject>();
    /// <summary>
    /// 分离的力
    /// </summary>
    [Header("分离的力")]
    public Vector3 separationFirce = Vector3.zero;

    /// <summary>
    /// 队列的集合
    /// </summary>
    [Header("队列的集合")]
    public List<GameObject> alignmentNeighbors = new List<GameObject>();
    /// <summary>
    ///  队列  的距离
    /// </summary>
    [Header(" 队列 的距离 ")]
    public float alignmentDistance = 6;
    [Header(" 队列 的力的全中 ")]
    public float alignmentWeight = 1;
    /// <summary>
    /// 队列的力
    /// </summary>
    [Header("队列的力")]
    public Vector3 alignmentForce = Vector3.zero;
    /// <summary>
    /// 聚集的力的全中
    /// </summary>
    public float conhesionWeight = 1;
    /// <summary>
    /// 聚集的力
    /// </summary>
    [Header("聚集的力")]
    public Vector3 cohesionForce = Vector3.zero;

    /// <summary>
    /// 检查的时间间隔 (一秒计算五次)
    /// </summary>
    [Header("检查的时间间隔")]
    public float chechInterval = 0.2f;
    public Animation animatio;
    private IEnumerator Start()
    {
        targetTranform = GameObject.Find("target").transform;

        startVector3 = velocity;
        InvokeRepeating("CalcForce", 0, chechInterval);
        // targetTranform = GameObject.Find("TargetPos").transform;
        animatio = GetComponentInChildren<Animation>();
        yield return new WaitForSeconds(UnityEngine.Random.Range(0, animRandom));
        animatio.Play();
    }

    void CalcForce()
    {
        //每一次计算力前都把力归零
        sumForce = Vector3.zero;
        separationFirce = Vector3.zero;
        alignmentForce = Vector3.zero;
        cohesionForce = Vector3.zero;

        seaprationNeighbors.Clear();
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, separationDistance);

        foreach (Collider item in colliders)
        {
            if (item != null && item.gameObject != this.gameObject)
            {
                seaprationNeighbors.Add(item.gameObject);
            }
        }
        //1 分离的力
        foreach (GameObject neighbor in seaprationNeighbors)
        {
            Vector3 dir = transform.position - neighbor.transform.position;

            separationFirce += dir.normalized / dir.magnitude;  // dir /   dir的长度(自己到neighbor的距离）
                                                                //dir的长度 越小 得到的 力就越大  长度越大产生的力就越小
        }
        //旁边有乌鸦才会更新
        if (seaprationNeighbors.Count > 0)
        {
            //现在 separationWeight 的力为1 没有影响  如果发现separationWeight  比较小  就刷一个全中 让他变大
            separationFirce *= separationWeight;
            sumForce += separationFirce;
        }
        // 计算队列的力 
        alignmentNeighbors.Clear();
        colliders = Physics.OverlapSphere(this.transform.position, alignmentDistance);

        foreach (Collider c in colliders)
        {
            if (c != null && c.gameObject != this.gameObject)
            {
                alignmentNeighbors.Add(c.gameObject);
            }

        }
        //平均朝向
        Vector3 avgDir = Vector3.zero;

        foreach (GameObject n in alignmentNeighbors)
        {
            //总的朝向
            avgDir += n.transform.forward;
        }
        if (alignmentNeighbors.Count > 0)
        {
            //除以总数等于   平均朝向
            avgDir /= alignmentNeighbors.Count;
            //根据 平均方向 - 当前方向  就等于 要朝向的 朝向   根据下面图片 做参考
            alignmentForce = avgDir - transform.forward;
            //刷个全中  如果力太小的话可以放大
            alignmentForce *= alignmentWeight;
            sumForce += alignmentForce;
        }
        //聚集的力  
        //seaprationNeighbors  分离的数组大于0  只有小于等于0的时候 才能聚集 
        //alignmentNeighbors  队列的力小于等于0   都没办法计算 队列的力了
        if (/*seaprationNeighbors.Count < 0 &&*/ alignmentNeighbors.Count > 0)
        {
            //中心点
            Vector3 center = Vector3.zero;

            foreach (GameObject item in alignmentNeighbors)
            {
                center += item.transform.position;
            }
            center /= alignmentNeighbors.Count;
            //得到移动的力  center 越大  得到的力就越大 刚好符合
            Vector3 dirToCenter = center - this.transform.position;
            //如果这种聚集力不合适的话就使用下面的
            cohesionForce += dirToCenter;
            // 如果上面的力不行的话就是用这种方法
            // cohesionForce += dirToCenter .normalized *velocity.magnitude; 
            cohesionForce *= conhesionWeight;
            sumForce += cohesionForce;
        } 

        ////保持恒定飞行速度的力   
         Vector3 engineForce = startVector3 - velocity;
      //  Vector3 engineForce = velocity.normalized * startVector3.magnitude;
        sumForce += engineForce * 0.1f;


        Vector3 targetDir = targetTranform.position - transform.position;
        //temp方向减去 当前方向 
        sumForce += (targetDir.normalized - transform.forward) * speed;

    }
    private void Update()
    {
        //加速度
        Vector3 a = sumForce / m;

        velocity += a * Time.deltaTime;
        //因为 初始的队列的方向是一致的 就会导致  队列的力不起作用 所以必须加上下面的代码
        //当前的朝向 和速度的朝向保持一致
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(velocity), Time.deltaTime);

        // transform.Translate(transform.forward * Time.deltaTime *velocity.magnitude, Space.World);
        transform.Translate(Time.deltaTime * velocity, Space.World);
    }

}
