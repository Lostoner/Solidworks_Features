using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Diagnostics;

namespace solidworks_plugin
{
    class MySketch
    {
        Feature feature;
        public List<MySketchRelation> mySketchRelation = new List<MySketchRelation>();
     
        public MySketch(Feature feature)
        {
            this.feature = feature;
        }
        public class MySketchRelation
        {
            public int Type;//类型
            public int Count;
            public List<Entity> entity = new List<Entity>();
        }
        public class Entity
        {
            public string EntityType;
            public Dictionary<int, int> SkPointId = new Dictionary<int, int>();
        }

        public void doFunc()
        {

            SelectData swSelData = default;
            SketchRelation swSkRel;
            DisplayDimension dispDim;

            int[] vEntTypeArr = null;
            object[] vEntArr = null;
            object[] vDefEntArr = null;
            SketchSegment swSkSeg;
            SketchPoint swSkPt;
            int i = 0;
            int j = 0;
            bool bRet = false;
            object[] vSkRelArr = null;
            Sketch sketch = (Sketch)feature.GetSpecificFeature2();
            SketchRelationManager SkRelMgr = sketch.RelationManager;
            vSkRelArr = (object[])SkRelMgr.GetRelations((int)swSketchRelationFilterType_e.swAll);
            if ((vSkRelArr == null)) return;

            foreach (SketchRelation vRel in vSkRelArr)
            {
                swSkRel = (SketchRelation)vRel;
                MySketchRelation SketchRelation = new MySketchRelation();
                Entity entity = new Entity();
                //Debug.Print("    Relation(" + i + ")");
                SketchRelation.Count = i + 1;
                //Debug.Print("      Type         = " + swSkRel.GetRelationType());
                SketchRelation.Type = swSkRel.GetRelationType();
                /*dispDim = (DisplayDimension)swSkRel.GetDisplayDimension();
                if (dispDim != null)
                {
                    Debug.Print("      Display dimension         = " + dispDim.GetNameForSelection());
                }*/

                vEntTypeArr = (int[])swSkRel.GetEntitiesType();
                vEntArr = (object[])swSkRel.GetEntities();

                vDefEntArr = (object[])swSkRel.GetDefinitionEntities2();
                if ((vDefEntArr == null))
                {
                }
                else
                {
                    //Debug.Print("    Number of definition entities in this relation: " + vDefEntArr.GetUpperBound(0));
                }

                if ((vEntTypeArr != null) & (vEntArr != null))
                {

                    if (vEntTypeArr.GetUpperBound(0) == vEntArr.GetUpperBound(0))
                    {
                        j = 0;

                        foreach (swSketchRelationEntityTypes_e vType in vEntTypeArr)
                        {
                            //Debug.Print("        EntType    = " + vType);
                            switch (vType)
                            {
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Unknown:
                                    //Debug.Print("Not known");
                                    entity.EntityType = "Unknown";
                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_SubSketch:
                                    //Debug.Print("SubSketch");
                                    entity.EntityType = "SubSketch";
                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Point:
                                    swSkPt = (SketchPoint)vEntArr[j];
                                    Debug.Assert((swSkPt != null));
                                    //Debug.Print("          SkPoint ID = [" + ((int[])(swSkPt.GetID()))[0] + ", " + ((int[])(swSkPt.GetID()))[1] + "]");
                                    entity.SkPointId[((int[])(swSkPt.GetID()))[0]] = ((int[])(swSkPt.GetID()))[1];
                                    bRet = swSkPt.Select4(true, swSelData);
                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Line:
                                    //Debug.Print("Line");
                                    entity.EntityType = "Line";
                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Arc:
                                    //Debug.Print("Arc");
                                    entity.EntityType = "Arc";
                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Ellipse:
                                    //Debug.Print("Ellipse");
                                    entity.EntityType = "Ellipse";
                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Parabola:
                                    //Debug.Print("Parabola");
                                    entity.EntityType = "Parabola";
                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Spline:
                                    swSkSeg = (SketchSegment)vEntArr[j];
                                    //Debug.Print("          SkSeg   ID = [" + ((int[])(swSkSeg.GetID()))[0] + ", " + ((int[])(swSkSeg.GetID()))[1] + "]");
                                    bRet = swSkSeg.Select4(true, swSelData);
                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Hatch:
                                    //Debug.Print("Hatch");
                                    entity.EntityType = "Hatch";
                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Text:
                                    //Debug.Print("Text");
                                    entity.EntityType = "Text";
                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Plane:
                                    //Debug.Print("Plane");
                                    entity.EntityType = "Plane";
                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Cylinder:
                                    //Debug.Print("Cylinder");
                                    entity.EntityType = "Cylinder";
                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Sphere:
                                    //Debug.Print("Sphere");
                                    entity.EntityType = "Sphere";
                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Surface:
                                    //Debug.Print("Surface");
                                    entity.EntityType = "Surface";
                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Dimension:
                                    //Debug.Print("Dimension");
                                    entity.EntityType = "Dimension";
                                    break;
                                default:
                                    //Debug.Print("Something else");
                                    entity.EntityType = "Something else";
                                    break;
                            }
                            j = j + 1;
                            SketchRelation.entity.Add(entity);

                        }
                    }
                }

                i = i + 1;
                mySketchRelation.Add(SketchRelation);
            }

        }
    }
}
