using System;
using translateLib;

namespace translateSolution
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //Console.WriteLine("test: " + TranslationHelper.GetTranslation("en", "这是一个测试"));
            wordHelper.WordTranslate("cn","pt");
            // wordHelper.WordTranslateForTable("cn","pt");
            Console.WriteLine("翻译完成");
        }
    }
}
