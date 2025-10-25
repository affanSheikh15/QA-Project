using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;

[assembly: Xunit.TestFramework("QA_Project.CustomXunitTestFramework", "QA-Project")]

namespace QA_Project
{
    public class CustomXunitTestFramework(IMessageSink messageSink) : XunitTestFramework(messageSink)
    {
        protected override ITestFrameworkExecutor CreateExecutor(AssemblyName assemblyName)
            => new CustomTestFrameworkExecutor(assemblyName, SourceInformationProvider, DiagnosticMessageSink);
    }

    public class CustomTestFrameworkExecutor(
        AssemblyName assemblyName,
        ISourceInformationProvider sourceInformationProvider,
        IMessageSink diagnosticMessageSink) : XunitTestFrameworkExecutor(assemblyName, sourceInformationProvider, diagnosticMessageSink)
    {
        protected override async void RunTestCases(
            IEnumerable<IXunitTestCase> testCases,
            IMessageSink executionMessageSink,
            ITestFrameworkExecutionOptions executionOptions)
        {
            var loggingSink = new Utilities.XunitReportListener(executionMessageSink);
            base.RunTestCases(testCases, loggingSink, executionOptions);
        }
    }
}