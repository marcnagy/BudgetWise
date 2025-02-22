import { User } from "./user";

export interface Expense {
    id: number;
    amount: number;
    date: string;
    name: string;
    description: string;
    userId: number;
    user: User;
}
export type ExpenseInput = {
    name: string;
    amount: number;
    date: string;
    description: string;
  };
  