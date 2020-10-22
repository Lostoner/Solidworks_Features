using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test
{
    public class MTreeNode
{
        private string _name;//节点名
        //private int _nChildren;//子节点数
       // private int _level = -1;// 记录该节点在多叉树中的层数
        List<MTreeNode> _children;// 指向其自身的子节点，children一个链表，该链表中的元素是MTreeNode类型的指针

        public MTreeNode()
        {
            _children = new List<MTreeNode>();
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /*public int NChildren
        {
            get { return _nChildren; }
            set { _nChildren = value; }
        }*/

        /*public int Level
        {
            get { return _level; }
            set { _level = value; }
        }
        */
        public List<MTreeNode> Children
        {
            get { return _children; }
            set { _children = value; }
        }

        public static MTreeNode search_node_r(string name, MTreeNode head)
        {
            MTreeNode temp = null;
            if (head != null)
            {
                if (name.Equals(head.Name))
                {
                    temp = head; //如果名字匹配
                }
                else //如果不匹配，则查找其子节点
                {
                    for (int i = 0; i < head._children.Count && temp == null; i++)
                    {
                        temp = search_node_r(name, head.Children[i]);
                    }
                }
            }
            return temp;
        }

        public static void addNode(ref MTreeNode head, string fatherName,string sonName,ref Dictionary<string,int> dic)
        {
           if (sonName.Equals("Front") || sonName.Equals("Origin")) return;
           if (dic.ContainsKey(sonName)) return;
           MTreeNode temp = null;
           if (head == null) //若为空
            {
                head.Name = fatherName; //赋名
                dic[fatherName] = 1;
                return;
            }
           else
            { 
                temp = search_node_r(fatherName, head);
            }
            //找不到
            if (temp == null) return;
            //找到节点后，对子节点进行处理
            MTreeNode node = new MTreeNode();
            node.Name = sonName;
            temp.Children.Add(node);
            dic[sonName] = 1;

        }

        public static void f1(MTreeNode head,int k)
        {
            Debug.Print(head.Name+"  "+k);
            if (head.Children != null)
            {
                int count = head._children.Count;
                Debug.Print(count+"");
                for (int i = 0; i < head._children.Count; i++)
                {
                    f1(head._children[i],k+1);
                }
            }
            
        }


    }


}
