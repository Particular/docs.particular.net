﻿// change file type to Compile to test
using System.IO;
using System.Threading.Tasks;
#pragma warning disable 1998
#pragma warning disable 4014

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

    // ReSharper disable ConsiderUsingConfigureAwait
    #region AsyncMethodMissingOneAwait

    public static async Task AsyncMethodMissingOneAwait()
    {
        var writer = new StreamWriter("stub");
        writer.WriteLineAsync();
        await writer.WriteLineAsync();
    }

    #endregion
    // ReSharper restore ConsiderUsingConfigureAwait

}