using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Solidworks_Features.segs;

namespace Solidworks_Features
{
    class newSketch
    {
        const int INF = 0x3f3f3f3f;

        public List<loopSeg> loopSegs;
        public List<List<int>> loops;

        Sketch sket;

        public List<SketchSegment> segs;
        public List<newPoint> pois;
        public Dictionary<KeyValuePair<int, int>, int> idToIndex;
        public Dictionary<int, KeyValuePair<int, int>> indexToId;
        public List<segs.Arc> segArc;
        public List<segs.Line> segLin;
        public List<segs.Ellipse> segEll;
        public List<segs.Parabola> segPar;
        public List<segs.Spline> segSpl;
        //public List<List<KeyValuePair<int, int>>> adj;
        public int[, ] adj;
        public int verNum;

        public void storePoints()                           //获取点（被取代）
        {
            Debug.Print("Storing points: ");
            object[] temPoi = sket.GetSketchPoints2();
            for(int i = 0; i < temPoi.Length; i++)
            {
                newPoint tem = new newPoint((SketchPoint)temPoi[i]);
                tem.setIndex(i);
                idToIndex.Add(tem.ID, tem.index);
                indexToId.Add(tem.index, tem.ID);
                pois.Add(tem);
                Debug.Print("Point " + i.ToString() + " : (" + tem.ID.Key + ", " + tem.ID.Value + ")");
                Debug.Print(tem.x + ", " + tem.y + ", " + tem.z);
            }
        }

        public int addPoint(SketchPoint point)     //加入点（被取代）
        {
            KeyValuePair<int, int> temID = new KeyValuePair<int, int>(point.GetID()[0], point.GetID()[1]);
            if(!idToIndex.ContainsKey(temID))
            {
                for(int i = 0; i < pois.Count; i++)
                {
                    if(point.X == pois[i].x && point.Y == pois[i].y && point.Z == pois[i].z)
                    {
                        return i;
                    }
                }

                newPoint temPoint = new newPoint(point);
                pois.Add(temPoint);
                temPoint.setIndex(pois.Count - 1);
                idToIndex.Add(temID, pois.Count - 1);
                indexToId.Add(pois.Count - 1, temID);
                return (pois.Count - 1);
            }
            else
            {
                return idToIndex[temID];
            }
        }

