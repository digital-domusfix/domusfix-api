namespace DomusFix.Api.Application.Common.Models;

// src/Application/Common/Models/Result.cs
public class Result
{
    public bool IsSuccess { get; private set; }
    public List<string> Errors { get; private set; } = new();

    public static Result Success() => new Result { IsSuccess = true };
    public static Result<T> Success<T>(T value) => new Result<T> { IsSuccess = true, Value = value };

    public static Result Failure(List<string> errors) => new Result { IsSuccess = false, Errors = errors };
    public static Result Failure(string errors) => new Result { IsSuccess = false, Errors = new List<string> { errors } };
    public static Result<T> Failure<T>(string error) => new Result<T> { IsSuccess = false, Errors = new List<string> { error } };
}

public class Result<T> : Result
{
    public T? Value { get; set; }
}

