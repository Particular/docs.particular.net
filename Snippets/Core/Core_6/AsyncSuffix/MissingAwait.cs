// change file type to Compile to test
namespace Core6
{
    using System.Threading.Tasks;

    class MissingAwait
    {
        #region VoidMethodMissingAwait
        public static void VoidMethodMissingAwait()
        {
            ServiceWithAsync.Method();
        }
        #endregion

        #region TaskMethodMissingAwait
        public static Task TaskMethodMissingAwait()
        {
            ServiceWithAsync.Method();
        }
        #endregion
        #region AsyncMethodMissingAwait
        public static async Task AsyncMethodMissingAwait()
        {
            ServiceWithAsync.Method();
        }
        #endregion
        #region AsyncMethodMissingOneAwait
        public static async Task AsyncMethodMissingOneAwait()
        {
            ServiceWithAsync.Method();
            await ServiceWithAsync.Method();
        }
        #endregion

    }
}