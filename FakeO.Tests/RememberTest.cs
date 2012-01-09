using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FakeO.Tests
{
	[TestClass]
	public class RememberTest
	{
		[TestMethod]
		public void Forget_CanClearActionsForSingleClass()
		{
			var faker = new FakeCreator();
			faker.Fake<InternalTestClass>(x => x.Prop1 = "hi");
			faker.Fake<PublicTestClass>(x => x.Field1 = "bye");
			faker.Forget<InternalTestClass>();
			Assert.AreEqual(0, faker.GetRememberedActions<InternalTestClass>().Length);
			Assert.AreEqual(1, faker.GetRememberedActions<PublicTestClass>().Length);
		}

		[TestMethod]
		public void ForgetAll_ClearsAllActionsForAllClasses()
		{
			var faker = new FakeCreator();
			faker.Remember<InternalTestClass>(x => x.Prop1 = "hi");
			faker.Remember<PublicTestClass>(x => x.Field1 = "bye");
			faker.ForgetAll();
			Assert.AreEqual(0, faker.GetRememberedActions<InternalTestClass>().Length);
			Assert.AreEqual(0, faker.GetRememberedActions<PublicTestClass>().Length);
		}

		[TestMethod]
		public void Remeber_AddsMappingsToRemembered()
		{
			var faker = new FakeCreator();
			faker.Remember<InternalTestClass>(x => x.Prop1 = "hi", x => x.Field1 = "bye");
			Assert.AreEqual(2, faker.GetRememberedActions<InternalTestClass>().Length);
		}

		[TestMethod]
		public void Create_UsesRememberedActions()
		{
			var faker = new FakeCreator();
			faker.Remember<InternalTestClass>(x => x.Prop1 = "hi", x => x.Field1 = "bye");
			var obj = Create.Fake<InternalTestClass>();
			Assert.AreEqual("hi", obj.Prop1);
			Assert.AreEqual("bye", obj.Field1);
		}

		[TestMethod]
		public void Create_SpecifiedActionsTakePrecedenceOverRememberedActions_WhenBothRememberedAndSpecified()
		{
			var faker = new FakeCreator();
			faker.Remember<InternalTestClass>(x => x.Prop1 = "hi");
			var obj = Create.Fake<InternalTestClass>(x => x.Prop1 = "bye");
			Assert.AreEqual("bye", obj.Prop1);
		}
	}
}
