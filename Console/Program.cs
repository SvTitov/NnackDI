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

            var getType = Container.ResolveType<IFooTwo>();

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

    #endregion
}
