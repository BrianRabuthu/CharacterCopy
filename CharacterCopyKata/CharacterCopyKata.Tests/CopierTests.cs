using NSubstitute;
using Xunit;

namespace CharacterCopy.Tests
{
    public class CopierTests
    {
        [Fact]
        public void Ctor_GivenNullSource_ShouldThrowArgumentNullException()
        {
            // Arrange
            var destination = Substitute.For<IDestination>();

            // Act & Assert
            var result = Assert.Throws<ArgumentNullException>(() => new Copier(null, destination));
            Assert.Equal("source", result.ParamName);
        }

        [Fact]
        public void Ctor_GivenNullDestination_ShouldThrowArgumentNullException()
        {
            // Arrange
            var source = Substitute.For<ISource>();

            // Act & Assert
            var result = Assert.Throws<ArgumentNullException>(() => new Copier(source, null));
            Assert.Equal("destination", result.ParamName);
        }

        [Fact]
        public void Copy_GivenSingleChar_ShouldCopyChar()
        {
            // Arrange
            var expectedChar = 'a';
            var source = CreateSource('a', '\n');
            var destination = CreateDestination();
            var sut = new Copier(source, destination);

            // Act
            sut.Copy();

            // Assert
            source.Received(2).ReadChar();
            destination.Received(1).WriteChar(expectedChar);
        }

        [Fact]
        public void Copy_GivenManyChar_ShouldCopyAllCharUntilNewline()
        {
            // Arrange
            var source = CreateSource('a', 'b', 'c', 'd', '\n');
            var destination = CreateDestination();
            var sut = new Copier(source, destination);

            // Act
            sut.Copy();

            // Assert
            source.Received(5).ReadChar();
            destination.Received(4).WriteChar(Arg.Any<char>());
        }

        [Fact]
        public void CopyBatch_GivenBatchSize_ShouldReadAndWriteInBatches()
        {
            // Arrange
            var source = Substitute.For<ISource>();
            var destination = Substitute.For<IDestination>();

            // Simulate batch processing (source returns batches of data)
            source.ReadChars(5).Returns("Hello", "World", "\nExtra");

            var sut = new Copier(source, destination);

            // Act
            sut.CopyBatch(5);

            // Assert
            source.Received(3).ReadChars(5); // Read in batches
            destination.Received(1).WriteChars("Hello"); // First batch
            destination.Received(1).WriteChars("World"); // Second batch
            destination.DidNotReceive().WriteChars("Extra"); // Stops at newline
        }

        [Fact]
        public void CopyBatch_GivenVeryLargeInput_ShouldProcessAllBatchesUntilNewline()
        {
            // Arrange
            var source = Substitute.For<ISource>();
            var destination = Substitute.For<IDestination>();

            // Simulate very large input data in multiple batches
            source.ReadChars(1000).Returns(
                new string('A', 1000), // First batch
                new string('B', 1000), // Second batch
                new string('C', 500) + "\nExtra" // Final batch with newline
            );

            var sut = new Copier(source, destination);

            // Act
            sut.CopyBatch(1000);

            // Assert
            source.Received(3).ReadChars(1000); // Read 3 batches
            destination.Received(1).WriteChars(new string('A', 1000)); // First batch
            destination.Received(1).WriteChars(new string('B', 1000)); // Second batch
            destination.Received(1).WriteChars(new string('C', 500)); // Partial batch before newline
            destination.DidNotReceive().WriteChars("Extra"); // Stops at newline
        }

        // Factory methods for mocking source and destination
        private ISource CreateSource(params char[] dataToRead)
        {
            var source = Substitute.For<ISource>();
            source.ReadChar().Returns(dataToRead[0], dataToRead.Skip(1).ToArray());
            return source;
        }

        private IDestination CreateDestination()
        {
            return Substitute.For<IDestination>();
        }
    }
}