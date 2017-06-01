using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections;

namespace Sheng.SailingEase.Kernal
{
    /// <summary>
    /// 描述用于绑定到树节点的对象类型
    /// </summary>
    public class TypeBinderTreeViewNodeCodon
    {
        #region 公开属性

        /// <summary>
        /// 所对应的绑定数据类型
        /// </summary>
        public Type DataBoundType
        {
            get;
            set;
        }

        private bool _actOnSubClass = false;
        /// <summary>
        /// 是否对 DataBoundType 的子类型有效
        /// 默认无效
        /// 如果设置为 true，又同时添加了基类与子类的 codon，则运行时会取到哪个codon不确定
        /// 通常取先添加的那个
        /// </summary>
        public bool ActOnSubClass
        {
            get { return _actOnSubClass; }
            set { _actOnSubClass = value; }
        }

        public string TextMember
        {
            get;
            set;
        }

        /// <summary>
        /// 子集的对象类型
        /// </summary>
        public Type ItemType
        {
            get;
            set;
        }

        public string ItemsMember
        {
            get;
            set;
        }

        /// <summary>
        /// 该类型所对应的上下文菜单（右键菜单）
        /// </summary>
        public ContextMenuStrip ContextMenuStrip
        {
            get;
            set;
        }

        private int _imageIndex = 0;
        public int ImageIndex
        {
            get { return _imageIndex; }
            set { _imageIndex = value; }
        }

        private int _selectedImageIndex = -1;
        /// <summary>
        /// 默认为-1
        /// 即，使用与ImageIndex同样的值
        /// </summary>
        public int SelectedImageIndex
        {
            get { return _selectedImageIndex; }
            set { _selectedImageIndex = value; }
        }

        //object 是dataBoundItem
        private Func<object, IList> _getItemsFunc;
        /// <summary>
        /// 在没有ItemsMember的情况下，通过此委托获取子项（如果有）
        /// 但是在这种情况下，必须要注意的就是每次获取子项集合时，得到的都是不同的集合对象
        /// 在进行比较判断时要特别注意
        /// </summary>
        public Func<object, IList> GetItemsFunc
        {
            get { return _getItemsFunc; }
            set { _getItemsFunc = value; }
        }

        #endregion

        #region 构造

        public TypeBinderTreeViewNodeCodon()
        {

        }

        /// <summary>
        /// 用于此节点没有任何子项的情况
        /// </summary>
        /// <param name="dataBoundType"></param>
        /// <param name="textMember"></param>
        public TypeBinderTreeViewNodeCodon(Type dataBoundType, string textMember)
            : this(dataBoundType, null, textMember, null)
        {
        }

        //public TypeBinderTreeViewNodeCodon(Type dataBoundType, string textMember, string itemsMember)
        //    : this(dataBoundType, null, textMember, itemsMember)
        //{
        //}

        /// <summary>
        /// 不指定 itemsMember ，将通过 GetItemsFunc 获得子项
        /// </summary>
        /// <param name="dataBoundType"></param>
        /// <param name="itemType"></param>
        /// <param name="textMember"></param>
        public TypeBinderTreeViewNodeCodon(Type dataBoundType, Type itemType, string textMember)
            : this(dataBoundType, itemType, textMember, null)
        {
        }

        public TypeBinderTreeViewNodeCodon(Type dataBoundType, Type itemType, string textMember, string itemsMember)
        {
            DataBoundType = dataBoundType;
            ItemType = itemType;
            TextMember = textMember;
            ItemsMember = itemsMember;
        }

        #endregion

        #region 公开方法

        /// <summary>
        /// 判断本Codon能否与指定的对象的类型相兼容
        /// 即：指定的对象的类型是否是 DataBoundType ，或DataBoundType的子类型（如果ActOnSubClass）
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Compatible(object obj)
        {
            if (obj == null)
            {
                Debug.Assert(false, "obj为null");
                throw new ArgumentNullException();
            }

            return Compatible(obj.GetType());
        }

        /// <summary>
        /// 判断本Codon能否与指定的Type相兼容
        /// 即：指定的类型是否是 DataBoundType ，或DataBoundType的子类型（如果ActOnSubClass）
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool Compatible(Type type)
        {
            if (type == null)
            {
                /*
                 * 这里type是有可能为null的，是正常情况，如：
                 *  _gridView.GridViewController.DataBind(e.Node.Items, e.Node.ItemType, e.Node.DataBoundItem);
                 *  其中的 e.Node.ItemType 为null，因为没有配子类型
                 *  这时进到这里来的时候，type就为null
                 *  按讲，外部调用者应确认type是否为null，但为保证健壮性，这里视type为null时，返回false
                 *  不抛异常，但断言，因为应该在外部调用之前判断type是否为null
                 */

                Debug.Assert(false, "type为null");
                return false;
            }

            bool compatible = type.Equals(DataBoundType) ||
                (_actOnSubClass && type.IsSubclassOf(DataBoundType));

            return compatible;
        }

        public bool UpwardCompatible(object obj)
        {
            if (obj == null)
            {
                Debug.Assert(false, "obj为null");
                throw new ArgumentNullException();
            }

            return UpwardCompatible(obj.GetType());
        }

        /// <summary>
        /// 判断本Codon所绑定的对象类型是否与指定的Type向上兼容
        /// 即：指定的类型是否是 DataBoundType ，或 DataBoundType 是指定类型的子类型
        /// 向上判断是否兼容，主要用在选择对象时，如以object类型做为泛型参数，可以定位任何子元素
        /// 在类型关系上是由上至下的指认关系，而Compatible方法是反之的
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool UpwardCompatible(Type type)
        {
            if (type == null)
            {
                Debug.Assert(false, "type为null");
                throw new ArgumentNullException();
            }

            bool compatible = type.Equals(DataBoundType) || DataBoundType.IsSubclassOf(type);

            return compatible;
        }

        public override string ToString()
        {
            if (DataBoundType != null)
                return DataBoundType.ToString();
            else
                return base.ToString();
        }

        #endregion

    }
}
