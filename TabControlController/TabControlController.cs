using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace Sheng.SailingEase.Kernal
{
    /*
     *  把每个tab页在内部和一个类型关系起来，对外接口直接以类型替代tab页进行操作
     *  同时，每个页对应一个用户控件，等于说，一个类型对应一个用户控件
     */

    public class TabControlController
    {
        #region 私有成员

        private TabControl _tabControl;

        private Dictionary<Type, TypeBinderTabPage> _tabPages = new Dictionary<Type, TypeBinderTabPage>();

        #endregion

        #region 公开属性

        /// <summary>
        /// 当前选定的Tab页所绑定的类型
        /// 如果没有选定的tab页返回null
        /// </summary>
        public Type SelectedTabType
        {
            get
            {
                if (_tabControl.SelectedTab == null)
                    return null;
                else
                {
                    TypeBinderTabPage tabPage = (TypeBinderTabPage)_tabControl.SelectedTab;
                    return tabPage.BoundType;
                }
            }
        }

        /// <summary>
        /// 当前选定的tab页所对应的视图
        /// 如果没有选定的tab页返回null
        /// </summary>
        public Control SelectedView
        {
            get
            {
                if (_tabControl.SelectedTab == null)
                    return null;
                else
                {
                    TypeBinderTabPage tabPage = (TypeBinderTabPage)_tabControl.SelectedTab;
                    return tabPage.View;
                }
            }
        }

        #endregion

        #region 构造

        public TabControlController(TabControl tabControl)
        {
            _tabControl = tabControl;

            Debug.Assert(_tabControl.TabPages.Count == 0, "不能把带有tab页的TabControl放进来");
            _tabControl.TabPages.Clear();

            _tabControl.Selected += new TabControlEventHandler(_tabControl_Selected);
        }

        #endregion

        #region 事件处理

        void _tabControl_Selected(object sender, TabControlEventArgs e)
        {
            if (e.Action == TabControlAction.Selected)
            {
                if (TabPageChanged != null)
                {
                    TypeBinderTabPage tabPage = (TypeBinderTabPage)e.TabPage;

                    TabControlControllerEventArgs args = new TabControlControllerEventArgs(tabPage.BoundType, tabPage.View);
                    TabPageChanged(args);
                }
            }
        }

        #endregion

        #region 私有方法
        #endregion

        #region 公开方法

        public void AddTabPage<T>(string text, Control view)
        {
            Debug.Assert(view != null, "view为null");

            Type tType = typeof(T);
            TypeBinderTabPage tabPage = new TypeBinderTabPage(text, tType, view);
            _tabPages.Add(tType, tabPage);

            _tabControl.TabPages.Add(tabPage);
        }

        public void Select<T>()
        {
            Type tType = typeof(T);

            Debug.Assert(_tabPages.Keys.Contains(tType), "没有与指定类型绑定的tab页");

            if (_tabPages.Keys.Contains(tType))
            {
                _tabControl.SelectedTab = _tabPages[tType];
            }
        }

        #endregion

        #region 事件

        public delegate void OnTabPageChangedEventHandler(TabControlControllerEventArgs e);
        /// <summary>
        /// 切换当前选择的tab页
        /// </summary>
        public event OnTabPageChangedEventHandler TabPageChanged;

        #endregion
    }

    public class TabControlControllerEventArgs : EventArgs
    {
        private Type _boundType;
        /// <summary>
        /// 所绑定的类型
        /// </summary>
        public Type BoundType
        {
            get { return _boundType; }
        }

        private Control _view;
        /// <summary>
        /// 呈现在tab页中的视图
        /// </summary>
        public Control View
        {
            get { return _view; }
        }

        public TabControlControllerEventArgs(Type boundType, Control view)
        {
            _boundType = boundType;
            _view = view;
        }
    }
}
