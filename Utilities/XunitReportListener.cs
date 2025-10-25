using Xunit.Abstractions;
using Xunit.Sdk;

namespace QA_Project.Utilities
{
    /// <summary>
    /// Intercepts xUnit test messages and logs results to TestReport.txt
    /// </summary>
    public class XunitReportListener(IMessageSink innerSink) : LongLivedMarshalByRefObject, IMessageSink, IDisposable
    {
        private readonly IMessageSink _innerSink = innerSink;

        public bool OnMessage(IMessageSinkMessage message)
        {
            try
            {
                switch (message)
                {
                    case ITestPassed passed:
                        ReportManager.LogResult(passed.Test.DisplayName, true);
                        break;

                    case ITestFailed failed:
                        var errorMsg = failed.Messages?.Length > 0
                            ? string.Join("; ", failed.Messages)
                            : "Unknown error";

                        if (failed.StackTraces?.Length > 0)
                        {
                            errorMsg += $"\n{failed.StackTraces[0]}";
                        }

                        ReportManager.LogResult(failed.Test.DisplayName, false, errorMsg);
                        break;

                    case ITestSkipped skipped:
                        ReportManager.LogResult(skipped.Test.DisplayName + " [SKIPPED]", false, skipped.Reason);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ ReportListener Error: {ex.Message}");
            }

            return _innerSink.OnMessage(message);
        }

        public void Dispose()
        {
            (_innerSink as IDisposable)?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}