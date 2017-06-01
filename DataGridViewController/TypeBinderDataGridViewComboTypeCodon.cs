using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections;

namespace Sheng.Winform.Controls.Controller
{
    /// <summary>
    /// 复合显示多种类型的对象（同时在列表中显示多个类型的对象）
    /// </summary>
    public class TypeBinderDataGridViewComboTypeCodon<T> : ITypeBinderDataGridViewTypeCodon
    {
        #region 私有成员

        private List<TypeBinderDataGridViewTypeCodon> _codons = new List<TypeBinderDataGridViewTypeCodon>();

        #endregion

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
        /// 此属性在此无用，因为复合多种类型的对象后，子集的对象类型不确定
        /// </summary>
        public Type ItemType
        {
            get
            {
                Debug.Assert(false, "不应该调用到这里");
                return null;
            }
        }

        /// <summary>
        /// 此属性在此无用，因为复合多种类型的对象后，子集的对象类型不确定
        /// </summary>
        public string ItemsMember
        {
            get
            {
                Debug.Assert(false, "不应该调用到这里");
                return null;
            }
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

        public TypeBinderDataGridViewComboTypeCodon()
        {
            DataBoundType = typeof(T);
        }

        #endregion

        #region 公开方法

        public void AddCodon(TypeBinderDataGridViewTypeCodon codon)
        {
            if (codon == null)
            {
                Debug.Assert(false, "codon 为 null");
                throw new ArgumentNullException();
            }

            if (_codons.Contains(codon))
            {
                Debug.Assert(false, "_typeBinderDataGridViewTypeCodons 重复添加:" + codon.ToString());
                return;
            }

            Type dataBoundType = codon.DataBoundType;

            Debug.Assert(GetCodon(dataBoundType) == null,
                "_typeBinderDataGridViewTypeCodons 重复添加类型:" + codon.ToString());

            //判断要添加的codon所绑定的类型是不是此复合codon所指定的类型的子类型
            //这里不能调用 Compatible 方法，因为 Compatible 方法受 ActOnSubClass 属性的影响
            bool compatible = false;

            if (DataBoundType.IsClass)
            {
                compatible = dataBoundType.Equals(DataBoundType) || dataBoundType.IsSubclassOf(DataBoundType);
            }
            else if (DataBoundType.IsInterface)
            {
                compatible = dataBoundType == DataBoundType || 
                    dataBoundType.GetInterface(DataBoundType.ToString()) != null;
            }

            if (compatible == false)
            {
                Debug.Assert(false, "指定的 codon 所绑定的对象类型不是该 ComboCodon 绑定类型的子类型:" + codon.ToString());
                //    throw new ArgumentException();
            }

            _codons.Add(codon);
        }

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

            if (type.IsClass)
            {
                compatible = type.Equals(DataBoundType) || DataBoundType.IsSubclassOf(type);
            }
            else if (type.IsInterface)
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
            BindingListEx<T> bindingList = new BindingListEx<T>();
            return bindingList;
        }

        public IBindingListEx InitializeBindingList(IList list)
        {
            BindingListEx<T> bindingList = new BindingListEx<T>(list.Cast<T>().ToList());
            return bindingList;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 获取与指定类型兼容的Codon
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private TypeBinderDataGridViewTypeCodon GetCodon(Type type)
        {
            if (type == null)
                return null;

            foreach (var code in _codons)
            {
                if (code.DataBoundType == null)
                    continue;

                if (code.Compatible(type))
                    return code;
            }

            return null;
        }

        #endregion
    }
}