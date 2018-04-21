# ezAria2
一个ARIA2的windows GUI项目，完全基于.net，而非web ui加壳。

## 开发进度

* 180311 现在允许添加一个HTTP/HTTPS的下载地址，能够正确将下载任务添加到ARIA2，并且实时更新下载进度和速度等信息。
* 180312 更新了一些显示元素
* 180314 实现了主页面的第二个按钮功能，选择一个任务后点击可以暂停/重新开始该任务
* 180314 修正了两个会导致程序崩溃的逻辑错误
* 180321 由于工作失误，丢失了最近一周的工作进度。
* 180322 修复多个错误.
* 180421 修复多个错误，实现历史任务列表功能
## 预期功能

1. 使用GUI实现aria2的所有命令
2. 完成对aria2 Notice的解析
3. 通过其他开源库，完成对ed2k协议的支持（有生之年系列，求推荐些简单易懂的emule开源库）

## 许可

* 本项目的全部原创和新增源码遵循GPL V2许可
* 本项目引用了Arthas的WPF开源控件类库，原作者使用MIT许可
* 本项目引用了Websocket-Sharp项目，原作者使用MIT许可
