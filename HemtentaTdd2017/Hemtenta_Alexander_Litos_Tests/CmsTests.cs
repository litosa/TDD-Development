using Hemtenta_Alexander_Litos.cms;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Hemtenta_Alexander_Litos_Tests
{
    public class CmsTests
    {
        private string username;
        private Blog blog;
        private Mock<IAuthenticator> mockAuth;
        private Page page;

        public CmsTests()
        {
            username = "Alex";
            blog = new Blog();
            mockAuth = new Mock<IAuthenticator>();
            blog.Authenticator = mockAuth.Object;
            page = new Page { Title = "Zlatan", Content = "E se numbero Uno" };
        }

        private void DoLogin()
        {
            mockAuth.Setup(m => m.GetUserFromDatabase(username)).Returns(new User(username));
            blog.LoginUser(new User(username));
        }

        [Fact]
        public void Should_loginUser_InvalidValues_Throws()
        {
            Assert.Throws<NotImplementedException>(() => blog.LoginUser(new User("")));
            Assert.Throws<NotImplementedException>(() => blog.LoginUser(new User(null)));
        }

        [Fact]
        public void Should_Fail_loginUser_WrongUsername()
        {
            mockAuth.Setup(m => m.GetUserFromDatabase(It.IsAny<string>())).Returns(new User(username));

            blog.LoginUser(new User(username + "bullen"));
            Assert.False(blog.UserIsLoggedIn);

            mockAuth.Verify(m => m.GetUserFromDatabase(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void Should_Success_loginUser_CorrectUsername()
        {
            DoLogin();

            Assert.True(blog.UserIsLoggedIn);
            mockAuth.Verify(m => m.GetUserFromDatabase(username), Times.Once);
        }

        private User DoLogout()
        {
            mockAuth.Setup(m => m.GetUserFromDatabase(It.IsAny<string>())).Returns(new User(username));
            User user = new User(username);

            return user;
        }

        [Fact]
        public void Should_logoutUser_InvalidValues_Throws()
        {
            Assert.Throws<NotImplementedException>(() => blog.LogoutUser(null));
        }

        [Fact]
        public void Should_logoutUser_LoggedIn_Success()
        {
            var user = DoLogout();

            blog.LoginUser(user);
            blog.LogoutUser(user);
            Assert.False(blog.UserIsLoggedIn);

            mockAuth.Verify(m => m.GetUserFromDatabase(It.IsAny<string>()), Times.Exactly(2));
        }       

        [Fact]
        public void Should_logoutUser_NotLoggedIn_Success()
        {
            var user = DoLogout();

            blog.LogoutUser(user);
            Assert.False(blog.UserIsLoggedIn);

            mockAuth.Verify(m => m.GetUserFromDatabase(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void Should_Publish_InvalidValues_Throws()
        {
            Assert.Throws<NotImplementedException>(() => blog.PublishPage(new Page { Title = "", Content = "" }));
            Assert.Throws<NotImplementedException>(() => blog.PublishPage(new Page { Title = "", Content = null }));
            Assert.Throws<NotImplementedException>(() => blog.PublishPage(new Page { Title = null, Content = null }));
            Assert.Throws<NotImplementedException>(() => blog.PublishPage(new Page { Title = null, Content = "" }));
        }

        [Fact]
        public void PublishPage_PageIsNull_Throws()
        {
            Assert.Throws<NotImplementedException>(() => blog.PublishPage(null));
        }

        [Fact]
        public void Should_PublishPage_ReturnsTrue()
        {
            DoLogin();
            mockAuth.Verify(x => x.GetUserFromDatabase(username), Times.Exactly(1));

            bool result = blog.PublishPage(page);
            Assert.True(result);
        }

        [Fact]
        public void Should_NotPublishPage_ReturnsFalse()
        {
            bool result = blog.PublishPage(page);
            Assert.False(result);
        }

        [Fact]
        public void Should_SendEmail_InvalidValues_Throws()
        {
            string address = "alex@mail.com", caption = "Zlatan", body = "Es e numbero uno";
          
            Assert.Throws<NotImplementedException>(() => blog.SendEmail(null, caption, body));
            Assert.Throws<NotImplementedException>(() => blog.SendEmail(address, null, body));
            Assert.Throws<NotImplementedException>(() => blog.SendEmail(address, caption, null));
            Assert.Throws<NotImplementedException>(() => blog.SendEmail("", caption, body));
            Assert.Throws<NotImplementedException>(() => blog.SendEmail(address, "", body));
            Assert.Throws<NotImplementedException>(() => blog.SendEmail(address, caption, ""));
            Assert.Throws<NotImplementedException>(() => blog.SendEmail("", "", ""));
            Assert.Throws<NotImplementedException>(() => blog.SendEmail(null, null, null));
        }

        [Fact]
        public void Should_SendEmail_Returns_1()
        {
            string to = "destination", header = "Great opportunity", body = "I am the Nigerian finance minister";
            
            DoLogin();
            mockAuth.Verify(x => x.GetUserFromDatabase(username), Times.Exactly(1));

            int result = blog.SendEmail(to, header, body);

            Assert.Equal(1, result);
        }

        [Fact]
        public void Should_NotSendEmail_Returns_0()
        {
            string to = "destination", header = "Great opportunity", body = "I am the Nigerian finance minister";
            
            int result = blog.SendEmail(to, header, body);

            Assert.Equal(0, result);
        }
    }
}
