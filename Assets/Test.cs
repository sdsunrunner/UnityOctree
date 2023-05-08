using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Test : MonoBehaviour
{
    public bool isAutoTest = true;
    public Transform mContainer;
    public Transform posObj;

    public float mGetNearbyDis = 20;
    public int mObjNuim = 100000;


    float mMaxDis = 100;   
    // Initial size (metres), initial centre position, minimum node size (metres)
    PointOctree<GameObject> mPointTree = null;
  
    GameObject[] mNearByObj;
    List<GameObject> mAllObj;
   void Start()
    {
        mPointTree = new PointOctree<GameObject>(mMaxDis, mContainer.position, 1);

        GameObject tempObj;
        Vector3 tempPos;
        for (int i = 0; i < mObjNuim; i++)
        {
            tempObj = new GameObject();
          
            float x = Random.Range(-1 * mMaxDis, mMaxDis);
            float y = Random.Range(-1 * mMaxDis, mMaxDis);
            float z = Random.Range(-1 * mMaxDis, mMaxDis);
            tempPos = new Vector3(x, y, z);
            tempObj.transform.position = tempPos;
            tempObj.name = i.ToString();
            
            mPointTree.Add(tempObj, tempPos);
        }

        mAllObj = mPointTree.GetAll() as List<GameObject>;
    }

    private void Update()
    {
        if (null == mPointTree) return;
        if (!isAutoTest) return;

        float t = Time.time;
        TestMethod();
        UnityEngine.Debug.Log(string.Format("total: {0} ms", Time.time - t));

        Stopwatch sw = new Stopwatch();
        sw.Start();
        TestMethod();
        sw.Stop();
        UnityEngine.Debug.Log(string.Format("total: {0} ms", sw.ElapsedMilliseconds));

    }

    void TestMethod()
    {
        mGetNearbyDis = Random.Range(0, mMaxDis);
        mNearByObj = mPointTree.GetNearby(posObj.transform.position, mGetNearbyDis);
    }
    void OnDrawGizmos()
    {
        if (null == mPointTree) return;

        if (null != mAllObj)
        {
            Gizmos.color = Color.gray;
            int allCount = mAllObj.Count;
            for (int i = 0; i < allCount; i++)
            {
                Gizmos.DrawCube(mAllObj[i].transform.position, Vector3.one);
            }
        }

        if (null != mNearByObj)
        {
            Gizmos.color = Color.red;
            int nearByCount = mNearByObj.Length;
            for (int i = 0; i < nearByCount; i++)
            {
                Gizmos.DrawCube(mNearByObj[i].transform.position, Vector3.one);
            }
        }        

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(posObj.position, mGetNearbyDis);
    }

}
