using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] product = { "a", "b", "c" };
            Console.WriteLine(product);
            //将对象转化成JSON字符串
            string output = JsonConvert.SerializeObject(product);
            Console.WriteLine("JSON内容");
            Console.WriteLine(output);
            //将JSON字符串转化成对象
            object product2 = JsonConvert.DeserializeObject(output);
            Console.WriteLine("JSON转对象内容");
            Console.WriteLine(product2);
            Console.ReadLine();
        }
    }


}
