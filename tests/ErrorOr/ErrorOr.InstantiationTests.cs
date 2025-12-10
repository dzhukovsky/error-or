using System.Collections.Immutable;

namespace Tests;

public class ErrorOrInstantiationTests
{
    private record Person(string Name);

    [Fact]
    public void CreateFromFactory_WhenAccessingValue_ShouldReturnValue()
    {
        // Arrange
        IEnumerable<string> value = ["value"];

        // Act
        var errorOrPerson = ErrorOrFactory.Create(value);

        // Assert
        errorOrPerson.IsError.ShouldBeFalse();
        errorOrPerson.Value.ShouldBeSameAs(value);
    }

    [Fact]
    public void CreateFromFactory_WhenAccessingFirstError_ShouldReturnNull()
    {
        // Arrange
        IEnumerable<string> value = ["value"];
        var errorOrPerson = ErrorOrFactory.Create(value);

        // Act & Assert
        errorOrPerson.FirstError.ShouldBe(default);
    }

    [Fact]
    public void CreateFromFactory_WhenAccessingErrors_ShouldReturnEmptyList()
    {
        // Arrange
        IEnumerable<string> value = ["value"];
        var errorOrPerson = ErrorOrFactory.Create(value);

        // Act & Assert
        errorOrPerson.Errors.ShouldBeEmpty();
    }

    [Fact]
    public void CreateFromValue_WhenAccessingValue_ShouldReturnValue()
    {
        // Arrange
        IEnumerable<string> value = ["value"];

        // Act
        var errorOrPerson = ErrorOrFactory.Create(value);

        // Assert
        errorOrPerson.IsError.ShouldBeFalse();
        errorOrPerson.Value.ShouldBeSameAs(value);
    }

    [Fact]
    public void CreateFromValue_WhenAccessingFirstError_ShouldReturnNull()
    {
        // Arrange
        IEnumerable<string> value = ["value"];
        var errorOrPerson = ErrorOrFactory.Create(value);

        // Act & Assert
        errorOrPerson.FirstError.ShouldBe(default);
    }

    [Fact]
    public void CreateFromValue_WhenAccessingErrors_ShouldReturnEmptyList()
    {
        // Arrange
        IEnumerable<string> value = ["value"];
        var errorOrPerson = ErrorOrFactory.Create(value);

        // Act & Assert
        errorOrPerson.Errors.ShouldBeEmpty();
    }

    [Fact]
    public void CreateFromErrorList_WhenAccessingErrors_ShouldReturnErrorList()
    {
        // Arrange
        List<Error> errors = [Error.Validation("User.Name", "Name is too short")];
        var errorOrPerson = ErrorOrFactory.Create<Person>(errors);

        // Act & Assert
        errorOrPerson.IsError.ShouldBeTrue();
        errorOrPerson.Errors.ShouldHaveSingleItem().ShouldBe(errors.Single());
    }

    [Fact]
    public void CreateFromErrorList_WhenAccessingValue_ShouldReturnNull()
    {
        // Arrange
        List<Error> errors = [Error.Validation("User.Name", "Name is too short")];
        var errorOrPerson = ErrorOrFactory.Create<Person>(errors);

        // Act & Assert
        errorOrPerson.Value.ShouldBeNull();
    }

    [Fact]
    public void ImplicitCastResult_WhenAccessingResult_ShouldReturnValue()
    {
        // Arrange
        Person result = new Person("Amici");

        // Act
        ErrorOr<Person> errorOr = result;

        // Assert
        errorOr.IsError.ShouldBeFalse();
        errorOr.Value.ShouldBe(result);
    }

    [Fact]
    public void ImplicitCastResult_WhenAccessingErrors_ShouldReturnEmptyList()
    {
        ErrorOr<Person> errorOrPerson = new Person("Dmitry");

        // Act & Assert
        errorOrPerson.Errors.ShouldBeEmpty();
    }

    [Fact]
    public void ImplicitCastResult_WhenAccessingFirstError_ShouldReturnNull()
    {
        ErrorOr<Person> errorOrPerson = new Person("Dmitry");

        // Act & Assert
        errorOrPerson.FirstError.ShouldBe(default);
    }

    [Fact]
    public void ImplicitCastPrimitiveResult_WhenAccessingResult_ShouldReturnValue()
    {
        // Arrange
        const int result = 4;

        // Act
        ErrorOr<int> errorOrInt = result;

        // Assert
        errorOrInt.IsError.ShouldBeFalse();
        errorOrInt.Value.ShouldBe(result);
    }

