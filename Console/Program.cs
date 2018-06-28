using NnackDI;
using System;

namespace Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Container.RegistrationType<IFoo, Foo>();
            Container.RegistrationType<IFooTwo, FooTwo>();
            Container.RegistrationType<IFooThree, FooThree>();
            Container.RegistrationWithParameters<IFooWithParam, FooWithParam>(("updateValue", 14),
                                                                              ("value", 5),
                                                                              ("str"  , "HELLO WORLD!"));

            //var getType = Container.ResolveType<IFooThree>();
            var getType = Container.ResolveType<IFooWithParam>();

            System.Console.ReadKey(true);
        }
    }

    #region Test classes
    public interface IFoo
    {
        void PrintAny();
    }
    public class Foo : IFoo
    {
        public void PrintAny()
        {
            System.Console.WriteLine("Hello from Foo class");
        }
    }
    public interface IFooTwo
    {
    }
    public class FooTwo : IFooTwo
    {
        public FooTwo(IFoo foo)
        {
            foo.PrintAny();
        }
    }
    public class FooThree : IFooThree
    {
        private readonly IFoo foo;
        private readonly IFooTwo fooTwo;

        public FooThree(IFoo foo, IFooTwo fooTwo)
        {
            this.foo = foo;
            this.fooTwo = fooTwo;
        }
    }
    public interface IFooThree
    {
    }
    public class FooWithParam : IFooWithParam
    {
        public int IntValue { get; set; }
        public string StringValue { get; set; }
        public int AnotherIntValue {get;set;}

        public FooWithParam(String str, int value, int updateValue)
        {
            IntValue = value;
            StringValue = str;
            AnotherIntValue = updateValue;
        }
    }
    public interface IFooWithParam
    {
    }

    #endregion
}