        public void storeSegments()                     //获取边（被取代）
        {
            verNum = sket.GetSketchPoints2().Length;
            adj = new int[verNum, verNum];
            //Debug.Print("Storing segments: ");
            object[] segments = sket.GetSketchSegments();
            foreach(SketchSegment seg in segments)
            {
                bool flag = false;
                int type = seg.GetType();
                switch(type)
                {
                    case 0:
                        flag = false;
                        segs.Line temLine = new segs.Line(seg);
                        SketchLine line = (SketchLine)seg;
                        temLine.setPoint(addPoint(line.GetStartPoint2()), addPoint(line.GetEndPoint2()));
                        //temLine.setPoint(findPoint(line.GetStartPoint2()), findPoint(line.GetEndPoint2()));
                        for(int i = 0; i < segLin.Count; i++)
                        {
                            if(temLine.same(segLin[i]))
                            {
                                flag = true;
                            }
                        }
                        if(flag)
                        {
                            break;
                        }
                        else
                        {
                            segLin.Add(temLine);
                            pois[temLine.sPoint].setNext(temLine.ePoint);
                            pois[temLine.ePoint].setNext(temLine.sPoint);

                            pois[temLine.sPoint].setNextSeg(0, segLin.Count - 1);
                            pois[temLine.ePoint].setNextSeg(0, segLin.Count - 1);

                            adj[temLine.sPoint, temLine.ePoint] = 1;
                            adj[temLine.ePoint, temLine.sPoint] = 1;
                        }
                        //Debug.Print("Segment " + count.ToString() + ": " + seg.GetType() + ", (" + findPoint(line.GetStartPoint2()) + "->" + findPoint(line.GetEndPoint2()) + ")");
                        break;
                    case 1:
                        segs.Arc temArc = new segs.Arc(seg);
                        SketchArc arc = (SketchArc)seg;
                        temArc.setPoint(addPoint(arc.GetStartPoint2()), addPoint(arc.IGetEndPoint2()));
                        //temArc.setPoint(findPoint(arc.GetStartPoint2()), findPoint(arc.IGetEndPoint2()));
                        for (int i = 0; i < segArc.Count; i++)
                        {
                            if (temArc.same(segArc[i]))
                            {
                                flag = true;
                            }
                        }
                        if (flag)
                        {
                            break;
                        }
                        else
                        {
                            segArc.Add(temArc);
                            pois[temArc.sPoint].setNext(temArc.ePoint);
                            pois[temArc.ePoint].setNext(temArc.sPoint);

                            pois[temArc.sPoint].setNextSeg(1, segArc.Count - 1);
                            pois[temArc.ePoint].setNextSeg(1, segArc.Count - 1);

                            adj[temArc.sPoint, temArc.ePoint] = 1;
                            adj[temArc.ePoint, temArc.sPoint] = 1;
                        }
                        //Debug.Print("Segment " + count.ToString() + ": " + seg.GetType() + ", (" + findPoint(arc.GetStartPoint2()) + "->" + findPoint(arc.GetEndPoint2()) + ")");
                        break;
                    case 2:
                        flag = false;
                        segs.Ellipse temEllipse = new segs.Ellipse(seg);
                        SketchEllipse ellipse = (SketchEllipse)seg;
                        temEllipse.setPoint(addPoint(ellipse.GetStartPoint2()), addPoint(ellipse.GetEndPoint2()), addPoint(ellipse.GetCenterPoint2()));
                        //temEllipse.setPoint(findPoint(ellipse.GetStartPoint2()), findPoint(ellipse.GetEndPoint2()), findPoint(ellipse.GetCenterPoint2()));
                        for (int i = 0; i < segEll.Count; i++)
                        {
                            if (temEllipse.same(segEll[i]))
                            {
                                flag = true;
                            }
                        }
                        if (flag)
                        {
                            break;
                        }
                        else
                        {
                            segEll.Add(temEllipse);
                            pois[temEllipse.sPoint].setNext(temEllipse.ePoint);
                            pois[temEllipse.ePoint].setNext(temEllipse.sPoint);

                            pois[temEllipse.sPoint].setNextSeg(2, segEll.Count - 1);
                            pois[temEllipse.ePoint].setNextSeg(2, segEll.Count - 1);

                            adj[temEllipse.sPoint, temEllipse.ePoint] = 1;
                            adj[temEllipse.ePoint, temEllipse.sPoint] = 1;
                        }
                        //Debug.Print("Segment " + count.ToString() + ": " + seg.GetType() + ", (" + findPoint(ellipse.GetStartPoint2()) + "->" + findPoint(ellipse.GetEndPoint2()) + ")");
                        break;
                    case 3:
                        flag = false;
                        segs.Spline temSpline = new segs.Spline(seg);
                        SketchSpline spline = (SketchSpline)seg;
                        SketchPoint[] tempoints = spline.GetPoints2();
                        for (int i = 0; i < tempoints.Length; i++)
                        {
                            int index = addPoint(tempoints[i]);
                            temSpline.setPoint(index);
                        }
                        /*
                        for(int i = 0; i < tempoints.Length; i++)
                        {
                            int index = findPoint(tempoints[i]);
                            temSpline.setPoint(index);
                        }
                        */
                        for (int i = 0; i < segSpl.Count; i++)
                        {
                            if (temSpline.same(segSpl[i]))
                            {
                                flag = true;
                            }
                        }
                        if (flag)
                        {
                            break;
                        }
                        else
                        {
                            segSpl.Add(temSpline);
                            pois[temSpline.sPoint].setNext(temSpline.ePoint);
                            pois[temSpline.ePoint].setNext(temSpline.sPoint);

                            pois[temSpline.sPoint].setNextSeg(3, segSpl.Count - 1);
                            pois[temSpline.ePoint].setNextSeg(3, segSpl.Count - 1);

                            adj[temSpline.sPoint, temSpline.ePoint] = 1;
                            adj[temSpline.ePoint, temSpline.sPoint] = 1;
                        }
                        //Debug.Print("Spline");
                        break;
                    case 5:
                        segs.Parabola temParabola = new segs.Parabola(seg);
                        SketchParabola parabola = (SketchParabola)seg;
                        temParabola.setPoint(addPoint(parabola.GetStartPoint2()), addPoint(parabola.IGetEndPoint2()));
                        //temParabola.setPoint(findPoint(parabola.GetStartPoint2()), findPoint(parabola.IGetEndPoint2()));
                        for (int i = 0; i < segPar.Count; i++)
                        {
                            if (temParabola.same(segPar[i]))
                            {
                                flag = true;
                            }
                        }
                        if (flag)
                        {
                            break;
                        }
                        else
                        {
                            segPar.Add(temParabola);
                            pois[temParabola.sPoint].setNext(temParabola.ePoint);
                            pois[temParabola.ePoint].setNext(temParabola.sPoint);

                            pois[temParabola.sPoint].setNextSeg(5, segPar.Count - 1);
                            pois[temParabola.ePoint].setNextSeg(5, segPar.Count - 1);

                            adj[temParabola.sPoint, temParabola.ePoint] = 1;
                            adj[temParabola.ePoint, temParabola.sPoint] = 1;
                        }
                        //Debug.Print("Segment " + count.ToString() + ": " + seg.GetType() + ", (" + findPoint(parabola.GetStartPoint2()) + "->" + findPoint(parabola.GetEndPoint2()) + ")");
                        break;
                    case 4:
                        break;
                }
            }
        }

