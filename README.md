# Sheng.Winform.Controls.Controller

在我们的客户端应用程序开发中，可能会涉及大量的控件操作的代码，如 TreeView，DataGridView，ListBox 等等，这些控件虽然都提供了基本的数据操作接口，但是这些接口的功能都非常的基础和简单，考虑如下操作：

+ 在控件中使指定的数据处于选中状态；
+ 在控件中删除符合条件的数据；
+ 向控件中的指定位置添加数据，并判断数据的类型是否符合预期；
+ 在控件中查找符合条件的数据；
+ 获取控件中选中的数据，直接返回强类型结果；
+ 移动指定的数据到另一个数据项目之前/之后；
+ 展开树控件中符合条件的树节点；

这些操作有一个重要的共同点，都是针对“数据”进行操作，但是基本的的控件接口，没有这么多功能，既有的接口也多是以 object 作为参数来操作的，如果要实现这些功能，很多时候程序员需要写一些“业务代码”来完成，在业务代码中迭代数据源，写条件判断，做类型转换，最后调用控件的基本操作接口。

Sheng.Winform.Controls.Controller 使用控制器，来为这些控制提供这些共通的操作，包括：

+ DataGridViewController
+ TabControlController
+ TreeViewController
+ ListBoxController

详细说明可访问：https://shengxunwei.com

