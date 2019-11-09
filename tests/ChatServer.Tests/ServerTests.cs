using Xunit;
using System.Linq;
using System.Collections.Generic;
using System;

namespace ChatServer.Tests
{
    public class ServerTests
    {
        public class Register : IDisposable
        {
            private readonly Server _server;

            public Register()
            {
                string prefix = nameof(Register);
                _server = new Server($"{prefix}Clients", $"{prefix}Buffer");
            }

            [Fact]
            public void adds_new_client_to_list_and_returns_true()
            {
                // Arrange
                string msg;
                _server.Register("SOME_CLIENT", out msg);

                // Act
                bool result = _server.Register("SOME_OTHER_CLIENT", out msg);

                // Assert
                Assert.True(result);
            }

            [Theory]
            [InlineData("SOME_CLIENT")]
            [InlineData("some_client")]
            public void returns_false_if_client_is_already_registered(string existingClientId)
            {
                // Arrange
                const string clientId = "SOME_CLIENT";
                string msg;

                _server.Register(existingClientId, out msg);

                // Act
                bool result = _server.Register(clientId, out msg);

                // Assert
                Assert.False(result);
            }

            public void Dispose()
            {
                _server.Dispose();
            }
        }

        public class Send : IDisposable
        {
            private const string _clientId = "SOME_CLIENT";
            private readonly Server _server;

            public Send()
            {
                string prefix = nameof(Send);
                _server = new Server($"{prefix}Clients", $"{prefix}Buffer");
                _server.Register(_clientId, out string msg);
            }

            [Fact]
            public void appends_message_to_buffer()
            {
                // Act
                _server.Send(new Message(_clientId, "A New Hope"));
                _server.Send(new Message(_clientId, "The Empire Strikes Back"));
                _server.Send(new Message(_clientId, "Return of the Jedi"));

                // Assert
                Assert.Equal(3, _server.ReadAllMessagesForClient(_clientId).Count());
            }

            public void Dispose()
            {
                _server.Dispose();
            }
        }

        public class ReadAllMessagesForClient : IDisposable
        {
            private const string _senderId = "SOME_SENDER";
            private const string _recipientId = "SOME_RECIPIENT";
            private readonly Server _server;

            public ReadAllMessagesForClient()
            {
                string prefix = nameof(ReadAllMessagesForClient);
                _server = new Server($"{prefix}Clients", $"{prefix}Buffer");

                string msg;
                _server.Register(_senderId, out msg);
                _server.Register(_recipientId, out msg);
            }

            [Fact]
            public void returns_public_messages()
            {
                // Arrange
                const string msgBody = "Hello world!";

                _server.Send(new Message(_senderId, msgBody));

                // Act
                IEnumerable<Message> messages = _server.ReadAllMessagesForClient(_recipientId);

                // Assert
                Assert.Contains(messages, msg => msg.Body == msgBody);
            }

            [Fact]
            public void returns_messages_meant_for_the_client()
            {
                // Arrange
                const string msgBody = "Hello world!";

                _server.Send(new Message(_senderId, msgBody, _recipientId));

                // Act
                IEnumerable<Message> messages = _server.ReadAllMessagesForClient(_recipientId);

                // Assert
                Assert.Contains(messages, msg => msg.Body == msgBody);
            }

            [Fact]
            public void returns_messages_meant_for_other_clients_but_sent_by_the_client()
            {
                // Arrange
                const string msgBody = "Hello world!";

                _server.Send(new Message(_senderId, msgBody, _recipientId));

                // Act
                IEnumerable<Message> messages = _server.ReadAllMessagesForClient(_senderId);

                // Assert
                Assert.Contains(messages, msg => msg.Body == msgBody);
            }

            [Fact]
            public void does_not_return_messages_meant_for_other_clients()
            {

                // Arrange
                const string msgBody = "Hello world!";
                const string otherRecipientId = "OTHER_RECIPIENT";

                _server.Send(new Message(_senderId, msgBody, otherRecipientId));

                // Act
                IEnumerable<Message> messages = _server.ReadAllMessagesForClient(_recipientId);

                // Assert
                Assert.DoesNotContain(messages, msg => msg.Body == msgBody);
            }

            [Fact]
            public void returns_empty_list_if_buffer_is_empty()
            {
                Assert.Empty(_server.ReadAllMessagesForClient(_recipientId));
            }

            [Fact]
            public void throws_if_client_not_registered()
            {
                Assert.Throws<InvalidOperationException>(() => 
                {
                    _server.ReadAllMessagesForClient("UNREGISTERED_CLIENT");
                });
            }

            public void Dispose()
            {
                _server.Dispose();
            }
        }
    }
}