        public void storeSegments2()                    //获取边与点，取代上面三个函数，目前使用中
        {
            if(sket.GetSketchPoints2() == null)
            {
                Debug.Print("No points!");
                return;
            }
            verNum = sket.GetSketchPoints2().Length;            //以下为保存边、边的邻接点；点、点的邻接边的操作
            adj = new int[verNum, verNum];
            for(int i = 0; i< verNum; i++)                                  //初始化邻接点数组，为之后的最小环提取做准备，在存储点与边的同时需要存储邻接关系
            {
                for(int j = 0; j < verNum; j++)
                {
                    adj[i, j] = INF;
                }
            }
            object[] segments = sket.GetSketchSegments();
            foreach (SketchSegment seg in segments)             //此时遍历所有边，判断边的类型，而后分类存储
            {
                bool flag = false;
                int type = seg.GetType();
                switch (type)
                {
                    case 0:
                        flag = false;
                        loopSeg temLine = new loopSeg(seg);
                        SketchLine line = (SketchLine)seg;
                        temLine.setPoint(addPoint(line.GetStartPoint2()), addPoint(line.GetEndPoint2()));
                        temLine.setIndex(loopSegs.Count);
                        for(int i = 0; i < loopSegs.Count; i++)
                        {
                            if(temLine.same(loopSegs[i]))
                            {
                                flag = true;
                            }
                        }
                        if(!flag)
                        {
                            loopSegs.Add(temLine);

                            pois[temLine.start].setNext(temLine.end);
                            pois[temLine.end].setNext(temLine.start);

                            pois[temLine.start].setNextSeg2(temLine.index);
                            pois[temLine.end].setNextSeg2(temLine.index);

                            adj[temLine.start, temLine.end] = 1;
                            adj[temLine.end, temLine.start] = 1;
                        }
                        break;
                    case 1:
                        flag = false;
                        loopSeg temArc = new loopSeg(seg);
                        SketchArc arc = (SketchArc)seg;
                        temArc.setPoint(addPoint(arc.GetStartPoint2()), addPoint(arc.GetEndPoint2()));
                        temArc.setIndex(loopSegs.Count);
                        for (int i = 0; i < loopSegs.Count; i++)
                        {
                            if (temArc.same(loopSegs[i]))
                            {
                                flag = true;
                            }
                        }
                        if(!flag)
                        {
                            loopSegs.Add(temArc);

                            pois[temArc.start].setNext(temArc.end);
                            pois[temArc.end].setNext(temArc.start);

                            pois[temArc.start].setNextSeg2(temArc.index);
                            pois[temArc.end].setNextSeg2(temArc.index);

                            adj[temArc.start, temArc.end] = 1;
                            adj[temArc.end, temArc.start] = 1;
                        }
                        break;
                    case 2:
                        flag = false;
                        loopSeg temEll = new loopSeg(seg);
                        SketchEllipse ell = (SketchEllipse)seg;
                        temEll.setPoint(addPoint(ell.GetStartPoint2()), addPoint(ell.GetEndPoint2()));
                        temEll.setIndex(loopSegs.Count);
                        for (int i = 0; i < loopSegs.Count; i++)
                        {
                            if (temEll.same(loopSegs[i]))
                            {
                                flag = true;
                            }
                        }
                        if(!flag)
                        {
                            loopSegs.Add(temEll);

                            pois[temEll.start].setNext(temEll.end);
                            pois[temEll.end].setNext(temEll.start);

                            pois[temEll.start].setNextSeg2(temEll.index);
                            pois[temEll.end].setNextSeg2(temEll.index);

                            adj[temEll.start, temEll.end] = 1;
                            adj[temEll.end, temEll.start] = 1;
                        }
                        break;
                    case 3:
                        flag = false;
                        loopSeg temSpl = new loopSeg(seg);
                        SketchSpline spl = (SketchSpline)seg;
                        object[] tem= spl.GetPoints2();
                        temSpl.setPoint(addPoint((SketchPoint)tem[0]), addPoint((SketchPoint)tem[tem.Length - 1]));
                        temSpl.setIndex(loopSegs.Count);
                        for (int i = 0; i < loopSegs.Count; i++)
                        {
                            if (temSpl.same(loopSegs[i]))
                            {
                                flag = true;
                            }
                        }
                        if (!flag)
                        {
                            loopSegs.Add(temSpl);

                            pois[temSpl.start].setNext(temSpl.end);
                            pois[temSpl.end].setNext(temSpl.start);

                            pois[temSpl.start].setNextSeg2(temSpl.index);
                            pois[temSpl.end].setNextSeg2(temSpl.index);

                            adj[temSpl.start, temSpl.end] = 1;
                            adj[temSpl.end, temSpl.start] = 1;
                        }
                        break;
                    case 5:
                        flag = false;
                        loopSeg temPar = new loopSeg(seg);
                        SketchParabola par = (SketchParabola)seg;
                        temPar.setPoint(addPoint(par.GetStartPoint2()), addPoint(par.GetEndPoint2()));
                        temPar.setIndex(loopSegs.Count);
                        for (int i = 0; i < loopSegs.Count; i++)
                        {
                            if (temPar.same(loopSegs[i]))
                            {
                                flag = true;
                            }
                        }
                        if (!flag)
                        {
                            loopSegs.Add(temPar);

                            pois[temPar.start].setNext(temPar.end);
                            pois[temPar.end].setNext(temPar.start);

                            pois[temPar.start].setNextSeg2(temPar.index);
                            pois[temPar.end].setNextSeg2(temPar.index);

                            adj[temPar.start, temPar.end] = 1;
                            adj[temPar.end, temPar.start] = 1;
                        }
                        break;
                    case 4:
                        break;
                }
            }
        }

