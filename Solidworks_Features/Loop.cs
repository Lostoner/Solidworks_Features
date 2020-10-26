using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using SolidWorks.Interop.swconst;
using System.Windows.Forms;

namespace Solidworks_Features
{
    class singleElement
    {
        string type;
        
    }

    
    class Loop
    {
        int numOfPoints;
        List<SketchPoint> points = new List<SketchPoint>();
        List<SketchSegment> arcs = new List<SketchSegment>();
        List<SketchSegment> loop = new List<SketchSegment>();
        SketchPoint nPoint = new SketchPoint();
        SketchPoint sPoint = new SketchPoint();
        bool flag = false;
        Dictionary<int, string> TypePairs = new Dictionary<int, string>();

        public Loop()
        {
            TypePairs.Add(1, "swSketchARC");
            TypePairs.Add(2, "swSketchELLIPSE");
            TypePairs.Add(0, "swSketchLINE");
            TypePairs.Add(5, "swSketchPARABOLA");
            TypePairs.Add(3, "swSketchSPLINE");
            TypePairs.Add(4, "swSketchTEXT");
        }

        public SketchPoint PointCheck(SketchPoint uncheckedPoint1, SketchPoint uncheckedPoint2)
        {
            if ((uncheckedPoint1.GetID()[0] == nPoint.GetID()[0]) && (uncheckedPoint1.GetID()[1] == nPoint.GetID()[1]))
            {
                return uncheckedPoint2;
            }
            else if ((uncheckedPoint2.GetID()[0] == nPoint.GetID()[0]) && (uncheckedPoint2.GetID()[1] == nPoint.GetID()[1]))
            {
                return uncheckedPoint1;
            }
            else
            {
                return null;
            }
        }

        public void storeLoop(Sketch swSketch)
        {
            arcs = swSketch.GetSketchSegments();
            int count = loop.Count;
            while(count < arcs.Count)
            {
                foreach (SketchSegment segment in arcs)
                {
                    switch (segment.GetType())
                    {
                        case 0:
                            SketchLine temLine = (SketchLine)segment;
                            SketchPoint startPoint = temLine.GetStartPoint2();
                            SketchPoint endPoint = temLine.IGetEndPoint2();
                            if (flag == false)
                            {
                                sPoint = startPoint;
                                nPoint = endPoint;
                                flag = true;
                            }
                            else
                            {
                                SketchPoint temPoint = PointCheck(startPoint, endPoint);
                                if (temPoint != null)
                                {
                                    loop.Add(segment);
                                    nPoint = temPoint;
                                    count++;
                                }
                            }
                            break;
                        case 1:
                            SketchArc temArc = (SketchArc)segment;
                            startPoint = temArc.GetStartPoint2();
                            endPoint = temArc.GetEndPoint2();
                            if (flag == false)
                            {
                                sPoint = startPoint;
                                nPoint = endPoint;
                                flag = true;
                            }
                            else
                            {
                                SketchPoint temPoint = PointCheck(startPoint, endPoint);
                                if (temPoint != null)
                                {
                                    loop.Add(segment);
                                    nPoint = temPoint;
                                    count++;
                                }
                            }
                            break;
                        case 2:
                            SketchEllipse temEllipse = (SketchEllipse)segment;
                            startPoint = temEllipse.GetStartPoint2();
                            endPoint = temEllipse.GetEndPoint2();
                            if (flag == false)
                            {
                                sPoint = startPoint;
                                nPoint = endPoint;
                                flag = true;
                            }
                            else
                            {
                                SketchPoint temPoint = PointCheck(startPoint, endPoint);
                                if (temPoint != null)
                                {
                                    loop.Add(segment);
                                    nPoint = temPoint;
                                    count++;
                                }
                            }
                            break;
                        case 3:
                            count++;
                            /*
                            SketchSpline temSpline = (SketchSpline)segment;
                            //startPoint = temSpline.GetStartPoint2();
                            //endPoint = temSpline.GetEndPoint2();
                            startPoint = temSpline.GetPoints2();
                            if (flag == false)
                            {
                                sPoint = startPoint;
                                nPoint = endPoint;
                                flag = true;
                            }
                            else
                            {
                                SketchPoint temPoint = PointCheck(startPoint, endPoint);
                                if (temPoint != null)
                                {
                                    loop.Add(segment);
                                    nPoint = temPoint;
                                }
                            }
                            */
                            break;
                        case 4:
                            count++;
                            break;
                        case 5:
                            SketchParabola temParabola = (SketchParabola)segment;
                            startPoint = temParabola.GetStartPoint2();
                            endPoint = temParabola.GetEndPoint2();
                            if (flag == false)
                            {
                                sPoint = startPoint;
                                nPoint = endPoint;
                                flag = true;
                            }
                            else
                            {
                                SketchPoint temPoint = PointCheck(startPoint, endPoint);
                                if (temPoint != null)
                                {
                                    loop.Add(segment);
                                    nPoint = temPoint;
                                    count++;
                                }
                            }
                            break;
                    }
                }
            }
        }

        public void showLoop(TextBox textBox1)
        {
            for(int i = 0; i < loop.Count; i++)
            {
                textBox1.AppendText(TypePairs[loop[i].GetType()]);
            }
        }
    }
}
