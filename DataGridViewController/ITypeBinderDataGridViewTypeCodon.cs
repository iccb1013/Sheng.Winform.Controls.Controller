using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace Sheng.Winform.Controls.Controller
{
    public interface ITypeBinderDataGridViewTypeCodon
    {
        #region 属性

        /// <summary>
        /// 所对应的绑定数据类型
        /// </summary>
        Type DataBoundType { get; }

        /// <summary>
        /// 是否对 DataBoundType 的子类型有效
        /// 默认无效
        /// 如果设置为 true，又同时添加了基类与子类的 codon，则运行时会取到哪个codon不确定
        /// 通常取先添加的那个
        /// </summary>
        bool ActOnSubClass { get; }

        /// <summary>
        /// 子集的对象类型
        /// </summary>
        Type ItemType { get; }

        string ItemsMember { get; }

        /// <summary>
        /// 该类型所对应的上下文菜单（右键菜单）
        /// </summary>
        ContextMenuStrip ContextMenuStrip { get; }

        /// <summary>
        /// 该类型所对应的列集合
        /// </summary>
        List<DataGridViewColumn> Columns { get; }

        #endregion

        #region 方法

        /// <summary>
        /// 判断本Codon能否与指定的对象的类型相兼容
        /// 即：指定的对象的类型是否是 DataBoundType ，或DataBoundType的子类型（如果ActOnSubClass）
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool Compatible(object obj);

        /// <summary>
        /// 判断本Codon能否与指定的Type相兼容
        /// 即：指定的类型是否是 DataBoundType ，或DataBoundType的子类型（如果ActOnSubClass）
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool Compatible(Type type);

        bool UpwardCompatible(object obj);

        /// <summary>
        /// 判断本Codon所绑定的对象类型是否与指定的Type向上兼容
        /// 即：指定的类型是否是 DataBoundType ，或 DataBoundType 是指定类型的子类型
        /// 向上判断是否兼容，主要用在选择对象时，如以object类型做为泛型参数，可以定位任何子元素
        /// 在类型关系上是由上至下的指认关系，而Compatible方法是反之的
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool UpwardCompatible(Type type);

        IBindingListEx InitializeBindingList();

        /// <summary>
        /// 使用指定的 IList 初始化一个新的 BindgList
        /// 这个方法的作用是在复合 codon 中，根据泛型参数初始化特定类型的 BindingList
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        IBindingListEx InitializeBindingList(IList list);

        #endregion
    }
}