        public int findPoint(SketchPoint point)     //通过点的源数据找到点的索引
        {
            KeyValuePair<int, int> ID = new KeyValuePair<int, int>(point.GetID()[0], point.GetID()[1]);
            //int id1 = point.GetID()[0];
            //int id2 = point.GetID()[1];
            for(int i = 0; i < pois.Count; i++)
            {
                if(ID.Key == pois[i].ID.Key && ID.Value == pois[i].ID.Value)
                {
                    return pois[i].index;
                }
            }
            return -1;
        }

        public void printData()                             //Debug用打印函数
        {
            Debug.Print("------------------------Points--------------------------");
            for(int i = 0; i < pois.Count; i++)
            {
                Debug.Print("Point " + i.ToString() + " : (" + pois[i].ID.Key + ", " + pois[i].ID.Value + "); " + "index: " + pois[i].index);
                Debug.Print(pois[i].x + ", " + pois[i].y + ", " + pois[i].z);
                for(int j = 0; j < pois[i].next.Count; j++)
                {
                    Debug.Print("Point to point: ->" + pois[i].next[j]);
                    Debug.Print("Point to segment: ->" + pois[i].nextSeg[j]);
                }
            }
            Debug.Print("----------------------Segments------------------------");
            for(int i = 0; i < segLin.Count; i++)
            {
                Debug.Print("Line " + i + ": " + segLin[i].sPoint + "->" + segLin[i].ePoint);
            }

            for (int i = 0; i < segArc.Count; i++)
            {
                Debug.Print("Arc " + i + ": " + segArc[i].sPoint + "->" + segArc[i].ePoint);
            }

            for (int i = 0; i < segEll.Count; i++)
            {
                Debug.Print("Ellipse " + i + ": " + segEll[i].sPoint + "->" + segEll[i].ePoint);
            }

            for (int i = 0; i < segPar.Count; i++)
            {
                Debug.Print("Parabola " + i + ": " + segPar[i].sPoint + "->" + segPar[i].ePoint);
            }

            for(int i = 0; i < segSpl.Count; i++)
            {
                Debug.Print("Spline " + i + ": " + segSpl[i].sPoint + "->" + segSpl[i].ePoint);
            }
        }

