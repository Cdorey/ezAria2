# ezAria2
一个ARIA2的windows GUI项目，完全基于.net，而非web ui加壳。

## 项目截图

![image](http://github.com/Cdorey/ezAria2/blob/master/Pictures/20180521165256.png)

![image](http://github.com/Cdorey/ezAria2/blob/master/Pictures/20180521165319.png)

![image](http://github.com/Cdorey/ezAria2/blob/master/Pictures/20180521165413.png)

![image](http://github.com/Cdorey/ezAria2/blob/master/Pictures/20180521165433.png)

## 开发进度
* 180422 更新主页面的控件，修复一些脑洞大开的错误，增加系统托盘菜单
* 180510 修复shutdown命令中的错误
* 180513 增加多行URI粘贴，一键添加多个下载任务,添加BT和metalink支持
* 180514 修正多个错误，增加机制降低轮询频率，增加关闭窗口后台运行
* 180516 注释掉了主页面那个并没有什么卵用的按钮，加入避免多实例运行的机制
* 180519 完成设置页面的设计和变量绑定

## 预期功能

1. 使用GUI实现aria2的所有命令
2. 完成对aria2 Notice的解析
3. 自动转换来自其他下载工具的专用链接，如果支持则自动下载
## 许可和引用

* 依照GPL V2许可发布
* [Arthas的WPF开源控件类库](https://github.com/1217950746/Arthas-WPFUI)MIT许可
* [Websocket-Sharp](https://github.com/sta/websocket-sharp)MIT许可
