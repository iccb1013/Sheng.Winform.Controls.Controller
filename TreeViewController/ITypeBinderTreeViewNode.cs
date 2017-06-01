using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;

namespace Sheng.Winform.Controls.Controller
{
    public interface ITypeBinderTreeViewNode
    {
        /// <summary>
        /// 所绑定的对象
        /// </summary>
        object DataBoundItem { get; set; }

        TypeBinderTreeViewNodeCodon Codon { get; set; }

        /// <summary>
        /// Items中的对象的类型，不是DataBoundItem 的 Type！
        /// </summary>
        Type ItemType { get; }

        /// <summary>
        /// 此属性用于获取此节点下的数据集，用于呈现在右侧列表中
        /// </summary>
        IList Items { get; }

        /// <summary>
        /// 用于移除节点时，把绑定对象从所属列表中一并移除
        /// </summary>
        IList OwnerList { get; set; }

        string TextMember { get; }

        string ItemsMember { get; }

        /// <summary>
        /// 刷新节点
        /// 通常用来更新节点的Text
        /// </summary>
        void Update();

        /// <summary>
        /// 刷新节点
        /// 用指定的 obj 中的属性来更新，用于所绑定的对象与obj不是同一个对象实例的情况
        /// </summary>
        void Update(object obj);

        /// <summary>
        /// 树节点对象
        /// </summary>
        TreeNode Node { get; }

        /// <summary>
        /// 获取上一个同级树节点
        /// </summary>
        ITypeBinderTreeViewNode PrevNode { get; }

        /// <summary>
        /// 获取下一个同级树节点
        /// </summary>
        ITypeBinderTreeViewNode NextNode { get; }

        /// <summary>
        /// 父节点，若无，返回null
        /// </summary>
        ITypeBinderTreeViewNode Parent { get; }

        /// <summary>
        /// 添加子节点
        /// 并把子节点所绑定的对象，同步加到Items集合中
        /// </summary>
        /// <param name="obj"></param>
        void AddChild(ITypeBinderTreeViewNode node);

        /// <summary>
        /// 删除子节点
        /// 并把子节点所绑定的对象，同步从Items集合中删除
        /// </summary>
        void Remove();
    }
}
