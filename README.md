# translateWord
翻译word文档，先将其翻译成英文，再翻译成目标语言，提高准确性
## 目的
实现目标语言 > 英语 > 目标语言的word文档的翻译功能

## 运行环境
1.安装 .NET Core 2.2 Runtime 
下载地址：https://dotnet.microsoft.com/download
2.安装 .NET Framework 4.8 Runtime
下载地址：https://dotnet.microsoft.com/download

## 使用步骤
1.将需要翻译的中文word文档（也可以是其他语言，需自定义目录）放入\translateDoc\cn 目录下
2.win+r 打开 cmd 切换至\translateSolution目录后运行
~~~
dotnet run
~~~

## 事项
1. 将一些参数加入至配置文件
2. 继续测试


## 注意问题与目前缺陷
1. google 翻译的api如果频繁访问会导致接口被禁用，提示访问次数过多
2. 建议不要翻译过大文本的word文档

## 疑问
是否调用google收费版的api就不会因访问次数过多而受限