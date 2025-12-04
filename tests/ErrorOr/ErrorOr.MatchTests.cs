using System.Collections.Immutable;

namespace Tests;

public class MatchTests
{
    private record Person(string Name);

    [Fact]
    public void CallingMatch_WhenIsSuccess_ShouldExecuteThenAction()
    {
        // Arrange
        ErrorOr<Person> errorOrPerson = new Person("Dmitry");
        string ThenAction(Person person)
        {
            person.ShouldBe(errorOrPerson.Value);
            return "Nice";
        }

        string ElsesAction(ImmutableArray<Error> _) => throw new Exception("Should not be called");

        // Act
        string result = errorOrPerson.Match(
            ThenAction,
            ElsesAction);

        // Assert
        result.ShouldBe("Nice");
    }

    [Fact]
    public void CallingMatch_WhenIsError_ShouldExecuteElseAction()
    {
        // Arrange
        ErrorOr<Person> errorOrPerson = new List<Error> { Error.Validation(), Error.Conflict() };
        string ThenAction(Person _) => throw new Exception("Should not be called");

        string ElsesAction(ImmutableArray<Error> errors)
        {
            errors.ShouldBe(errorOrPerson.Errors);
            return "Nice";
        }

        // Act
        string result = errorOrPerson.Match(
            ThenAction,
            ElsesAction);

        // Assert
        result.ShouldBe("Nice");
    }

    [Fact]
    public void CallingMatchFirst_WhenIsSuccess_ShouldExecuteThenAction()
    {
        // Arrange
        ErrorOr<Person> errorOrPerson = new Person("Dmitry");
        string ThenAction(Person person)
        {
            person.ShouldBe(errorOrPerson.Value);
            return "Nice";
        }

        string OnFirstErrorAction(Error _) => throw new Exception("Should not be called");

        // Act
        string result = errorOrPerson.MatchFirst(
            ThenAction,
            OnFirstErrorAction);

        // Assert
        result.ShouldBe("Nice");
    }

    [Fact]
    public void CallingMatchFirst_WhenIsError_ShouldExecuteOnFirstErrorAction()
    {
        // Arrange
        ErrorOr<Person> errorOrPerson = new List<Error> { Error.Validation(), Error.Conflict() };
        string ThenAction(Person _) => throw new Exception("Should not be called");
        string OnFirstErrorAction(Error errors)
        {
            errorOrPerson.IsError.ShouldBeTrue();
            errors.ShouldBe(errorOrPerson.Errors[0]);
            errors.ShouldBe(errorOrPerson.FirstError.Value);

            return "Nice";
        }

        // Act
        string result = errorOrPerson.MatchFirst(
            ThenAction,
            OnFirstErrorAction);

        // Assert
        result.ShouldBe("Nice");
    }

    [Fact]
    public async Task CallingMatchFirstAfterThenAsync_WhenIsSuccess_ShouldExecuteThenAction()
    {
        // Arrange
        ErrorOr<Person> errorOrPerson = new Person("Dmitry");
        string ThenAction(Person person)
        {
            person.ShouldBe(errorOrPerson.Value);
            return "Nice";
        }

        string OnFirstErrorAction(Error _) => throw new Exception("Should not be called");

        // Act
        string result = await errorOrPerson
            .ThenAsync(person => Task.FromResult(person))
            .MatchFirst(ThenAction, OnFirstErrorAction);

        // Assert
        result.ShouldBe("Nice");
    }

    [Fact]
    public async Task CallingMatchAfterThenAsync_WhenIsSuccess_ShouldExecuteThenAction()
    {
        // Arrange
        ErrorOr<Person> errorOrPerson = new Person("Dmitry");
        string ThenAction(Person person)
        {
            person.ShouldBe(errorOrPerson.Value);
            return "Nice";
        }

        string ElsesAction(ImmutableArray<Error> _) => throw new Exception("Should not be called");

        // Act
        string result = await errorOrPerson
            .ThenAsync(person => Task.FromResult(person))
            .Match(ThenAction, ElsesAction);

        // Assert
        result.ShouldBe("Nice");
    }
}
