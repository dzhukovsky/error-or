using System.Collections.Immutable;

namespace Tests;

public class SwitchTests
{
    private record Person(string Name);

    [Fact]
    public void CallingSwitch_WhenIsSuccess_ShouldExecuteThenAction()
    {
        // Arrange
        ErrorOr<Person> errorOrPerson = new Person("Dmitry");
        void ThenAction(Person person) => person.ShouldBe(errorOrPerson.Value);
        void ElsesAction(ImmutableArray<Error> _) => throw new Exception("Should not be called");

        // Act
        Action action = () => errorOrPerson.Switch(
            ThenAction,
            ElsesAction);

        // Assert
        Should.NotThrow(action);
    }

    [Fact]
    public void CallingSwitch_WhenIsError_ShouldExecuteElseAction()
    {
        // Arrange
        ErrorOr<Person> errorOrPerson = new List<Error> { Error.Validation(), Error.Conflict() };
        void ThenAction(Person _) => throw new Exception("Should not be called");
        void ElsesAction(ImmutableArray<Error> errors) => errors.ShouldBe(errorOrPerson.Errors);

        // Act
        Action action = () => errorOrPerson.Switch(
            ThenAction,
            ElsesAction);

        // Assert
        Should.NotThrow(action);
    }

    [Fact]
    public void CallingSwitchFirst_WhenIsSuccess_ShouldExecuteThenAction()
    {
        // Arrange
        ErrorOr<Person> errorOrPerson = new Person("Dmitry");
        void ThenAction(Person person) => person.ShouldBe(errorOrPerson.Value);
        void OnFirstErrorAction(Error _) => throw new Exception("Should not be called");

        // Act
        Action action = () => errorOrPerson.SwitchFirst(
            ThenAction,
            OnFirstErrorAction);

        // Assert
        Should.NotThrow(action);
    }

    [Fact]
    public void CallingSwitchFirst_WhenIsError_ShouldExecuteOnFirstErrorAction()
    {
        // Arrange
        ErrorOr<Person> errorOrPerson = new List<Error> { Error.Validation(), Error.Conflict() };
        void ThenAction(Person _) => throw new Exception("Should not be called");
        void OnFirstErrorAction(Error errors)
        {
            errorOrPerson.IsError.ShouldBeTrue();
            errors.ShouldBe(errorOrPerson.Errors[0]);
            errors.ShouldBe(errorOrPerson.FirstError.Value);
        }

        // Act
        Action action = () => errorOrPerson.SwitchFirst(
            ThenAction,
            OnFirstErrorAction);

        // Assert
        Should.NotThrow(action);
    }

    [Fact]
    public async Task CallingSwitchFirstAfterThenAsync_WhenIsSuccess_ShouldExecuteThenAction()
    {
        // Arrange
        ErrorOr<Person> errorOrPerson = new Person("Dmitry");
        void ThenAction(Person person) => person.ShouldBe(errorOrPerson.Value);
        void OnFirstErrorAction(Error _) => throw new Exception("Should not be called");

        // Act
        Func<Task> action = () => errorOrPerson
            .ThenAsync(person => Task.FromResult(person))
            .SwitchFirst(ThenAction, OnFirstErrorAction);

        // Assert
        await Should.NotThrowAsync(action);
    }

    [Fact]
    public async Task CallingSwitchAfterThenAsync_WhenIsSuccess_ShouldExecuteThenAction()
    {
        // Arrange
        ErrorOr<Person> errorOrPerson = new Person("Dmitry");
        void ThenAction(Person person) => person.ShouldBe(errorOrPerson.Value);
        void ElsesAction(ImmutableArray<Error> _) => throw new Exception("Should not be called");

        // Act
        Func<Task> action = () => errorOrPerson
            .ThenAsync(person => Task.FromResult(person))
            .Switch(ThenAction, ElsesAction);

        // Assert
        await Should.NotThrowAsync(action);
    }
}