    [Fact]
    public void ImplicitCastErrorOrType_WhenAccessingResult_ShouldReturnValue()
    {
        // Act
        ErrorOr<Success> errorOrSuccess = Result.Success;
        ErrorOr<Created> errorOrCreated = Result.Created;
        ErrorOr<Deleted> errorOrDeleted = Result.Deleted;
        ErrorOr<Updated> errorOrUpdated = Result.Updated;

        // Assert
        errorOrSuccess.IsError.ShouldBeFalse();
        errorOrSuccess.Value.ShouldBe(Result.Success);

        errorOrCreated.IsError.ShouldBeFalse();
        errorOrCreated.Value.ShouldBe(Result.Created);

        errorOrDeleted.IsError.ShouldBeFalse();
        errorOrDeleted.Value.ShouldBe(Result.Deleted);

        errorOrUpdated.IsError.ShouldBeFalse();
        errorOrUpdated.Value.ShouldBe(Result.Updated);
    }

    [Fact]
    public void ImplicitCastSingleError_WhenAccessingErrors_ShouldReturnErrorList()
    {
        // Arrange
        Error error = Error.Validation("User.Name", "Name is too short");

        // Act
        ErrorOr<Person> errorOrPerson = error;

        // Assert
        errorOrPerson.IsError.ShouldBeTrue();
        errorOrPerson.Errors.ShouldHaveSingleItem().ShouldBe(error);
    }

    [Fact]
    public void ImplicitCastError_WhenAccessingValue_ShouldReturnNull()
    {
        // Arrange
        ErrorOr<Person> errorOrPerson = Error.Validation("User.Name", "Name is too short");

        // Act & Assert
        errorOrPerson.Value.ShouldBeNull();
    }

    [Fact]
    public void ImplicitCastSingleError_WhenAccessingFirstError_ShouldReturnError()
    {
        // Arrange
        Error error = Error.Validation("User.Name", "Name is too short");

        // Act
        ErrorOr<Person> errorOrPerson = error;

        // Assert
        errorOrPerson.IsError.ShouldBeTrue();
        errorOrPerson.FirstError.ShouldBe(error);
    }

    [Fact]
    public void ImplicitCastErrorList_WhenAccessingErrors_ShouldReturnErrorList()
    {
        // Arrange
        List<Error> errors =
        [
            Error.Validation("User.Name", "Name is too short"),
            Error.Validation("User.Age", "User is too young"),
        ];

        // Act
        ErrorOr<Person> errorOrPerson = errors;

        // Assert
        errorOrPerson.IsError.ShouldBeTrue();
        errorOrPerson.Errors.Length.ShouldBe(errors.Count);
        errorOrPerson.Errors.ShouldBe(errors);
    }

    [Fact]
    public void ImplicitCastErrorArray_WhenAccessingErrors_ShouldReturnErrorArray()
    {
        // Arrange
        Error[] errors =
        [
            Error.Validation("User.Name", "Name is too short"),
            Error.Validation("User.Age", "User is too young"),
        ];

        // Act
        ErrorOr<Person> errorOrPerson = errors;

        // Assert
        errorOrPerson.IsError.ShouldBeTrue();
        errorOrPerson.Errors.Length.ShouldBe(errors.Length);
        errorOrPerson.Errors.ShouldBe(errors);
    }

    [Fact]
    public void ImplicitCastErrorList_WhenAccessingFirstError_ShouldReturnFirstError()
    {
        // Arrange
        List<Error> errors =
        [
            Error.Validation("User.Name", "Name is too short"),
            Error.Validation("User.Age", "User is too young"),
        ];

        // Act
        ErrorOr<Person> errorOrPerson = errors;

        // Assert
        errorOrPerson.IsError.ShouldBeTrue();
        errorOrPerson.FirstError.ShouldBe(errors[0]);
    }

    [Fact]
    public void ImplicitCastErrorArray_WhenAccessingFirstError_ShouldReturnFirstError()
    {
        // Arrange
        Error[] errors =
        [
            Error.Validation("User.Name", "Name is too short"),
            Error.Validation("User.Age", "User is too young"),
        ];

        // Act
        ErrorOr<Person> errorOrPerson = errors;

        // Assert
        errorOrPerson.IsError.ShouldBeTrue();
        errorOrPerson.FirstError.ShouldBe(errors[0]);
    }

