using System;

namespace ResultOfTask
{
	public class None
	{
		private None()
		{
		}
	}
	public struct Result<T>
	{
		public Result(string error, T value = default(T))
		{
			Error = error;
			Value = value;
		}
		public string Error { get; }
		internal T Value { get; }
		public T GetValueOrThrow()
		{
			if (IsSuccess) return Value;
			throw new InvalidOperationException($"No value. Only Error {Error}");
		}
		public bool IsSuccess => Error == null;
	}

	public static class Result
	{
		public static Result<T> AsResult<T>(this T value)
		{
			return Ok(value);
		}

		public static Result<T> Ok<T>(T value)
		{
			return new Result<T>(null, value);
		}

	    public static Result<None> Ok()
	    {
	        return Ok<None>(null);
	    }

        public static Result<T> Fail<T>(string e)
		{
			return new Result<T>(e);
		}

		public static Result<T> Of<T>(Func<T> f, string error = null)
		{
			try
			{
				return Ok(f());
			}
			catch (Exception e)
			{
				return Fail<T>(error ?? e.Message);
			}
		}

	    public static Result<None> OfAction(Action f, string error = null)
	    {
	        try
	        {
	            f();
	            return Ok();
	        }
	        catch (Exception e)
	        {
	            return Fail<None>(error ?? e.Message);
	        }
	    }

        public static Result<TOutput> Then<TInput, TOutput>(
			this Result<TInput> input,
			Func<TInput, TOutput> continuation)
		{
		    if (input.Error != null)
		        return Fail<TOutput>(input.Error);
		    TOutput result;
		    try
		    {
		        result = continuation.Invoke(input.Value);
		    }
		    catch (Exception e)
		    {
		        return new Result<TOutput>(e.Message);
		    }
            return new Result<TOutput>(null, result);
		}

		public static Result<TOutput> Then<TInput, TOutput>(
			this Result<TInput> input,
			Func<TInput, Result<TOutput>> continuation)
		{
		    return input.Error != null 
                ? Fail<TOutput>(input.Error) 
                : continuation(input.Value);
		}

		public static Result<TInput> OnFail<TInput>(
			this Result<TInput> input,
			Action<string> handleError)
		{
		    if (input.Error != null)
                handleError(input.Error);
		    return input;
		}

	    public static Result<T> ReplaceError<T>(this Result<T> result, Func<string, string> replacer)
	    {
	        return result.Error == null ? result : Fail<T>(replacer(result.Error));
	    }

	    public static Result<T> RefineError<T>(this Result<T> result, string errorPrefix)
	    {
	        return Fail<T>($"{errorPrefix}. {result.Error}");
	    }
	}
}