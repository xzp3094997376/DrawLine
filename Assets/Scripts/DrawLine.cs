using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{

    List<Vector3> posList;
    LineRenderer lineR;
    Line line;
    Color32 c = new Color32(255, 0, 0, 255);
    // Use this for initialization
    void Start()
    {
        posList = new List<Vector3>();
        lineR = this.GetComponent<LineRenderer>();
        if (lineR == null)
        {
            lineR = gameObject.AddComponent<LineRenderer>();
                 
        }
        lineR.startWidth = lineR.endWidth = 1f;
        line = new Line();

    }  
    Vector3 RayHit()
    {
        Vector3 hitPoint = Vector3.zero;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit rhit;
        if (Physics.Raycast(ray, out rhit))
        {
            hitPoint=rhit.point;
        }
        return hitPoint;
    }    
    [Header("点数")]
    public int pCount = 0;
   [Header("转折点")]
    public int turnCount = 0;
    public float Mangle;
    Vector3 lastPos=new Vector3(Mathf.NegativeInfinity,0,0);
    bool isOver = false;
    public float angle = 0;
    Vector3 dir1, dir2;
    Vector3 startPos;
    // Update is called once per frame   
    void Update()
    {
        if (isOver)
        {
            return;
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 pos= RayHit();           
            if (pos!=Vector3.zero)
            {
                if (pos.x-lastPos.x<0||pos==lastPos)
                {
                    return;
                }
                Debug.Log(lastPos+"    ,   "+pos);
                posList.Add(pos);
                if (pCount == 0)
                {
                    startPos = pos;
                }               

                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                go.GetComponent<BoxCollider>().enabled = false;
                go.transform.SetParent(transform);
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = transform.InverseTransformPoint(pos);

                      

                if (pCount>= 1)
                {
                    Vector3 curDir = Vector3.Normalize(pos - lastPos);                   
                    if (pCount==1)
                    {
                        //Debug.LogError(pos+"  ,   last:  "+lastPos);
                        dir1 = Vector3.Normalize(pos - lastPos);
                        float k = (curDir.y / curDir.x);
                        Debug.DrawLine(startPos, pos, c, 1000);
                        c.g += 50;             
                    }
                    else
                    {                                                                        
                        float angle = Vector3.Angle(dir1.normalized, curDir.normalized);

                        if (angle > 30)
                        {                            
                            go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                            go.GetComponent<SphereCollider>().enabled = false;
                            go.transform.SetParent(transform);
                            go.transform.localScale = Vector3.one;
                            go.transform.localPosition = transform.InverseTransformPoint(lastPos);

                            curDir = Vector3.Normalize(pos - lastPos);

                            Debug.LogError(Vector3.Angle(dir1, curDir));
                            dir1= curDir;
                            float k = (curDir.y / curDir.x);
                            Debug.DrawLine(lastPos, new Vector3(lastPos.x + 10000, k * 10000 + lastPos.y, lastPos.z), c, 1000);
                            c.g += 50;
                            if (turnCount++ == 1)
                            {
                                isOver = true;
                            }
                        }
                    }

                    lineR.positionCount = posList.Count;
                    if (isOver)
                    {
                        lineR.positionCount = posList.Count-1;
                    }
                   
                    for (int i = 0; i < lineR.positionCount; i++)
                    {
                        lineR.SetPosition(i, posList[i]);
                    }
                }
                pCount++;
                lastPos = pos;                
            }                  
        }          
    }
    

}

[System.Serializable]
public class Line
{
    Vector3 startPos;
    Vector3 endPos;   
    public Line()
    {
        DIR = new Vector3[3];
        endPos = startPos = Vector3.zero;
    }
    public Vector3 START
    {
        get
        {
            return startPos;
        }
        set
        {
            startPos = value;
        }
    }
    public Vector3 END
    {
        get
        {
            return endPos;
        }
        set
        {
            endPos = value;
        }
    }
    public Vector3[] DIR;//方向    
}