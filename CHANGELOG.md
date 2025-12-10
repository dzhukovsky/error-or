# Changelog

All notable changes to this project are documented in this file.

## [4.0.0] - 2025-12-10

### Breaking Changes

- Removed the `NumericType` property from `Error`; consumers should rely on `ErrorType` directly.
- `ErrorOr<T>` now throws when constructed with a default or empty `ImmutableArray<Error>`, enforcing that error collections are always non-empty for failure cases.

### Changed

- Refactored `Error` equality and hash code implementations to use `DictionaryEqualityComparer` for `Metadata` comparison and hashing.
- Added `Error.IsDefault` to distinguish uninitialized error instances from valid errors.
- Normalized `Error.Metadata` to always be exposed as a read-only dictionary, improving safety and consistency of metadata usage.
- Improved `IErrorOr` XML documentation and interface contracts for clearer behavior and usage guidance.

### Added

- Introduced `DictionaryEqualityComparer` to provide value-based equality and hash code support for dictionaries.
- Added comprehensive unit tests for `DictionaryEqualityComparer` covering key/value equality and hash code behavior.
- Expanded tests for `ErrorOr<T>` instantiation, `ToErrorOr` conversions, and error array handling to validate the new contracts.

## [3.0.0] - 2025-12-04

### Added

- Support for .NET 9.0 and .NET 10.0 – the library now multi-targets `net8.0`, `net9.0`, and `net10.0`.
- Updated CI workflows to build, test, and publish using .NET 10.0.x.

### Changed

- Internals of `ErrorOr<TValue>` were refactored to use `ImmutableArray<Error>` instead of `List<Error>` for better immutability and performance. All public APIs that expose or accept collections of errors now use `ImmutableArray<Error>`.
- Implicit conversion from `Error` to `ErrorOr<TValue>` now wraps the single error into a non-empty errors collection, ensuring consistent `Errors` behavior.
- Equality and hash code implementations were updated to use sequence equality for the underlying immutable error collection.
- Tests were updated and aligned with the new error representation and target frameworks.
- README and badges were refreshed to point to the new repository and CI setup.

### Breaking Changes

- All APIs that previously accepted or returned `List<Error>` now use `ImmutableArray<Error>`. Call sites relying on the previous collection types may require adjustments.
- Public constructors of `ErrorOr<TValue>` were made `internal`. Creating instances should now be done via `ErrorOrFactory`, `ToErrorOr`, or implicit conversions.
- For successful results, the `Errors` collection no longer contains a special "no error" sentinel item. It is now an empty collection. Code that relied on this sentinel (for example, by assuming `Errors` is always non-empty and contains a "no error" value for success) must be updated to treat an empty `Errors` as the indicator of success.

## [2.0.0] - 2024-03-26

### Added

- `FailIf`

```csharp
public ErrorOr<TValue> FailIf(Func<TValue, bool> onValue, Error error)
```

```csharp
ErrorOr<int> errorOr = 1;
errorOr.FailIf(x => x > 0, Error.Failure());
```

### Breaking Changes

- `Then` that receives an action is now called `ThenDo`

```diff
-public ErrorOr<TValue> Then(Action<TValue> action)
+public ErrorOr<TValue> ThenDo(Action<TValue> action)
```

```diff
-public static async Task<ErrorOr<TValue>> Then<TValue>(this Task<ErrorOr<TValue>> errorOr, Action<TValue> action)
+public static async Task<ErrorOr<TValue>> ThenDo<TValue>(this Task<ErrorOr<TValue>> errorOr, Action<TValue> action)
```

- `ThenAsync` that receives an action is now called `ThenDoAsync`

```diff
-public async Task<ErrorOr<TValue>> ThenAsync(Func<TValue, Task> action)
+public async Task<ErrorOr<TValue>> ThenDoAsync(Func<TValue, Task> action)
```

```diff
-public static async Task<ErrorOr<TValue>> ThenAsync<TValue>(this Task<ErrorOr<TValue>> errorOr, Func<TValue, Task> action)
+public static async Task<ErrorOr<TValue>> ThenDoAsync<TValue>(this Task<ErrorOr<TValue>> errorOr, Func<TValue, Task> action)
```

## [1.10.0] - 2024-02-14

### Added

- `ErrorType.Forbidden`
- README to NuGet package

## [1.9.0] - 2024-01-06

### Added

- `ToErrorOr`
