import { Expense } from "./expense";

export interface User {
    id: number;
    username: string;
    email: string;
    passwordHash: string;
    expenses: Expense[]; 
}