    [Fact]
    public void ImplicitCastImmutableArrayOfErrors_WhenAccessingErrors_ShouldReturnErrors()
    {
        // Arrange
        ImmutableArray<Error> errors =
        [
            Error.Validation("User.Name", "Name is too short"),
            Error.Validation("User.Age", "User is too young"),
        ];

        // Act
        ErrorOr<Person> errorOrPerson = errors;

        // Assert
        errorOrPerson.IsError.ShouldBeTrue();
        errorOrPerson.Errors.Length.ShouldBe(errors.Length);
        errorOrPerson.Errors.ShouldBe(errors);
    }

    [Fact]
    public void ImplicitCastImmutableArrayOfErrors_WhenAccessingFirstError_ShouldReturnFirstError()
    {
        // Arrange
        ImmutableArray<Error> errors =
        [
            Error.Validation("User.Name", "Name is too short"),
            Error.Validation("User.Age", "User is too young"),
        ];

        // Act
        ErrorOr<Person> errorOrPerson = errors;

        // Assert
        errorOrPerson.IsError.ShouldBeTrue();
        errorOrPerson.FirstError.ShouldBe(errors[0]);
    }

    [Fact]
    public void CreateErrorOr_WhenUsingEmptyConstructor_ShouldThrow()
    {
        // Act & Assert
        Should.Throw<InvalidOperationException>(() => { _ = new ErrorOr<int>(); });
    }

    [Fact]
    public void CreateErrorOr_WhenEmptyErrorsList_ShouldThrow()
    {
        // Act & Assert
        var exception = Should.Throw<ArgumentException>(() => { ErrorOr<int> errorOr = new List<Error>(); });
        exception.Message.ShouldContain("Cannot create an ErrorOr<> from an empty collection of errors. Provide at least one error.");
        exception.ParamName.ShouldBe("errors");
    }

    [Fact]
    public void CreateErrorOr_WhenEmptyErrorsArray_ShouldThrow()
    {
        // Act & Assert
        var exception = Should.Throw<ArgumentException>(() => { ErrorOr<int> errorOr = Array.Empty<Error>(); });
        exception.Message.ShouldContain("Cannot create an ErrorOr<> from an empty collection of errors. Provide at least one error.");
        exception.ParamName.ShouldBe("errors");
    }

    [Fact]
    public void CreateErrorOr_WhenNullIsPassedAsErrorsList_ShouldThrowArgumentNullException()
    {
        var exception = Should.Throw<ArgumentNullException>(() => { ErrorOr<int> errorOr = default(List<Error>)!; });
        exception.ParamName.ShouldBe("errors");
    }

    [Fact]
    public void CreateErrorOr_WhenNullIsPassedAsErrorsArray_ShouldThrowArgumentNullException()
    {
        var exception = Should.Throw<ArgumentNullException>(() => { ErrorOr<int> errorOr = default(Error[])!; });
        exception.ParamName.ShouldBe("errors");
    }

    [Fact]
    public void CreateErrorOr_WhenEmptyImmutableArrayIsUsed_ShouldThrow()
    {
        // Arrange
        ImmutableArray<Error> errors = [];

        // Act & Assert
        var exception = Should.Throw<ArgumentException>(() => { ErrorOr<int> errorOr = errors; });
        exception.Message.ShouldContain("Cannot create an ErrorOr<> from an empty collection of errors. Provide at least one error.");
        exception.ParamName.ShouldBe("errors");
    }

    [Fact]
    public void CreateErrorOr_WhenDefaultImmutableArrayIsUsed_ShouldThrow()
    {
        // Arrange
        ImmutableArray<Error> errors = default;

        // Act & Assert
        var exception = Should.Throw<ArgumentException>(() => { ErrorOr<int> errorOr = errors; });
        exception.Message.ShouldContain("Cannot create an ErrorOr<> from an empty collection of errors. Provide at least one error.");
        exception.ParamName.ShouldBe("errors");
    }

    [Fact]
    public void CreateErrorOr_WhenValueIsNull_ShouldThrowArgumentNullException()
    {
        var exception = Should.Throw<ArgumentNullException>(() => { ErrorOr<int?> errorOr = default(int?); });
        exception.ParamName.ShouldBe("value");
    }
}
