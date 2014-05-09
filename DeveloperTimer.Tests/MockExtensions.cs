namespace DeveloperTimer.Tests
{
    public static class MockExtensions
    {
        // Got Pattern from: http://lostechies.com/jimmybogard/2010/06/09/capturing-rhino-mocks-arguments-in-c-4-0/
        public static CaptureExpression<T> Capture<T>(this T stub)
            where T : class
        {
            return new CaptureExpression<T>(stub);
        }
    }
}