namespace AsyncBoilerplate
{
    using System.Threading.Tasks;

#pragma warning disable CS1998

    #region PreCsharp7-1AsyncBoilerplate
    class Program
    {
        static void Main()
        {
            AsyncMain().GetAwaiter().GetResult();
        }


        static async Task AsyncMain()
        {

        }
    }
    #endregion

#pragma warning restore CS1998
}
