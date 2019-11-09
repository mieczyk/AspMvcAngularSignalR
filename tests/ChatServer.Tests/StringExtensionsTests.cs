using Xunit;

namespace ChatServer.Tests
{
    public class StringExtensionsTests
    {
        public class TryExtractRecipient
        {
            [Theory]
            [InlineData("@recipient:message")]
            [InlineData("@recipient: message")]
            [InlineData(" @recipient: message")]
            public void returns_recipient_name_if_string_starts_with_valid_recipient_mark(string input)
            {
                Assert.Equal("recipient", input.TryExtractRecipient());
            }

            [Theory]
            [InlineData("message")]
            [InlineData("@recipient message")]
            [InlineData("recipient: message")]
            public void returns_null_if_no_valid_recipient_mark_given(string input)
            {
                Assert.Null(input.TryExtractRecipient());
            }

            [Theory]
            [InlineData("Some @recipient: message")]
            [InlineData("Some message @recipient:")]
            public void returns_null_if_recipient_mark_is_not_placed_at_beginning(string input)
            {
                Assert.Null(input.TryExtractRecipient());
            }
        }
    }
}
