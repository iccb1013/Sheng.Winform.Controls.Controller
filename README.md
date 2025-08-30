# Sheng.Winform.Controls.Controller

🙋‍♂️ https://me.shendesk.com
📨 cao.silhouette@gmail.com

Please visit the original code repository for the latest updates: https://github.com/iccb1013/Sheng.Winform.Controls.Controller

It is open-source under the MIT License, and you are free to use it. However, you must retain the copyright notice and the link to my website in the source code and product screens. Thank you.

In our client application development, we may have a lot of code related to control operations, such as TreeView, DataGridView, ListBox, etc. While these controls provide basic data operation interfaces, the functionality of these interfaces is quite basic and simple. Consider the following operations:

- Make specified data in the control selected;
- Delete data that meets certain conditions in the control;
- Add data to a specified position in the control, and check if the data type is as expected;
- Search for data that meets certain conditions in the control;
- Get the selected data in the control and directly return a strongly typed result;
- Move specified data before/after another data item;
- Expand tree nodes that meet certain conditions in the tree control;

These operations share a common characteristic—they all operate on "data." However, the basic control interfaces don't offer such features. Most of the existing interfaces use 'object' as parameters for operations. To implement these features, programmers often need to write "business logic" to iterate through the data source, add condition checks, perform type conversions, and then call the basic control operation interfaces.

Sheng.Winform.Controls.Controller uses controllers to provide these common operations for these controls, including:

- DataGridViewController
- TabControlController
- TreeViewController
- ListBoxController
