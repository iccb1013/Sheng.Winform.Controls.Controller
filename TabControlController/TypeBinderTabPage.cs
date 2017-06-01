using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace Sheng.Winform.Controls.Controller
{
    class TypeBinderTabPage : TabPage
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

        public TypeBinderTabPage(string text, Type boundType, Control view)
        {
            Text = text;
            _boundType = boundType;
            _view = view;

            Debug.Assert(view != null, "view为null");

            if (view != null)
            {
                view.Dock = DockStyle.Fill;
                this.Controls.Add(view);
            }
        }
    }
}
