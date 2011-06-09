using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FakeO.Tests
{
  [TestClass]
  public class ObjectCreationTest
  {
    //[TestMethod]
    //public void CanCreateSystemTypes()
    //{
    //  var w = FakeO.New<DateTime>();
    //  Assert.AreEqual(default(DateTime), w);

    //  var x = FakeO.New<int>();
    //  Assert.AreEqual(default(int), x);

    //  var y = FakeO.New<string>();
    //  Assert.AreEqual(default(string), y);

    //  var z = FakeO.New<Int64>();
    //  Assert.AreEqual(default(Int64), z);
    //}

    [TestMethod]
    public void CanCreateInternalTypes()
    {
      var x = Create.New<InternalTestClass>();
      Assert.IsNotNull(x);
      Assert.IsNull(x.Prop1);
    }

    [TestMethod]
    public void CanCreateSpecificNumberOfObjects()
    {
      var x = Create.New<InternalTestClass>(5);
      Assert.AreEqual(5, x.Count());
    }

    [TestMethod]
    public void CanCreateZeroNumberOfObjects()
    {
      var x = Create.New<InternalTestClass>(0);
      Assert.AreEqual(0, x.Count());
    }

    [TestMethod]
    public void CreatesRightNumberOfObjectsWhenMinAndMaxAreEqual()
    {
      var x = Create.New<InternalTestClass>(2, 2);
      Assert.AreEqual(2, x.Count());
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void CanNotCreateNegativeNumberOfObjects()
    {
      Create.New<InternalTestClass>(-5).ToList();
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void CanNotCreateRandomNegativeNumberOfObjects()
    {
      Create.New<InternalTestClass>(10, 2).ToList();
    }

    [TestMethod]
    public void NewDoesNotInstantiateNested()
    {
      var obj = Create.New<NestedTestClass>();
      Assert.IsNull(obj.Nested);
      Assert.AreEqual(0, obj.Property1);
    }

    [TestMethod]
    public void FakeDoesNotInstantiateNested()
    {
      var obj = Create.Fake<NestedTestClass>();
      Assert.IsNull(obj.Nested);
      Assert.AreNotEqual(0, obj.Property1);
    }

    [TestMethod]
    public void NewCanNestedObjects()
    {
      var obj = Create.New<NestedTestClass>(
        x => x.Nested = Create.New<PublicTestClass>()
        );
      Assert.IsNotNull(obj);
      Assert.AreEqual(0, obj.Property1);
      Assert.IsNotNull(obj.Nested);
      Assert.AreEqual(null, obj.Nested.Prop1);
      Assert.AreEqual(null, obj.Nested.Field1);
    }
  }
}
