using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakeO;
using System.Text.RegularExpressions;

namespace FakeO.Tests
{
  [TestClass]
  public class RandomStringTest
  {
    private void AssertMatch(string str)
    {
      for(int i = 0; i < 10; i++)
      {
        var result = String.Random(str);
        Assert.IsTrue(Regex.IsMatch(result, string.Format("^{0}$", str)), string.Format("{0} does not match regex {1}.", result, str));
      }
    }

    [TestMethod]
    public void TestDigit()
    {
      AssertMatch(@"\d");
      AssertMatch(@"\d\d");
      AssertMatch(@"\d\d\d");
    }


    [TestMethod]
    public void TestWord()
    {
      AssertMatch(@"\w");
      AssertMatch(@"\w\w");
      AssertMatch(@"\w\w\w");
    }

    [TestMethod]
    public void TestAny()
    {
      AssertMatch(@".");
      AssertMatch(@"..");
      AssertMatch(@"...");
    }

    [TestMethod]
    public void TestRange()
    {
      AssertMatch(@"[A-Z]");
      AssertMatch(@"[A-Z][a-z]");
      AssertMatch(@"[A-Z][a-z][0-9]");
    }

    [TestMethod]
    public void TestQuantity()
    {
      AssertMatch(@"x{0}");
      AssertMatch(@"x{1}");
      AssertMatch(@"x{11}");
    }

    [TestMethod]
    public void TestRandomQuantity()
    {
      AssertMatch(@"x{0,0}");
      AssertMatch(@"x{0,10}");
      AssertMatch(@"x{11,22}");
    }
  }
}
