using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FakeO.Tests
{
  [TestClass]
  public class FakeDataTest
  {
    [TestMethod]
    public void CanSetInternalMembersManually()
    {
      var obj = Create.Fake<InternalTestClass>(x => x.Prop1 = "hi", x => x.Field1 = "bye");
      Assert.AreEqual("hi", obj.Prop1);
      Assert.AreEqual("bye", obj.Field1);
    }

    [TestMethod]
    public void CanSetInternalProperty()
    {
      var obj = Create.Fake<InternalTestClass>(x => x.Field1 = "x");
      Assert.IsNotNull(obj.Prop1);
      Assert.AreEqual("x", obj.Field1);
    }

    [TestMethod]
    public void CanSetInternalField()
    {
      var obj = Create.Fake<InternalTestClass>(x => x.Prop1 = "x");
      Assert.IsNotNull(obj.Field1);
      Assert.AreEqual("x", obj.Prop1);
    }

    [TestMethod]
    public void PicksRandomEnums()
    {
      var obj = Create.Fake<EnumTestClass>();
      Assert.IsTrue(Enum.IsDefined(typeof(TestEnum), obj.Enum1));
      Assert.IsTrue(Enum.IsDefined(typeof(TestEnum), obj.Enum2));
    }

    [TestMethod]
    public void IgnoresPrivateOrMissingPropertySettersTest()
    {
      var obj = Create.Fake<NoSettersTestClass>();
      Assert.IsNull(obj.Property1);
      Assert.IsNull(obj.Property2);
    }

    [TestMethod]
    public void CanSetRandomDateTimeAndTimeStamps()
    {
      var obj = Create.Fake<DateTimeSpanTestClass>(x => x.ManuallySetTime = new TimeSpan(1, 1, 1));
      Assert.IsTrue(new TimeSpan(1, 1, 1) == obj.ManuallySetTime);
      Assert.IsFalse(default(TimeSpan) == obj.Time);
      Assert.IsFalse(default(DateTime) == obj.Date);
    }

    [TestMethod]
    public void FakeCanNestedObjects()
    {
      var obj = Create.Fake<NestedTestClass>(
        x => x.Nested = Create.Fake<PublicTestClass>()
        );
      Assert.IsNotNull(obj);
      Assert.AreNotEqual(0, obj.Property1);
      Assert.IsNotNull(obj.Nested);
      Assert.AreNotEqual(null, obj.Nested.Prop1);
      Assert.AreNotEqual(null, obj.Nested.Field1);
    }

      [TestMethod]
      public void Fake_GeneratesStringsOfCorrectLength_WhenStringLengthAttributeProvided()
      {
          var obj = Create.Fake<StringLengthTestClass>();
          Assert.AreEqual(5, obj.Property1.Length);
          Assert.AreEqual(1, obj.Property2.Length);
      }

      [TestMethod]
      public void Fake_GeneratesNumbersInRange_WhenRangeAttributeProvided()
      {
          var obj = Create.Fake<RangeTestClass>();
          Assert.AreEqual(5, obj.Property1);
          Assert.AreEqual(1, obj.Property2);          
      }

      [TestMethod]
      public void Fake_CanHandleDecimalsAndDoubles()
      {
          var obj = Create.Fake<DoubleAndDecimalTestClass>();
          Assert.AreNotEqual(0, obj.Decimal);
          Assert.AreNotEqual(0, obj.Double);
      }

      [TestMethod]
      public void Fake_CanHandleGuids()
      {
          var guidObject = Create.Fake<TestClassWithAGuid>();

          Assert.AreNotEqual(Guid.Empty, guidObject.UniqueId);
      }
  }
}
