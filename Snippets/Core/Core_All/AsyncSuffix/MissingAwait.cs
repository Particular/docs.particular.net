// change file type to Compile to test
using System.IO;
using System.Threading.Tasks;

class MissingAwait
{
    /**
    #region TaskMethodMissingAwait

    public static Task TaskMethodMissingAwait()
    {
        var writer = new StreamWriter("stub");
        writer.WriteLineAsync();
    }

    #endregion

    **/
    #region AsyncMethodMissingAwait

    public static async Task AsyncMethodMissingAwait()
    {
        var writer = new StreamWriter("stub");
        writer.WriteLineAsync();
    }

    #endregion

    #region AsyncMethodMissingOneAwait

    public static async Task AsyncMethodMissingOneAwait()
    {
        var writer = new StreamWriter("stub");
        writer.WriteLineAsync();
        await writer.WriteLineAsync();
    }

    #endregion

}