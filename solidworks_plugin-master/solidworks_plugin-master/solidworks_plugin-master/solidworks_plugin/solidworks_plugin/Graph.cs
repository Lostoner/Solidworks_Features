using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace solidworks_plugin
{
    class Graph
    {
        public VertexNode[] AdjList;//顶点表
        public int VertexNodeCount;//顶点数

        public Graph(int n)
        {
            VertexNodeCount = n;
            AdjList = new VertexNode[n];
            init();
        }

        //边表结点类
        public class EdgeNode
        {
            public int adjvex;
            public EdgeNode next;
            public EdgeNode()
            {
                next = null;
            }
        }

        //顶点表结点类
        public class VertexNode
        {
            //草图
            public Feature feature;
            public MySketch sketch;
            public object vertex;
            public EdgeNode firstedge;
            public VertexNode()
            {
                firstedge = null;
            }
        }

        public void init()
        {
            for (int vnc = 0; vnc < VertexNodeCount; vnc++)
            {
                VertexNode cuVN = new VertexNode();
                cuVN.vertex = vnc;
                cuVN.firstedge = null;
                AdjList[vnc] = cuVN;
            }
        }

        public void add(int father,int son)
        {
            EdgeNode node = new EdgeNode();
            node.adjvex = son;
            node.next = AdjList[father].firstedge;
            AdjList[father].firstedge = node;
        }

        StringBuilder sb = new StringBuilder();
        
        public void DFSTraverse(Graph G)
        {
            bool[] visited = new bool[G.VertexNodeCount];
            for (int i = 0; i < G.VertexNodeCount; i++)
            {
                visited[i] = false;
            }
            for (int i = 0; i < G.VertexNodeCount; i++)
            {
                if (!visited[i])
                {
                    DFS(G, i,ref visited);
                }

            }
        }

        private void DFS(Graph G, int i, ref bool[] visited)
        {
            EdgeNode p;
            if (G.AdjList[i].sketch == null)
            {
                System.Diagnostics.Debug.Print(G.AdjList[i].vertex.ToString());
            }
            else
            {
                System.Diagnostics.Debug.Print(G.AdjList[i].vertex.ToString()+"----"+G.AdjList[i].sketch.mySketchRelation.Count);
            }
            Debug.Print(G.AdjList[i].vertex.ToString() +":"+ G.AdjList[i].feature.Name+"---"+ G.AdjList[i].feature.GetTypeName());

            visited[i] = true;
            p = G.AdjList[i].firstedge;
            while (p != null)
            {
                if (!visited[p.adjvex])
                {
                    DFS(G, p.adjvex,ref visited);//递归
                }
                p = p.next;
            }

        }

    }
}
