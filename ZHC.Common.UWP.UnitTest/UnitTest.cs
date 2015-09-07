using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using ZHC.Common.UWP.Storage;
using System.Threading.Tasks;

namespace ZHC.Common.UWP.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestGetCache()
        {
            //var ret = SettingHelper.GetCache<Person>(DateTime.Now.Date.ToString(), async () =>
            //{
            //    return await Task.Run(() =>
            //    {
            //        new Person
            //        {
            //            Age = 27,
            //            Name = "不正常"
            //        };
            //    });
            //});

            //Assert.AreEqual(ret.Age, 28);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var ret = SettingHelper.GetCache<Person>(DateTime.Now.Date.ToString(), () =>
            {
                return new Person { Age = 27, Name = "不正常" };
            });

            Assert.AreEqual(ret.Age, 28);
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
