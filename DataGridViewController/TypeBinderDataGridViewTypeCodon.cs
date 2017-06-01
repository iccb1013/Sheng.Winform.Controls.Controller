using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections;

namespace Sheng.Winform.Controls.Controller
{
    public class TypeBinderDataGridViewTypeCodon : ITypeBinderDataGridViewTypeCodon
    {
        #region 公开属性

        /// <summary>
        /// 所对应的绑定数据类型
        /// </summary>
        public Type DataBoundType
        {
            get;
            private set;
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

        /// <summary>
        /// 该类型所对应的列集合
        /// </summary>
        public List<DataGridViewColumn> Columns
        {
            get;
            set;
        }

        #endregion

        #region 构造

        public TypeBinderDataGridViewTypeCodon(Type dataBoundType)
            : this(dataBoundType, null, null)
        {
        }

        public TypeBinderDataGridViewTypeCodon(Type dataBoundType, Type itemType,
            string itemsMember)
        {
            DataBoundType = dataBoundType;
            ItemType = itemType;
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

            //在判断类型兼容性时需要考虑接口！
            bool compatible = false;

            if (DataBoundType.IsClass)
            {
                compatible = type.Equals(DataBoundType) ||
                    (_actOnSubClass && type.IsSubclassOf(DataBoundType));
            }
            else if (DataBoundType.IsInterface)
            {
                compatible = type == DataBoundType || type.GetInterface(DataBoundType.ToString()) != null;
            }

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

            //在判断类型兼容性时需要考虑接口！
            bool compatible = false;

            if (DataBoundType.IsClass)
            {
                compatible = type.Equals(DataBoundType) || DataBoundType.IsSubclassOf(type);
            }
            else if (DataBoundType.IsInterface)
            {
                compatible = DataBoundType == type || DataBoundType.GetInterface(type.ToString()) != null;
            }

            return compatible;
        }

        public override string ToString()
        {
            if (DataBoundType != null)
                return DataBoundType.ToString();
            else
                return base.ToString();
        }

        public IBindingListEx InitializeBindingList()
        {
            BindingListEx<object> bindingList = new BindingListEx<object>();
            return bindingList;
        }

        public IBindingListEx InitializeBindingList(IList list)
        {
            BindingListEx<object> bindingList = new BindingListEx<object>(list.Cast<object>().ToList());
            return bindingList;
        }

        #endregion

    }
}