        public void delPoint(int index)                 //废弃函数（从数组中删除点）
        {
            for(int i = 0; i< pois[index].nextSegs.Count; i++)
            {
                loopSegs.Remove(loopSegs[pois[index].nextSegs[i]]);
            }
            pois.Remove(pois[index]);
        }

        public void getLoop()                               //获取一个草图中所有最小环
        {
            for(int i = 0; i < pois.Count; i++)         //以下主要为Dijkstra算法，基于上面存储边与点的基础上，故以下不加以算法注释
            {
                if(pois[i].nextSegs.Count < 2)
                {
                    continue;
                }
                for(int j = 0; j < pois[i].nextSegs.Count; j++)
                {
                    List<int> temLoop = new List<int>();
                    int[] path = new int[pois.Count];
                    int[] dis = new int[pois.Count];
                    int[] visited = new int[pois.Count];
                    for(int k = 0; k < pois.Count; k++)
                    {
                        path[k] = -1;
                        visited[k] = 0;
                        if (adj[i, k] != INF)
                        {
                            dis[k] = adj[i, k];
                        }
                        else
                        {
                            dis[k] = INF;
                        }
                    }
                    visited[i] = 1;
                    /*
                    adj[i, pois[i].next[j]] = INF;
                    adj[pois[i].next[j], i] = INF;
                    dis[pois[i].next[j]] = INF;
                    */
                    int tem_count = 0;
                    for(int k = 0; k < pois[i].next.Count; k++)
                    {
                        if(pois[i].next[k] == pois[i].next[j])
                        {
                            tem_count++;
                        }
                    }
                    if(tem_count < 2)
                    {
                        adj[i, pois[i].next[j]] = INF;
                        adj[pois[i].next[j], i] = INF;
                        dis[pois[i].next[j]] = INF;
                    }

                    int minIndex = -1;
                    for (int k = 1; k < pois.Count; k++)
                    {
                        int min = INF;
                        for(int m = 0; m < pois.Count; m++)
                        {
                            if(visited[m] != 1 && dis[m] < min)
                            {
                                min = dis[m];
                                minIndex = m;
                            }
                        }
                        if(minIndex == -1)
                        {
                            break;
                        }
                        if(k ==1)
                        {
                            path[minIndex] = i;
                        }
                        visited[minIndex] = 1;
                        for(int m = 0; m < pois.Count; m++)
                        {
                            if (dis[m] > adj[minIndex, m] + min && visited[m] != 1)
                            {
                                dis[m] = adj[minIndex, m] + min;
                                path[m] = minIndex;
                            }
                        }
                    }
                    adj[i, pois[i].next[j]] = 1;
                    adj[pois[i].next[j], i] = 1;

                    /*
                    Debug.Print("Path:" + i);
                    for(int k = 0; k < path.Length; k++)
                    {
                        Debug.Print("==>" + path[k]);
                    }
                    */
                    
                    int next = path[pois[i].next[j]];
                    int last = pois[i].next[j];
                    bool flag = false;
                    while(next != i)
                    {
                        if(next == -1)
                        {
                            flag = true;
                            break;
                        }
                        for(int k = 0; k < loopSegs.Count; k++)
                        {
                            if((loopSegs[k].end == next && loopSegs[k].start == last) || (loopSegs[k].end == last && loopSegs[k].start == next))
                            {
                                temLoop.Add(k);
                            }
                        }
                        last = next;
                        next = path[next];
                    }
                    if(flag)
                    {
                        break;
                    }
                    for (int k = 0; k < loopSegs.Count; k++)
                    {
                        if ((loopSegs[k].end == last && loopSegs[k].start == next) || (loopSegs[k].end == next && loopSegs[k].start == last))
                        {
                            temLoop.Add(k);
                        }
                    }
                    for (int k = 0; k < loopSegs.Count; k++)
                    {
                        if ((loopSegs[k].end == i && loopSegs[k].start == pois[i].next[j]) || (loopSegs[k].end == pois[i].next[j] && loopSegs[k].start == i))
                        {
                            temLoop.Add(k);
                        }
                    }

                    if(loopCheck(temLoop))
                    {
                        loops.Add(temLoop);
                    }
                }
            }

            for(int i = 0; i < loopSegs.Count; i++)
            {
                if(loopSegs[i].seg.GetType() == 1)
                {
                    SketchArc temArc = (SketchArc)loopSegs[i].seg;
                    if(temArc.GetStartPoint2() == temArc.IGetEndPoint2())
                    {
                        List<int> tem = new List<int>();
                        tem.Add(i);
                        loops.Add(tem);
                    }
                }
            }
        }

