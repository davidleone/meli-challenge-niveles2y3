using ChallengeMeLiServices.DataAccess;
using Moq;
using NHibernate;

namespace ChallengeMeLiServices.Services.Tests
{
    public abstract class ServiceTests
    {
        protected Mock<ISessionFactory> _sessionFactoryMock;
        protected Mock<ISession> _sessionMock;
        protected Mock<ITransaction> _transactionMock;

        protected void MockSessionManager()
        {
            _sessionFactoryMock = new Mock<ISessionFactory>();
            _sessionMock = new Mock<ISession>();
            _transactionMock = new Mock<ITransaction>();

            _sessionFactoryMock.Setup(x => x.OpenSession()).Returns(_sessionMock.Object).Verifiable();
            _sessionMock.Setup(x => x.BeginTransaction()).Returns(_transactionMock.Object).Verifiable();
            _transactionMock.Setup(x => x.Commit()).Verifiable();

            SessionManager.SetMockedSessionForTests(_sessionFactoryMock.Object);
        }
    }
}
