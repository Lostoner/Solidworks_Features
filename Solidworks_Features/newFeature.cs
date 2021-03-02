using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Solidworks_Features
{
    class newFeature
    {
        public int type;                                    //该Feature的类型
        public List<int> sketchs;                      //该Feature对应的草图的索引数组
        public Feature ori;                                //该Feature的源数据
        public List<int> sons;                           //该Feature的子Feature索引数组
        public typedata feaData;                       //该Feature本身的数据，详情查看typedata.cs

        public newFeature(Feature swFeat)
        {
            ori = swFeat;
            sketchs = new List<int>();
            sons = new List<int>();
            type = -1;
            feaData = new typedata(classifyType());
        }

        public void setSon(int son)
        {
            sons.Add(son);
        }

        public void setSketch(int ske)
        {
            sketchs.Add(ske);
        }

        public int classifyType()
        {
            switch (ori.GetTypeName())
            {
                case "BaseBody":
                    return 0;               //IExtrudeFeatureData2
                    break;
                case "Blend":
                    return 1;               //ILoftFeatureData
                    break;
                case "BlendCut":
                    return 1;
                    break;
                case "Boss":
                    return 0;
                    break;
                case "BossThin":
                    return 0;
                    break;
                case "Cut":
                    return 0;
                    break;
                case "CutThin":
                    return 0;
                    break;
                case "Extrusion":
                    return 0;
                    break;
                case "NetBlend":
                    return 2;               //IBoundaryBossFeatureData
                    break;
                case "RevCut":
                    return 3;               //IRevolveFeatureData2
                    break;
                case "Revolution":
                    return 3;
                    break;
                case "RevolutionThin":
                    return 3;
                    break;
                case "Sweep":
                    return 4;               //ISweepFeatureData
                    break;
                case "SweepCut":
                    return 4;
                    break;
                default:
                    return -1;
                    break;
            }

        }

        public void getFeaData()                     //获取Feature本身的数据
        {
            switch (feaData.type)
            {
                case 0:
                    IExtrudeFeatureData2 extrudeData = (IExtrudeFeatureData2)ori.GetDefinition();

                    feaData.EbothDirections = extrudeData.BothDirections;
                    feaData.Edepth = extrudeData.GetDepth(true) + extrudeData.GetDepth(false);
                    Debug.Print("Depth: " + feaData.Edepth.ToString());
                    feaData.EreverseOffset = extrudeData.GetReverseOffset(true);
                    feaData.EwallThickness = extrudeData.GetWallThickness(true);
                    switch (extrudeData.ThinWallType)
                    {
                        case 0:
                            feaData.EwallThickness = extrudeData.GetWallThickness(true);
                            //Debug.Print("Thickness: " + feaData.EwallThickness.ToString());
                            break;
                        case 1:
                            feaData.EwallThickness = extrudeData.GetWallThickness(false);
                            //Debug.Print("Thickness: " + feaData.EwallThickness.ToString());
                            break;
                        case 2:
                            feaData.EwallThickness = extrudeData.GetWallThickness(true);
                            //Debug.Print("Thickness: " + feaData.EwallThickness.ToString());
                            break;
                        case 3:
                            feaData.EwallThickness = extrudeData.GetWallThickness(true) + extrudeData.GetWallThickness(false);
                            //Debug.Print("Thickness: " + feaData.EwallThickness.ToString());
                            break;
                    }
                    break;
                case 1:
                    ILoftFeatureData loftData = (ILoftFeatureData)ori.GetDefinition();

                    //object vec1 = loftData.StartDirectionVector;
                    //Vector vec2 = (Vector)loftData.EndDirectionVector;
                    //feaData.LwallThickness = loftData.GetWallThickness(true);
                    switch (loftData.ThinWallType)
                    {
                        case 0:
                            feaData.LwallThickness = loftData.GetWallThickness(true);
                            break;
                        case 1:
                            feaData.LwallThickness = loftData.GetWallThickness(false);
                            break;
                        case 2:
                            feaData.LwallThickness = loftData.GetWallThickness(true);
                            break;
                        case 3:
                            feaData.LwallThickness = loftData.GetWallThickness(true) + loftData.GetWallThickness(false);
                            break;
                    }
                    feaData.LstartTangentLength = loftData.StartTangentLength;
                    feaData.LendTangentLength = loftData.EndTangentLength;
                    //Debug.Print(feaData.LwallThickness.ToString());

                    break;
                case 2:
                    IBoundaryBossFeatureData boundaryBossData = (BoundaryBossFeatureData)ori.GetDefinition();
                    //feaData.BtangentLength = boundaryBossData.GetTangentLength(2, );
                    switch (boundaryBossData.ThinFeatureType)
                    {
                        case 0:
                            feaData.BwallThickness = boundaryBossData.ThinFeatureThickness[true];
                            break;
                        case 1:
                            feaData.BwallThickness = boundaryBossData.ThinFeatureThickness[true];
                            break;
                        case 2:
                            feaData.BwallThickness = boundaryBossData.ThinFeatureThickness[true];
                            break;
                        case 3:
                            feaData.BwallThickness = boundaryBossData.ThinFeatureThickness[true] + boundaryBossData.ThinFeatureThickness[false];
                            break;
                    }
                    //feaData.BwallThickness1 = boundaryBossData.ThinFeatureThickness[false];
                    //feaData.BwallThickness2 = boundaryBossData.ThinFeatureThickness[true];

                    break;
                case 3:
                    IRevolveFeatureData2 revolveData = (IRevolveFeatureData2)ori.GetDefinition();
                    feaData.Rangle = revolveData.GetRevolutionAngle(true);
                    //feaData.RwallThickness = revolveData.GetWallThickness(true);
                    switch (revolveData.ThinWallType)
                    {
                        case 0:
                            feaData.RwallThickness = revolveData.GetWallThickness(true);
                            break;
                        case 1:
                            feaData.RwallThickness = revolveData.GetWallThickness(false);
                            break;
                        case 2:
                            feaData.RwallThickness = revolveData.GetWallThickness(true);
                            break;
                        case 3:
                            feaData.RwallThickness = revolveData.GetWallThickness(true) + revolveData.GetWallThickness(false);
                            break;
                    }

                    break;
                case 4:
                    ISweepFeatureData sweepData = (ISweepFeatureData)ori.GetDefinition();
                    //feaData.SwallThickness = sweepData.GetWallThickness(true);
                    switch (sweepData.ThinWallType)
                    {
                        case 0:
                            feaData.SwallThickness = sweepData.GetWallThickness(true);
                            break;
                        case 1:
                            feaData.SwallThickness = sweepData.GetWallThickness(false);
                            break;
                        case 2:
                            feaData.SwallThickness = sweepData.GetWallThickness(true);
                            break;
                        case 3:
                            feaData.SwallThickness = sweepData.GetWallThickness(true) + sweepData.GetWallThickness(false);
                            break;
                    }

                    break;
            }
            /*
            feaData.EbothDirections = extrudeData.BothDirections;
            feaData.Edepth = extrudeData.GetDepth(true);
            feaData.EreverseOffset = extrudeData.GetReverseOffset(true);
            feaData.EwallThickness = extrudeData.GetWallThickness(true);
            */


        }
    }
}
