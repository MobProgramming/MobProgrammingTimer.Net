using System;
using System.Collections.Generic;
using Rhino.Mocks;

namespace DeveloperTimer.Tests
{
    public class CaptureExpression<T>
        where T : class
    {
        // Got Pattern from: http://lostechies.com/jimmybogard/2010/06/09/capturing-rhino-mocks-arguments-in-c-4-0/
        private readonly T _stub;

        public CaptureExpression(T stub)
        {
            _stub = stub;
        }

        public IList<U> Args<U>(Action<T, U> methodExpression)
        {
            var argsCaptured = new List<U>();

            Action<U> captureArg = argsCaptured.Add;
            Action<T> stubArg = stub => methodExpression(stub, default(U));

            _stub.Stub(stubArg).IgnoreArguments().Do(captureArg);

            return argsCaptured;
        }

        public IList<Tuple<U1, U2>> Args<U1, U2>(Action<T, U1, U2> methodExpression)
        {
            var argsCaptured = new List<Tuple<U1, U2>>();

            Action<U1, U2> captureArg = (u1, u2) => argsCaptured.Add(Tuple.Create(u1, u2));
            Action<T> stubArg = stub => methodExpression(stub, default(U1), default(U2));

            _stub.Stub(stubArg).IgnoreArguments().Do(captureArg);

            return argsCaptured;
        }
    }
}