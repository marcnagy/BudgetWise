import { Expense, ExpenseInput } from "../models/expense";
import Cookies from "js-cookie";

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export const loginUser = async (email: string, password: string) => {
  try {
    const response = await fetch(`${API_BASE_URL}/auth/login`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ email, password }),
    });

    if (!response.ok) {
      throw new Error("Invalid email or password");
    }

    return await response.json();
  } catch (error) {
    console.error("Login failed:", error);
    throw error;
  }
};

export const registerUser = async (name: string, email: string, password: string) => {
  try {
    const response = await fetch(`${API_BASE_URL}/auth/register`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ username: name, email, password }),
    });

    if (!response.ok) {
      const errorData = await response.json();
      throw new Error(errorData.message || "Signup failed");
    }

    return await response.json();
  } catch (error) {
    console.error("Signup error:", error);
    throw error;
  }
};

/**
 * Fetch all expenses from the API and cast them into `Expense[]`.
 * @returns {Promise<Expense[]>} A list of expenses.
 */
export const getExpenses = async (): Promise<Expense[]> => {
    console.log(API_BASE_URL);
    
    
    try {
      const response = await fetch(`${API_BASE_URL}/Expense`, {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${Cookies.get("auth_token")}`,
        },
      });
  
      if (!response.ok) {
        throw new Error("Failed to fetch expenses");
      }
  
      const data: Expense[] = await response.json(); // Directly cast response to Expense[]
      return data;
    } catch (error) {
      console.error("Error fetching expenses:", error);
      throw error;
    }
  };

  export const createExpense = async (expense: ExpenseInput) => {
    try {
      const response = await fetch(`${API_BASE_URL}/Expense`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${Cookies.get("auth_token")}`,
        },
        body: JSON.stringify(expense), // ✅ Send only necessary fields
      });
  
      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || "Failed to create expense");
      }
  
      return await response.json(); // Return created expense
    } catch (error) {
      console.error("Error creating expense:", error);
      throw error;
    }
  };


/**
 * Update an existing expense.
 * @param {number} id - Expense ID to update.
 * @param {ExpenseInput} updatedExpense - Updated expense data.
 * @returns {Promise<Expense>} Updated expense object.
 */
export const updateExpense = async (id: number, updatedExpense: ExpenseInput): Promise<Expense> => {
  console.log(updatedExpense);
  
    try {
      const response = await fetch(`${API_BASE_URL}/Expense/${id}`, {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${Cookies.get("auth_token")}`,
        },
        body: JSON.stringify({
          name: updatedExpense.name,
          amount: updatedExpense.amount,
          date: updatedExpense.date,
          description: updatedExpense.description,
        }),
      });
  
      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || "Failed to update expense");
      }
  
      return await response.json();
    } catch (error) {
      console.error("Error updating expense:", error);
      throw error;
    }
  };
  
  /**
   * Delete an expense by ID.
   * @param {number} id - Expense ID to delete.
   * @returns {Promise<void>} Resolves when deletion is successful.
   */
  export const deleteExpense = async (id: number): Promise<void> => {
    try {
      const response = await fetch(`${API_BASE_URL}/Expense/${id}`, {
        method: "DELETE",
        headers: {
          Authorization: `Bearer ${Cookies.get("auth_token")}`,
        },
      });
  
      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || "Failed to delete expense");
      }
    } catch (error) {
      console.error("Error deleting expense:", error);
      throw error;
    }
  };