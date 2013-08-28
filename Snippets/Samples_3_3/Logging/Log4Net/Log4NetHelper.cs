using System.Collections.Generic;
using System.Linq;
using log4net.Appender;
using log4net.Repository.Hierarchy;

public class Log4NetHelper
{

    public  static IEnumerable<string> GetMessagesFromMemoryAppender()
    {
        var repository = (Hierarchy)log4net.LogManager.GetRepository();
        var memoryAppender = (MemoryAppender)repository.Root.Appenders[0];
        return memoryAppender.GetEvents().Select(x=>x.RenderedMessage);
    }
}