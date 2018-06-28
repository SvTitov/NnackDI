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

            var getType = Container.ResolveType<IFooThree>();

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

    public interface IFooTwo { }

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

    #endregion
}
