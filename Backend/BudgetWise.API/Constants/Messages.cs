namespace BudgetWise.API.Constants;

/// <summary>
/// Constants for error messages and success messages.
/// </summary>
public static class Messages
{
    public const string NoExpensesFound = "No expenses found.";
    public const string ExpenseNotFound = "Expense not found.";
    public const string InvalidExpenseId = "Invalid expense ID.";
    public const string ExpenseDataRequired = "Expense data is required.";
    public const string InvalidExpenseData = "Invalid expense data. Ensure all required fields are filled correctly.";
    public const string ExpenseCreated = "Expense created successfully.";
    public const string ExpenseUpdated = "Expense updated successfully.";
    public const string ExpenseDeleted = "Expense deleted successfully.";
    public const string ExpenseDeleteFailed = "Expense not found or could not be deleted.";
    public const string InternalServerError = "An error occurred while processing your request.";
    public const string InvalidToken = "User ID not found in token.";

    // Validation Errors
    public const string InvalidRequest = "Invalid request data.";
    public const string InvalidEmailOrPassword = "Invalid email or password.";
    public const string EmailAlreadyExists = "An account with this email already exists.";
    public const string UsernameAlreadyExists = "An account with this username already exists.";
    public const string PasswordTooWeak = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, and one digit.";

    // Success Messages
    public const string UserRegistered = "User registered successfully.";
}