        public bool loopCheck(List<int> toCheck)        //检查最小环数组中的重复
        {
            for(int i = 0; i < loops.Count; i++)
            {
                if(loops[i].Count != toCheck.Count)
                {
                    return true;
                }
                else
                {
                    int start = -1;
                    for (int j = 0; j < loops[i].Count; j++)
                    {
                        if (loops[i][j] == toCheck[0])
                        {
                            start = j;
                        }
                    }

                    if(start == -1)
                    {
                        continue;
                    }
                    else
                    {
                        bool flag1 = false;
                        bool flag2 = false;
                        for (int j = 0; j < toCheck.Count; j++)
                        {
                            if (toCheck[j] != loops[i][(j + start) % toCheck.Count])
                            {
                                flag1 = true;
                            }
                        }

                        for(int j = 0; j < toCheck.Count; j++)
                        {
                            if(toCheck[j] != loops[i][(start - j + toCheck.Count) % toCheck.Count])
                            {
                                flag2 = true;
                            }
                        }

                        if(flag1 && flag2)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public void changePois(ISldWorks swApp)         //将点变为绝对坐标下
        {
            MathUtility mathUtil = (MathUtility)swApp.GetMathUtility();
            for(int i = 0; i < pois.Count; i++)
            {
                double[] vPnt = new double[3];
                vPnt[0] = pois[i].x;
                vPnt[1] = pois[i].y;
                vPnt[2] = pois[i].z;
                MathPoint mp = (MathPoint)mathUtil.CreatePoint(vPnt);
                //double[] temPoi = mp.ArrayData;
                //MathTransform trans = sket.ModelToSketchTransform;
                MathTransform trans = sket.ModelToSketchTransform.Inverse();
                mp = mp.MultiplyTransform(trans);
                double[] temPoi = mp.ArrayData;

                pois[i].setLocation(temPoi[0], temPoi[1], temPoi[2]);

                Debug.Print("Point " + i + ": " + pois[i].ox + ", " + pois[i].oy + ", " + pois[i].oz);
            }
        }

        public void printLoop()
        {
            Debug.Print("===========================================Loop===========================================\n");
            for (int i = 0; i < loops.Count; i++)
            {
                Debug.Print("-------------------------------------------------------------------------------------------------------------------\n");
                Debug.Print("Loop " + i + ": \n");
                for(int j = 0; j < loops[i].Count; j++)
                {
                    Debug.Print(loops[i][j] + "->");
                    Debug.Print("(" + loopSegs[loops[i][j]].start + "->" + loopSegs[loops[i][j]].end + ")");
                }
            }
        }

        public void printPoint()
        {
            for(int i = 0; i < pois.Count; i++)
            {
                Debug.Print("Point " + i + ": ");
                Debug.Print("next: " + pois[i].next.Count);
                Debug.Print("nextSegs: " + pois[i].nextSegs.Count);
                for(int j = 0; j < pois[i].next.Count; j++)
                {
                    Debug.Print("->" + pois[i].next[j]);
                }
                for(int j = 0; j < pois[i].nextSegs.Count; j++)
                {
                    Debug.Print("=>" + pois[i].nextSegs[j]);
                }
            }
        }

/*
        public bool dfs(int ori, int index, KeyValuePair<int, int> seg, Loop unfinished)
        {
            switch(seg.Key)
            {
                case 0:
                    Line temseg = segLin[seg.Value];
                    int otherP = temseg.getAnother(index);
                    if(otherP != ori)
                    {
                        if (pois[otherP].nextSeg.Count - 1 != 0)
                        {
                            unfinished.store(temseg.ori, pois[otherP].ori);
                            for(int i = 0; i < pois[otherP].nextSeg.Count; i++)
                            {
                                if(!pois[otherP].nextSeg[i].Equals(seg))
                                {
                                    bool flag = dfs(ori, otherP, pois[otherP].nextSeg[i], unfinished);
                                    if (flag)
                                    {
                                        if(pois[otherP].nextSeg.Count == 2)
                                        {
                                            pois[otherP].nextSeg.Remove(seg);
                                            pois[index].nextSeg.Remove(seg);
                                        }
                                        return true;
                                    }
                                    else
                                    {
                                        unfinished.delete();
                                        return false;
                                    }
                                }
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if(pois[index].nextSeg.Count == 2)
                        {

                        }
                        return true;
                    }
                    break;
            }
        }
*/

        public newSketch(Sketch ske)
        {
            loopSegs = new List<loopSeg>();
            loops = new List<List<int>>();

            sket = ske;

            segs = new List<SketchSegment>();
            pois = new List<newPoint>();
            idToIndex = new Dictionary<KeyValuePair<int, int>, int>();
            indexToId = new Dictionary<int, KeyValuePair<int, int>>();
            segArc = new List<segs.Arc>();
            segLin = new List<segs.Line>();
            segEll = new List<segs.Ellipse>();
            segPar = new List<segs.Parabola>();
            segSpl = new List<segs.Spline>();
        }
    }
}
